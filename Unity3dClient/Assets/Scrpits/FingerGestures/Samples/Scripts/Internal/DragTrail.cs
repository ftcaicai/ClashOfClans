using UnityEngine;
using System.Collections;

public class DragTrail : MonoBehaviour 
{
	public LineRenderer lineRendererPrefab;
	LineRenderer lineRenderer;

	// Use this for initialization
	void Start()
	{
		lineRenderer = Instantiate( lineRendererPrefab, transform.position, transform.rotation ) as LineRenderer;
		lineRenderer.transform.parent = this.transform;
		lineRenderer.enabled = false;
	}

	// call triggered by the Draggable script
    void OnDragBegin()
	{
		// initialize the line renderer
		lineRenderer.enabled = true;
		lineRenderer.SetPosition( 0, transform.position );
		lineRenderer.SetPosition( 1, transform.position );

		// keep end point width in sync with object's current scale
		lineRenderer.SetWidth( 0.01f, transform.localScale.x );
	}

    // call triggered by the Draggable script
    void OnDragEnd()
	{
		lineRenderer.enabled = false;
	}

	void Update()
	{
		if( lineRenderer.enabled )
		{
			// update position of the line's end point
			lineRenderer.SetPosition( 1, transform.position );
		}
	}
}
