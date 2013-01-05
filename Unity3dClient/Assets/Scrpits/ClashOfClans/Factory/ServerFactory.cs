using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerFactory  {

	private IServer		_server;
	private int 		_iPlayerID = 0;
	
	public ServerFactory (IServer server){
		_server = server;
	}
	
	public List<UnitData>	GetUnitDataList (){
		return _server.GetUnitDataList (_iPlayerID);
	}
	
}
