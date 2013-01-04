using UnityEngine;
using System.Collections;

/// <summary>
/// This sample demonstrates some of the supported one-finger gestures:
/// - LongPress
/// - Tap
/// - Drag
/// - Swipe
/// </summary>
public class OneFingerGestureSample : SampleBase
{
    #region Properties exposed to the editor

    public GameObject longPressObject;
    public GameObject tapObject;
    public GameObject swipeObject;
    public GameObject dragObject;

    public int requiredTapCount = 2;

    #endregion

    #region Misc

    protected override string GetHelpText()
    {
        return @"This sample demonstrates some of the supported single-finger gestures:

- Drag: press the red sphere and move your finger to drag it around  

- LongPress: keep your finger pressed on the cyan sphere for at least " + FingerGestures.Defaults.Fingers[0].LongPress.Duration + @" seconds

- Tap: rapidly press & release the purple sphere 

- Swipe: press the yellow sphere and move your finger in one of the four cardinal directions, then release. The speed of the motion is taken into account.";
    }

    #endregion

    #region Gesture event registration/unregistration

void OnEnable()
{
    Debug.Log( "Registering finger gesture events from C# script" );

    // register input events
    FingerGestures.OnFingerLongPress += FingerGestures_OnFingerLongPress;
    FingerGestures.OnFingerTap += FingerGestures_OnFingerTap;
    FingerGestures.OnFingerSwipe += FingerGestures_OnFingerSwipe;
    FingerGestures.OnFingerDragBegin += FingerGestures_OnFingerDragBegin;
    FingerGestures.OnFingerDragMove += FingerGestures_OnFingerDragMove;
    FingerGestures.OnFingerDragEnd += FingerGestures_OnFingerDragEnd; 
}
  
    void OnDisable()
    {
        // unregister finger gesture events
        FingerGestures.OnFingerLongPress -= FingerGestures_OnFingerLongPress;
        FingerGestures.OnFingerTap -= FingerGestures_OnFingerTap;
        FingerGestures.OnFingerSwipe -= FingerGestures_OnFingerSwipe;
        FingerGestures.OnFingerDragBegin -= FingerGestures_OnFingerDragBegin;
        FingerGestures.OnFingerDragMove -= FingerGestures_OnFingerDragMove;
        FingerGestures.OnFingerDragEnd -= FingerGestures_OnFingerDragEnd;
    }

    #endregion

    #region Reaction to gesture events

    void FingerGestures_OnFingerLongPress( int fingerIndex, Vector2 fingerPos )
    {
        if( CheckSpawnParticles( fingerPos, longPressObject ) )
        {
            UI.StatusText = "Performed a long-press with finger " + fingerIndex;
        }
    }

    void FingerGestures_OnFingerTap( int fingerIndex, Vector2 fingerPos, int tapCount )
    {
        // spawn some particles when tapping the object at least requiredTapCount times
        if( tapCount >= requiredTapCount )
        {
            Debug.Log( "Tapcount: " + tapCount );

            if( CheckSpawnParticles( fingerPos, tapObject ) )
            {
                UI.StatusText = "Tapped " + tapCount + " times with finger " + fingerIndex;
            }
        }
    }

    // spin the yellow cube when swipping it
    void FingerGestures_OnFingerSwipe( int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity )
    {
        // make sure we started the swipe gesture on our swipe object
        GameObject selection = PickObject( startPos );
        if( selection == swipeObject )
        {
            UI.StatusText = "Swiped " + direction + " with finger " + fingerIndex;

            SwipeParticlesEmitter emitter = selection.GetComponentInChildren<SwipeParticlesEmitter>();
            if( emitter )
                emitter.Emit( direction, velocity );
        }
    }

    #region Drag & Drop Gesture

    int dragFingerIndex = -1;

    void FingerGestures_OnFingerDragBegin( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
    {
        // make sure we raycast from the initial finger position, not the current finger position (see remark about dragTreshold in comments)
        GameObject selection = PickObject( startPos );
        if( selection == dragObject )
        {
            UI.StatusText = "Started dragging with finger " + fingerIndex;
            
            // remember which finger is dragging dragObject
            dragFingerIndex = fingerIndex;
            
            // spawn some particles because it's cool.
            SpawnParticles( selection );
        }
    }

    void FingerGestures_OnFingerDragMove( int fingerIndex, Vector2 fingerPos, Vector2 delta )
    {
        // we make sure that this event comes from the finger that is dragging our dragObject
        if( fingerIndex == dragFingerIndex )
        {
            // update the position by converting the current screen position of the finger to a world position on the Z = 0 plane
            dragObject.transform.position = GetWorldPos( fingerPos );
        }
    }

    void FingerGestures_OnFingerDragEnd( int fingerIndex, Vector2 fingerPos )
    {
        // we make sure that this event comes from the finger that is dragging our dragObject
        if( fingerIndex == dragFingerIndex )
        {
            UI.StatusText = "Stopped dragging with finger " + fingerIndex;

            // reset our drag finger index
            dragFingerIndex = -1;

            // spawn some particles because it's cool.
            SpawnParticles( dragObject );
        }
    }

    #endregion

    #endregion

    #region Utils

    // attempt to pick the scene object at the given finger position and compare it to the given requiredObject. 
    // If it's this object spawn its particles.
    bool CheckSpawnParticles( Vector2 fingerPos, GameObject requiredObject )
    {
        GameObject selection = PickObject( fingerPos );

        if( !selection || selection != requiredObject )
            return false;

        SpawnParticles( selection );
        return true;
    }

    void SpawnParticles( GameObject obj )
    {
        ParticleEmitter emitter = obj.GetComponentInChildren<ParticleEmitter>();
        if( emitter )
            emitter.Emit();
    }

    #endregion
}
