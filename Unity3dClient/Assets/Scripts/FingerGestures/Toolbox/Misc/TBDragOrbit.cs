using UnityEngine;
using System.Collections;

/// <summary>
/// Adaptation of the standard MouseOrbit script to use the finger drag gesture to rotate the current object using
/// the fingers/mouse around a target object
/// NOTE: TBInputManager NOT required
/// </summary>
[AddComponentMenu( "FingerGestures/Toolbox/Misc/DragOrbit" )]
public class TBDragOrbit : MonoBehaviour
{
    public enum PanMode
    {
        Disabled,
        OneFinger,
        TwoFingers
    }

    /// <summary>
    /// The object to orbit around
    /// </summary>
    public Transform target;

    /// <summary>
    /// Initial camera distance to target
    /// </summary>
    public float initialDistance = 10.0f;

    /// <summary>
    /// Minimum distance between camera and target
    /// </summary>
    public float minDistance = 1.0f;

    /// <summary>
    /// Maximum distance between camera and target
    /// </summary>
    public float maxDistance = 20.0f;

    /// <summary>
    /// Affects horizontal rotation speed
    /// </summary>
    public float yawSensitivity = 80.0f;

    /// <summary>
    /// Affects vertical rotation speed
    /// </summary>
    public float pitchSensitivity = 80.0f;

    /// <summary>
    /// Keep pitch angle value between minPitch and maxPitch?
    /// </summary>
    public bool clampPitchAngle = true;
    public float minPitch = -20;
    public float maxPitch = 80;

    /// <summary>
    /// Allow the user to affect the orbit distance using the pinch zoom gesture
    /// </summary>
    public bool allowPinchZoom = true;

    /// <summary>
    /// Affects pinch zoom speed
    /// </summary>
    public float pinchZoomSensitivity = 2.0f;

    /// <summary>
    /// Use smooth camera motions?
    /// </summary>
    public bool smoothMotion = true;
    public float smoothZoomSpeed = 3.0f;
    public float smoothOrbitSpeed = 4.0f;

    /// <summary>
    /// Two-Finger camera panning.
    /// Panning will apply an offset to the pivot/camera target point
    /// </summary>
    public bool allowPanning = false;
    public bool invertPanningDirections = false;
    public float panningSensitivity = 1.0f;
    public Transform panningPlane;  // reference transform used to apply the panning translation (using panningPlane.right and panningPlane.up vectors)
    public bool smoothPanning = true;
    public float smoothPanningSpeed = 8.0f;
    float lastPanTime = 0;

    float distance = 10.0f;
    float yaw = 0;
    float pitch = 0;

    float idealDistance = 0;
    float idealYaw = 0;
    float idealPitch = 0;

    Vector3 idealPanOffset = Vector3.zero;
    Vector3 panOffset = Vector3.zero;
    
    public float Distance
    {
        get { return distance; }
    } 

    public float IdealDistance
    {
        get { return idealDistance; }
        set { idealDistance = Mathf.Clamp( value, minDistance, maxDistance ); }
    }

    public float Yaw
    {
        get { return yaw; }
    } 

    public float IdealYaw
    {
        get { return idealYaw; }
        set { idealYaw = value; }
    }

    public float Pitch
    {
        get { return pitch; }
    } 

    public float IdealPitch
    {
        get { return idealPitch; }
        set { idealPitch = clampPitchAngle ? ClampAngle( value, minPitch, maxPitch ) : value; }
    }

    public Vector3 IdealPanOffset
    {
        get { return idealPanOffset; }
        set { idealPanOffset = value; }
    }

    public Vector3 PanOffset
    {
        get { return panOffset; }
    }

    void Start()
    {
        if( !panningPlane )
            panningPlane = this.transform;

        Vector3 angles = transform.eulerAngles;

        distance = IdealDistance = initialDistance;
        yaw = IdealYaw  = angles.y;
        pitch = IdealPitch = angles.x;

        // Make the rigid body not change rotation
        if( rigidbody )
            rigidbody.freezeRotation = true;

        Apply();
    }

    void OnEnable()
    {
        FingerGestures.OnDragMove += FingerGestures_OnDragMove;
        FingerGestures.OnPinchMove += FingerGestures_OnPinchMove;
        FingerGestures.OnTwoFingerDragMove += FingerGestures_OnTwoFingerDragMove;
    }
        
    void OnDisable()
    {
        FingerGestures.OnDragMove -= FingerGestures_OnDragMove;
        FingerGestures.OnPinchMove -= FingerGestures_OnPinchMove;
        FingerGestures.OnTwoFingerDragMove -= FingerGestures_OnTwoFingerDragMove;
    }

    void FingerGestures_OnDragMove( Vector2 fingerPos, Vector2 delta )
    {
        // if we panned recently, give a bit of time for all the fingers to lift off before we allow for one-finger drag
        if( Time.time - lastPanTime < 0.25f )
            return;

        if( target )
        {
            IdealYaw += delta.x * yawSensitivity * 0.02f;
            IdealPitch -= delta.y * pitchSensitivity * 0.02f;
        }
    }

    void FingerGestures_OnPinchMove( Vector2 fingerPos1, Vector2 fingerPos2, float delta )
    {
        if( allowPinchZoom )
            IdealDistance -= delta * pinchZoomSensitivity;
    }

    void FingerGestures_OnTwoFingerDragMove( Vector2 fingerPos, Vector2 delta )
    {
        if( allowPanning )
        {
            Vector3 move = -0.02f * panningSensitivity * ( panningPlane.right * delta.x + panningPlane.up * delta.y );
            
            if( invertPanningDirections )
                IdealPanOffset -= move; 
            else
                IdealPanOffset += move;

            lastPanTime = Time.time;
        }
    }

    void Apply()
    {
        if( smoothMotion )
        {
            distance = Mathf.Lerp( distance, IdealDistance, Time.deltaTime * smoothZoomSpeed );
            yaw = Mathf.Lerp( yaw, IdealYaw, Time.deltaTime * smoothOrbitSpeed );
            pitch = Mathf.Lerp( pitch, IdealPitch, Time.deltaTime * smoothOrbitSpeed );
        }
        else
        {
            distance = IdealDistance;
            yaw = IdealYaw;
            pitch = IdealPitch;
        }

        if( smoothPanning )
            panOffset = Vector3.Lerp( panOffset, idealPanOffset, Time.deltaTime * smoothPanningSpeed );
        else
            panOffset = idealPanOffset;

        transform.rotation = Quaternion.Euler( pitch, yaw, 0 );
        transform.position = ( target.position + panOffset ) - distance * transform.forward;
    }

    void LateUpdate()
    {
        Apply();
    }

    static float ClampAngle( float angle, float min, float max )
    {
        if( angle < -360 )
            angle += 360;
        
        if( angle > 360 )
            angle -= 360;

        return Mathf.Clamp( angle, min, max );
    }

    // recenter the camera
    public void ResetPanning()
    {
        IdealPanOffset = Vector3.zero;
    }
}


