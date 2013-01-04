// FingerGestures v2.2 (November 15th 2011)
// The FingerGestures library is copyright (c) of William Ravaine
// Please send feedback or bug reports to spk@fatalfrog.com
// More FingerGestures information at http://www.fatalfrog.com/?page_id=140
// Visit my website @ http://www.fatalfrog.com

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The main interface to the FingerGestures library.
/// Most of the methods are static because FingerGestures is meant to be a singleton.
/// </summary>
public abstract class FingerGestures : MonoBehaviour
{
    // If you are not familiar with events and delegates, or need a refresher, please refer to this youtube video made by the guys 
    // at Prime31 Studios - http://www.youtube.com/watch?v=N2zdwKIsXJs

    #region Global Events (Quick Use Mode)

    // The following events are wrapper/proxies for the default gesture recognizers that are automatically installed and configured at start, provided the relevant options are enabled.
    // Only the most relevant data for each event are passed as arguments to the event delegate. This allows developers to quickly use FingerGestures with minimal knowledge of the inner
    // workings of the library.

    // The default gesture recognizers can be accessed via the FingerGestures.Defaults interface.

    #region Per-Finger events

    //NOTE: events in this category fire independently for each finger, regardless of the state of other fingers
    // You can use FingerGestures.GetFinger(fingerIndex) to retrieve the Finger object corresponding to the fingerIndex event parameter

    #region Delegates

    /// <summary>
    /// Delegate for the OnFingerDown event
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="fingerPos">Current position of the finger on the screen</param>
    public delegate void FingerDownEventHandler( int fingerIndex, Vector2 fingerPos );

    /// <summary>
    /// Delegate for the OnFingerUp event
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="fingerPos">Current position of the finger on the screen</param>
    /// <param name="timeHeldDown">How long the finger has been held down before getting released, in seconds</param>
    public delegate void FingerUpEventHandler( int fingerIndex, Vector2 fingerPos, float timeHeldDown );

    /// <summary>
    /// Delegate for the OnFingerStationaryBegin event
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="fingerPos">Current position of the finger on the screen</param>
    public delegate void FingerStationaryBeginEventHandler( int fingerIndex, Vector2 fingerPos );

    /// <summary>
    /// Delegate for the OnFingerStationary event
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="fingerPos">Current position of the finger on the screen</param>
    /// <param name="elapsedTime">How much time has elapsed, in seconds, since the last OnFingerStationaryBegin fired on this finger</param>
    public delegate void FingerStationaryEventHandler( int fingerIndex, Vector2 fingerPos, float elapsedTime );

    /// <summary>
    /// Delegate for the OnFingerStationaryEnd event
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="fingerPos">Current position of the finger on the screen</param>
    /// <param name="elapsedTime">How much time has elapsed, in seconds, since the last OnFingerStationaryBegin fired on this finger</param>
    public delegate void FingerStationaryEndEventHandler( int fingerIndex, Vector2 fingerPos, float elapsedTime );

    /// <summary>
    /// Delegate for the OnFingerMoveBegin, OnFingerMove, OnFingerMoveEnd events
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="fingerPos">Current position of the finger on the screen</param>
    public delegate void FingerMoveEventHandler( int fingerIndex, Vector2 fingerPos );

    /// <summary>
    /// Delegate for the OnFingernLongPress event
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="fingerPos">Current position of the finger on the screen</param>
    public delegate void FingerLongPressEventHandler( int fingerIndex, Vector2 fingerPos );

    /// <summary>
    /// Delegate for the OnFingernTap event
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="fingerPos">Current position of the finger on the screen</param>
    /// <param name="tapCount">How many times the user has consecutively tapped his finger at this location</param>
    public delegate void FingerTapEventHandler( int fingerIndex, Vector2 fingerPos, int tapCount );

    /// <summary>
    /// Delegate for the OnFingernSwipe event
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="startPos">Initial position of the finger</param>
    /// <param name="direction">Direction of the swipe gesture</param>
    /// <param name="velocity">How quickly the finger has moved (in screen pixels per second)</param>
    public delegate void FingerSwipeEventHandler( int fingerIndex, Vector2 startPos, SwipeDirection direction, float velocity );

    /// <summary>
    /// Delegate for the OnFingernDragBegin event
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="fingerPos">Current position of the finger on the screen</param>
    /// <param name="startPos">The initial finger position on the screen.</param>
    /// <remark>Since the finger has to move beyond a certain treshold distance (specified by the moveThreshold property) 
    /// before the gesture registers as a drag motion, fingerPos and startPos are likely to be different if you specified a non-zero moveThreshold.</remark>
    public delegate void FingerDragBeginEventHandler( int fingerIndex, Vector2 fingerPos, Vector2 startPos );

    /// <summary>
    /// Delegate for the OnFingernDragMove event
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="fingerPos">Current position of the finger on the screen</param>
    /// <param name="delta">How much the finger has moved since the last update. This is the difference between the previous finger position and the new one.</param>
    public delegate void FingerDragMoveEventHandler( int fingerIndex, Vector2 fingerPos, Vector2 delta );

    /// <summary>
    /// Delegate for the OnFingernDragEnd event
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="fingerPos">Current position of the finger on the screen</param>
    public delegate void FingerDragEndEventHandler( int fingerIndex, Vector2 fingerPos );

    #endregion

    #region Events

    /// <summary>
    /// Event fired when a finger's OnDown event fires
    /// <seealso cref="Finger.OnDown"/>
    /// </summary>
    public static event FingerDownEventHandler OnFingerDown;

    /// <summary>
    /// Event fired when a finger's OnUp event fires
    /// <seealso cref="Finger.OnUp"/>
    /// </summary>
    public static event FingerUpEventHandler OnFingerUp;

    /// <summary>
    /// Event fired when a finger's default motion detector OnStationaryBegin event fires
    /// <seealso cref="FingerMotionDetector.OnStationaryBegin"/>
    /// <seealso cref="FingerGestures.Defaults.Fingers.Motion"/>
    /// </summary>
    public static event FingerStationaryBeginEventHandler OnFingerStationaryBegin;

    /// <summary>
    /// Event fired when a finger's default motion detector OnStationary event fires
    /// <seealso cref="FingerMotionDetector.OnStationary"/>
    /// <seealso cref="FingerGestures.Defaults.Fingers.Motion"/>
    /// </summary>
    public static event FingerStationaryEventHandler OnFingerStationary;

    /// <summary>
    /// Event fired when a finger's default motion detector OnStationaryEnd event fires
    /// <seealso cref="FingerMotionDetector.OnStationaryEnd"/>
    /// <seealso cref="FingerGestures.Defaults.Fingers.Motion"/>
    /// </summary>
    public static event FingerStationaryEndEventHandler OnFingerStationaryEnd;

    /// <summary>
    /// Event fired when a finger's default motion detector OnMoveBegin event fires
    /// <seealso cref="FingerMotionDetector.OnMoveBegin"/>
    /// <seealso cref="FingerGestures.Defaults.Fingers.Motion"/>
    /// </summary>
    public static event FingerMoveEventHandler OnFingerMoveBegin;

    /// <summary>
    /// Event fired when a finger's default motion detector OnMove event fires
    /// <seealso cref="FingerMotionDetector.OnMove"/>
    /// <seealso cref="FingerGestures.Defaults.Fingers.Motion"/>
    /// </summary>
    public static event FingerMoveEventHandler OnFingerMove;

