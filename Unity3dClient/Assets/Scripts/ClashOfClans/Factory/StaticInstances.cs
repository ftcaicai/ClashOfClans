using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticInstances : MonoBehaviour {

	private static StaticInstances	_Instance;
	public static StaticInstances	Instance {
		get {
			if (_Instance == null){
				_Instance = (StaticInstances)GameObject.FindObjectOfType(typeof(StaticInstances));
			}
			return _Instance;
		}
	}
	
	public string 			strServerClass;
	
	
	private ServerFactory	_serverFactory;
	private Player			_CurrentPlayer;
	private List<UnitData>	_ListUnitData;
	
	public ServerFactory	ServerFactory{
		get{
			return _serverFactory;
		}
	}
	
	public Player 			CurrentPlayer{
		get{
			if (_CurrentPlayer == null){
				return Player.Guest;
			}
			return _CurrentPlayer;
		}
		set{
			_CurrentPlayer = value;
		}
	}
	
	public List<UnitData>	CurrentListUnitData {
		get{
			return _ListUnitData;
		}
		set{
			_ListUnitData = value;
		}
	}
	
	public bool Init (out string errMsg){
		if (string.IsNullOrEmpty(strServerClass)){
			errMsg = "Please Set the Server Class Name!";
			return false;
		}
		System.Type t = System.Type.GetType(strServerClass);
		IServer server = (IServer)System.Activator.CreateInstance(t);
		if (server == null){
			errMsg = "ServerFactory Init Error!";
			return false;
		}
		_serverFactory = new ServerFactory( server );
		errMsg="";
		return true;
	}
	
	public bool LoadPlayerListUnitData (){
		bool bRet = true;
		
		CurrentListUnitData = ServerFactory.GetUnitDataList();
		
		return bRet;
	}
}
