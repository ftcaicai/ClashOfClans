using UnityEngine;
using System.Collections;

/// <summary>
/// Toolbox InputManager
/// 
/// This class acts as a HUB/manager for the various input gesture events. It dispatches calls to the various TB* classes, 
/// such as TBDrag, TBTap, etc... There should be exactly one instance of this class in the scene. 
/// 
/// The set of toolbox scripts currently supports:
/// - FingerDown
/// - FingerUp
/// - Drag & Drop
/// - Tap
/// - Long-Press
/// - Swipe
/// 
/// Please check the ToolboxDragDrop sample scenes for a solid example on how to use this system.
/// 
/// - Drag & Drop Implementation Details -
/// This class listens to the FingerGestures.OnFingerDragBegin event and raycasts into the scene at the given finger position 
/// in order to find a valid object equipped with a TBDrag component. If it finds one, it calls TBDrag.BeginDrag() on this object,
/// and starts listening to TBDrag.OnDragMove and TBDrag.OnDragEnd. The TBDrag object position is updated in dragged_OnDragMove.
/// </summary>
[AddComponentMenu( "FingerGestures/Toolbox/Input Manager" )]
public class TBInputManager : MonoBehaviour
{
    // changing these at runtime wont have any effect unless you disable and re-enable the Input Manager
    public bool trackFingerUp = true;
    public bool trackFingerDown = true;
    public bool trackDrag = true;
    public bool trackTap = true;
    public bool trackLongPress = true;
    public bool trackSwipe = true;

    public Camera raycastCamera;            // which camera to fire the raycats through. If set to null, this will default to the main camera
    public LayerMask ignoreLayers = 0;      // layers to ignore when doing the pick raycasts

    void Start()
    {
        if( !raycastCamera )
            raycastCamera = Camera.main;
    }

    void OnEnable()
    {
        // subscribe to the FingerGestures events
        if( trackFingerDown )
            FingerGestures.OnFingerDown += FingerGestures_OnFingerDown;

        if( trackFingerUp )
            FingerGestures.OnFingerUp += FingerGestures_OnFingerUp;

        if( trackDrag )
            FingerGestures.OnFingerDragBegin += FingerGestures_OnFingerDragBegin;
        
        if( trackTap )
            FingerGestures.OnFingerTap += FingerGestures_OnFingerTap;
        
        if( trackLongPress)
            FingerGestures.OnFingerLongPress += FingerGestures_OnFingerLongPress;

        if( trackSwipe )
            FingerGestures.OnFingerSwipe += FingerGestures_OnFingerSwipe;
    }

    void OnDisable()
    {
        // unsubscribe to FingerGestures events
        FingerGestures.OnFingerDown -= FingerGestures_OnFingerDown;
        FingerGestures.OnFingerUp -= FingerGestures_OnFingerUp;
        FingerGestures.OnFingerDragBegin -= FingerGestures_OnFingerDragBegin;
        FingerGestures.OnFingerTap -= FingerGestures_OnFingerTap;
        FingerGestures.OnFingerLongPress -= FingerGestures_OnFingerLongPress;
        FingerGestures.OnFingerSwipe -= FingerGestures_OnFingerSwipe;
    }

    #region Fingers Input Events

    void FingerGestures_OnFingerUp( int fingerIndex, Vector2 fingerPos, float timeHeldDown )
    {
        TBFingerUp fingerUpComp = PickComponent<TBFingerUp>( fingerPos );
        if( fingerUpComp )
            fingerUpComp.RaiseFingerUp( fingerIndex, fingerPos, timeHeldDown );
    }

    void FingerGestures_OnFingerDown( int fingerIndex, Vector2 fingerPos )
    {
        TBFingerDown fingerDownComp = PickComponent<TBFingerDown>( fingerPos );
        if( fingerDownComp )
            fingerDownComp.RaiseFingerDown( fingerIndex, fingerPos );
    }

    #endregion

    #region Drag & Drop

    public enum DragPlaneType
    {
        XY, // drag along the absolute XY plane (regular view)
        XZ, // drag along the absolute XZ plane (topdown view)
        ZY, // drag along the absolute ZY plane (side view)
        UseCollider, // project on the collider specified by dragPlaneCollider
        Camera, // drag along a plane parallal to the camera's screen plane (XY)
    }

    public DragPlaneType dragPlaneType = DragPlaneType.Camera; // current drag plane type used to convert the 2d finger position to a 3d world position
    public Collider dragPlaneCollider;      // collider used when dragPlaneType is set to DragPlaneType.UseCollider
    public float dragPlaneOffset = 0.0f;    // distance between dragged object and drag constraint plane
    
