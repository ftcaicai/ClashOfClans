using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This sample helps visualize the following finger events:
/// - OnFingerDown
/// - OnFingerMoveBegin
/// - OnFingerMove
/// - OnFingerMoveEnd
/// - OnFingerUp
/// </summary>
public class FingerEventsSamplePart2 : SampleBase
{
    #region Properties exposed to the editor

    public LineRenderer lineRendererPrefab;
    public GameObject fingerDownMarkerPrefab;
    public GameObject fingerMoveBeginMarkerPrefab;
    public GameObject fingerMoveEndMarkerPrefab;
    public GameObject fingerUpMarkerPrefab;

    #endregion

    #region Utility class that represent a single finger path

    class PathRenderer
    {
        LineRenderer lineRenderer;

        // passage points
        List<Vector3> points = new List<Vector3>();

        // list of marker objects currently instantiated
        List<GameObject> markers = new List<GameObject>();

        public PathRenderer( int index, LineRenderer lineRendererPrefab )
        {
            lineRenderer = Instantiate( lineRendererPrefab ) as LineRenderer;
            lineRenderer.name = lineRendererPrefab.name + index;
            lineRenderer.enabled = true;

            UpdateLines();
        }

        public void Reset()
        {
            points.Clear();
            UpdateLines();
            
            // destroy markers
            foreach( GameObject marker in markers )
                Destroy( marker );

            markers.Clear();
        }

        public void AddPoint( Vector2 screenPos )
        {
            AddPoint( screenPos, null );
        }

        public void AddPoint( Vector2 screenPos, GameObject markerPrefab )
        {
            Vector3 pos = SampleBase.GetWorldPos( screenPos );

            if( markerPrefab )
                AddMarker( pos, markerPrefab );

            points.Add( pos );
            UpdateLines();
        }

        GameObject AddMarker( Vector2 pos, GameObject prefab )
        {
            GameObject instance = Instantiate( prefab, pos, Quaternion.identity ) as GameObject;
            instance.name = prefab.name + "(" + markers.Count + ")";
            markers.Add( instance );
            return instance;
        }

        void UpdateLines()
        {
            lineRenderer.SetVertexCount( points.Count );
            for( int i = 0; i < points.Count; ++i )
                lineRenderer.SetPosition( i, points[i] );
        }
    }

    #endregion

    // one PathRenderer per finger
    PathRenderer[] paths;

    #region Setup

    protected override void Start()
    {
        base.Start();

        UI.StatusText = "Drag your fingers anywhere on the screen";

        // create one PathRenderer per finger
        paths = new PathRenderer[FingerGestures.Instance.MaxFingers];
        for( int i = 0; i < paths.Length; ++i )
            paths[i] = new PathRenderer( i, lineRendererPrefab );
    }

    protected override string GetHelpText()
    {
        return @"This sample lets you visualize the FingerDown, FingerMoveBegin, FingerMove, FingerMoveEnd and FingerUp events.

INSTRUCTIONS:
Move your finger accross the screen and observe what happens.

LEGEND:
- Red Circle = FingerDown position
- Yellow Square = FingerMoveBegin position
- Green Sphere = FingerMoveEnd position
- Blue Circle = FingerUp position";
    }

    #endregion


    #region Gesture event registration/unregistration

    void OnEnable()
    {
        Debug.Log( "Registering finger gesture events from C# script" );

        // register input events
        FingerGestures.OnFingerDown += FingerGestures_OnFingerDown;
        FingerGestures.OnFingerUp += FingerGestures_OnFingerUp; 
        FingerGestures.OnFingerMoveBegin += FingerGestures_OnFingerMoveBegin;
        FingerGestures.OnFingerMove += FingerGestures_OnFingerMove;
        FingerGestures.OnFingerMoveEnd += FingerGestures_OnFingerMoveEnd;
    }
  
    void OnDisable()
    {
        // unregister finger gesture events
        FingerGestures.OnFingerDown -= FingerGestures_OnFingerDown;
        FingerGestures.OnFingerUp -= FingerGestures_OnFingerUp;
        FingerGestures.OnFingerMoveBegin -= FingerGestures_OnFingerMoveBegin;
        FingerGestures.OnFingerMove -= FingerGestures_OnFingerMove;
        FingerGestures.OnFingerMoveEnd -= FingerGestures_OnFingerMoveEnd;
    }

    #endregion

    #region Reaction to finger events

    void FingerGestures_OnFingerDown( int fingerIndex, Vector2 fingerPos )
    {
        PathRenderer path = paths[fingerIndex];
        path.Reset();
        path.AddPoint( fingerPos, fingerDownMarkerPrefab );
    }

    void FingerGestures_OnFingerUp( int fingerIndex, Vector2 fingerPos, float timeHeldDown )
    {
        PathRenderer path = paths[fingerIndex];
        path.AddPoint( fingerPos, fingerUpMarkerPrefab );

        UI.StatusText = "Finger " + fingerIndex + " was held down for " + timeHeldDown.ToString( "N2" ) + " seconds";
    }
    
    void FingerGestures_OnFingerMoveBegin( int fingerIndex, Vector2 fingerPos )
    {
        UI.StatusText = "Started moving finger " + fingerIndex;

        PathRenderer path = paths[fingerIndex];
        path.AddPoint( fingerPos, fingerMoveBeginMarkerPrefab );
    }

    void FingerGestures_OnFingerMove( int fingerIndex, Vector2 fingerPos )
    {
        PathRenderer path = paths[fingerIndex];
        path.AddPoint( fingerPos );
    }

    void FingerGestures_OnFingerMoveEnd( int fingerIndex, Vector2 fingerPos )
    {
        UI.StatusText = "Stopped moving finger " + fingerIndex;

        PathRenderer path = paths[fingerIndex];
        path.AddPoint( fingerPos, fingerMoveEndMarkerPrefab );
    }

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
