function OnEnable()
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

function OnDisable()
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

//--------------------------------------------------------------------------------------
//                      Per-Finger Gestures Callbacks
//--------------------------------------------------------------------------------------

function FingerGestures_OnFingerDown( fingerIndex : int, fingerPos : Vector2 )
{

}

function FingerGestures_OnFingerUp( fingerIndex : int, fingerPos : Vector2, timeHeldDown : float )
{

}

function FingerGestures_OnFingerMoveBegin( fingerIndex : int, fingerPos : Vector2 )
{

}

function FingerGestures_OnFingerMove( fingerIndex : int, fingerPos : Vector2 )
{

}

function FingerGestures_OnFingerMoveEnd( fingerIndex : int, fingerPos : Vector2 )
{

}

function FingerGestures_OnFingerStationaryBegin( fingerIndex : int, fingerPos : Vector2 )
{

}

function FingerGestures_OnFingerStationary( fingerIndex : int, fingerPos : Vector2, elapsedTime : float )
{

}

function FingerGestures_OnFingerStationaryEnd( fingerIndex : int, fingerPos : Vector2, elapsedTime : float )
{

}

function FingerGestures_OnFingerLongPress( fingerIndex : int, fingerPos : Vector2 )
{

}

function FingerGestures_OnFingerTap( fingerIndex : int, fingerPos : Vector2, tapCount : int )
{

}

function FingerGestures_OnFingerSwipe( fingerIndex : int, startPos : Vector2, direction : FingerGestures.SwipeDirection, velocity : float )
{

}

function FingerGestures_OnFingerDragBegin( fingerIndex : int, fingerPos : Vector2, startPos : Vector2 )
{

}

function FingerGestures_OnFingerDragMove( fingerIndex : int, fingerPos : Vector2, delta : Vector2 )
{

}

function FingerGestures_OnFingerDragEnd( fingerIndex : int, fingerPos : Vector2 )
{

}

//--------------------------------------------------------------------------------------
//                      Global Gestures Callbacks
//--------------------------------------------------------------------------------------

function FingerGestures_OnLongPress( fingerPos : Vector2 )
{

}

function FingerGestures_OnTap( fingerPos : Vector2, tapCount : int )
{

}

function FingerGestures_OnSwipe( startPos : Vector2, direction : FingerGestures.SwipeDirection, velocity : float )
{

}

function FingerGestures_OnDragBegin( fingerPos : Vector2, startPos : Vector2 )
{

}

function FingerGestures_OnDragMove( fingerPos : Vector2, delta : Vector2 )
{

}

function FingerGestures_OnDragEnd( fingerPos : Vector2 )
{

}

function FingerGestures_OnPinchBegin( fingerPos1 : Vector2, fingerPos2 : Vector2 )
{
        
}

function FingerGestures_OnPinchMove( fingerPos1 : Vector2, fingerPos2 : Vector2, delta : float )
{
        
}

function FingerGestures_OnPinchEnd( fingerPos1 : Vector2, fingerPos2 : Vector2 )
{

}

function FingerGestures_OnRotationBegin( fingerPos1 : Vector2, fingerPos2 : Vector2 )
{

}

function FingerGestures_OnRotationMove( fingerPos1 : Vector2, fingerPos2 : Vector2, rotationAngleDelta : float )
{

}

function FingerGestures_OnRotationEnd( fingerPos1 : Vector2, fingerPos2 : Vector2, totalRotationAngle : float  )
{

}

function FingerGestures_OnTwoFingerLongPress( fingerPos : Vector2 )
{

}

function FingerGestures_OnTwoFingerTap( fingerPos : Vector2, tapCount : int )
{

}

function FingerGestures_OnTwoFingerSwipe( startPos : Vector2, direction : FingerGestures.SwipeDirection, velocity : float )
{

}

function FingerGestures_OnTwoFingerDragBegin( fingerPos : Vector2, startPos : Vector2 )
{

}

function FingerGestures_OnTwoFingerDragMove( fingerPos : Vector2, delta : Vector2 )
{

}

function FingerGestures_OnTwoFingerDragEnd( fingerPos : Vector2 )
{

}