    void FingerGestures_OnFingerDragBegin( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
    {
        // check if the object is draggable
        TBDrag draggable = PickComponent<TBDrag>( startPos );
        if( draggable && !draggable.Dragging )
        {
            // initiate the drag operation
            draggable.BeginDrag( fingerIndex, fingerPos );

            // register to the drag move & end events so we can update this object's position and unsubscribe to these events when done.
            draggable.OnDragMove += draggable_OnDragMove;
            draggable.OnDragEnd += draggable_OnDragEnd;  
        }
    }

    // converts a screen-space position to a world-space position constrained to the current drag plane type
    // returns false if it was unable to get a valid world-space position
    bool ProjectScreenPointOnDragPlane( Vector3 refPos, Vector2 screenPos, out Vector3 worldPos )
    {
        worldPos = refPos;

        switch( dragPlaneType )
        {
            case DragPlaneType.XY:
                worldPos = raycastCamera.ScreenToWorldPoint( new Vector3( screenPos.x, screenPos.y, Mathf.Abs( refPos.z - raycastCamera.transform.position.z ) ) );
                return true;

            case DragPlaneType.XZ:
                worldPos = raycastCamera.ScreenToWorldPoint( new Vector3( screenPos.x, screenPos.y, Mathf.Abs( refPos.y - raycastCamera.transform.position.y ) ) );
                return true;

            case DragPlaneType.ZY:
                worldPos = raycastCamera.ScreenToWorldPoint( new Vector3( screenPos.x, screenPos.y, Mathf.Abs( refPos.x - raycastCamera.transform.position.x ) ) );
                return true;

            case DragPlaneType.UseCollider:
                {
                    Ray ray = raycastCamera.ScreenPointToRay( screenPos );
                    RaycastHit hit;

                    if( !dragPlaneCollider.Raycast( ray, out hit, float.MaxValue ) )
                        return false;

                    worldPos = hit.point + dragPlaneOffset * hit.normal;
                }
                return true;

            case DragPlaneType.Camera:
                {
                    Transform camTransform = raycastCamera.transform;

                    // create a plane passing through refPos and facing toward the camera
                    Plane plane = new Plane( -camTransform.forward, refPos );

                    Ray ray = raycastCamera.ScreenPointToRay( screenPos );

                    float t = 0;
                    if( !plane.Raycast( ray, out t ) )
                        return false;

                    worldPos = ray.GetPoint( t );
                }
                return true;
        }

        return false;
    }

    // one of the fingers holding a draggable object is moving. 
    // Update the dragged object position accordingly.
    void draggable_OnDragMove( TBDrag sender )
    {
        // figure out our previous screen space finger position
        Vector2 prevFingerPos = sender.FingerPos - sender.MoveDelta;
        
        Vector3 fingerPos3d, prevFingerPos3d;

        // convert these to world-space coordinates, and compute the amount of motion we need to apply to the object
        if( ProjectScreenPointOnDragPlane( sender.transform.position, prevFingerPos, out prevFingerPos3d ) &&
            ProjectScreenPointOnDragPlane( sender.transform.position, sender.FingerPos, out fingerPos3d ) )
        {
            Vector3 move = fingerPos3d - prevFingerPos3d;
            sender.transform.position += move;
        }
    }

    void draggable_OnDragEnd( TBDrag source )
    {
        // unsubscribe from this object's drag events
        source.OnDragMove -= draggable_OnDragMove;
        source.OnDragEnd -= draggable_OnDragEnd;
    }

    #endregion 

    #region Tap

    void FingerGestures_OnFingerTap( int fingerIndex, Vector2 fingerPos, int tapCount )
    {
        TBTap tapComp = PickComponent<TBTap>( fingerPos );
        if( tapComp )
            tapComp.RaiseTap( fingerIndex, fingerPos, tapCount );
    }

    #endregion

    #region LongPress

    void FingerGestures_OnFingerLongPress( int fingerIndex, Vector2 fingerPos )
    {
        TBLongPress longPressComp = PickComponent<TBLongPress>( fingerPos );
        if( longPressComp )
            longPressComp.RaiseLongPress( fingerIndex, fingerPos );
    }

    #endregion

    #region Swipe

    void FingerGestures_OnFingerSwipe( int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity )
    {
        TBSwipe swipeComp = PickComponent<TBSwipe>( startPos );
        if( swipeComp )
            swipeComp.RaiseSwipe( fingerIndex, startPos, direction, velocity );
    }

    #endregion

    #region Utils

    // Return the GameObject at the given screen position, or null if no valid object was found
    GameObject PickObject( Vector2 screenPos )
    {
        Ray ray = Camera.main.ScreenPointToRay( screenPos );
        RaycastHit hit;

        if( Physics.Raycast( ray, out hit, float.MaxValue, ~ignoreLayers ) )
            return hit.collider.gameObject;

        return null;
    }

    T PickComponent<T>( Vector2 screenPos ) where T:TBComponent
    {
        GameObject go = PickObject( screenPos );
        
        if( !go )
            return null;

        return go.GetComponent<T>();
    }

    #endregion
}
