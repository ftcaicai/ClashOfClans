using UnityEngine;
using System.Collections;

public class SwipeParticlesEmitter : MonoBehaviour 
{
    public ParticleEmitter emitter;
    public float baseSpeed = 4.0f;
    public float swipeVelocityScale = 0.001f;

    void Start()
    {
        if( !emitter )
            emitter = particleEmitter;

        emitter.emit = false;
    }

    public void Emit( FingerGestures.SwipeDirection direction, float swipeVelocity )
    {
        Vector3 heading;

        // convert the swipe direction to a 3D vector we can use as our new forward direction for the particle emitter
        if( direction == FingerGestures.SwipeDirection.Up )
            heading = Vector3.up;
        else if( direction == FingerGestures.SwipeDirection.Down )
            heading = Vector3.down;
        else if( direction == FingerGestures.SwipeDirection.Right )
            heading = Vector3.right;
        else
            heading = Vector3.left;

        // orient our emitter towards the swipe direction
        emitter.transform.rotation = Quaternion.LookRotation( heading );

        Vector3 localEmitVelocity = emitter.localVelocity;
        localEmitVelocity.z = baseSpeed * swipeVelocityScale * swipeVelocity;
        emitter.localVelocity = localEmitVelocity;

        // fire away!
        emitter.Emit();
    }
}
