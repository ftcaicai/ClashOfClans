using UnityEngine;
using System.Collections;

/// <summary>
/// Base class used by most common gestures that can be performed with 
/// and arbitrary number of fingers, such as drag, tap, swipe...
/// 
/// The position of the fingers are averaged and stored in the
/// StartPosition and/or Position fields
/// </summary>
public abstract class AveragedGestureRecognizer : GestureRecognizer
{
    /// <summary>
    /// Exact number of touches required for the gesture to be recognized
    /// </summary>
    public int RequiredFingerCount = 1;

    Vector2 startPos = Vector2.zero;
    Vector2 pos = Vector2.zero;

    protected override int GetRequiredFingerCount()
    {
        return RequiredFingerCount;
    }

    /// <summary>
    /// Initial finger(s) position
    /// </summary>
    public Vector2 StartPosition
    {
        get { return startPos; }
        protected set { startPos = value; }
    }

    /// <summary>
    /// Current finger(s) position
    /// </summary>
    public Vector2 Position
    {
        get { return pos; }
        protected set { pos = value; }
    }
}