using UnityEngine;
using System.Collections;

/// <summary>
/// Tap gesture: single or multiple consecutive press and release gestures at the same location
/// </summary>
[AddComponentMenu( "FingerGestures/Gesture Recognizers/Tap" )]
public class TapGestureRecognizer : AveragedGestureRecognizer
{
    /// <summary>
    /// Event fired when a tap occurs (if RequiredTaps is 0) or when the exact number of RequiredTaps has been reached
    /// </summary>
    public event EventDelegate<TapGestureRecognizer> OnTap;

    /// <summary>
    /// Exact number of taps required to succesfully recognize the tap gesture.
    /// If RequiredTaps is set to a positive value, the gesture recognizer will reset once the number of consecutive taps performed is equal to 
    /// this value.
    /// </summary>
    /// <seealso cref="Taps"/>
    public int RequiredTaps = 0;

    /// <summary>
    /// When set to true, the OnTap event will fire each time a tap occurs.
    /// When set to false, the OnTap event will only be fired on the last tap produced (either by time-out or when reaching the RequiredTaps count)
    /// </summary>
    /// <seealso cref="RequiredTaps"/>
    public bool RaiseEventOnEachTap = false;

    /// <summary>
    /// The maximum amount of the time that can elapse between two consecutive taps without causing the recognizer to reset.
    /// Set to 0 to ignore this setting.
    /// </summary>
    public float MaxDelayBetweenTaps = 0.25f;

    /// <summary>
    /// The maximum total duration of a tap sequence, in seconds. The tap recognizer automatically resets after this duration.
    /// Set to 0 to ignore this setting.
    /// </summary>
    public float MaxDuration = 0.0f;
    
    /// <summary>
    /// How far the finger can move from its initial position without making the gesture fail
    /// </summary>
    public float MoveTolerance = 5.0f;

    int taps = 0;
    bool down = false;
    bool wasDown = false;
    float lastDownTime = 0;
    float lastTapTime = 0;
    float startTime = 0;

    /// <summary>
    /// Get the current number of consecutive taps achieved
    /// </summary>
    public int Taps
    {
        get { return taps; }
    }

    bool MovedTooFar( Vector2 curPos )
    {
        Vector2 delta = curPos - StartPosition;
        return delta.sqrMagnitude >= ( MoveTolerance * MoveTolerance );
    }

    bool HasTimedOut()
    {
        // check elapsed time since last tap
        if( MaxDelayBetweenTaps > 0 && ( Time.time - lastTapTime > MaxDelayBetweenTaps ) )
            return true;

        // check elapsed time since beginning of gesture
        if( MaxDuration > 0 && ( Time.time - startTime > MaxDuration ) )
            return true;

        return false;
    }

    protected override void Reset()
    {
        taps = 0;
        down = false;
        wasDown = false;
        base.Reset();
    }

    protected override void OnBegin( FingerGestures.IFingerList touches )
    {
        Position = touches.GetAveragePosition();
        StartPosition = Position;
        lastTapTime = Time.time;
        startTime = Time.time;
    }
    
    protected override GestureState OnActive( FingerGestures.IFingerList touches )
    {
        wasDown = down;
        down = false;

        if( touches.Count == RequiredFingerCount )
        {
            down = true;
            lastDownTime = Time.time;
        }
        else if( touches.Count == 0 )
        {
            down = false;
        }
        else
        {
            // some fingers were lifted off
            if( touches.Count < RequiredFingerCount )
            {
                // give a bit of buffer time to lift-off the remaining fingers
                if( Time.time - lastDownTime > 0.25f )
                    return GestureState.Failed;
            }
            else // fingers were added
            {
                return GestureState.Failed;
            }
        }

        if( HasTimedOut() )
        {
            // if we requested unlimited taps and landed at least one, consider this a success
            if( RequiredTaps == 0 && Taps > 0 )
            {
                // if we didn't raise a tap event on each tap, at least raise the event once at the end of the tap sequence
                if( !RaiseEventOnEachTap )
                    RaiseOnTap();

                return GestureState.Recognized;
            }

            // else, timed out
            return GestureState.Failed;
        }

        if( down )
        {
            Vector2 curPos = touches.GetAveragePosition();

            // check if finger moved too far from start position
            if( MovedTooFar( curPos ) )
                return GestureState.Failed;
        }

        if( wasDown != down )
        {
            // fingers were just released
            if( !down )
            {
                ++taps;
                lastTapTime = Time.time;

                // If the requested tap count has been reached, validate the gesture and stop
                if( RequiredTaps > 0 && taps >= RequiredTaps )
                {
                    RaiseOnTap();
                    return GestureState.Recognized;
                }

                if( RaiseEventOnEachTap )
                    RaiseOnTap();
            }
        }

        return GestureState.InProgress;
    }

    protected void RaiseOnTap()
    {
        if( OnTap != null )
            OnTap( this );
    }
}
