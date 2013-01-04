using UnityEngine;
using System.Collections;

/// <summary>
/// ToolBox LongPress Component
/// 
/// Put this script on any 3D GameObject to detect when they are long-pressed
/// </summary>
[AddComponentMenu( "FingerGestures/Toolbox/LongPress" )]
public class TBLongPress : TBComponent
{
    public Message message = new Message( "OnLongPress" );
    
    public event EventHandler<TBLongPress> OnLongPress;

    public bool RaiseLongPress( int fingerIndex, Vector2 fingerPos )
    {
        FingerIndex = fingerIndex;
        FingerPos = fingerPos;

        if( OnLongPress != null )
            OnLongPress( this );

        Send( message );
        return true;
    }
}
