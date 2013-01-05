using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for all the TB* gesture components (TBDrag, TBTap, TBLongPress, TBSwipe...).
/// Implements commonly use methods and data structures.
/// 
/// NOTE: the GameObject must have a collider (it's used by the TBInputManager when raycasting into the scene to find the object under the finger).
/// Currently, this won't work out of the box with GUIText objects, due to the way they are behind rendered (2D). It will work with a 3D TextMesh though.
/// </summary>
public abstract class TBComponent : MonoBehaviour
{
    public delegate void EventHandler<T>( T sender ) where T : TBComponent;

    // index of finger that triggered the latest input event
    int fingerIndex = -1;
    public int FingerIndex
    {
        get { return fingerIndex; }
        protected set { fingerIndex = value; }
    }

    // finger screen position provided by the latest input event
    Vector2 fingerPos;
    public Vector2 FingerPos
    {
        get { return fingerPos; }
        protected set { fingerPos = value; }
    }

    // Use this for initialization
    protected virtual void Start()
    {
        if( !collider )
        {
            Debug.LogError( this.name + " must have a valid collider." );
            enabled = false;
        }
    }

    #region Message sending

    [System.Serializable]
    public class Message
    {
        public bool enabled = true;
        public string methodName = "MethodToCall";
        public GameObject target = null;

        public Message() { }
        public Message( string methodName )
        {
            this.methodName = methodName;
        }

        public Message( string methodName, bool enabled )
        {
            this.enabled = enabled;
            this.methodName = methodName;
        }
    }

    protected bool Send( Message msg )
    {
        if( !msg.enabled )
            return false;

        GameObject target = msg.target;
        if( !target )
            target = this.gameObject;

        target.SendMessage( msg.methodName, SendMessageOptions.DontRequireReceiver );
        return true;
    }

    #endregion
}
