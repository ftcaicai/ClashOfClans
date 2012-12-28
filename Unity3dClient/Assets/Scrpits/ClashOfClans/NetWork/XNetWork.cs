using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RakNet;
using System.Threading;

public class XNetWork : System.IDisposable {
	
	public enum XNetworkDisconnection
	{
		LostConnection = 20,
		Disconnected = 19
	}
	
	public enum XNetworkConnectionError {
		NoError,
		RSAPublicKeyMismatch = 21,
		InvalidPassword = 23,
		ConnectionFailed = 15,
		TooManyConnectedPlayers = 18,
		ConnectionBanned = 22,
		AlreadyConnectedToServer = 16,
		AlreadyConnectedToAnotherServer = -1,
		CreateSocketOrThreadFailure = -2,
		IncorrectParameters = -3,
		EmptyConnectTarget = -4,
		InternalDirectConnectFailed = -5,
		NATTargetNotConnected = 69,
		NATTargetConnectionLost = 71,
		NATPunchthroughFailed = 73
	}
	
	internal struct XWaitForProcessData {
		public int 			id { get; set; }
		public byte[] 		data { get; set;}
		public bool 		enable {get; set;}
	}
	
	internal struct XWaitForSendData {
		public int 	id { get; set; }
		public RakNet.BitStream stream { get; set;}
		public bool enable {get; set;}
	}
	
	public event System.Action	OnConnectedToServerHandler;
	public event System.Action<XNetworkConnectionError>	OnFailedToConnectHandler;
	public event System.Action<XNetworkDisconnection>	OnDisconnectedFromServerHandler;
	
	private RakPeerInterface		_peer;
	private SocketDescriptor		_socketDes;
	private string 					_ServerIP;
	private ushort					_ServerPort;
	private string 					_ConnectPassword;
	private Thread					_ThreadForReadMessage;
	private Thread					_ThreadForProcessData;
	private Packet					_Packet;
	private bool					_bHasConnectSuccess;
	
	private List<XWaitForProcessData>	_WaitForProcessData;
	private List<RakNet.BitStream>		_WaitForSendData;
	private	XWaitForProcessData[]		_ProcessDataPools;
	private XWaitForSendData[]			_SendDataPools;
	private int							_ProcessDataPoolsMaxCount;
	private int 						_SendDataPoolsMaxCount;
	private int 						_OutAddCount;
	
	private Dictionary<byte,List<System.Action<byte[]>>> _registerHandler;
	
	public XNetWork (){
		_Awake ();
	}
	
	internal virtual void _Awake (){
		_OutAddCount = 4;
		_ProcessDataPoolsMaxCount = 20;
		_SendDataPoolsMaxCount = 20;
		
		_WaitForSendData = new List<RakNet.BitStream>();
		_WaitForProcessData = new List<XWaitForProcessData>();
		
		_ProcessDataPools = new XWaitForProcessData[_ProcessDataPoolsMaxCount];
		_SendDataPools = new XWaitForSendData[_SendDataPoolsMaxCount];
		
		int minFor = Mathf.Min ( _ProcessDataPoolsMaxCount, _SendDataPoolsMaxCount );
		int outFor = Mathf.Abs ( _SendDataPoolsMaxCount - _ProcessDataPoolsMaxCount );
		for (int i = 0; i < minFor; i++){
			_ProcessDataPools[i] = new XWaitForProcessData(){ id = i, enable = false, data = null };
			_SendDataPools[i] = new XWaitForSendData(){ id = i, enable = false, stream = null };
		}
		
		if (_ProcessDataPoolsMaxCount > _SendDataPoolsMaxCount){
			for (int i = 0; i < outFor; i++){
				_ProcessDataPools[i] = new XWaitForProcessData(){ id = minFor + i, enable = false, data = null };
			}
		}
		else if (_ProcessDataPoolsMaxCount < _SendDataPoolsMaxCount){
			for (int i = 0; i < outFor; i++){
				_SendDataPools[i] = new XWaitForSendData(){ id = minFor + i, enable = false, stream = null };
			}
		}
		_registerHandler = new Dictionary<byte, List<System.Action<byte[]>>>();
	}
	
	internal virtual void _Init (){
		if (_peer == null){
			_peer = RakPeerInterface.GetInstance();
			_socketDes = new SocketDescriptor();
			_peer.Startup(1,_socketDes,1);
			
			RegisterHandler( 
				(byte)DefaultMessageIDTypes.ID_CONNECTION_ATTEMPT_FAILED, 
				(data)=>{ OnFailedToConnect (XNetworkConnectionError.ConnectionFailed); }
			); 
			
			RegisterHandler( 
				(byte)DefaultMessageIDTypes.ID_CONNECTION_REQUEST_ACCEPTED, 
				(data)=>{ OnConnectedToServer (); }
			); 
			
			RegisterHandler( 
				(byte)DefaultMessageIDTypes.ID_CONNECTION_LOST, 
				(data)=>{ OnDisconnectedFromServer (XNetworkDisconnection.LostConnection); }
			); 
			
			RegisterHandler( 
				(byte)DefaultMessageIDTypes.ID_DISCONNECTION_NOTIFICATION, 
				(data)=>{ OnDisconnectedFromServer (XNetworkDisconnection.Disconnected); }
			); 
		}
	}
			
	internal int GetUnUseSendDataInPool (){
		for (int i = 0; i < _SendDataPoolsMaxCount; i++){
			if (!_SendDataPools[i].enable){
				return i;
			}
		}
		ExtendSendDataPool();
		return _SendDataPoolsMaxCount - _OutAddCount;
	}
	
