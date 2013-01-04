using UnityEngine;
using System.Collections;

/// <summary>
/// ToolBox Swipe Component
/// Put this script on any 3D GameObject to detect when they are swipped
/// </summary>
[AddComponentMenu( "FingerGestures/Toolbox/Swipe" )]
public class TBSwipe : TBComponent
{
    public bool swipeLeft = true;
    public bool swipeRight = true;
    public bool swipeUp = true;
    public bool swipeDown = true;
    public float minVelocity = 0;

    public Message swipeMessage = new Message( "OnSwipe" );
    public Message swipeLeftMessage = new Message( "OnSwipeLeft", false );
    public Message swipeRightMessage = new Message( "OnSwipeRight", false );
    public Message swipeUpMessage = new Message( "OnSwipeUp", false );
    public Message swipeDownMessage = new Message( "OnSwipeDown", false );

    public event EventHandler<TBSwipe> OnSwipe;

    FingerGestures.SwipeDirection direction;
    public FingerGestures.SwipeDirection Direction
    {
        get { return direction; }
        protected set { direction = value; }
    }

    float velocity;
    public float Velocity
    {
        get { return velocity; }
        protected set { velocity = value; }
    }

    public bool IsValid( FingerGestures.SwipeDirection direction )
    {
        if( direction == FingerGestures.SwipeDirection.Left )
            return swipeLeft;

        if( direction == FingerGestures.SwipeDirection.Right )
            return swipeRight;
        
        if( direction == FingerGestures.SwipeDirection.Up )
            return swipeUp;
        
        if( direction == FingerGestures.SwipeDirection.Down )
            return swipeDown;

        return false;
    }

    Message GetMessageForSwipeDirection( FingerGestures.SwipeDirection direction )
    {
        if( direction == FingerGestures.SwipeDirection.Left )
            return swipeLeftMessage;

        if( direction == FingerGestures.SwipeDirection.Right )
            return swipeRightMessage;

        if( direction == FingerGestures.SwipeDirection.Up )
            return swipeUpMessage;

        return swipeDownMessage;
    }

    public bool RaiseSwipe( int fingerIndex, Vector2 fingerPos, FingerGestures.SwipeDirection direction, float velocity )
    {
        if( velocity < minVelocity )
            return false;

        if( !IsValid( direction ) )
            return false;

        FingerIndex = fingerIndex;
        FingerPos = fingerPos;
        Direction = direction;
        Velocity = velocity;

        if( OnSwipe != null )
            OnSwipe( this );

        Send( swipeMessage );
        Send( GetMessageForSwipeDirection( direction ) );

        return true;
    }
}
