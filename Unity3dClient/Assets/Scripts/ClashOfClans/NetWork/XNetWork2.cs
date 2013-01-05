using UnityEngine;
using System.Collections;
using System.Threading;

public class XNetWork2  {
	
	internal class XConnectProcessResultHandler : RakNet.FT_ConnectProcessResultHandler {
		
		public override void OnConnectedToServer ()
		{
			Debug.Log ("OnConnectedToServer");
		}
				
		public override void OnDisconnectedFromServer ()
		{
			Debug.Log ("OnDisconnectedFromServer");
		}
		
		public override void OnFailedToConnect ()
		{
			Debug.Log ("OnFailedToConnect");
		}
		
		public override void OnLostConnection ()
		{
			Debug.Log ("OnLostConnection");
		}
		
		public override void ReceiveLog ()
		{
			Debug.Log("ReceiveLog");
		}
		
		public override void DebugReceive (int flag)
		{
			Debug.Log("DebugReceive");
		}
		
	}
	
	private RakNet.RakPeerInterface		_peer;
	private RakNet.SocketDescriptor		_socketDes;
	private RakNet.Packet				_packet;
	private string 					_ServerIP;
	private ushort					_ServerPort;
	private string 					_ConnectPassword;
	private RakNet.FT_ConnectProcess	_process;
	private XConnectProcessResultHandler _resultHandler;
	private bool					_bHasConnectSuccess;
	private Thread					_ThreadForReadMessage;
	
	internal virtual void _Init (){
		if (_peer == null){
			_peer = RakNet.RakPeerInterface.GetInstance();
			_socketDes = new RakNet.SocketDescriptor();
			_peer.Startup(1,_socketDes,1);
			
			_process = RakNet.FT_ConnectProcess.GetInstance();
			_resultHandler = new XConnectProcessResultHandler();
			_process.SetResultHandler( _resultHandler );
			_peer.AttachPlugin ( _process );
			
		}
	}
	
	private void _InitReadThread (){
		if (_ThreadForReadMessage == null){
			_ThreadForReadMessage = new Thread(new ThreadStart(ReadMessage));
			_ThreadForReadMessage.IsBackground = true;
			_ThreadForReadMessage.Start ();
		}
	}
	
	void ReadMessage (){
		while (true){
			_packet = _peer.Receive ();
			while (_packet != null ){
				ProcessPacket ( _packet );
				_peer.DeallocatePacket( _packet );
				_packet = _peer.Receive ();
			}
			Thread.Sleep ( 30 );
		}
	}
	
	void ProcessPacket (RakNet.Packet packet){
		
	}
	
	public RakNet.ConnectionAttemptResult Connect (string ip, ushort remotePort, string password){
		_ServerIP = ip;
		_ServerPort = remotePort;
		_ConnectPassword = password;
		
		RakNet.ConnectionAttemptResult result = _peer.Connect (_ServerIP, _ServerPort, _ConnectPassword, _ConnectPassword.Length);
		if (result == RakNet.ConnectionAttemptResult.CONNECTION_ATTEMPT_STARTED){
			_bHasConnectSuccess = true;
			_InitReadThread ();
		}
		return result;
	}
	
	public void Stop (){
		if (_ThreadForReadMessage != null){
			_ThreadForReadMessage.Abort ();
			_ThreadForReadMessage = null;
		}
		if (_peer != null && _bHasConnectSuccess){
			_peer.Shutdown ( 300 );
		}
	}
}
