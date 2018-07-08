using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {

	public string nextLevel;
	public bool won;

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.tag == "Ball" && !won) {
			StartCoroutine(LoadAsyncScene());
		}
	}
	IEnumerator LoadAsyncScene()
	{
		won = true;
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
			World.Instance.RightClick ();
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextLevel);
			//Wait until the last operation fully loads to return anything
			while (!asyncLoad.isDone)
			{
				yield return null;
			}
		}

	}
}
