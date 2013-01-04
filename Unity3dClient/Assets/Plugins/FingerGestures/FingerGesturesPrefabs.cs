using UnityEngine;
using System.Collections;

public class FingerGesturesPrefabs : MonoBehaviour
{
    public FingerMotionDetector fingerMotion;
    public DragGestureRecognizer fingerDrag;
    public TapGestureRecognizer fingerTap;
    public SwipeGestureRecognizer fingerSwipe;
    public LongPressGestureRecognizer fingerLongPress;

    public DragGestureRecognizer drag;
    public TapGestureRecognizer tap;
    public SwipeGestureRecognizer swipe;
    public LongPressGestureRecognizer longPress;
    public PinchGestureRecognizer pinch;
    public RotationGestureRecognizer rotation;

    public DragGestureRecognizer twoFingerDrag;
    public TapGestureRecognizer twoFingerTap;
    public SwipeGestureRecognizer twoFingerSwipe;
    public LongPressGestureRecognizer twoFingerLongPress;
}
