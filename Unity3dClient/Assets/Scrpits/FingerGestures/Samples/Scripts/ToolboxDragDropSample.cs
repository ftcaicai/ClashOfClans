using UnityEngine;
using System.Collections;

/// <summary>
/// This sample demonstrates the use of the Toolbox's Drag & Drop scripts
/// </summary>
public class ToolboxDragDropSample : SampleBase
{
    #region Properties exposed to the editor

    public TBInputManager inputMgr;
    public Transform[] dragObjects;

    public Collider dragSphere;
    public Collider dragPlane;

    public Light pointlight;

    #endregion

    #region Drag Plane Mode
    
    enum DragPlaneMode
    {
        XY,
        Plane,
        Sphere
    }

    DragPlaneMode dragPlaneMode = DragPlaneMode.XY;

    void SetDragPlaneMode( DragPlaneMode mode )
    {
        switch( mode )
        {
            case DragPlaneMode.XY:
                RestoreInitialPositions();
                dragSphere.gameObject.active = false;
                dragPlane.gameObject.active = false;
                inputMgr.dragPlaneType = TBInputManager.DragPlaneType.XY;
                break;

            case DragPlaneMode.Plane:
                RestoreInitialPositions();
                dragSphere.gameObject.active = false;
                dragPlane.gameObject.active = true;
                inputMgr.dragPlaneCollider = dragPlane;
                inputMgr.dragPlaneType = TBInputManager.DragPlaneType.UseCollider;
                break;

            case DragPlaneMode.Sphere:
                RestoreInitialPositions();
                dragSphere.gameObject.active = true;
                dragPlane.gameObject.active = false;
                inputMgr.dragPlaneCollider = dragSphere;
                inputMgr.dragPlaneType = TBInputManager.DragPlaneType.UseCollider;
                break;
        }

        dragPlaneMode = mode;
    }

    #endregion

    #region Initial positions save / restore

    Vector3[] initialPositions;
        
    void SaveInitialPositions()
    {
        initialPositions = new Vector3[dragObjects.Length];

        for( int i = 0; i < initialPositions.Length; ++i )
            initialPositions[i] = dragObjects[i].position;
    }

    void RestoreInitialPositions()
    {
        for( int i = 0; i < initialPositions.Length; ++i )
            dragObjects[i].position = initialPositions[i];
    }

    #endregion

    #region Setup

    protected override string GetHelpText()
    {
        return @"This sample demonstrates the use of the Toolbox's Drag & Drop scripts";
    }

    protected override void Start()
    {
        base.Start();

        SaveInitialPositions();
        SetDragPlaneMode( DragPlaneMode.XY );
    }

    #endregion

    #region GUI

    public Rect dragModeButtonRect;

    void OnGUI()
    {
        if( UI.showHelp )
            return;

        SampleUI.ApplyVirtualScreen();

        string buttonText;
        DragPlaneMode nextDragPlaneMode;

        switch( dragPlaneMode )
        {
            case DragPlaneMode.Plane:
                buttonText = "Drag On Plane";
                nextDragPlaneMode = DragPlaneMode.Sphere;
                break;

            case DragPlaneMode.Sphere:
                buttonText = "Drag On Sphere";
                nextDragPlaneMode = DragPlaneMode.XY;
                break;

            default:
                buttonText = "Drag On XZ";
                nextDragPlaneMode = DragPlaneMode.Plane;
                break;
        }

        if( GUI.Button( dragModeButtonRect, buttonText ) )
            SetDragPlaneMode( nextDragPlaneMode );
    }

    #endregion

    void ToggleLight()
    {
        pointlight.enabled = !pointlight.enabled;
    }
}