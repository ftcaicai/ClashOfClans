using UnityEngine;
using System.Collections;

/// <summary>
/// This sample demonstrates how to use the two-fingers Pinch and Rotation gesture events to control the scale and orientation of a rectangle on the screen
/// </summary>
public class AdvancedPinchRotationSample : SampleBase
{
    public PinchGestureRecognizer pinchGesture;
    public RotationGestureRecognizer rotationGesture;

    public Transform target;
    public Material rotationMaterial;
    public Material pinchMaterial;
    public Material pinchAndRotationMaterial;
    public float pinchScaleFactor = 0.02f;

    Material originalMaterial;
    
    protected override void Start()
    {
        base.Start();

        UI.StatusText = "Use two fingers anywhere on the screen to rotate and scale the green object.";

        originalMaterial = target.renderer.sharedMaterial;

        pinchGesture.OnStateChanged += Gesture_OnStateChanged;
        pinchGesture.OnPinchMove += OnPinchMove;
        pinchGesture.SetCanBeginDelegate( CanBeginPinch );

        rotationGesture.OnStateChanged += Gesture_OnStateChanged;
        rotationGesture.OnRotationMove += OnRotationMove;
        rotationGesture.SetCanBeginDelegate( CanBeginRotation );
    }

    bool CanBeginRotation( GestureRecognizer gr, FingerGestures.IFingerList touches )
    {
        return !pinchGesture.IsActive;
    }

    bool CanBeginPinch( GestureRecognizer gr, FingerGestures.IFingerList touches )
    {
        return !rotationGesture.IsActive;
    }

    void Gesture_OnStateChanged( GestureRecognizer source )
    {
        if( source.PreviousState == GestureRecognizer.GestureState.Ready && source.State == GestureRecognizer.GestureState.InProgress )
            UI.StatusText = source + " gesture started";
        else if( source.PreviousState == GestureRecognizer.GestureState.InProgress )
        {
            if( source.State == GestureRecognizer.GestureState.Failed )
                UI.StatusText = source + " gesture failed";
            else if( source.State == GestureRecognizer.GestureState.Recognized )
                UI.StatusText = source + " gesture ended";
        }

        UpdateTargetMaterial();
    }
    
    void OnPinchMove( PinchGestureRecognizer source )
    {
        UI.StatusText = "Pinch updated by " + source.Delta + " degrees";

        // change the scale of the target based on the pinch delta value
        target.transform.localScale += source.Delta * pinchScaleFactor * Vector3.one;
    }

    void OnRotationMove( RotationGestureRecognizer source )
    {
        UI.StatusText = "Rotation updated by " + source.RotationDelta + " degrees";

        // apply a rotation around the Z axis by rotationAngleDelta degrees on our target object
        target.Rotate( 0, 0, source.RotationDelta );
    }

    #region Misc 

    protected override string GetHelpText()
    {
        return @"This sample demonstrates advanced use of the GestureRecognizer classes for Pinch and Rotation";
    }

    void UpdateTargetMaterial()
    {
        Material m;

        if( pinchGesture.IsActive && rotationGesture.IsActive )
            m = pinchAndRotationMaterial;
        else if( pinchGesture.IsActive )
            m = pinchMaterial;
        else if( rotationGesture.IsActive )
            m = rotationMaterial;
        else
            m = originalMaterial;

        target.renderer.sharedMaterial = m;
    }

    #endregion

}
