using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InitTownForPlay : MonoBehaviour {
	
	public Transform	initParentObjectT;
	private List<Unit>	_InitUnit;
	
	
	void Awake (){
		_InitUnit = new List<Unit>();
	}
	
	void Start (){
		_Init ();
	}
	
	void _Init (){
		List<UnitData> listData = StaticInstances.Instance.CurrentListUnitData;
		
		int iDataLength = listData.Count;
		for (int i = 0; i < iDataLength; i++ ){
			UnitData unitData = listData[i];
			string strPathAndName = UnitFactory.GetUnitPerfabPathAndName( unitData );
			GameObject unitObject = (GameObject)Instantiate( Resources.Load(strPathAndName) );
			if (unitObject != null){
				unitObject.transform.parent = initParentObjectT;
				unitObject.transform.position = GridConvert.Convert2PutLocation( unitData.vPos, unitObject.transform.position.y );
				_InitUnit.Add( UnitFactory.AddComponent( unitData, unitObject ));
			}
		}
	}
	
}
