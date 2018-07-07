using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Translation : MonoBehaviour {

	public string nextLevel;
	// Update is called once per frame
	void Update () {
		transform.position += Vector3.right * 0.01f;
		if (Input.GetMouseButtonDown (1))
			StartCoroutine (LoadAsyncScene());
	}
		
	IEnumerator LoadAsyncScene()
	{
		if(nextLevel == "")
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
			//Wait until the last operation fully loads to return anything
			while (!asyncLoad.isDone)
			{
				yield return null;
			}
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
