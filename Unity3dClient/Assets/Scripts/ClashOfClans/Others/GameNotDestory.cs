using UnityEngine;
using System.Collections;

public class GameNotDestory : MonoBehaviour {

	void Awake (){
		DontDestroyOnLoad( this );
	}
	
}
