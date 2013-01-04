using UnityEngine;
using System.Collections;

/// <summary>
/// Swipe gesture: quick drag/drop motion & release in a cardinal direction (e.g. a page flip with the finger)
/// </summary>
[AddComponentMenu( "FingerGestures/Gesture Recognizers/Swipe" )]
public class SwipeGestureRecognizer : AveragedGestureRecognizer
{
    /// <summary>
    /// Event fired when a valid swipe gesture has been detected, upon release of the finger(s)
    /// <see cref="Direction"/>
    /// <see cref="Velocity"/>
    /// </summary>
    public event EventDelegate<SwipeGestureRecognizer> OnSwipe;

    /// <summary>
    /// Directions we want the swipe recognizer to detect
    /// </summary>
    public FingerGestures.SwipeDirection ValidDirections = FingerGestures.SwipeDirection.All;

    /// <summary>
    /// Minimum swipe distance
    /// </summary>
    public float MinDistance = 1.0f;

    /// <summary>
    /// Minimum swipe velocity
    /// </summary>
    public float MinVelocity = 1.0f;

    /// <summary>
    /// Amount of tolerance when determining if the finger motion was performed along one of the supported swipe directions.
    /// This amount should be kept between 0 and 0.5f, where 0 means no tolerance and 0.5f means you can move within 45 degrees away from the allowed direction
    /// </summary>
    public float DirectionTolerance = 0.2f; //DOT

    Vector2 move;
    FingerGestures.SwipeDirection direction = FingerGestures.SwipeDirection.None;
    float velocity = 0;
    float startTime = 0;

    /// <summary>
    /// Get the total move from start position to last/current position
    /// </summary>
    public Vector2 Move
    {
        get { return move; }
        private set { move = value; }
    }

    /// <summary>
    /// Get the swipe direction detected
    /// </summary>
    public FingerGestures.SwipeDirection Direction
    {
        get { return direction; }
    }

    /// <summary>
    /// Get the current swipe velocity (in screen units per second)
    /// </summary>
    public float Velocity
    {
        get { return velocity; }
    }

    /// <summary>
    /// Return true if the input direction is supported
    /// </summary>
    public bool IsValidDirection( FingerGestures.SwipeDirection dir )
    {
        if( dir == FingerGestures.SwipeDirection.None )
            return false;

        return ( ( ValidDirections & dir ) == dir );
    }

    protected override bool CanBegin( FingerGestures.IFingerList touches )
    {
        if( !base.CanBegin( touches ) )
            return false;

        if( touches.GetAverageDistanceFromStart() < 0.5f )
            return false;

        return true;
    }

    protected override void OnBegin( FingerGestures.IFingerList touches )
    {
        Position = touches.GetAveragePosition();
        StartPosition = Position;
        direction = FingerGestures.SwipeDirection.None;
        startTime = Time.time;
    }

    protected override GestureState OnActive( FingerGestures.IFingerList touches )
    {
        if( touches.Count != RequiredFingerCount )
        {
            // fingers were lifted off
            if( touches.Count < RequiredFingerCount )
            {
                if( direction != FingerGestures.SwipeDirection.None )
                {
                    if( OnSwipe != null )
                        OnSwipe( this );

                    return GestureState.Recognized;
                }
            }

            return GestureState.Failed;
        }

        Position = touches.GetAveragePosition();
        Move = Position - StartPosition;

        float distance = Move.magnitude;

        // didnt move far enough
        if( distance < MinDistance )
            return GestureState.InProgress;
        
        float elapsedTime = Time.time - startTime;
        if( elapsedTime > 0 )
            velocity = distance / elapsedTime;
        else
            velocity = 0;

        // we're going too slow
        if( velocity < MinVelocity )
            return GestureState.Failed;
        
        FingerGestures.SwipeDirection newDirection = FingerGestures.GetSwipeDirection( Move.normalized, DirectionTolerance );
        
        // we went in a bad direction
        if( !IsValidDirection( newDirection ) || ( direction != FingerGestures.SwipeDirection.None && newDirection != direction ) )
            return GestureState.Failed;
        
        direction = newDirection;
        return GestureState.InProgress;
    }
}
