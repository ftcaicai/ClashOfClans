using UnityEngine;
using System.Collections;
using System;

public class FingerGesture : MonoBehaviour {
	
	private bool		_bMoveCamera;
	private bool		_bMoveUnit;
	private Unit		_CurrentUnit;
	private int 		_dragFingerIndex;
	
	private LimitCameraMoveSize		_LimitCameraMoveSize;
	private event Action<Vector2>	_moveCameraHandler;
	
	void Awake (){
		_bMoveCamera = false;
		_bMoveUnit = false;
	}
	
	void OnEnable (){
		if (_LimitCameraMoveSize == null){
			_LimitCameraMoveSize = (LimitCameraMoveSize)GameObject.FindObjectOfType(typeof(LimitCameraMoveSize));
		}
		if (_LimitCameraMoveSize != null){
			_moveCameraHandler += _LimitCameraMoveSize.Move;
		}
		
		FingerGestures.OnFingerDragBegin += FingerGestures_OnFingerDragBegin;
	    FingerGestures.OnFingerDragMove += FingerGestures_OnFingerDragMove;
	    FingerGestures.OnFingerDragEnd += FingerGestures_OnFingerDragEnd; 
		FingerGestures.OnFingerUp += FingerGestures_OnFingerUp;
	}
	
	void OnDisable (){
		if (_LimitCameraMoveSize != null){
			_moveCameraHandler -= _LimitCameraMoveSize.Move;
		}
		
		FingerGestures.OnFingerDragBegin -= FingerGestures_OnFingerDragBegin;
        FingerGestures.OnFingerDragMove -= FingerGestures_OnFingerDragMove;
        FingerGestures.OnFingerDragEnd -= FingerGestures_OnFingerDragEnd;
		FingerGestures.OnFingerUp -= FingerGestures_OnFingerUp;
	}
	
	void FingerGestures_OnFingerDragBegin( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
    {
		if (_CurrentUnit == null){
			_bMoveCamera = true;
			_bMoveUnit = false;
			_dragFingerIndex = fingerIndex;
		}
		else{
			_bMoveCamera = false;
			_bMoveUnit = true;
		}
    }
	
	void FingerGestures_OnFingerDragMove( int fingerIndex, Vector2 fingerPos, Vector2 delta )
    {
		if (_dragFingerIndex == fingerIndex){
			if (_bMoveCamera){
				_moveCameraHandler(delta);
			}
			else if (_bMoveUnit){
			
			}
		}
	}
	
	void FingerGestures_OnFingerDragEnd( int fingerIndex, Vector2 fingerPos )
    {
		_bMoveCamera = false;
		_bMoveUnit = false;
	}

	void FingerGestures_OnFingerUp (int fingerIndex, Vector2 fingerPos, float timeHeldDown){
		Debug.Log("FingerGestures_OnFingerUp");
			
		if (!_bMoveCamera){
			
			GameObject hitObject = PickObject ( fingerPos, LayerMaskManager.LAYER_UNIT );
			if (hitObject != null){
				Unit unitObject = hitObject.GetComponent<Unit> ();
				if (unitObject != null){
					_CurrentUnit = unitObject;
				}
			}
		}
	}
	
	public static GameObject PickObject( Vector2 screenPos, LayerMask? mask )
	{
	    Ray ray = Camera.main.ScreenPointToRay( screenPos );
	    RaycastHit hit;
		bool bHit = false;
		if (mask.HasValue){
			bHit = Physics.Raycast( ray, out hit, Mathf.Infinity, 1<< mask.Value);
		}
		else{
			bHit = Physics.Raycast( ray, out hit );
		}
	    if( bHit ){
			
			return hit.collider.gameObject;
		}
	    return null;
	}

	public static Vector3 GetWorldPos( Vector2 screenPos )
    {
        Ray ray = Camera.main.ScreenPointToRay( screenPos );
		
        float t = -ray.origin.y / ray.direction.y;

        return ray.GetPoint( t );
    }
	
}
