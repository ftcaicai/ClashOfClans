using UnityEngine;
using System.Collections;

/// <summary>
/// Pinch gesture: two fingers moving closer or further away from each other
/// 
/// NOTE: it is recommanded to set ResetMode to GestureResetMode.NextFrame for this gesture
/// </summary>
[AddComponentMenu( "FingerGestures/Gesture Recognizers/Pinch" )]
public class PinchGestureRecognizer : MultiFingerGestureRecognizer
{
    /// <summary>
    /// Event fired when the 
    /// </summary>
    public event EventDelegate<PinchGestureRecognizer> OnPinchBegin;

    /// <summary>
    /// Event fired when the distance between the two fingers has changed
    /// <see cref="Delta"/>
    /// </summary>
    public event EventDelegate<PinchGestureRecognizer> OnPinchMove;

    /// <summary>
    /// Event fired when the gesture has ended (e.g. at least one of the fingers was lifted off)
    /// </summary>
    public event EventDelegate<PinchGestureRecognizer> OnPinchEnd;
    
    /// <summary>
    /// Pinch DOT product treshold - this controls how tolerant the pinch gesture detector is to the two fingers
    /// moving in opposite directions.
    /// Setting this to -1 means the fingers have to move in exactly opposite directions to each other.
    /// this value should be kept between -1 and 0 excluded.
    /// </summary>    
    public float MinDOT = -0.7f;

    /// <summary>
    /// Minimum pinch distance required to trigger the pinch gesture
    /// </summary>
    public float MinDistance = 5.0f;

    /// <summary>
    /// How much to scale the internal pinch delta by before raising the OnPinchMove event
    /// </summary>
    public float DeltaScale = 1.0f;
    
    protected float delta = 0.0f;

    /// <summary>
    /// Signed change in distance between the two fingers since last update
    /// A negative value means the two fingers got closer, while a positive value means they moved further apart
    /// </summary>
    public float Delta
    {
        get { return delta; }
    }
    
    // Only support 2 simultaneous fingers right now
    protected override int GetRequiredFingerCount()
    {
        return 2;
    }

    protected override bool CanBegin( FingerGestures.IFingerList touches )
    {
        if( !base.CanBegin( touches ) )
            return false;

        FingerGestures.Finger finger0 = touches[0];
        FingerGestures.Finger finger1 = touches[1];

        if( !FingerGestures.AllFingersMoving( finger0, finger1 ) )
            return false;

        if( !FingersMovedInOppositeDirections( finger0, finger1 ) )
            return false;

        float gapDelta = ComputeGapDelta( finger0, finger1, finger0.StartPosition, finger1.StartPosition );
        if( Mathf.Abs( gapDelta ) < MinDistance )
            return false;

        return true;
    }

    protected override void OnBegin( FingerGestures.IFingerList touches )
    {
        FingerGestures.Finger finger0 = touches[0];
        FingerGestures.Finger finger1 = touches[1];

        StartPosition[0] = finger0.StartPosition;
        StartPosition[1] = finger1.StartPosition;

        Position[0] = finger0.Position;
        Position[1] = finger1.Position;

        RaiseOnPinchBegin();

        float startDelta = ComputeGapDelta( finger0, finger1, finger0.StartPosition, finger1.StartPosition );
        delta = DeltaScale * ( startDelta - Mathf.Sign( startDelta ) * MinDistance );

        RaiseOnPinchMove();
    }

    protected override GestureState OnActive( FingerGestures.IFingerList touches )
    {
        if( touches.Count != RequiredFingerCount )
        {
            // fingers were lifted?
            if( touches.Count < RequiredFingerCount )
            {
                RaiseOnPinchEnd();
                return GestureState.Recognized;
            }

            // more fingers added, gesture failed
            return GestureState.Failed;
        }

        FingerGestures.Finger finger0 = touches[0];
        FingerGestures.Finger finger1 = touches[1];

        Position[0] = finger0.Position;
        Position[1] = finger1.Position;

        // dont do anything if both fingers arent moving
        if( !FingerGestures.AllFingersMoving( finger0, finger1 ) )
            return GestureState.InProgress;

        float newDelta = ComputeGapDelta( finger0, finger1, finger0.PreviousPosition, finger1.PreviousPosition );

        if( Mathf.Abs( newDelta ) > 0.001f )
        {
            if( !FingersMovedInOppositeDirections( finger0, finger1 ) )
                return GestureState.InProgress; //TODO: might want to make this configurable, so the recognizer can fail if fingers move in same direction

            delta = DeltaScale * newDelta;

            RaiseOnPinchMove();
        }

        return GestureState.InProgress;
    }

    #region Event-Raising Wrappers

    protected void RaiseOnPinchBegin()
    {
        if( OnPinchBegin != null )
            OnPinchBegin( this );
    }

    protected void RaiseOnPinchMove()
    {
        if( OnPinchMove != null )
            OnPinchMove( this );
    }

    protected void RaiseOnPinchEnd()
    {
        if( OnPinchEnd != null )
            OnPinchEnd( this );
    }

    #endregion

    #region Utils

    bool FingersMovedInOppositeDirections( FingerGestures.Finger finger0, FingerGestures.Finger finger1 )
    {
        return FingerGestures.FingersMovedInOppositeDirections( finger0, finger1, MinDOT );
    }

    float ComputeGapDelta( FingerGestures.Finger finger0, FingerGestures.Finger finger1, Vector2 refPos1, Vector2 refPos2 )
    {
        Vector2 curDelta = finger0.Position - finger1.Position;
        Vector2 refDelta = refPos1 - refPos2;
        return curDelta.magnitude - refDelta.magnitude;
    }

    #endregion
}
