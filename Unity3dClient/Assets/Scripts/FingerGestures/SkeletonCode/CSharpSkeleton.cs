using UnityEngine;
using System.Collections;

public class CSharpSkeleton : MonoBehaviour 
{
    void OnEnable()
    {
        // Register to FingerGestures events

        // per-finger gestures
        FingerGestures.OnFingerDown += FingerGestures_OnFingerDown;
        FingerGestures.OnFingerStationaryBegin += FingerGestures_OnFingerStationaryBegin;
        FingerGestures.OnFingerStationary += FingerGestures_OnFingerStationary;
        FingerGestures.OnFingerStationaryEnd += FingerGestures_OnFingerStationaryEnd;
        FingerGestures.OnFingerMoveBegin += FingerGestures_OnFingerMoveBegin;
        FingerGestures.OnFingerMove += FingerGestures_OnFingerMove;
        FingerGestures.OnFingerMoveEnd += FingerGestures_OnFingerMoveEnd;
        FingerGestures.OnFingerUp += FingerGestures_OnFingerUp;
        FingerGestures.OnFingerLongPress += FingerGestures_OnFingerLongPress;
        FingerGestures.OnFingerTap += FingerGestures_OnFingerTap;
        FingerGestures.OnFingerSwipe += FingerGestures_OnFingerSwipe;
        FingerGestures.OnFingerDragBegin += FingerGestures_OnFingerDragBegin;
        FingerGestures.OnFingerDragMove += FingerGestures_OnFingerDragMove;
        FingerGestures.OnFingerDragEnd += FingerGestures_OnFingerDragEnd;
        
        // global gestures
        FingerGestures.OnLongPress += FingerGestures_OnLongPress;
        FingerGestures.OnTap += FingerGestures_OnTap;
        FingerGestures.OnSwipe += FingerGestures_OnSwipe;
        FingerGestures.OnDragBegin += FingerGestures_OnDragBegin;
        FingerGestures.OnDragMove += FingerGestures_OnDragMove;
        FingerGestures.OnDragEnd += FingerGestures_OnDragEnd;
        FingerGestures.OnPinchBegin += FingerGestures_OnPinchBegin;
        FingerGestures.OnPinchMove += FingerGestures_OnPinchMove; 
        FingerGestures.OnPinchEnd += FingerGestures_OnPinchEnd;
        FingerGestures.OnRotationBegin += FingerGestures_OnRotationBegin;
        FingerGestures.OnRotationMove += FingerGestures_OnRotationMove;
        FingerGestures.OnRotationEnd += FingerGestures_OnRotationEnd;
        FingerGestures.OnTwoFingerLongPress += FingerGestures_OnTwoFingerLongPress;
        FingerGestures.OnTwoFingerTap += FingerGestures_OnTwoFingerTap;
        FingerGestures.OnTwoFingerSwipe += FingerGestures_OnTwoFingerSwipe;
        FingerGestures.OnTwoFingerDragBegin += FingerGestures_OnTwoFingerDragBegin;
        FingerGestures.OnTwoFingerDragMove += FingerGestures_OnTwoFingerDragMove;
        FingerGestures.OnTwoFingerDragEnd += FingerGestures_OnTwoFingerDragEnd;
    }

    void OnDisable()
    {
        // Unregister FingerGestures events so we will no longer receive notifications after this object is disabled

        // per-finger gestures
        FingerGestures.OnFingerDown -= FingerGestures_OnFingerDown;
        FingerGestures.OnFingerStationaryBegin -= FingerGestures_OnFingerStationaryBegin;
        FingerGestures.OnFingerStationary -= FingerGestures_OnFingerStationary;
        FingerGestures.OnFingerStationaryEnd -= FingerGestures_OnFingerStationaryEnd;
        FingerGestures.OnFingerMoveBegin -= FingerGestures_OnFingerMoveBegin;
        FingerGestures.OnFingerMove -= FingerGestures_OnFingerMove;
        FingerGestures.OnFingerMoveEnd -= FingerGestures_OnFingerMoveEnd;
        FingerGestures.OnFingerUp -= FingerGestures_OnFingerUp;
        FingerGestures.OnFingerLongPress -= FingerGestures_OnFingerLongPress;
        FingerGestures.OnFingerTap -= FingerGestures_OnFingerTap;
        FingerGestures.OnFingerSwipe -= FingerGestures_OnFingerSwipe;
        FingerGestures.OnFingerDragBegin -= FingerGestures_OnFingerDragBegin;
        FingerGestures.OnFingerDragMove -= FingerGestures_OnFingerDragMove;
        FingerGestures.OnFingerDragEnd -= FingerGestures_OnFingerDragEnd;

        // global gestures
        FingerGestures.OnLongPress -= FingerGestures_OnLongPress;
        FingerGestures.OnTap -= FingerGestures_OnTap;
        FingerGestures.OnSwipe -= FingerGestures_OnSwipe;
        FingerGestures.OnDragBegin -= FingerGestures_OnDragBegin;
        FingerGestures.OnDragMove -= FingerGestures_OnDragMove;
        FingerGestures.OnDragEnd -= FingerGestures_OnDragEnd;
        FingerGestures.OnPinchBegin -= FingerGestures_OnPinchBegin;
        FingerGestures.OnPinchMove -= FingerGestures_OnPinchMove;
        FingerGestures.OnPinchEnd -= FingerGestures_OnPinchEnd;
        FingerGestures.OnRotationBegin -= FingerGestures_OnRotationBegin;
        FingerGestures.OnRotationMove -= FingerGestures_OnRotationMove;
        FingerGestures.OnRotationEnd -= FingerGestures_OnRotationEnd;
        FingerGestures.OnTwoFingerLongPress -= FingerGestures_OnTwoFingerLongPress;
        FingerGestures.OnTwoFingerTap -= FingerGestures_OnTwoFingerTap;
        FingerGestures.OnTwoFingerSwipe -= FingerGestures_OnTwoFingerSwipe;
        FingerGestures.OnTwoFingerDragBegin -= FingerGestures_OnTwoFingerDragBegin;
        FingerGestures.OnTwoFingerDragMove -= FingerGestures_OnTwoFingerDragMove;
        FingerGestures.OnTwoFingerDragEnd -= FingerGestures_OnTwoFingerDragEnd;
    }

