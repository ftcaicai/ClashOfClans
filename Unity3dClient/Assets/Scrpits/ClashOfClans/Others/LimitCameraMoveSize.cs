using UnityEngine;
using System.Collections;

public class LimitCameraMoveSize : MonoBehaviour {

	[System.Serializable]
	public class CameraBounds {
		public CameraBound left;						// left
		public CameraBound right;						// right
		public CameraBound top;							// top
		public CameraBound bottom;						// bottom
		
		public Vector2 FilterDelta (Vector2 delta){
			Vector2 retV2 = Vector2.zero;
			retV2.x = delta.x < 0 ? (right.bVisiable ? 0 : delta.x):(left.bVisiable ? 0 : delta.x);
			retV2.y = delta.y > 0 ? (bottom.bVisiable ? 0 : delta.y):(top.bVisiable ? 0 : delta.y);
			
			float fMinX;
			float fMaxX;
			float fMinY;
			float fMaxY;
						
			if (retV2.x > 0){
				fMinX = 0;
				fMaxX = 20;
			}
			else{
				fMinX = -20;
				fMaxX = 0;
			}
			if (retV2.y > 0){
				fMinY = 0;
				fMaxY = 20;
			}
			else{
				fMinY = -20;
				fMaxY = 0;
			}
			retV2.x = Mathf.Clamp(retV2.x, fMinX, fMaxX);
			retV2.y = Mathf.Clamp(retV2.y, fMinY, fMaxY);			
			return retV2;
		}
		public bool bVisiable{
			get{
				return left.bVisiable || right.bVisiable || top.bVisiable || bottom.bVisiable;
			}
		}
	}
	
	public CameraBounds bounds;
	public Transform	cameraT;
	public float		fMoveSpeed = 0.2f;
	
	private bool	_bMove = false;	
	
	void Start (){
		if (!cameraT){
			cameraT = Camera.mainCamera.transform;
		}
		fMoveSpeed = Mathf.Clamp( fMoveSpeed, 0.1f, 0.4f);

	}

	public void Move (Vector2 delta){
		if (_bMove)return;
		_bMove = true;
		Vector2 moveDelta = delta = bounds.FilterDelta( delta );
		cameraT.position += new Vector3( -moveDelta.x * fMoveSpeed, 0, -moveDelta.y * fMoveSpeed); 
		_bMove = false;
	}
	
}