	internal int GetUnUseProcessDataInPool (){
		for (int i = 0; i < _ProcessDataPoolsMaxCount; i++){
			if (!_ProcessDataPools[i].enable){
				return i;
			}
		}
		ExtendProcessDataPool ();
		return _ProcessDataPoolsMaxCount - _OutAddCount;
	}
	
	internal void ExtendSendDataPool (){
		for (int i = 0; i < _OutAddCount; i++){
			_SendDataPools[i] = new XWaitForSendData(){ id = _SendDataPoolsMaxCount + i, enable = false, stream = null };
		}
		_SendDataPoolsMaxCount += _OutAddCount;
	}
	
	internal void ExtendProcessDataPool (){
		for (int i = 0; i < _OutAddCount; i++){
			_ProcessDataPools[i] = new XWaitForProcessData(){ id = _ProcessDataPoolsMaxCount + i, enable = false, data = null };
		}
		_ProcessDataPoolsMaxCount += _OutAddCount;
	}

	internal void OnConnectedToServer (){
		if (OnConnectedToServerHandler != null ){
			OnConnectedToServerHandler ();
		}
	}
	
	internal void OnFailedToConnect (XNetworkConnectionError error){
		if (OnFailedToConnectHandler != null){
			OnFailedToConnectHandler (error);
		}
	}
	
	internal void OnDisconnectedFromServer (XNetworkDisconnection info){
		if (OnDisconnectedFromServerHandler != null){
			OnDisconnectedFromServerHandler (info);
		}
	}
	
	private ConnectionAttemptResult _Connect (){
		_Init ();	
		ConnectionAttemptResult result = _peer.Connect (_ServerIP, _ServerPort, _ConnectPassword, _ConnectPassword.Length);
		if (result == ConnectionAttemptResult.CONNECTION_ATTEMPT_STARTED){
			_bHasConnectSuccess = true;
			_StartThreadForReadMessage ();
			_StartThreadForProcessData ();
		}
		return result;
	}
	
	private void _StartThreadForProcessData (){
		if (_ThreadForProcessData == null){
			_ThreadForProcessData = new Thread(new ThreadStart(_ProcessData));
			_ThreadForProcessData.Name = "_ThreadForProcessData";
			_ThreadForProcessData.IsBackground = true;
			_ThreadForProcessData.Start();
		}
	}
	
	private void _ProcessData (){
		while (true){
			
			if (_WaitForProcessData.Count >= 1){
				XWaitForProcessData processStream = _WaitForProcessData[0];
				_WaitForProcessData.RemoveAt (0);
				_HandlerMessage ( processStream );
			}
			
			System.Threading.Thread.Sleep(10);
		}
	}
	
	private void _HandlerMessage (XWaitForProcessData stream){
		byte flag = stream.data[0];
		
		DefaultMessageIDTypes idtypes = (DefaultMessageIDTypes)flag;
		Debug.Log(idtypes.ToString() + " ["+ System.DateTime.UtcNow.ToString() + "]");
		
		List<System.Action<byte[]>> handler = GetHandlers( flag );
		int iHandlerCount = handler.Count;
		for (int i = 0; i < iHandlerCount; i++){
			if (handler[i] != null){
				try{
					handler[i]( stream.data );
				}
				finally{
				}
			}
		}
		stream.enable = false;
	}
	
	private void _StartThreadForReadMessage (){
		if (_ThreadForReadMessage == null){
			_ThreadForReadMessage = new Thread(new ThreadStart(_ReadMessage));
			_ThreadForReadMessage.Name = "_ThreadForProcessMessage";
			_ThreadForReadMessage.IsBackground = true;
			_ThreadForReadMessage.Start();
		}
	}
		
	private void _ReadMessage (){
		while (true && _peer != null){
			_Packet = _peer.Receive ();
			while(_Packet != null){
				
				XWaitForProcessData pool = _ProcessDataPools[GetUnUseProcessDataInPool()];
				{
					pool.enable = true;
					pool.data = _Packet.data;
				}
				_WaitForProcessData.Add ( pool );
				
				_peer.DeallocatePacket(_Packet);
				_Packet = _peer.Receive ();
				System.Threading.Thread.Sleep(10);
			}
			System.Threading.Thread.Sleep(50);
		}
	}
	
	private List<System.Action<byte[]>> GetHandlers (byte flag){
		return _registerHandler[flag];
	}
	
	public void UnRegisterHandler (byte flag, System.Action<byte[]> handler){
		if (_registerHandler.ContainsKey(flag)){
			List<System.Action<byte[]>> handlers = _registerHandler[flag];
			if (handlers != null && handlers.Count > 0){
				handlers.Remove( handler );
			}
		}
	}
	
	public void RegisterHandler (byte flag, System.Action<byte[]> handler){
		if (_registerHandler.ContainsKey(flag)){
			_registerHandler[flag].Add( handler );
		}
		else{
			_registerHandler.Add( flag, new List<System.Action<byte[]>>(){ handler } );
		}
	}
	
	public ConnectionAttemptResult Connect (string ip, ushort remotePort, string password){
		_ServerIP = ip;
		_ServerPort = remotePort;
		_ConnectPassword = password;
		return _Connect();
	}
	
	public void Stop (){
		Debug.Log("Thread Stop");
		if (_ThreadForProcessData != null){
			_ThreadForProcessData.Abort();
			_ThreadForProcessData = null;
		}
		if (_ThreadForReadMessage != null){
			_ThreadForReadMessage.Abort();
			_ThreadForReadMessage = null;
		}
		if (_peer != null && _bHasConnectSuccess){
			_peer.Shutdown ( 300 );
		}
	}
	
	public void Dispose (){
		Stop ();
	}
}
