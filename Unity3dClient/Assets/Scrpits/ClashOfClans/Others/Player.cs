using UnityEngine;
using System.Collections;

public class Player {
	
	public int iID {
		get;
		set;
	}
	
	public string strName {
		get;
		set;
	}
	
	
	private static Player _Guest;
	public static Player Guest {
		get{
			if (_Guest == null){
				_Guest = new Player(){
					iID = 0,
					strName = "Guest",
				};
			}
			return _Guest;
		}
	}
}
