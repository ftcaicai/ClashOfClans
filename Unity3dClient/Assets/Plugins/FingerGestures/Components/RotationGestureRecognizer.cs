using UnityEngine;
using System.Collections;

/// <summary>
/// Rotation gesture, also known as twist gesture
/// This gesture is performed by moving two fingers around a point of reference in opposite directions 
/// 
/// NOTE: it is recommanded to set ResetMode to GestureResetMode.NextFrame for this gesture
/// </summary>
[AddComponentMenu( "FingerGestures/Gesture Recognizers/Rotation" )]
public class RotationGestureRecognizer : MultiFingerGestureRecognizer
{
    /// <summary>
    /// Event fired when the rotation gesture starts
    /// <see cref="MinRotation"/>
    /// <see cref="MinDOT"/>
    /// </summary>
    public event EventDelegate<RotationGestureRecognizer> OnRotationBegin;

    /// <summary>
    /// Event fired when the rotation angle has changed. 
    /// Query RotationDelta to get the angle difference since last frame
    /// Query TotalRotation to get the total angular motion since the beginning of the gesture
    /// <see cref="RotationDelta"/>
    /// <see cref="TotalRotation"/>
    /// </summary>
    public event EventDelegate<RotationGestureRecognizer> OnRotationMove;

    /// <summary>
    /// Event fired when the rotation gesture is finished
    /// <see cref="TotalRotation"/>
    /// </summary>
    public event EventDelegate<RotationGestureRecognizer> OnRotationEnd;

    /// <summary>
    /// Rotation DOT product treshold - this controls how tolerant the twist gesture detector is to the two fingers
    /// moving in opposite directions.
    /// Setting this to -1 means the fingers have to move in exactly opposite directions to each other.
    /// this value should be kept between -1 and 0 excluded.
    /// </summary>
    public float MinDOT = -0.7f;

    /// <summary>
    /// Minimum amount of rotation required to start the rotation gesture (in degrees)
    /// </summary>
    public float MinRotation = 1.0f;

    float totalRotation = 0.0f;
    float rotationDelta = 0.0f;

    /// <summary>
    /// Get total rotation angle since gesture started (in degrees)
    /// </summary>
    public float TotalRotation
    {
        get { return totalRotation; }
    }

    /// <summary>
    /// Get rotation angle change since last move (in degrees)
    /// </summary>
    public float RotationDelta
    {
        get { return rotationDelta; }
    }

    #region Utils

    bool FingersMovedInOppositeDirections( FingerGestures.Finger finger0, FingerGestures.Finger finger1 )
    {
        return FingerGestures.FingersMovedInOppositeDirections( finger0, finger1, MinDOT );
    }

    // return signed angle in degrees between current finger position and ref positions
    static float SignedAngularGap( FingerGestures.Finger finger0, FingerGestures.Finger finger1, Vector2 refPos0, Vector2 refPos1 )
    {
        Vector2 curDir = ( finger0.Position - finger1.Position ).normalized;
        Vector2 refDir = ( refPos0 - refPos1 ).normalized;

        // check if we went past the minimum rotation amount treshold
        return Mathf.Rad2Deg * FingerGestures.SignedAngle( refDir, curDir );
    }

    #endregion

    protected override int GetRequiredFingerCount() { return 2; }

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

        // check if we went past the minimum rotation amount treshold
        float rotation = SignedAngularGap( finger0, finger1, finger0.StartPosition, finger1.StartPosition );
        if( Mathf.Abs( rotation ) < MinRotation )
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

        float angle = SignedAngularGap( finger0, finger1, finger0.StartPosition, finger1.StartPosition );
        totalRotation = Mathf.Sign( angle ) * MinRotation;
        rotationDelta = 0;

        if( OnRotationBegin != null )
            OnRotationBegin( this );

        rotationDelta = angle - totalRotation;
        totalRotation = angle;

        if( OnRotationMove != null )
            OnRotationMove( this );
    }

    protected override GestureState OnActive( FingerGestures.IFingerList touches )
    {
        if( touches.Count != RequiredFingerCount )
        {
            // fingers were lifted?
            if( touches.Count < RequiredFingerCount )
            {
                if( OnRotationEnd != null )
                    OnRotationEnd( this );

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

        rotationDelta = SignedAngularGap( finger0, finger1, finger0.PreviousPosition, finger1.PreviousPosition );
        totalRotation += rotationDelta;

        if( OnRotationMove != null )
            OnRotationMove( this );

        return GestureState.InProgress;
    }
}
