using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadMainLevel : MonoBehaviour 
{
	private bool levelLoaded = false;
	
	
	void Update() 
	{
		KinectManager manager = KinectManager.Instance;
		
		if(!levelLoaded && manager && KinectManager.IsKinectInitialized())
		{
			levelLoaded = true;
			SceneManager.LoadScene(4);
		}
	}
	
}