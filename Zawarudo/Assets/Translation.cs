using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Translation : MonoBehaviour {

	public string nextLevel;
	// Update is called once per frame
	void Update () {
		transform.position += Vector3.right * 0.02f;
		if (Input.GetMouseButtonDown (1) && nextLevel != null)
			StartCoroutine (LoadAsyncScene());
	}
		
	IEnumerator LoadAsyncScene()
	{
		if(nextLevel == "")
		{
			yield return null;
		}
		else
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextLevel);
			//Wait until the last operation fully loads to return anything
			while (!asyncLoad.isDone)
			{
				yield return null;
			}
		}

	}
}
