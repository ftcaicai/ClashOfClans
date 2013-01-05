using UnityEngine;
using System.Collections;

public class UnitFactory {

	public static string GetUnitPerfabPathAndName (UnitData unitData){
		string strPath = "";
		switch(unitData.eUnit){
		case EUnit.Stone:
			
			break;
		case EUnit.Trees:
			strPath = string.Format("Unit/Tree/Tree_{0}", unitData.iLevel);
			break;
		}
		return strPath;
	}
	
	public static Unit AddComponent (UnitData unitData, GameObject unitObject){
		Unit addUnit = null;
		switch(unitData.eUnit){
		case EUnit.Stone:
			
			break;
		case EUnit.Trees:
			addUnit = unitObject.AddComponent<Unit> ();
			break;
		}
		return addUnit;
	}
}
