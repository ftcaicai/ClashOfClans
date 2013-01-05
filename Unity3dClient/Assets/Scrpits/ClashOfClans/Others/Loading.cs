using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Loading : MonoBehaviour {
	
	class InitHandler {
		public System.Func<bool>	initFunc;
		public System.Action		errorHandler;
		
		public InitHandler (System.Func<bool> func, System.Action action){
			initFunc = func;
			errorHandler = action;
		}
	}
	
	public Texture			backgroundTexture;
	public StaticInstances	staticInstances;
	
	#region System Method

	void Awake (){
		
	}
	
	void Start (){
		_Init ();
	}
	
	void OnGUI (){
		
		GUI.DrawTexture(new Rect( 0,0, Screen.width, Screen.height), backgroundTexture, ScaleMode.StretchToFill);
		
	}
	
	#endregion
	
	void _Init (){
		List<InitHandler> initAction = new List<InitHandler>(){
			new InitHandler(_InitUnity3d, _InitUnity3dErrorHandler),
			//new InitHandler(_CheckNetWork, _Init),
			//new InitHandler(_CheckLogin, _Init)
			new InitHandler( _GetInitUnitData, _GetInitUnitDataErrorHandler),
		};
		
		int iLoop = 0;
		int iCount = initAction.Count;
		do
		{
			if (!initAction[iLoop].initFunc()){
				initAction[iLoop].errorHandler();
				break;
			}
		}
		while( ++iLoop < iCount);
		
		Application.LoadLevel("TownForPlay");
	}
	
	#region Loading Step
	
	// Step 0 
	bool _InitUnity3d (){
		bool bInit = false;
		string errMsg = "";
		bInit = staticInstances.Init (out errMsg);
		if (!bInit){
			Debug.Log(errMsg);
		}		
		return bInit;
	}
	
	void _InitUnity3dErrorHandler (){
		Debug.LogError("Init Unity3d Class Has Error!");
	}
	
	// Step 1
	bool _CheckNetWork (){
		return false;
	}

	// Step 2
	bool _CheckLogin (){
		return false;	
	}
	
	// Step 3
	bool _GetInitUnitData (){
		return StaticInstances.Instance.LoadPlayerListUnitData();
	}
	
	void _GetInitUnitDataErrorHandler (){
		
	}
	
	#endregion

	Player _GetPlayer (){
		return Player.Guest;
	}
	
}
