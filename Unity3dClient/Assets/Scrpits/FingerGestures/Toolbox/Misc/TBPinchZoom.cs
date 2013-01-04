using UnityEngine;
using System.Collections;

/// <summary>
/// Put this script on a Camera object to allow for pinch-zoom gesture.
/// NOTE: this script does NOT require a TBInputManager instance to be present in the scene.
/// </summary>
[RequireComponent(typeof(Camera))]
[AddComponentMenu( "FingerGestures/Toolbox/Misc/Pinch-Zoom" )]
public class TBPinchZoom : MonoBehaviour
{
    public enum ZoomMethod
    {
        // move the camera position forward/backward
        Position,

        // change the field of view of the camera, or projection size for orthographic cameras
        FOV,
    }

    public ZoomMethod zoomMethod = ZoomMethod.Position;
    public float zoomSpeed = 1.5f;
    public float minZoomAmount = 0;
    public float maxZoomAmount = 50;

    Vector3 defaultPos = Vector3.zero;
    public Vector3 DefaultPos
    {
        get { return defaultPos; }
        set { defaultPos = value; }
    }

    float defaultFov = 0;
    public float DefaultFov
    {
        get { return defaultFov; }
        set { defaultFov = value; }
    }

    float defaultOrthoSize = 0;
    public float DefaultOrthoSize
    {
        get { return defaultOrthoSize; }
        set { defaultOrthoSize = value; }
    }

    float zoomAmount = 0;
    public float ZoomAmount
    {
        get { return zoomAmount; }
        set 
        {
            zoomAmount = Mathf.Clamp( value, minZoomAmount, maxZoomAmount ); 
        
            switch( zoomMethod )
            {
                case ZoomMethod.Position:
                    transform.position = defaultPos + zoomAmount * transform.forward;
                    break;

                case ZoomMethod.FOV:
                    if( camera.orthographic )
                        camera.orthographicSize = Mathf.Max( defaultOrthoSize - zoomAmount, 0.1f );
                    else
                        camera.fov = Mathf.Max( defaultFov - zoomAmount, 0.1f );
                    break;
            }
        }
    }

    void Start()
    {
        SetDefaults();
    }

    public void SetDefaults()
    {
        DefaultPos = transform.position;
        DefaultFov = camera.fov;
        DefaultOrthoSize = camera.orthographicSize;
    }

    #region FingerGestures events

    void OnEnable()
    {
        FingerGestures.OnPinchMove += FingerGestures_OnPinchMove;
    }

    void OnDisable()
    {
        FingerGestures.OnPinchMove -= FingerGestures_OnPinchMove;
    }

    void FingerGestures_OnPinchMove( Vector2 fingerPos1, Vector2 fingerPos2, float delta )
    {
        ZoomAmount += zoomSpeed * delta;
    }

    #endregion
}