    /// <summary>
    /// Event fired when a finger's default motion detector OnMoveEnd event fires
    /// <seealso cref="FingerMotionDetector.OnMoveEnd"/>
    /// <seealso cref="FingerGestures.Defaults.Fingers.Motion"/>
    /// </summary>
    public static event FingerMoveEventHandler OnFingerMoveEnd;

    /// <summary>
    /// Event fired when a finger's long-press gesture recognizer OnLongPress event fires
    /// </summary>
    /// <seealso cref="LongPressGestureRecognizer.OnLongPress"/>
    /// <seealso cref="FingerGestures.Defaults.Fingers.LongPress"/>
    public static event FingerLongPressEventHandler OnFingerLongPress;

    /// <summary>
    /// Event fired when a finger's drag gesture recognizer OnDragBegin event fires
    /// <seealso cref="DragGestureRecognizer.OnDragBegin"/>
    /// <seealso cref="FingerGestures.Defaults.Fingers.Drag"/>
    /// </summary>
    public static event FingerDragBeginEventHandler OnFingerDragBegin;

    /// <summary>
    /// Event fired when a finger's drag gesture recognizer OnDragMove event fires
    /// <seealso cref="DragGestureRecognizer.OnDragMove"/>
    /// <seealso cref="FingerGestures.Defaults.Fingers.Drag"/>
    /// </summary>
    public static event FingerDragMoveEventHandler OnFingerDragMove;

    /// <summary>
    /// Event fired when a finger's drag gesture recognizer OnDragEnd event fires
    /// <see cref="DragGestureRecognizer.OnDragEnd"/>
    /// <seealso cref="FingerGestures.Defaults.Fingers.Drag"/>
    /// </summary>
    public static event FingerDragEndEventHandler OnFingerDragEnd;

    /// <summary>
    /// Event fired when a finger's tap gesture recognizer OnTap event fires
    /// </summary>
    /// <seealso cref="TapGestureRecognizer.OnTap"/>
    /// <seealso cref="FingerGestures.Defaults.Fingers.Tap"/>
    public static event FingerTapEventHandler OnFingerTap;

    /// <summary>
    /// Event fired when a finger's swipe gesture recognizer OnSwipe event fires
    /// </summary>
    /// <seealso cref="SwipeGestureRecognizer.OnSwipe"/>
    /// <seealso cref="FingerGestures.Defaults.Fingers.Swipe"/>
    public static event FingerSwipeEventHandler OnFingerSwipe;

    #endregion

    #region Event-Raising Wrappers

    internal static void RaiseOnFingerDown( int fingerIndex, Vector2 fingerPos )
    {
        if( OnFingerDown != null )
            OnFingerDown( fingerIndex, fingerPos );
    }

    internal static void RaiseOnFingerUp( int fingerIndex, Vector2 fingerPos, float timeHeldDown )
    {
        if( OnFingerUp != null )
            OnFingerUp( fingerIndex, fingerPos, timeHeldDown );
    }

    internal static void RaiseOnFingerStationaryBegin( int fingerIndex, Vector2 fingerPos )
    {
        if( OnFingerStationaryBegin != null )
            OnFingerStationaryBegin( fingerIndex, fingerPos );
    }

    internal static void RaiseOnFingerStationary( int fingerIndex, Vector2 fingerPos, float elapsedTime )
    {
        if( OnFingerStationary != null )
            OnFingerStationary( fingerIndex, fingerPos, elapsedTime );
    }

    internal static void RaiseOnFingerStationaryEnd( int fingerIndex, Vector2 fingerPos, float elapsedTime )
    {
        if( OnFingerStationaryEnd != null )
            OnFingerStationaryEnd( fingerIndex, fingerPos, elapsedTime );
    }

    internal static void RaiseOnFingerMoveBegin( int fingerIndex, Vector2 fingerPos )
    {
        if( OnFingerMoveBegin != null )
            OnFingerMoveBegin( fingerIndex, fingerPos );
    }

    internal static void RaiseOnFingerMove( int fingerIndex, Vector2 fingerPos )
    {
        if( OnFingerMove != null )
            OnFingerMove( fingerIndex, fingerPos );
    }

    internal static void RaiseOnFingerMoveEnd( int fingerIndex, Vector2 fingerPos )
    {
        if( OnFingerMoveEnd != null )
            OnFingerMoveEnd( fingerIndex, fingerPos );
    }

    internal static void RaiseOnFingerLongPress( int fingerIndex, Vector2 fingerPos )
    {
        if( OnFingerLongPress != null )
            OnFingerLongPress( fingerIndex, fingerPos );
    }

