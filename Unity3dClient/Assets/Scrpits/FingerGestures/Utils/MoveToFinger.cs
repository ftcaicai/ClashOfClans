using UnityEngine;
using System.Collections;

/// <summary>
/// Put this script on any object you want to move to the first contact position of a finger
/// </summary>
public class MoveToFinger : MonoBehaviour 
{
    void OnEnable()
    {
        // subscribe to the finger down event
        FingerGestures.OnFingerDown += this.OnFingerDown;
    }

    void OnDisable()
    {
        // unsubscribe from the finger down event
        FingerGestures.OnFingerDown -= this.OnFingerDown;
    }

    // handle the fingerdown event
    void OnFingerDown( int fingerIndex, Vector2 fingerPos )
    {
        transform.position = GetWorldPos( fingerPos );
    }

    // convert from screen-space coordinates to world-space coordinates in the XY plane
    Vector3 GetWorldPos( Vector2 screenPos )
    {
        Camera mainCamera = Camera.main;
        return mainCamera.ScreenToWorldPoint( new Vector3( screenPos.x, screenPos.y, Mathf.Abs( transform.position.z - mainCamera.transform.position.z ) ) ); 
    }
}
