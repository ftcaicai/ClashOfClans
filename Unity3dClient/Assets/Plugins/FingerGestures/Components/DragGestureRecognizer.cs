using UnityEngine;
using System.Collections;

/// <summary>
/// Drag gesture: a full finger press > move > release sequence
/// </summary>
[AddComponentMenu( "FingerGestures/Gesture Recognizers/Drag" )]
public class DragGestureRecognizer : AveragedGestureRecognizer
{
    /// <summary>
    /// Event fired when the finger moved past its MoveTolerance radius from the StartPosition
    /// <see cref="MoveTolerance"/>
    /// <see cref="StartPosition"/>
    /// </summary>
    public event EventDelegate<DragGestureRecognizer> OnDragBegin;

    /// <summary>
    /// Event fired when the finger moved since the last update
    /// Use MoveDelta to retrieve the amount of motion performed since last update
    /// <see cref="MoveDelta"/>
    /// </summary>
    public event EventDelegate<DragGestureRecognizer> OnDragMove;

    /// <summary>
    /// Event fired when the dragged finger is released
    /// </summary>
    public event EventDelegate<DragGestureRecognizer> OnDragEnd;

    /// <summary>
    /// How far the finger is allowed to move from its initial position without making the gesture fail
    /// </summary>
    public float MoveTolerance = 5.0f;

    Vector2 delta = Vector2.zero;
    Vector2 lastPos = Vector2.zero;

    /// <summary>
    /// Amount of motion performed since the last update
    /// </summary>
    public Vector2 MoveDelta
    {
        get { return delta; }
        private set { delta = value; }
    }

    protected override bool CanBegin( FingerGestures.IFingerList touches )
    {
        if( !base.CanBegin( touches ) )
            return false;

        if( touches.GetAverageDistanceFromStart() < MoveTolerance )
            return false;

        return true;
    }

    protected override void OnBegin( FingerGestures.IFingerList touches )
    {
        Position = touches.GetAveragePosition();
        StartPosition = Position;
        MoveDelta = Vector2.zero;
        lastPos = Position;

        RaiseOnDragBegin();
    }

    protected override GestureState OnActive( FingerGestures.IFingerList touches )
    {
        if( touches.Count != RequiredFingerCount )
        {
            // fingers were lifted off
            if( touches.Count < RequiredFingerCount )
            {
                RaiseOnDragEnd();
                return GestureState.Recognized;
            }

            return GestureState.Failed;
        }

        Position = touches.GetAveragePosition();

        MoveDelta = Position - lastPos;

        if( MoveDelta.sqrMagnitude > 0 )
        {
            RaiseOnDragMove();
            lastPos = Position;
        }
            
        return GestureState.InProgress;
    }

    #region Event-Raising Wrappers

    protected void RaiseOnDragBegin()
    {
        if( OnDragBegin != null )
            OnDragBegin( this );
    }

    protected void RaiseOnDragMove()
    {
        if( OnDragMove != null )
            OnDragMove( this );
    }

    protected void RaiseOnDragEnd()
    {
        if( OnDragEnd != null )
            OnDragEnd( this );
    }

    #endregion
}
