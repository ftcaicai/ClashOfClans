using UnityEngine;
using System.Collections;

/// <summary>
/// Input.Axis-based Pinch gesture replacement for mouse-device
/// Warning: it's a bit of a hack caused due to design limitations :( 
/// </summary>
[AddComponentMenu( "FingerGestures/Gesture Recognizers/Mouse Pinch" )]
public class MousePinchGestureRecognizer : PinchGestureRecognizer
{
    public string axis = "Mouse ScrollWheel";

    int requiredFingers = 2;

    protected override int GetRequiredFingerCount()
    {
        return requiredFingers;
    }

    protected override bool CanBegin( FingerGestures.IFingerList touches )
    {
        if( !CheckCanBeginDelegate( touches ) )
            return false;

        float motion = Input.GetAxis( axis );
        if( Mathf.Abs( motion ) < 0.0001f )
            return false;

        return true;
    }
    
    protected override void OnBegin( FingerGestures.IFingerList touches )
    {
        StartPosition[0] = StartPosition[1] = Input.mousePosition;
        Position[0] = Position[1] = Input.mousePosition;

        delta = 0;

        RaiseOnPinchBegin();

        delta = DeltaScale * Input.GetAxis( axis );
        resetTime = Time.time + 0.1f;

        RaiseOnPinchMove();
    }

    float resetTime = 0;

    protected override GestureState OnActive( FingerGestures.IFingerList touches )
    {
        float motion = Input.GetAxis( axis );

        if( Mathf.Abs( motion ) < 0.001f )
        {
            if( resetTime <= Time.time )
            {
                RaiseOnPinchEnd();
                return GestureState.Recognized;
            }
            
            return GestureState.InProgress;
        }
        else
        {
            resetTime = Time.time + 0.1f;
        }
    
        Position[0] = Position[1] = Input.mousePosition;

        delta = DeltaScale * motion;
        
        RaiseOnPinchMove();

        return GestureState.InProgress;
    }
}