    #region Per-Finger Event Callbacks

    void FingerGestures_OnFingerDown( int fingerIndex, Vector2 fingerPos )
    {

    }

    void FingerGestures_OnFingerMoveBegin( int fingerIndex, Vector2 fingerPos )
    {

    }

    void FingerGestures_OnFingerMove( int fingerIndex, Vector2 fingerPos )
    {

    }

    void FingerGestures_OnFingerMoveEnd( int fingerIndex, Vector2 fingerPos )
    {

    }

    void FingerGestures_OnFingerStationaryBegin( int fingerIndex, Vector2 fingerPos )
    {

    }

    void FingerGestures_OnFingerStationary( int fingerIndex, Vector2 fingerPos, float elapsedTime )
    {

    }

    void FingerGestures_OnFingerStationaryEnd( int fingerIndex, Vector2 fingerPos, float elapsedTime )
    {

    }

    void FingerGestures_OnFingerUp( int fingerIndex, Vector2 fingerPos, float timeHeldDown )
    {

    }

    void FingerGestures_OnFingerLongPress( int fingerIndex, Vector2 fingerPos )
    {

    }

    void FingerGestures_OnFingerTap( int fingerIndex, Vector2 fingerPos, int tapCount )
    {

    }

    void FingerGestures_OnFingerSwipe( int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity )
    {

    }

    void FingerGestures_OnFingerDragBegin( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
    {

    }

    void FingerGestures_OnFingerDragMove( int fingerIndex, Vector2 fingerPos, Vector2 delta )
    {

    }

    void FingerGestures_OnFingerDragEnd( int fingerIndex, Vector2 fingerPos )
    {

    }

    #endregion

    #region Global Gesture Callbacks
    
    void FingerGestures_OnLongPress( Vector2 fingerPos )
    {

    }

    void FingerGestures_OnTap( Vector2 fingerPos, int tapCount )
    {

    }

    void FingerGestures_OnSwipe( Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity )
    {

    }

    void FingerGestures_OnDragBegin( Vector2 fingerPos, Vector2 startPos )
    {

    }

    void FingerGestures_OnDragMove( Vector2 fingerPos, Vector2 delta )
    {

    }

    void FingerGestures_OnDragEnd( Vector2 fingerPos )
    {

    }

    void FingerGestures_OnPinchBegin( Vector2 fingerPos1, Vector2 fingerPos2 )
    {
        
    }

    void FingerGestures_OnPinchMove( Vector2 fingerPos1, Vector2 fingerPos2, float delta )
    {
        
    }

    void FingerGestures_OnPinchEnd( Vector2 fingerPos1, Vector2 fingerPos2 )
    {

    }

    void FingerGestures_OnRotationBegin( Vector2 fingerPos1, Vector2 fingerPos2 )
    {

    }

    void FingerGestures_OnRotationMove( Vector2 fingerPos1, Vector2 fingerPos2, float rotationAngleDelta )
    {

    }

    void FingerGestures_OnRotationEnd( Vector2 fingerPos1, Vector2 fingerPos2, float totalRotationAngle )
    {

    }

    void FingerGestures_OnTwoFingerLongPress( Vector2 fingerPos )
    {

    }

    void FingerGestures_OnTwoFingerTap( Vector2 fingerPos, int tapCount )
    {

    }

    void FingerGestures_OnTwoFingerSwipe( Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity )
    {

    }

    void FingerGestures_OnTwoFingerDragBegin( Vector2 fingerPos, Vector2 startPos )
    {

    }

    void FingerGestures_OnTwoFingerDragMove( Vector2 fingerPos, Vector2 delta )
    {

    }

    void FingerGestures_OnTwoFingerDragEnd( Vector2 fingerPos )
    {

    }

    #endregion
}
