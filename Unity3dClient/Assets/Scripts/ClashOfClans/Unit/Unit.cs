using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	
}

public struct UnitData {
	
	public int 			iID;
	public int 			iLevel;
	public EUnit		eUnit;
	public Vector2		vPos;
	
}

public enum EUnit 
{
	None = 0,
	Trees = 1,
	Stone = 2,
}
