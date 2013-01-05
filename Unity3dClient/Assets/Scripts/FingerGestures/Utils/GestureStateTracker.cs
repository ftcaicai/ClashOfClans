using UnityEngine;
using System.Collections;

public class GestureStateTracker : MonoBehaviour 
{
    public GestureRecognizer gesture;

	void Awake() 
    {
        if( !gesture )
            gesture = GetComponent<GestureRecognizer>();
	}

    void OnEnable()
    {
        if( gesture )
            gesture.OnStateChanged += gesture_OnStateChanged;
    }

    void OnDisable()
    {
        if( gesture )
            gesture.OnStateChanged -= gesture_OnStateChanged;
    }

    void gesture_OnStateChanged( GestureRecognizer source )
    {
        Debug.Log( "Gesture " + source + " changed from " + source.PreviousState + " to " + source.State );
    }
}
