using UnityEngine;
using System.Collections;

/// <summary>
/// This tracks input gestures for a touch-screen device (mobiles)
/// </summary>
public class TouchScreenGestures : FingerGestures
{
    /// <summary>
    /// Maximum number of simultaneous fingers to track
    /// </summary>
    public int maxFingers = 5;

    public override int MaxFingers
    {
        get { return maxFingers; }
    }

    protected override void Start()
    {
        finger2touchMap = new int[MaxFingers];

        base.Start();
    }

    #region FingerGestures Implementation

    protected override FingerGestures.FingerPhase GetPhase( Finger finger )
    {
        if( HasValidTouch( finger ) )
        {
            UnityEngine.Touch touch = GetTouch( finger );

            switch( touch.phase )
            {
                case UnityEngine.TouchPhase.Began:
                    return FingerPhase.Began;

                case UnityEngine.TouchPhase.Moved:
                    return FingerPhase.Moved;

                case UnityEngine.TouchPhase.Stationary:
                    return FingerPhase.Stationary;

                default:
                    return FingerPhase.Ended;
            }
        }

        return FingerPhase.None;
    }

    protected override Vector2 GetPosition( Finger finger )
    {
        UnityEngine.Touch touch = GetTouch( finger );
        return touch.position;
    }

    #endregion

    #region Touch > Finger mapping

    UnityEngine.Touch nullTouch = new UnityEngine.Touch();

    int[] finger2touchMap;  // finger.index -> touch index map

    void UpdateFingerTouchMap()
    {
        for( int i = 0; i < finger2touchMap.Length; ++i )
            finger2touchMap[i] = -1;

        for( int i = 0; i < Input.touchCount; ++i )
        {
            int fingerIndex = Input.touches[i].fingerId;

            if( fingerIndex < finger2touchMap.Length )
                finger2touchMap[fingerIndex] = i;
        }
    }

    bool HasValidTouch( Finger finger )
    {
        return finger2touchMap[finger.Index] != -1;
    }

    UnityEngine.Touch GetTouch( Finger finger )
    {
        int touchIndex = finger2touchMap[finger.Index];

        if( touchIndex == -1 )
            return nullTouch;

        return Input.touches[touchIndex];
    }

    #endregion

    protected override void Update()
    {
        UpdateFingerTouchMap();
        base.Update();
    }
}
