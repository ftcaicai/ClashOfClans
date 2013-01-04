using UnityEngine;
using System.Collections;

/// <summary>
/// Long-Press gesture: detects when the finger is held down without moving, for a specific duration
/// </summary>
[AddComponentMenu( "FingerGestures/Gesture Recognizers/Long Press" )]
public class LongPressGestureRecognizer : AveragedGestureRecognizer
{
    /// <summary>
    /// Event fired when the the gesture is recognized
    /// </summary>
    public event EventDelegate<LongPressGestureRecognizer> OnLongPress;

    /// <summary>
    /// How long the finger must stay down without moving in order to validate the gesture
    /// </summary>
    public float Duration = 1.0f;

    /// <summary>
    /// How far the finger is allowed to move around its starting position without breaking the gesture
    /// </summary>
    public float MoveTolerance = 5.0f;

    float startTime = 0;

    /// <summary>
    /// Time when the gesture last started
    /// </summary>
    public float StartTime
    {
        get { return startTime; }
    }

    protected override void OnBegin( FingerGestures.IFingerList touches )
    {
        Position = touches.GetAveragePosition();
        StartPosition = Position;
        startTime = Time.time;
    }

    protected override GestureState OnActive( FingerGestures.IFingerList touches )
    {
        if( touches.Count != RequiredFingerCount )
            return GestureState.Failed;

        float elapsedTime = Time.time - startTime;

        if( elapsedTime >= Duration )
        {
            RaiseOnLongPress();
            return GestureState.Recognized;
        }

        // check if we moved too far from initial position
        if( touches.GetAverageDistanceFromStart() > MoveTolerance )
            return GestureState.Failed;

        return GestureState.InProgress;
    }

    protected void RaiseOnLongPress()
    {
        if( OnLongPress != null )
            OnLongPress( this );
    }
}

