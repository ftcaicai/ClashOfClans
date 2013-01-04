using UnityEngine;
using System.Collections;

/// <summary>
/// The base class for all gesture recognizers
/// </summary>
public abstract class GestureRecognizer : FGComponent
{
    /// <summary>
    /// Possible gesture states
    /// </summary>
    public enum GestureState
    {
        /// <summary>
        /// The gesture recognizer is ready and waiting for the correct initial input conditions to begin
        /// </summary>
        Ready,

        /// <summary>
        /// The gesture recognition has started and is still going on
        /// </summary>
        InProgress,

        /// <summary>
        /// The gesture detected a user input that invalidated it
        /// </summary>
        Failed,

        /// <summary>
        /// The gesture was succesfully recognized
        /// </summary>
        Recognized,
    }

    /// <summary>
    /// The reset mode determines when to reset a GestureRecognizer after it fails or succeed (GestureState.Failed or GestureState.Recognized)
    /// </summary>
    public enum GestureResetMode
    {
        /// <summary>
        /// The gesture recognizer will reset on the next Update()
        /// </summary>
        NextFrame,

        /// <summary>
        /// The gesture recognizer will reset at the end of the current multitouch sequence
        /// </summary>
        EndOfTouchSequence,

        /// <summary>
        /// The gesture recognizer will reset at the beginning of the next multitouch sequence
        /// </summary>
        StartOfTouchSequence,
    }

    #region State

    /// <summary>
    /// Event fired whenever the gesture recognizer state changes
    /// <see cref="State"/>
    /// <see cref="PreviousState"/>
    /// </summary>
    public event EventDelegate<GestureRecognizer> OnStateChanged;

    GestureState prevState = GestureState.Ready;
    GestureState state = GestureState.Ready;

    /// <summary>
    /// Get the previous gesture state
    /// </summary>
    public GestureState PreviousState
    {
        get { return prevState; }
    }

    /// <summary>
    /// Get or set the current gesture state
    /// </summary>
    public GestureState State
    {
        get { return state; }
        protected set
        {
            if( state != value )
            {
                prevState = state;
                state = value;

                if( OnStateChanged != null )
                    OnStateChanged( this );
            }
        }
    }

    /// <summary>
    /// Return true if the gesture recognition has started and is on-going
    /// <see cref="State"/>
    /// </summary>
    public bool IsActive
    {
        get { return State == GestureState.InProgress; }
    }

    #endregion

    #region Reset

    /// <summary>
    /// Get or set the reset mode for this gesture recognizer
    /// </summary>
    public GestureResetMode ResetMode = GestureResetMode.StartOfTouchSequence;

    /// <summary>
    /// Put back the gesture recognizer in Ready state and reset any relevant data
    /// <see cref="ResetMode"/>
    /// </summary>
    protected virtual void Reset()
    {
        State = GestureState.Ready;
    }

    #endregion

    protected override void Start()
    {
        base.Start();
        Reset();
    }

    #region Touch Sequence

    /// <summary>
    /// Called when the first finger of a new multi-touch sequence has touched the screen
    /// </summary>
    protected virtual void OnTouchSequenceStarted()
    {
        if( ResetMode == GestureResetMode.StartOfTouchSequence )
        {
            if( State == GestureState.Recognized || State == GestureState.Failed )
                Reset();
        }
    }

    /// <summary>
    /// Called when all the fingers that participated in the current multi-touch sequence are no longer touching the screen
    /// </summary>
    protected virtual void OnTouchSequenceEnded()
    {
        if( ResetMode == GestureResetMode.EndOfTouchSequence )
        {
            if( State == GestureState.Recognized || State == GestureState.Failed )
                Reset();
        }
    }

    #endregion

    int lastTouchesCount = 0;

