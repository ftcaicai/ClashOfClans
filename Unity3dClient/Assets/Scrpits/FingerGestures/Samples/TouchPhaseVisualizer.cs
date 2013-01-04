using UnityEngine;
using System.Collections;

public class TouchPhaseVisualizer : MonoBehaviour
{

    public Rect rectLabel = new Rect( 50, 50, 200, 200 );

    bool touchDown = false;
    TouchPhase phase = TouchPhase.Canceled;

    public TouchPhase Phase
    {
        get { return phase; }
        set
        {
            if( phase != value )
            {
                Debug.Log( "Phase transition: " + phase + " -> " + value );
                phase = value;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        touchDown = Input.touchCount > 0;

        if( touchDown )
        {
            Phase = Input.touches[0].phase;
        }
    }

    void OnGUI()
    {
        if( touchDown )
            GUI.Label( rectLabel, Phase.ToString() );
        else
            GUI.Label( rectLabel, "N/A" );
    }
}
