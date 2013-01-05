using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalHostServer : IServer {
	
	#region IServer implementation
	List<UnitData> IServer.GetUnitDataList (int playerID)
	{
		List<UnitData> listUnitData = new List<UnitData>(){
			new UnitData(){
				iID = 1,
				iLevel = 1,
				eUnit = EUnit.Trees,
				vPos = new Vector2(2,3)
			},
			new UnitData(){
				iID = 2,
				iLevel = 1,
				eUnit = EUnit.Trees,
				vPos = new Vector2(5,3)
			},
			new UnitData(){
				iID = 3,
				iLevel = 1,
				eUnit = EUnit.Trees,
				vPos = new Vector2(-5,3)
			}
		};
		return listUnitData;
	}
	#endregion
	
}
