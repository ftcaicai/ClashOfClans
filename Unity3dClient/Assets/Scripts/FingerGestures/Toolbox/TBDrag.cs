using UnityEngine;
using System.Collections;

/// <summary>
/// ToolBox Drag Component
/// Put this script on any 3D GameObject that you want to drag around.
/// </summary>
[AddComponentMenu( "FingerGestures/Toolbox/Drag" )]
public class TBDrag : TBComponent
{
    public Message dragBeginMessage = new Message( "OnDragBegin" );
    public Message dragMoveMessage = new Message( "OnDragMove", false );
    public Message dragEndMessage = new Message( "OnDragEnd" );

    public event EventHandler<TBDrag> OnDragBegin;
    public event EventHandler<TBDrag> OnDragMove;
    public event EventHandler<TBDrag> OnDragEnd;

    // are we being dragged?
    bool dragging = false;
    public bool Dragging
    {
        get { return dragging; }
        private set
        {
            if( dragging != value )
            {
                dragging = value;

                if( dragging )
                {
                    // register to the drag events
                    FingerGestures.OnFingerDragMove += FingerGestures_OnDragMove;
                    FingerGestures.OnFingerDragEnd += FingerGestures_OnDragEnd;
                }
                else
                {
                    // unregister from the drag events
                    FingerGestures.OnFingerDragMove -= FingerGestures_OnDragMove;
                    FingerGestures.OnFingerDragEnd -= FingerGestures_OnDragEnd;
                }
            }
        }
    }

    Vector2 moveDelta;
    public Vector2 MoveDelta
    {
        get { return moveDelta; }
        private set { moveDelta = value; }
    }

    public bool BeginDrag( int fingerIndex, Vector2 fingerPos )
    {
        // already dragging
        if( Dragging )
            return false;

        FingerIndex = fingerIndex;
        FingerPos = fingerPos;
        Dragging = true;

        if( OnDragBegin != null )
            OnDragBegin( this );

        // notify other components on this object that we've started the drag operation
        Send( dragBeginMessage );

        return true;
    }

    public bool EndDrag()
    {
        if( !Dragging )
            return false;

        if( OnDragEnd != null )
            OnDragEnd( this );

        // notify other components on this object that we've just finished the drag operation
        Send( dragEndMessage );

        // reset
        Dragging = false;
        FingerIndex = -1;
        return true;
    }

    #region FingerGestures events

    void FingerGestures_OnDragMove( int fingerIndex, Vector2 fingerPos, Vector2 delta )
    {
        if( Dragging && FingerIndex == fingerIndex )
        {
            FingerPos = fingerPos;
            MoveDelta = delta;

            if( OnDragMove != null )
                OnDragMove( this );

            Send( dragMoveMessage );
        }
    }

    void FingerGestures_OnDragEnd( int fingerIndex, Vector2 fingerPos )
    {
        if( Dragging && FingerIndex == fingerIndex )
        {
            FingerPos = fingerPos;
            EndDrag();
        }
    }

    #endregion

    #region Unity callbacks

    void OnDisable()
    {
        // if this gets disabled while dragging, make sure we cancel the drag operation
        if( Dragging )
            EndDrag();
    }

    #endregion
}
