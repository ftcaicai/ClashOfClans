using UnityEngine;
using System.Collections;
using RakNet;

public class XNetWorkTest : MonoBehaviour {
	
	public string 			serverIP = "";
	public ushort			serverPort = 60000;
	public string			password = "";
	
	private XNetWork	_client;
	private XNetWork2	_client2;
	
	void _Init (){
		if (_client == null){
			_client = new XNetWork();
			_client._Init();
		}
	}
	
	void _Init2 (){
		if (_client2 == null){
			_client2 = new XNetWork2();
			_client2._Init();
		}
	}
	
	void _Connect (){
		_Init ();
		ConnectionAttemptResult result = _client.Connect(serverIP, serverPort, password);
		Debug.Log("ConnectionAttemptResult result = " + result);
	}

	void _Connect2 (){
		_Init2 ();
		ConnectionAttemptResult result = _client2.Connect(serverIP, serverPort, password);
		Debug.Log("ConnectionAttemptResult result = " + result);
	}

	void OnGUI (){
		if (GUI.Button(new Rect (10,10,200,50), "Connect")){
			_Connect ();
		}
		
		if (GUI.Button(new Rect (10,80,200,50), "Connect2")){
			_Connect2 ();
		}
	}
	
	void OnDisable (){
		if (_client != null){
			_client.Stop ();
		}
		if (_client2 != null){
			_client2.Stop ();
		}
	}
	
}
