using UnityEngine;
using System.Collections;

/// <summary>
/// This tracks input gestures for a mouse device
/// </summary>
public class MouseGestures : FingerGestures
{
    // Number of mouse buttons to track
    public int maxMouseButtons = 3;

    protected override void Start()
    {
        base.Start();
    }

    public override int MaxFingers
    {
        get { return maxMouseButtons; }
    }

    protected override FingerGestures.FingerPhase GetPhase( Finger finger )
    {
        int button = finger.Index;

        // mouse button down?
        if( Input.GetMouseButton( button ) )
        {
            // did we just press it?
            if( Input.GetMouseButtonDown( button ) )
                return FingerPhase.Began;

            // find out if the mouse has moved since last update
            Vector3 delta = GetPosition( finger ) - finger.Position;

            if( delta.sqrMagnitude < 1.0f )
                return FingerPhase.Stationary;

            return FingerPhase.Moved;
        }

        // did we just release the button?
        if( Input.GetMouseButtonUp( button ) )
            return FingerPhase.Ended;

        return FingerPhase.None;
    }

    protected override Vector2 GetPosition( Finger finger )
    {
        return Input.mousePosition;
    }
}