    internal static void RaiseOnFingerDragBegin( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
    {
        if( OnFingerDragBegin != null )
            OnFingerDragBegin( fingerIndex, fingerPos, startPos );
    }

    internal static void RaiseOnFingerDragMove( int fingerIndex, Vector2 fingerPos, Vector2 delta )
    {
        if( OnFingerDragMove != null )
            OnFingerDragMove( fingerIndex, fingerPos, delta );
    }

    internal static void RaiseOnFingerDragEnd( int fingerIndex, Vector2 fingerPos )
    {
        if( OnFingerDragEnd != null )
            OnFingerDragEnd( fingerIndex, fingerPos );
    }

    internal static void RaiseOnFingerTap( int fingerIndex, Vector2 fingerPos, int tapCount )
    {
        if( OnFingerTap != null )
            OnFingerTap( fingerIndex, fingerPos, tapCount );
    }

    internal static void RaiseOnFingerSwipe( int fingerIndex, Vector2 startPos, SwipeDirection direction, float velocity )
    {
        if( OnFingerSwipe != null )
            OnFingerSwipe( fingerIndex, startPos, direction, velocity );
    }

    #endregion

    #endregion

    #region Global Gesture Events

    //NOTE: events in this category are not global, in the sense that they take into account the current state of all the fingers. For instance,
    // a single-finger gesture event such as OnTap will only fire if there is exactly one finger touching the screen (and tapping). If there are more touches,
    // the gesture recognizer will fail, and the event will not be risen.

    #region Delegates

    /// <summary>
    /// Delegate for the OnLongPress event
    /// </summary>
    /// <param name="fingerPos">Screen position where the press occured</param>
    public delegate void LongPressEventHandler( Vector2 fingerPos );

    /// <summary>
    /// Delegate for the OnTap event
    /// </summary>
    /// <param name="fingerPos">Screen position where the tap occured</param>
    /// <param name="tapCount">Number of conseuctive taps the user has performed at this location</param>
    public delegate void TapEventHandler( Vector2 fingerPos, int tapCount );

    /// <summary>
    /// Delegate for the OnSwipe event
    /// </summary>
    /// <param name="startPos">Initial finger position when the swipe gesture started</param>
    /// <param name="direction">Direction of the swipe gesture</param>
    /// <param name="velocity">How quickly the finger has moved (in screen pixels per second)</param>
    public delegate void SwipeEventHandler( Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity );

    /// <summary>
    /// Delegate for the OnDragBegin event
    /// </summary>
    /// <param name="fingerPos">The current finger position on the screen</param>
    /// <param name="startPos">The initial screen position the gesture started from</param>
    /// <remark>These two values can differ if the drag gesture recognizer's MoveThreshold is non-zero</remark>
    public delegate void DragBeginEventHandler( Vector2 fingerPos, Vector2 startPos );

    /// <summary>
    /// Delegate for the OnDragMove event
    /// </summary>
    /// <param name="fingerPos">Current finger position on the screen</param>
    /// <param name="delta">How much the finger has moved since the last update. This is the difference between the previous finger position and the new one.</param>
    public delegate void DragMoveEventHandler( Vector2 fingerPos, Vector2 delta );

    /// <summary>
    /// Delegate for the OnDragEnd event
    /// </summary>
    /// <param name="fingerPos">Current position of the finger on the screen</param>
    public delegate void DragEndEventHandler( Vector2 fingerPos );

    /// <summary>
    /// Delegate for the OnPinchBegin and OnPinchEnd events
    /// </summary>
    /// <param name="fingerPos1">First finger screen position</param>
    /// <param name="fingerPos2">Second finger screen position</param>
    public delegate void PinchEventHandler( Vector2 fingerPos1, Vector2 fingerPos2 );

    /// <summary>
    /// Delegate for the OnPinchMove event
    /// </summary>
    /// <param name="fingerPos1">First finger screen position</param>
    /// <param name="fingerPos2">Second finger screen position</param>
    /// <param name="delta">How much the distance between the two fingers has changed since the last update. A negative value means the two fingers got closer, while a positive value means they moved further apart</param>
    public delegate void PinchMoveEventHandler( Vector2 fingerPos1, Vector2 fingerPos2, float delta );

    /// <summary>
    /// Delegate for the OnRotationBegin event
    /// </summary>
    /// <param name="fingerPos1">First finger screen position</param>
    /// <param name="fingerPos2">Second finger screen position</param>
    public delegate void RotationBeginEventHandler( Vector2 fingerPos1, Vector2 fingerPos2 );

    /// <summary>
    /// Delegate for the OnRotationMove event
    /// </summary>
    /// <param name="fingerPos1">First finger screen position</param>
    /// <param name="fingerPos2">Second finger screen position</param>
    /// <param name="rotationAngleDelta">Angle difference, in degrees, since the last update.</param>
    public delegate void RotationMoveEventHandler( Vector2 fingerPos1, Vector2 fingerPos2, float rotationAngleDelta );

    /// <summary>
    /// Delegate for the OnRotationEnd event
    /// </summary>
    /// <param name="fingerPos1">First finger screen position</param>
    /// <param name="fingerPos2">Second finger screen position</param>
    /// <param name="totalRotationAngle">Total rotation performed during the gesture, in degrees</param>
    public delegate void RotationEndEventHandler( Vector2 fingerPos1, Vector2 fingerPos2, float totalRotationAngle );

    #endregion

    #region Events

    /// <summary>
    /// Event fired when the default long-press gesture recognizer OnLongPress event fires
    /// </summary>
    /// <seealso cref="LongPressGestureRecognizer.OnLongPress"/>
    /// <seealso cref="FingerGestures.Defaults.LongPress"/>
    public static event LongPressEventHandler OnLongPress;

    /// <summary>
    /// Event fired when the default drag gesture recognizer's OnDragBegin event fires
    /// </summary>
    /// <seealso cref="DragGestureRecognizer.OnDragBegin"/>
    /// <seealso cref="FingerGestures.Defaults.Drag"/>
    public static event DragBeginEventHandler OnDragBegin;

    /// <summary>
    /// Event fired when the default drag gesture recognizer's OnDragMove event fires
    /// </summary>
    /// <seealso cref="DragGestureRecognizer.OnDragMove"/>
    /// <seealso cref="FingerGestures.Defaults.Drag"/>
    public static event DragMoveEventHandler OnDragMove;

    /// <summary>
    /// Event fired when the default drag gesture recognizer's OnDragEnd event fires
    /// </summary>
    /// <seealso cref="DragGestureRecognizer.OnDragEnd"/>
    /// <seealso cref="FingerGestures.Defaults.Drag"/>
    public static event DragEndEventHandler OnDragEnd;

    /// <summary>
    /// Event fired when the default tap gesture recognizer's OnTap event fires
    /// </summary>
    /// <seealso cref="TapGestureRecognizer.OnTap"/>
    /// <seealso cref="FingerGestures.Defaults.Tap"/>
    public static event TapEventHandler OnTap;

    /// <summary>
    /// Event fired when the default swipe gesture recognizer's OnSwipe event fires
    /// </summary>
    /// <seealso cref="DragGestureRecognizer.OnDragEnd"/>
    /// <seealso cref="FingerGestures.Defaults.Drag"/>
    public static event SwipeEventHandler OnSwipe;

    /// <summary>
    /// Event fired when the default pinch gesture recognizer's OnPinchBegin event fires
    /// </summary>
    /// <seealso cref="PinchGestureRecognizer.OnPinchBegin"/>
    /// <seealso cref="FingerGestures.Defaults.Pinch"/>
    public static event PinchEventHandler OnPinchBegin;

    /// <summary>
    /// Event fired when the default pinch gesture recognizer's OnPinchMove event fires
    /// </summary>
    /// <seealso cref="PinchGestureRecognizer.OnPinchMove"/>
    /// <seealso cref="FingerGestures.Defaults.Pinch"/>
    public static event PinchMoveEventHandler OnPinchMove;

    /// <summary>
    /// Event fired when the default pinch gesture recognizer's OnPinchEnd event fires
    /// <seealso cref="PinchGestureRecognizer.OnPinchEnd"/>
    /// <seealso cref="FingerGestures.Defaults.Pinch"/>
    /// </summary>
    public static event PinchEventHandler OnPinchEnd;

    /// <summary>
    /// Event fired when the default pinch gesture recognizer's OnRotationBegin event fires
    /// <seealso cref="RotationGestureRecognizer.OnRotationBegin"/>
    /// <seealso cref="FingerGestures.Defaults.Rotation"/>
    /// </summary>
    public static event RotationBeginEventHandler OnRotationBegin;

    /// <summary>
    /// Event fired when the default pinch gesture recognizer's OnRotationMove event fires
    /// <seealso cref="RotationGestureRecognizer.OnRotationMove"/>
    /// <seealso cref="FingerGestures.Defaults.Rotation"/>
    /// </summary>
    public static event RotationMoveEventHandler OnRotationMove;

    /// <summary>
    /// Event fired when the default pinch gesture recognizer's OnRotationEnd event fires
    /// <seealso cref="RotationGestureRecognizer.OnRotationEnd"/>
    /// <seealso cref="FingerGestures.Defaults.Rotation"/>
    /// </summary>
    public static event RotationEndEventHandler OnRotationEnd;

    #region Two-Finger Versions

    /// <summary>
    /// Event fired when the default two-finger drag gesture recognizer's OnDragBegin event fires
    /// </summary>
    /// <seealso cref="DragGestureRecognizer.OnDragBegin"/>
    /// <seealso cref="FingerGestures.Defaults.TwoFingerDrag"/>
    public static event DragBeginEventHandler OnTwoFingerDragBegin;

    /// <summary>
    /// Event fired when the default two-finger drag gesture recognizer's OnDragMove event fires
    /// </summary>
    /// <seealso cref="DragGestureRecognizer.OnDragMove"/>
    /// <seealso cref="FingerGestures.Defaults.TwoFingerDrag"/>
    public static event DragMoveEventHandler OnTwoFingerDragMove;

    /// <summary>
    /// Event fired when the default two-finger drag gesture recognizer's OnDragEnd event fires
    /// </summary>
    /// <seealso cref="DragGestureRecognizer.OnDragEnd"/>
    /// <seealso cref="FingerGestures.Defaults.TwoFingerDrag"/>
    public static event DragEndEventHandler OnTwoFingerDragEnd;

    /// <summary>
    /// Event fired when the default two-finger tap gesture recognizer's OnTap event fires
    /// </summary>
    /// <seealso cref="DragGestureRecognizer.OnTap"/>
    /// <seealso cref="FingerGestures.Defaults.TwoFingerTap"/>
    public static event TapEventHandler OnTwoFingerTap;

    /// <summary>
    /// Event fired when the default two-finger tap gesture recognizer's OnSwipe event fires
    /// </summary>
    /// <seealso cref="DragGestureRecognizer.OnSwipe"/>
    /// <seealso cref="FingerGestures.Defaults.TwoFingerSwipe"/>
    public static event SwipeEventHandler OnTwoFingerSwipe;

    /// <summary>
    /// Event fired when the default two-finger long-press gesture recognizer's OnLongPress event fires
    /// </summary>
    /// <seealso cref="DragGestureRecognizer.OnLongPress"/>
    /// <seealso cref="FingerGestures.Defaults.TwoFingerLongPress"/>
    public static event LongPressEventHandler OnTwoFingerLongPress;

    #endregion

    #endregion

    #region Event-Raising Wrappers

    internal static void RaiseOnLongPress( Vector2 fingerPos )
    {
        if( OnLongPress != null )
            OnLongPress( fingerPos );
    }

    internal static void RaiseOnDragBegin( Vector2 fingerPos, Vector2 startPos )
    {
        if( OnDragBegin != null )
            OnDragBegin( fingerPos, startPos );
    }

    internal static void RaiseOnDragMove( Vector2 fingerPos, Vector2 delta )
    {
        if( OnDragMove != null )
            OnDragMove( fingerPos, delta );
    }

    internal static void RaiseOnDragEnd( Vector2 fingerPos )
    {
        if( OnDragEnd != null )
            OnDragEnd( fingerPos );
    }

    internal static void RaiseOnTap( Vector2 fingerPos, int tapCount )
    {
        if( OnTap != null )
            OnTap( fingerPos, tapCount );
    }

    internal static void RaiseOnSwipe( Vector2 startPos, SwipeDirection direction, float velocity )
    {
        if( OnSwipe != null )
            OnSwipe( startPos, direction, velocity );
    }

    internal static void RaiseOnPinchBegin( Vector2 fingerPos1, Vector2 fingerPos2 )
    {
        if( OnPinchBegin != null )
            OnPinchBegin( fingerPos1, fingerPos2 );
    }

    internal static void RaiseOnPinchMove( Vector2 fingerPos1, Vector2 fingerPos2, float delta )
    {
        if( OnPinchMove != null )
            OnPinchMove( fingerPos1, fingerPos2, delta );
    }

    internal static void RaiseOnPinchEnd( Vector2 fingerPos1, Vector2 fingerPos2 )
    {
        if( OnPinchEnd != null )
            OnPinchEnd( fingerPos1, fingerPos2 );
    }

    internal static void RaiseOnRotationBegin( Vector2 fingerPos1, Vector2 fingerPos2 )
    {
        if( OnRotationBegin != null )
            OnRotationBegin( fingerPos1, fingerPos2 );
    }

    internal static void RaiseOnRotationMove( Vector2 fingerPos1, Vector2 fingerPos2, float rotationAngleDelta )
    {
        if( OnRotationMove != null )
            OnRotationMove( fingerPos1, fingerPos2, rotationAngleDelta );
    }

    internal static void RaiseOnRotationEnd( Vector2 fingerPos1, Vector2 fingerPos2, float totalRotationAngle )
    {
        if( OnRotationEnd != null )
            OnRotationEnd( fingerPos1, fingerPos2, totalRotationAngle );
    }

    #region Two Finger Versions

    internal static void RaiseOnTwoFingerLongPress( Vector2 fingerPos )
    {
        if( OnTwoFingerLongPress != null )
            OnTwoFingerLongPress( fingerPos );
    }

    internal static void RaiseOnTwoFingerDragBegin( Vector2 fingerPos, Vector2 startPos )
    {
        if( OnTwoFingerDragBegin != null )
            OnTwoFingerDragBegin( fingerPos, startPos );
    }

    internal static void RaiseOnTwoFingerDragMove( Vector2 fingerPos, Vector2 delta )
    {
        if( OnTwoFingerDragMove != null )
            OnTwoFingerDragMove( fingerPos, delta );
    }

    internal static void RaiseOnTwoFingerDragEnd( Vector2 fingerPos )
    {
        if( OnTwoFingerDragEnd != null )
            OnTwoFingerDragEnd( fingerPos );
    }

    internal static void RaiseOnTwoFingerTap( Vector2 fingerPos, int tapCount )
    {
        if( OnTwoFingerTap != null )
            OnTwoFingerTap( fingerPos, tapCount );
    }

    internal static void RaiseOnTwoFingerSwipe( Vector2 startPos, SwipeDirection direction, float velocity )
    {
        if( OnTwoFingerSwipe != null )
            OnTwoFingerSwipe( startPos, direction, velocity );
    }

    #endregion

    #endregion

    #endregion

    #endregion

    /// <summary>
    /// Access to the FingerGestures singleton instance
    /// </summary>
    public static FingerGestures Instance
    {
        get { return FingerGestures.instance; }
    }

    #region Finger

    /// <summary>
    /// Finger Phase
    /// </summary>
    public enum FingerPhase
    {
        None,

        /// <summary>
        /// The finger just touched the screen
        /// </summary>
        Began,

        /// <summary>
        /// The finger just moved
        /// </summary>
        Moved,

        /// <summary>
        /// The finger is stationary
        /// </summary>
        Stationary,

        /// <summary>
        /// The finger was lifted off the screen
        /// </summary>
        Ended,
    }

    /// <summary>
    /// Finger
    /// 
    /// This provides an abstraction for a finger that can touch and move around the screen.
    /// As opposed to Unity's Touch object, a Finger exists independently of whether it is 
    /// currently touching the screen or not
    /// </summary>
    public class Finger
    {
        #region Properties

        /// <summary>
        /// Unique identifier for this finger. 
        /// For touch screen gestures, this corresponds to Touch.index, and the button index for mouse gestures.
        /// </summary>
        public int Index
        {
            get { return index; }
        }

        /// <summary>
        /// Current phase
        /// </summary>
        public FingerPhase Phase
        {
            get { return phase; }
        }

        /// <summary>
        /// Return true if the finger is currently down
        /// </summary>
        public bool IsDown
        {
            get { return down; }
        }

        /// <summary>
        /// Return true if the finger was down during the previous update/frame
        /// </summary>
        public bool WasDown
        {
            get { return wasDown; }
        }

        /// <summary>
        /// Get the time of first screen contact
        /// </summary>
        public float StarTime
        {
            get { return startTime; }
        }

        /// <summary>
        /// Get the position of first screen contact
        /// </summary>
        public Vector2 StartPosition
        {
            get { return startPos; }
        }

        /// <summary>
        /// Get the current position
        /// </summary>
        public Vector2 Position
        {
            get { return pos; }
        }

        /// <summary>
        /// Get the position during the previous frame
        /// </summary>
        public Vector2 PreviousPosition
        {
            get { return prevPos; }
        }

        /// <summary>
        /// Get the difference between previous and current position
        /// </summary>
        public Vector2 DeltaPosition
        {
            get { return deltaPos; }
        }

        /// <summary>
        /// Get the distance traveled from initial position
        /// </summary>
        public float DistanceFromStart
        {
            get { return distFromStart; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Delegate for OnDown & OnUp
        /// </summary>
        /// <param name="finger">the finger firing the event</param>
        public delegate void FingerEventDelegate( Finger finger );

        /// <summary>
        /// Event fired on the first frame the finger is down
        /// </summary>
        public event FingerEventDelegate OnDown;

        /// <summary>
        /// Event fired when the finger, previously down, has just been released. e.g. this would be the equivalent of a "mouse up" button event.
        /// </summary>
        public event FingerEventDelegate OnUp;

        #endregion

        #region Internal

        int index = 0;
        bool wasDown = false;
        bool down = false;
        float startTime = 0;
        FingerPhase phase = FingerPhase.None;
        Vector2 startPos = Vector2.zero;
        Vector2 pos = Vector2.zero;
        Vector2 prevPos = Vector2.zero;
        Vector2 deltaPos = Vector2.zero;
        float distFromStart = 0;

        public Finger( int index )
        {
            this.index = index;
        }

        public override string ToString()
        {
            return "Finger" + index;
        }

        internal void Update( FingerPhase newPhase, Vector2 newPos )
        {
            // validate phase transitions
            if( phase != newPhase )
            {
                // In low framerate situations, it is possible to miss some input updates and thus 
                // skip the "Ended" phase
                if( newPhase == FingerPhase.None && phase != FingerPhase.Ended )
                {
                    Debug.LogWarning( "Correcting bad FingerPhase transition (FingerPhase.Ended skipped)" );
                    Update( FingerPhase.Ended, PreviousPosition );
                    return;
                }

                // cannot get a Moved or Stationary phase without being down first
                if( !down && ( newPhase == FingerPhase.Moved || newPhase == FingerPhase.Stationary ) )
                {
                    Debug.LogWarning( "Correcting bad FingerPhase transition (FingerPhase.Began skipped)" );
                    Update( FingerPhase.Began, newPos );
                    return;
                }

                if( ( down && newPhase == FingerPhase.Began ) || ( !down && newPhase == FingerPhase.Ended ) )
                {
                    Debug.LogWarning( "Invalid state FingerPhase transition from " + phase + " to " + newPhase + " - Skipping." );
                    return;
                }
            }
            else // same phase as before
            {
                if( newPhase == FingerPhase.Began || newPhase == FingerPhase.Ended )
                {
                    Debug.LogWarning( "Duplicated FingerPhase." + newPhase.ToString() + " - skipping." );
                    return;
                }
            }

            if( newPhase != FingerPhase.None )
            {
                if( newPhase == FingerPhase.Ended )
                {
                    // release
                    down = false;
                }
                else
                {
                    if( newPhase == FingerPhase.Began )
                    {
                        // activate
                        down = true;
                        startPos = newPos;
                        prevPos = newPos;
                        startTime = Time.time;
                    }

                    prevPos = pos;
                    pos = newPos;
                    deltaPos = pos - prevPos;
                    distFromStart = Vector3.Distance( startPos, pos );
                }
            }

            phase = newPhase;
        }

        /// <summary>
        /// PostUpdate
        /// We use PostUpdate() to raise the OnDown/OnUp events after all the fingers have been properly updated
        /// </summary>
        internal void PostUpdate()
        {
            if( wasDown != down )
            {
                if( down )
                {
                    if( OnDown != null )
                        OnDown( this );
                }
                else
                {
                    if( OnUp != null )
                        OnUp( this );
                }
            }

            wasDown = down;
        }

        #endregion
    }

    /// <summary>
    /// Get a finger by its index
    /// </summary>
    public static Finger GetFinger( int index )
    {
        return instance.fingers[index];
    }

    /// <summary>
    /// List of fingers currently touching the screen
    /// </summary>
    public static IFingerList Touches
    {
        get { return instance.touches; }
    }

    #endregion

    #region Engine Callbacks

    protected virtual void OnEnable()
    {
        instance = this;

        InitFingers( MaxFingers );
    }

    protected virtual void Start()
    {
        // reinit
        if( fingers == null )
            InitFingers( MaxFingers );
    }

    public delegate void FingersUpdatedEventDelegate();
    public static event FingersUpdatedEventDelegate OnFingersUpdated;

    protected virtual void Update()
    {
        UpdateFingers();

        if( OnFingersUpdated != null )
            OnFingersUpdated();
    }

    #endregion

    #region Overridable methods

    /// <summary>
    /// Maximum number of simultaneous fingers supported
    /// </summary>
    public abstract int MaxFingers { get; }

    /// <summary>
    /// Return the new phase of the finger for this frame
    /// </summary>
    protected abstract FingerPhase GetPhase( Finger finger );

    /// <summary>
    /// Return the new position of the finger on the screen for this frame
    /// </summary>
    protected abstract Vector2 GetPosition( Finger finger );

    #endregion

    #region Internal

    // access to the singleton
    static FingerGestures instance;

    #region Fingers Management

    Finger[] fingers;
    FingerList touches = new FingerList();

    void InitFingers( int count )
    {
        // pre-allocate a touch data entry for each finger
        if( fingers == null )
        {
            fingers = new Finger[count];

            for( int i = 0; i < count; ++i )
                fingers[i] = new Finger( i );
        }

        InitDefaultComponents();
    }

    void UpdateFingers()
    {
        touches.Clear();

        // update all fingers
        foreach( Finger finger in fingers )
        {
            Vector2 pos = Vector2.zero;
            FingerPhase phase = GetPhase( finger );

            if( phase != FingerPhase.None )
                pos = GetPosition( finger );

            finger.Update( phase, pos );

            if( finger.IsDown )
                touches.Add( finger );
        }

        // post-update
        foreach( Finger finger in fingers )
            finger.PostUpdate();
    }

    #endregion

    #region Default Per-Finger & Global Gesture Recognizers

    public FingerGesturesPrefabs defaultPrefabs;    // prefabs

    Transform globalComponentNode;
    Transform[] fingerComponentNodes;

    T CreateDefaultComponent<T>( T prefab, Transform parent ) where T : FGComponent
    {
        T comp = Instantiate( prefab ) as T;
        comp.gameObject.name = prefab.name;
        comp.transform.parent = parent;
        return comp;
    }

    T CreateDefaultGlobalComponent<T>( T prefab ) where T : FGComponent
    {
        return CreateDefaultComponent<T>( prefab, globalComponentNode );
    }

    T CreateDefaultFingerComponent<T>( Finger finger, T prefab ) where T : FGComponent
    {
        return CreateDefaultComponent<T>( prefab, fingerComponentNodes[finger.Index] );
    }

    [System.Serializable]
    public class DefaultComponentCreationFlags
    {
        [System.Serializable]
        public class PerFinger
        {
            public bool enabled = true;

            public bool touch = true;   // FingerDown & FingerUp
            public bool motion = true;  // FingerMove & FingerStationary
            public bool longPress = true;
            public bool drag = true;
            public bool swipe = true;
            public bool tap = true;
        }

        [System.Serializable]
        public class GlobalGestures
        {
            public bool enabled = true;

            public bool longPress = true;
            public bool drag = true;
            public bool swipe = true;
            public bool tap = true;
            public bool pinch = true;
            public bool rotation = true;
            public bool twoFingerLongPress = true;
            public bool twoFingerDrag = true;
            public bool twoFingerSwipe = true;
            public bool twoFingerTap = true;
        }

        public PerFinger perFinger;
        public GlobalGestures globalGestures;
    }

    /// <summary>
    /// This holds a reference to all the default components/gesture recognizers automatically created at initialization
    /// </summary>
    public class DefaultComponents
    {
        public DefaultComponents( int fingerCount )
        {
            fingers = new FingerComponents[fingerCount];
            for( int i = 0; i < fingers.Length; ++i )
                fingers[i] = new FingerComponents();
        }

        /// <summary>
        /// Per-Finger components 
        /// </summary>
        public class FingerComponents
        {
            public FingerMotionDetector Motion;
            public LongPressGestureRecognizer LongPress;
            public DragGestureRecognizer Drag;
            public TapGestureRecognizer Tap;
            public SwipeGestureRecognizer Swipe;
        }

        FingerComponents[] fingers;
        public FingerComponents[] Fingers
        {
            get { return fingers; }
        }

        // global components
        public LongPressGestureRecognizer LongPress;
        public DragGestureRecognizer Drag;
        public TapGestureRecognizer Tap;
        public SwipeGestureRecognizer Swipe;
        public PinchGestureRecognizer Pinch;
        public RotationGestureRecognizer Rotation;
        public LongPressGestureRecognizer TwoFingerLongPress;
        public DragGestureRecognizer TwoFingerDrag;
        public TapGestureRecognizer TwoFingerTap;
        public SwipeGestureRecognizer TwoFingerSwipe;
    }

    public DefaultComponentCreationFlags defaultCompFlags;

    DefaultComponents defaultComponents;

    /// <summary>
    /// Get access to the default components / gesture recognizers
    /// </summary>
    public static DefaultComponents Defaults
    {
        get { return instance.defaultComponents; }
    }

    Transform CreateNode( string name, Transform parent )
    {
        GameObject go = new GameObject( name );
        go.transform.parent = parent;
        return go.transform;
    }

    void InitDefaultComponents()
    {
        int fingerCount = fingers.Length;

        if( globalComponentNode )
            Destroy( globalComponentNode.gameObject );

        if( fingerComponentNodes != null )
        {
            foreach( Transform fingerCompNode in fingerComponentNodes )
                Destroy( fingerCompNode.gameObject );
        }
         
        globalComponentNode = CreateNode( "Global Components", this.transform );

        fingerComponentNodes = new Transform[fingerCount];
        for( int i = 0; i < fingerComponentNodes.Length; ++i )
            fingerComponentNodes[i] = CreateNode( "Finger" + i, this.transform );

        defaultComponents = new DefaultComponents( fingerCount );

        if( defaultCompFlags.globalGestures.enabled )
            InitGlobalGestures();

        if( defaultCompFlags.perFinger.enabled )
        {
            foreach( Finger finger in fingers )
                InitDefaultComponents( finger );
        }
    }

    void InitGlobalGestures()
    {
        // default long press gesture
        if( defaultCompFlags.globalGestures.longPress )
        {
            LongPressGestureRecognizer longPress = CreateDefaultGlobalComponent( defaultPrefabs.longPress );
            longPress.OnLongPress += delegate( LongPressGestureRecognizer rec ) { RaiseOnLongPress( rec.Position ); };
            defaultComponents.LongPress = longPress;
        }

        // default long press gesture
        if( defaultCompFlags.globalGestures.twoFingerLongPress )
        {
            LongPressGestureRecognizer longPress = CreateDefaultGlobalComponent( defaultPrefabs.twoFingerLongPress );
            longPress.RequiredFingerCount = 2;
            longPress.OnLongPress += delegate( LongPressGestureRecognizer rec ) { RaiseOnTwoFingerLongPress( rec.Position ); };
            defaultComponents.TwoFingerLongPress = longPress;
        }

        // default drag detector
        if( defaultCompFlags.globalGestures.drag )
        {
            DragGestureRecognizer drag = CreateDefaultGlobalComponent( defaultPrefabs.drag );
            drag.OnDragBegin += delegate( DragGestureRecognizer rec ) { RaiseOnDragBegin( rec.Position, rec.StartPosition ); };
            drag.OnDragMove += delegate( DragGestureRecognizer rec ) { RaiseOnDragMove( rec.Position, rec.MoveDelta ); }; ;
            drag.OnDragEnd += delegate( DragGestureRecognizer rec ) { RaiseOnDragEnd( rec.Position ); };
            defaultComponents.Drag = drag;
        }

        // default two-finger drag detector
        if( defaultCompFlags.globalGestures.twoFingerDrag )
        {
            DragGestureRecognizer drag = CreateDefaultGlobalComponent( defaultPrefabs.twoFingerDrag );
            drag.RequiredFingerCount = 2;
            drag.OnDragBegin += delegate( DragGestureRecognizer rec ) { RaiseOnTwoFingerDragBegin( rec.Position, rec.StartPosition ); };
            drag.OnDragMove += delegate( DragGestureRecognizer rec ) { RaiseOnTwoFingerDragMove( rec.Position, rec.MoveDelta ); }; ;
            drag.OnDragEnd += delegate( DragGestureRecognizer rec ) { RaiseOnTwoFingerDragEnd( rec.Position ); };
            defaultComponents.TwoFingerDrag = drag;
        }

        // default swipe detector
        if( defaultCompFlags.globalGestures.swipe )
        {
            SwipeGestureRecognizer swipe = CreateDefaultGlobalComponent( defaultPrefabs.swipe );
            swipe.OnSwipe += delegate( SwipeGestureRecognizer rec ) { RaiseOnSwipe( rec.StartPosition, rec.Direction, rec.Velocity ); };
            defaultComponents.Swipe = swipe;
        }

        // default two-finger swipe detector
        if( defaultCompFlags.globalGestures.twoFingerSwipe )
        {
            SwipeGestureRecognizer swipe = CreateDefaultGlobalComponent( defaultPrefabs.twoFingerSwipe );
            swipe.RequiredFingerCount = 2;
            swipe.OnSwipe += delegate( SwipeGestureRecognizer rec ) { RaiseOnTwoFingerSwipe( rec.StartPosition, rec.Direction, rec.Velocity ); };
            defaultComponents.TwoFingerSwipe = swipe;
        }

        // default tap detector
        if( defaultCompFlags.globalGestures.tap )
        {
            TapGestureRecognizer tap = CreateDefaultGlobalComponent( defaultPrefabs.tap );
            tap.RequiredTaps = 0;
            tap.OnTap += delegate( TapGestureRecognizer rec ) { RaiseOnTap( rec.Position, rec.Taps ); };
            defaultComponents.Tap = tap;
        }

        // default two-finger tap detector
        if( defaultCompFlags.globalGestures.twoFingerTap )
        {
            TapGestureRecognizer tap = CreateDefaultGlobalComponent( defaultPrefabs.twoFingerTap );
            tap.RequiredFingerCount = 2;
            tap.RequiredTaps = 0;
            tap.OnTap += delegate( TapGestureRecognizer rec ) { RaiseOnTwoFingerTap( rec.Position, rec.Taps ); };
            defaultComponents.TwoFingerTap = tap;
        }

        // default pinch recognizer
        if( defaultCompFlags.globalGestures.pinch )
        {
            PinchGestureRecognizer pinch = CreateDefaultGlobalComponent( defaultPrefabs.pinch );
            pinch.OnPinchBegin += delegate( PinchGestureRecognizer rec ) { RaiseOnPinchBegin( rec.GetPosition( 0 ), rec.GetPosition( 1 ) ); };
            pinch.OnPinchMove += delegate( PinchGestureRecognizer rec ) { RaiseOnPinchMove( rec.GetPosition( 0 ), rec.GetPosition( 1 ), rec.Delta ); };
            pinch.OnPinchEnd += delegate( PinchGestureRecognizer rec ) { RaiseOnPinchEnd( rec.GetPosition( 0 ), rec.GetPosition( 1 ) ); }; ;
            defaultComponents.Pinch = pinch;
        }

        // default rotation recognizer
        if( defaultCompFlags.globalGestures.rotation )
        {
            RotationGestureRecognizer rotation = CreateDefaultGlobalComponent( defaultPrefabs.rotation );
            rotation.OnRotationBegin += delegate( RotationGestureRecognizer rec ) { RaiseOnRotationBegin( rec.GetPosition( 0 ), rec.GetPosition( 1 ) ); };
            rotation.OnRotationMove += delegate( RotationGestureRecognizer rec ) { RaiseOnRotationMove( rec.GetPosition( 0 ), rec.GetPosition( 1 ), rec.RotationDelta ); };
            rotation.OnRotationEnd += delegate( RotationGestureRecognizer rec ) { RaiseOnRotationEnd( rec.GetPosition( 0 ), rec.GetPosition( 1 ), rec.TotalRotation ); };
            defaultComponents.Rotation = rotation;
        }
    }

    void InitDefaultComponents( Finger finger )
    {
        ITouchFilter touchFilter = new SingleFingerFilter( finger );
        DefaultComponents.FingerComponents defaultFingerComponents = defaultComponents.Fingers[finger.Index];

        // touch down & up events
        if( defaultCompFlags.perFinger.touch )
        {
            finger.OnDown += PerFinger_OnDown;
            finger.OnUp += PerFinger_OnUp;
        }

        // setup a default motion detector using the "global" moveThreshold specified on the FingerGestures object
        // this is for backward compatibility with previous 1.X versions
        if( defaultCompFlags.perFinger.motion )
        {
            FingerMotionDetector motion = CreateDefaultFingerComponent( finger, defaultPrefabs.fingerMotion );
            motion.Finger = finger;
            motion.OnMoveBegin += PerFinger_OnMoveBegin;
            motion.OnMove += PerFinger_OnMove;
            motion.OnMoveEnd += PerFinger_OnMoveEnd;
            motion.OnStationaryBegin += PerFinger_OnStationaryBegin;
            motion.OnStationary += PerFinger_OnStationary;
            motion.OnStationaryEnd += PerFinger_OnStationaryEnd;
            defaultFingerComponents.Motion = motion;
        }

        // default long press gesture
        if( defaultCompFlags.perFinger.longPress )
        {
            LongPressGestureRecognizer longPress = CreateDefaultFingerComponent( finger, defaultPrefabs.fingerLongPress );
            longPress.TouchFilter = touchFilter;
            longPress.OnLongPress += PerFinger_OnLongPress;
            defaultFingerComponents.LongPress = longPress;
        }

        // setup default drag detector
        if( defaultCompFlags.perFinger.drag )
        {
            DragGestureRecognizer drag = CreateDefaultFingerComponent( finger, defaultPrefabs.fingerDrag );
            drag.TouchFilter = touchFilter;
            drag.OnDragBegin += PerFinger_OnDragBegin;
            drag.OnDragMove += PerFinger_OnDragMove;
            drag.OnDragEnd += PerFinger_OnDragEnd;
            defaultFingerComponents.Drag = drag;
        }

        // setup default swipe detector
        if( defaultCompFlags.perFinger.swipe )
        {
            SwipeGestureRecognizer swipe = CreateDefaultFingerComponent( finger, defaultPrefabs.fingerSwipe );
            swipe.TouchFilter = touchFilter;
            swipe.OnSwipe += PerFinger_OnSwipe;
            defaultFingerComponents.Swipe = swipe;
        }

        // setup default tap detector
        if( defaultCompFlags.perFinger.tap )
        {
            TapGestureRecognizer tap = CreateDefaultFingerComponent( finger, defaultPrefabs.fingerTap );
            tap.TouchFilter = touchFilter;
            tap.RequiredTaps = 0;
            tap.OnTap += PerFinger_OnTap;
            defaultFingerComponents.Tap = tap;
        }
    }

    static Finger GetFingerFromTouchFilter( GestureRecognizer recognizer )
    {
        SingleFingerFilter filter = recognizer.TouchFilter as SingleFingerFilter;

        if( filter != null )
            return filter.Finger;

        return null;
    }

    #region Per-Finger Gestures Callbacks

    void PerFinger_OnDown( Finger source )
    {
        RaiseOnFingerDown( source.Index, source.Position );
    }

    void PerFinger_OnUp( Finger source )
    {
        RaiseOnFingerUp( source.Index, source.Position, Time.time - source.StarTime );
    }

    void PerFinger_OnStationaryBegin( FingerMotionDetector source )
    {
        RaiseOnFingerStationaryBegin( source.Finger.Index, source.AnchorPos );
    }

    void PerFinger_OnStationary( FingerMotionDetector source )
    {
        RaiseOnFingerStationary( source.Finger.Index, source.Finger.Position, source.ElapsedStationaryTime );
    }

    void PerFinger_OnStationaryEnd( FingerMotionDetector source )
    {
        RaiseOnFingerStationaryEnd( source.Finger.Index, source.Finger.PreviousPosition, source.ElapsedStationaryTime );
    }

    void PerFinger_OnMoveBegin( FingerMotionDetector source )
    {
        RaiseOnFingerMoveBegin( source.Finger.Index, source.AnchorPos );
    }

    void PerFinger_OnMove( FingerMotionDetector source )
    {
        RaiseOnFingerMove( source.Finger.Index, source.Finger.Position );
    }

    void PerFinger_OnMoveEnd( FingerMotionDetector source )
    {
        RaiseOnFingerMoveEnd( source.Finger.Index, source.Finger.Position );
    }

    void PerFinger_OnDragBegin( DragGestureRecognizer source )
    {
        Finger finger = GetFingerFromTouchFilter( source );
        RaiseOnFingerDragBegin( finger.Index, source.Position, source.StartPosition );
    }

    void PerFinger_OnDragMove( DragGestureRecognizer source )
    {
        Finger finger = GetFingerFromTouchFilter( source );
        RaiseOnFingerDragMove( finger.Index, source.Position, source.MoveDelta );
    }

    void PerFinger_OnDragEnd( DragGestureRecognizer source )
    {
        Finger finger = GetFingerFromTouchFilter( source );
        RaiseOnFingerDragEnd( finger.Index, source.Position );
    }

    void PerFinger_OnLongPress( LongPressGestureRecognizer source )
    {
        Finger finger = GetFingerFromTouchFilter( source );
        RaiseOnFingerLongPress( finger.Index, source.Position );
    }

    void PerFinger_OnSwipe( SwipeGestureRecognizer source )
    {
        Finger finger = GetFingerFromTouchFilter( source );
        RaiseOnFingerSwipe( finger.Index, source.StartPosition, source.Direction, source.Velocity );
    }

    void PerFinger_OnTap( TapGestureRecognizer source )
    {
        Finger finger = GetFingerFromTouchFilter( source );
        RaiseOnFingerTap( finger.Index, source.Position, source.Taps );
    }

    #endregion

    #endregion

    #endregion

    #region Finger List Data Structure

    /// <summary>
    /// Represent a read-only list of fingers, augmented with a bunch of utility methods
    /// </summary>
    public interface IFingerList : IEnumerable<Finger>
    {
        /// <summary>
        /// Get finger in array by index
        /// </summary>
        /// <param name="index">The array index</param>
        Finger this[int index] { get; }

        /// <summary>
        /// Number of fingers in the list
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Get the average position of all the fingers in the list
        /// </summary>
        Vector2 GetAveragePosition();

        /// <summary>
        /// Get the average previous position of all the fingers in the list
        /// </summary>
        Vector2 GetAveragePreviousPosition();

        /// <summary>
        /// Get the average distance from each finger's starting position in the list
        /// </summary>
        float GetAverageDistanceFromStart();

        /// <summary>
        /// Find the finger with the oldest StartTime
        /// </summary>
        Finger GetOldest();
    }

    /// <summary>
    /// A finger list implementation with support for write access
    /// </summary>
    public class FingerList : IFingerList
    {
        List<Finger> list;

        public FingerList()
        {
            list = new List<Finger>();
        }

        public FingerList( List<Finger> list )
        {
            this.list = list;
        }

        public Finger this[int index]
        {
            get { return list[index]; }
        }

        public int Count
        {
            get { return list.Count; }
        }

        public IEnumerator<Finger> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add( Finger touch )
        {
            list.Add( touch );
        }

        public void Clear()
        {
            list.Clear();
        }

        public delegate T FingerPropertyGetterDelegate<T>( Finger finger );

        public Vector2 AverageVector( FingerPropertyGetterDelegate<Vector2> getProperty )
        {
            Vector2 avg = Vector2.zero;

            if( Count > 0 )
            {
                foreach( Finger finger in list )
                    avg += getProperty( finger );

                avg /= Count;
            }

            return avg;
        }

        public float AverageFloat( FingerPropertyGetterDelegate<float> getProperty )
        {
            float avg = 0;

            if( Count > 0 )
            {
                foreach( Finger finger in list )
                    avg += getProperty( finger );

                avg /= Count;
            }

            return avg;
        }

        static Vector2 GetFingerPosition( Finger finger ) { return finger.Position; }
        static Vector2 GetFingerPreviousPosition( Finger finger ) { return finger.PreviousPosition; }
        static float GetFingerDistanceFromStart( Finger finger ) { return finger.DistanceFromStart; }

        public Vector2 GetAveragePosition()
        {
            return AverageVector( GetFingerPosition );
        }

        public Vector2 GetAveragePreviousPosition()
        {
            return AverageVector( GetFingerPreviousPosition );
        }

        public float GetAverageDistanceFromStart()
        {
            return AverageFloat( GetFingerDistanceFromStart );
        }

        public Finger GetOldest()
        {
            Finger oldest = null;

            foreach( Finger finger in list )
            {
                if( oldest == null || ( finger.StarTime < oldest.StarTime ) )
                    oldest = finger;
            }

            return oldest;
        }
    }

    #endregion

    #region Swipe Direction

    /// <summary>
    /// Supported swipe gesture directions
    /// </summary>
    [System.Flags]
    public enum SwipeDirection
    {
        /// <summary>
        /// Moved to the right
        /// </summary>
        Right = 1 << 0,

        /// <summary>
        /// Moved to the left
        /// </summary>
        Left = 1 << 1,

        /// <summary>
        /// Moved up
        /// </summary>
        Up = 1 << 2,

        /// <summary>
        /// Moved down
        /// </summary>
        Down = 1 << 3,

        //--------------------

        None = 0,
        All = Right | Left | Up | Down,
        Vertical = Up | Down,
        Horizontal = Right | Left,
    }

    /// <summary>
    /// Extract a swipe direction from a direction vector and a tolerance percent 
    /// </summary>
    /// <param name="dir">The non-constrained direction vector. Must be normalized.</param>
    /// <param name="tolerance">Percentage of tolerance</param>
    /// <returns>The swipe direction</returns>
    public static SwipeDirection GetSwipeDirection( Vector3 dir, float tolerance )
    {
        float minSwipeDot = Mathf.Clamp01( 1.0f - tolerance );

        if( Vector2.Dot( dir, Vector2.right ) >= minSwipeDot )
            return SwipeDirection.Right;

        if( Vector2.Dot( dir, -Vector2.right ) >= minSwipeDot )
            return SwipeDirection.Left;

        if( Vector2.Dot( dir, Vector2.up ) >= minSwipeDot )
            return SwipeDirection.Up;

        if( Vector2.Dot( dir, -Vector2.up ) >= minSwipeDot )
            return SwipeDirection.Down;

        // not a valid direction
        return SwipeDirection.None;
    }

    #endregion

    #region Single-Finger Touch Filter

    /// <summary>
    /// A touch filter can be used to alter the content of the input touches list initially given to each Gesture Recognizer
    /// </summary>
    public interface ITouchFilter
    {
        FingerGestures.IFingerList Apply( FingerGestures.IFingerList touches );
    }

    /// <summary>
    /// A single-finger touch filter that:
    /// - returns an list composed of its unique finger if it is contained in the the input list
    /// - returns an empty list otherwise
    /// This has the benefit of requiring no run-time dynamic allocations (except once at creation)
    /// </summary>
    public class SingleFingerFilter : ITouchFilter
    {
        FingerList fingerList = new FingerList();
        FingerList emptyList = new FingerList();

        Finger finger;
        public Finger Finger
        {
            get { return finger; }
        }

        public SingleFingerFilter( Finger finger )
        {
            this.finger = finger;
            fingerList.Add( finger );
        }

        public IFingerList Apply( IFingerList touches )
        {
            foreach( Finger touch in touches )
            {
                if( touch == Finger )
                    return fingerList;
            }

            return emptyList;
        }
    }

    #endregion

    #region Utils

    /// <summary>
    /// Check if all the fingers in the list are moving
    /// </summary>
    public static bool AllFingersMoving( params Finger[] fingers )
    {
        if( fingers.Length == 0 )
            return false;

        foreach( Finger finger in fingers )
        {
            if( finger.Phase != FingerPhase.Moved )
                return false;
        }

        return true;
    }

    /// <summary>
    /// Check if the input fingers are moving in opposite direction
    /// </summary>
    public static bool FingersMovedInOppositeDirections( Finger finger0, Finger finger1, float minDOT )
    {
        float dot = Vector2.Dot( finger0.DeltaPosition.normalized, finger1.DeltaPosition.normalized );
        return dot < minDOT;
    }

    /// <summary>
    /// returns signed angle in radians between "from" -> "to"
    /// </summary>
    public static float SignedAngle( Vector2 from, Vector2 to )
    {
        // perpendicular dot product
        float perpDot = ( from.x * to.y ) - ( from.y * to.x );
        return Mathf.Atan2( perpDot, Vector2.Dot( from, to ) );
    }

    #endregion
}
