using UnityEngine;
using System.Collections;
using RakNet;

public class XNetWorkTest : MonoBehaviour {
	
	public string 			serverIP = "";
	public ushort			serverPort = 60000;
	public string			password = "";
	
	private XNetWork	_client;
	
	void _Init (){
		if (_client == null){
			_client = new XNetWork();
			_client._Init();
		}
	}
	
	void _Connect (){
		_Init ();
		ConnectionAttemptResult result = _client.Connect(serverIP, serverPort, password);
		Debug.Log("ConnectionAttemptResult result = " + result);
	}
	
	void Start (){
		_Init();
	}
	
	void OnGUI (){
		if (GUI.Button(new Rect (10,10,200,50), "Connect")){
			_Connect ();
		}
	}
	
	void OnDisable (){
		if (_client != null){
			_client.Stop ();
		}
	}
	
}
