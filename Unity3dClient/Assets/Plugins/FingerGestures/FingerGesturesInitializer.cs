using UnityEngine;
using System.Collections;

/// <summary>
/// This class is used to create and initialise the proper FigureGesture implementation based on the current platform the application is being run on
/// </summary>
public class FingerGesturesInitializer : MonoBehaviour 
{
	public FingerGestures editorGestures;
	public FingerGestures desktopGestures;
	public FingerGestures iosGestures;
	public FingerGestures androidGestures;

	// whether to make the FingerGesture persist through scene loads
	public bool makePersistent = true;

	void Awake() 
	{
		if( !FingerGestures.Instance )
		{
			FingerGestures prefab;

			if( Application.isEditor )
			{
				prefab = editorGestures;
			}
			else
			{
#if UNITY_IPHONE
				prefab = iosGestures;
#elif UNITY_ANDROID
				prefab = androidGestures;
#else
				prefab = desktopGestures;
#endif
			}

			Debug.Log( "Creating FingerGestures using " + prefab.name );
			FingerGestures instance = Instantiate( prefab ) as FingerGestures;
			instance.name = prefab.name;
			
			if( makePersistent )
				DontDestroyOnLoad( instance.gameObject );
		}

		Destroy( this.gameObject );
	}
}
