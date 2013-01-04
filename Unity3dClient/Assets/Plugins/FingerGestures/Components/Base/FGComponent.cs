using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for any FingerGestures component
/// Its main task is to fire off OnUpdate() after the FingerGestures.Fingers have been updated during this frame.
/// </summary>
public abstract class FGComponent : MonoBehaviour
{
    public delegate void EventDelegate<T>( T source ) where T : FGComponent;

    protected virtual void Awake()
    {
        // made virtual in case of furture usage
    }

    protected virtual void Start()
    {
        // made virtual in case of furture usage
    }

    protected virtual void OnEnable()
    {
        FingerGestures.OnFingersUpdated += FingerGestures_OnFingersUpdated;
    }

    protected virtual void OnDisable()
    {
        FingerGestures.OnFingersUpdated -= FingerGestures_OnFingersUpdated;
    }

    void FingerGestures_OnFingersUpdated()
    {
        OnUpdate( FingerGestures.Touches );
    }

    /// <summary>
    /// This is called after FingerGestures has updated the state of each finger
    /// </summary>
    /// <param name="touches">The list of fingers currently down / touching the screen</param>
    protected abstract void OnUpdate( FingerGestures.IFingerList touches );
}