    protected override void OnUpdate( FingerGestures.IFingerList touches )
    {
        if( touchFilter != null )
            touches = touchFilter.Apply( touches );

        if( touches.Count > 0 && lastTouchesCount == 0 )
            OnTouchSequenceStarted();

        switch( State )
        {
            case GestureState.Recognized:
            case GestureState.Failed:
                if( ResetMode == GestureResetMode.NextFrame )
                    Reset();
                break;

            case GestureState.Ready:
                State = OnReady( touches );
                break;

            case GestureState.InProgress:
                State = OnActive( touches );
                break;

            default:
                Debug.LogError( this + " - Unhandled state: " + State + ". Failing recognizer." );
                State = GestureState.Failed;
                break;
        }

        if( touches.Count == 0 && lastTouchesCount > 0 )
            OnTouchSequenceEnded();

        lastTouchesCount = touches.Count;
    }

    protected virtual GestureState OnReady( FingerGestures.IFingerList touches )
    {
        if( ShouldFailFromReady( touches ) )
            return GestureState.Failed;

        if( CanBegin( touches ) )
        {
            OnBegin( touches );
            return GestureState.InProgress;
        }

        return GestureState.Ready;
    }

    protected virtual bool ShouldFailFromReady( FingerGestures.IFingerList touches )
    {
        if( touches.Count != GetRequiredFingerCount() )
        {
            if( touches.Count > 0 && !Young( touches ) )
                return true;
        }

        return false;
    }

    /// <summary>
    /// This controls whether or not the gesture recognition should begin
    /// </summary>
    /// <param name="touches">The active touches</param>
    protected virtual bool CanBegin( FingerGestures.IFingerList touches )
    {
        if( touches.Count != GetRequiredFingerCount() )
            return false;

        // check with the delegate (provided we have one set)
        if( !CheckCanBeginDelegate( touches ) )
            return false;

        return true;
    }

    public virtual bool CheckCanBeginDelegate( FingerGestures.IFingerList touches )
    {
        if( canBeginDelegate != null && !canBeginDelegate( this, touches ) )
            return false;

        return true;
    }

    #region CanBegin Delegate

    public delegate bool CanBeginDelegate( GestureRecognizer gr, FingerGestures.IFingerList touches );
    CanBeginDelegate canBeginDelegate;

    public void SetCanBeginDelegate( CanBeginDelegate f ) { canBeginDelegate = f; }
    public CanBeginDelegate GetCanBeginDelegate() { return canBeginDelegate; }

    #endregion

    #region Abstract methods to implement in derived classes

    /// <summary>
    /// Return the exact number of active touches required for the gesture to be valid
    /// </summary>
    protected abstract int GetRequiredFingerCount();

    /// <summary>
    /// Method called when the gesture recognizer has just started recognizing a valid gesture
    /// </summary>
    /// <param name="touches">The active touches</param>
    protected abstract void OnBegin( FingerGestures.IFingerList touches );

    /// <summary>
    /// Method called on each frame that the gesture recognizer is in an active state
    /// </summary>
    /// <param name="touches">The active touches</param>
    /// <returns>The new state the gesture recognizer should be in</returns>
    protected abstract GestureState OnActive( FingerGestures.IFingerList touches );

    #endregion

    #region Touch Filter

    FingerGestures.ITouchFilter touchFilter = null;

    /// <summary>
    /// Get or set the touch filter used to modify the list of active touches about to be processed by the gesture recognizer
    /// </summary>
    public FingerGestures.ITouchFilter TouchFilter
    {
        get { return touchFilter; }
        set { touchFilter = value; }
    }

    #endregion

    #region Utils

    /// <summary>
    /// Check if all the touches in the list started recently
    /// </summary>
    /// <param name="touches">The touches to evaluate</param>
    /// <returns>True if the age of each touch in the list is under a set threshold</returns>
    protected bool Young( FingerGestures.IFingerList touches )
    {
        FingerGestures.Finger oldestTouch = touches.GetOldest();

        if( oldestTouch == null )
            return false;

        float elapsedTimeSinceFirstTouch = Time.time - oldestTouch.StarTime;

        return elapsedTimeSinceFirstTouch < 0.25f;
    }

    #endregion
}