using UnityEngine;
using System.Collections;

/// <summary>
/// The finger motion detector component is not an actual gesture but is responsible for tracking the motion (or stillness) of a specific finger
/// </summary>
public class FingerMotionDetector : FGComponent
{
    /// <summary>
    /// Event fired when the finger started moving beyond the MoveThreshold.
    /// Use AnchorPos corresponds to retrieve the last starting position
    /// <see cref="AnchorPos"/>
    /// <see cref="MoveThreshold"/>
    /// </summary>
    public event EventDelegate<FingerMotionDetector> OnMoveBegin;

    /// <summary>
    /// Event fired when the finger has moved since last update.
    /// Use MoveDelta to retrieve the motion performed since last update
    /// <see cref="MoveDelta"/>
    /// </summary>
    public event EventDelegate<FingerMotionDetector> OnMove;

    /// <summary>
    /// Event fired when the finger has stopped moving
    /// </summary>
    public event EventDelegate<FingerMotionDetector> OnMoveEnd;

    /// <summary>
    /// Event fired when the finger starts being stationary (not moving)
    /// </summary>
    public event EventDelegate<FingerMotionDetector> OnStationaryBegin;

    /// <summary>
    /// Event fired on each frame that the finger remains stationary.
    /// Use ElapsedStationaryTime to retreive the amount of time elapsed since the finger started being stationary
    /// <see cref="ElapsedStationaryTime"/>
    /// </summary>
    public event EventDelegate<FingerMotionDetector> OnStationary;

    /// <summary>
    /// Event fired when the finger stops being stationary (starts moving)
    /// Use ElapsedStationaryTime to retreive the total amount of time the finger stayed stationary
    /// <see cref="ElapsedStationaryTime"/>
    /// </summary>
    public event EventDelegate<FingerMotionDetector> OnStationaryEnd;

    /// <summary>
    /// Motion state
    /// </summary>
    public enum MotionState
    {
        /// <summary>
        /// Undefined
        /// </summary>
        None,

        /// <summary>
        /// The finger is not moving
        /// </summary>
        Stationary,

        /// <summary>
        /// The finger is moving
        /// </summary>
        Moving,
    }

    /// <summary>
    /// Tolerance distance the finger is allowed to move from its initial position
    /// without being recognized as an actual motion
    /// </summary>
    public float MoveThreshold = 5.0f;

    FingerGestures.Finger finger;
    MotionState state = MotionState.None;
    MotionState prevState = MotionState.None;
    int moves = 0;
    float stationaryStartTime = 0;
    Vector2 anchorPos = Vector2.zero;
    bool wasDown = false; // this tracks if the finger was down in our previous update

    /// <summary>
    /// Get or set the finger to track
    /// </summary>
    public virtual FingerGestures.Finger Finger
    {
        get { return finger; }
        set { finger = value; }
    }

    /// <summary>
    /// Current finger motion state
    /// </summary>
    protected MotionState State
    {
        get { return state; }
        private set
        {
            state = value;
        }
    }

    /// <summary>
    /// Finger motion state during the previous update
    /// </summary>
    protected MotionState PreviousState
    {
        get { return prevState; }
        private set { prevState = value; }
    }

    /// <summary>
    /// Number of moves performed
    /// A complete MoveBegin/Move/MoveEnd sequence counts for 1 move.
    /// </summary>
    public int Moves
    {
        get { return moves; }
        private set { moves = value; }
    }

    /// <summary>
    /// Return true if at least one finger move was performed (see Moves property above)
    /// </summary>
    public bool Moved
    {
        get { return Moves > 0; }
    }

    /// <summary>
    /// Return true if Moving was true during the previous update
    /// </summary>
    public bool WasMoving
    {
        get { return PreviousState == MotionState.Moving; }
    }

    /// <summary>
    /// Is the finger currently moving this frame?
    /// </summary>
    public bool Moving
    {
        get { return State == MotionState.Moving; }
    }

    /// <summary>
    /// Amount of time spent during the last or current stationary state sequence 
    /// from OnBeginStationary up to OnEndStationary.
    /// </summary>
    public float ElapsedStationaryTime
    {
        get { return Time.time - stationaryStartTime; }
    }

    /// <summary>
    /// Reference position used to evaluate if we're still within the move threshold distance.
    /// This is the last initial stationary position (initial finger contact position, or 
    /// finger position during the last OnMoveEnd event)
    /// </summary>
    public Vector2 AnchorPos
    {
        get { return anchorPos; }
        private set { anchorPos = value; }
    }

    protected override void OnUpdate( FingerGestures.IFingerList touches )
    {
        if( Finger.IsDown )
        {
            if( !wasDown )
            {
                Moves = 0;
                AnchorPos = Finger.Position;
                State = MotionState.Stationary;
            }

            if( Finger.Phase == FingerGestures.FingerPhase.Moved )
            {
                if( State != MotionState.Moving )
                {
                    Vector2 delta = Finger.Position - AnchorPos;

                    // check if we moved beyond the threshold
                    if( delta.sqrMagnitude >= MoveThreshold * MoveThreshold )
                        State = MotionState.Moving;
                    else
                        State = MotionState.Stationary;
                }
            }
            else
            {
                State = MotionState.Stationary;
            }
        }
        else
        {
            State = MotionState.None;
        }

        RaiseEvents();

        PreviousState = State;
        wasDown = Finger.IsDown;
    }

    void RaiseEvents()
    {
        // state transition
        if( State != PreviousState )
        {
            // end previous state
            if( PreviousState == MotionState.Moving )
            {
                RaiseOnMoveEnd();

                AnchorPos = Finger.Position;
            }
            else if( PreviousState == MotionState.Stationary )
            {
                RaiseOnStationaryEnd();
            }

            // begin new state
            if( State == MotionState.Moving )
            {
                RaiseOnMoveBegin();

                ++Moves;
            }
            else if( State == MotionState.Stationary )
            {
                stationaryStartTime = Time.time;

                RaiseOnStationaryBegin();
            }
        }

        if( State == MotionState.Stationary )
        {
            RaiseOnStationary();
        }
        else if( State == MotionState.Moving )
        {
            RaiseOnMove();
        }
    }

    #region Event firing wrappers

    protected void RaiseOnMoveBegin()
    {
        if( OnMoveBegin != null )
            OnMoveBegin( this );
    }

    protected void RaiseOnMove()
    {
        if( OnMove != null )
            OnMove( this );
    }

    protected void RaiseOnMoveEnd()
    {
        if( OnMoveEnd != null )
            OnMoveEnd( this );
    }

    protected void RaiseOnStationaryBegin()
    {
        if( OnStationaryBegin != null )
            OnStationaryBegin( this );
    }

    protected void RaiseOnStationary()
    {
        if( OnStationary != null )
            OnStationary( this );
    }

    protected void RaiseOnStationaryEnd()
    {
        if( OnStationaryEnd != null )
            OnStationaryEnd( this );
    }

    #endregion
}
