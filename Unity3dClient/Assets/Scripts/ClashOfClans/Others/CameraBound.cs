using UnityEngine;
using System.Collections;

public class CameraBound : MonoBehaviour {

	private bool _bEnter = false;
	
	public bool bVisiable {
		get{
			return _bEnter;
		}
		private set{
			_bEnter = value;
		}
	}
	
	void OnBecameVisible (){
		bVisiable = true;
	}
	
	void OnBecameInvisible (){
		bVisiable = false;
	}	
}
