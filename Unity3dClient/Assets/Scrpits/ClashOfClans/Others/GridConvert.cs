using UnityEngine;
using System.Collections;

public class GridConvert {

	public static float		fWiresWidth = 4.0f;
	public static float		fAngle = Mathf.PI * 45.0f / 180.0f;
	public static int 		iMaxWiresWidth = 20;
	public static float		fOffset = fWiresWidth * Mathf.Cos(fAngle);
	
	public static Vector3 Convert2PutLocation (Vector2 gridPos, float y){
		
		Vector3 v3 = Vector3.zero;		
		
		if ( Mathf.Abs(gridPos.x) <= iMaxWiresWidth && Mathf.Abs(gridPos.y) <= iMaxWiresWidth) {
			float ux = gridPos.x * fWiresWidth * Mathf.Cos(fAngle) - gridPos.y * fWiresWidth * Mathf.Sin(fAngle);
			float uz = gridPos.x * fWiresWidth * Mathf.Cos(fAngle) + gridPos.y * fWiresWidth * Mathf.Sin(fAngle);
			uz = uz - fOffset;
			// uz = uz > 0 ? uz - fOffset : uz + fOffset;
			v3 = new Vector3( ux , y, uz  );
		}
		else{
			v3 = new Vector3( gridPos.x , y, gridPos.y  );
		}
		return v3;
	}
}
