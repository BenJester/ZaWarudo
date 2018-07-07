using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour {

	public int currentWorld = 1;
	private GameObject world1;
	private GameObject world2;

	void Start () {
		currentWorld = 1;
		world1 = GameObject.Find ("World1");
		world2 = GameObject.Find ("World2");
		if (currentWorld == 1){
			HideWorld (world2);
			ShowWorld (world1);

		} else {
			HideWorld (world1);
			ShowWorld (world2);
		}
	}

	void Update () {
		if (Input.GetMouseButtonDown(0)){

			RaycastHit2D hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
			if (hit && hit.collider.gameObject.GetComponent<Thing>() != null && hit.collider.gameObject.GetComponent<Thing>().displayed){
				hit.collider.gameObject.GetComponent<Thing> ().independent = !hit.collider.gameObject.GetComponent<Thing> ().independent;
				RefreshWorld ();
			}
		}

		if (Input.GetMouseButtonDown (1)) {
			SwitchWorld ();
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}

	void RefreshWorld() {
		if (currentWorld == 1){
			HideWorld (world2);
			ShowWorld (world1);

		} else {

			HideWorld (world1);
			ShowWorld (world2);
		}
	}
	void SwitchWorld() {
		if (currentWorld == 1){
			
			currentWorld = 2;
			HideWorld (world1);
			ShowWorld (world2);

		} else {
			
			currentWorld = 1;
			HideWorld (world2);
			ShowWorld (world1);
		}
	}

	void HideWorld (GameObject world) {
		for (int i = 0; i < world.transform.childCount; i ++) {
			GameObject child = world.transform.GetChild (i).gameObject;
			if (child.GetComponent<Thing>() != null && !child.GetComponent<Thing>().independent) {
				Hide (child);
			}
		}
	}

	void ShowWorld (GameObject world) {
		for (int i = 0; i < world.transform.childCount; i ++) {
			GameObject child = world.transform.GetChild (i).gameObject;
			if (child.GetComponent<Thing>() != null) {
				Show (child);
			}
		}
	}

	void Hide(GameObject thing) {
		// sprite
		thing.GetComponent<Thing>().displayed = false;
		if (thing.GetComponent<SpriteRenderer>() != null) {
			thing.GetComponent<SpriteRenderer> ().enabled = false;
		}
		// box collider
		if (thing.GetComponent<Collider2D>() != null) {
			thing.GetComponent<Collider2D> ().enabled = false;
		}
			
	}

	void Show(GameObject thing) {
		thing.GetComponent<Thing> ().displayed = true;

		// sprite
		if (thing.GetComponent<SpriteRenderer>() != null) {
			thing.GetComponent<SpriteRenderer> ().enabled = true;
		}
		// box collider
		if (thing.GetComponent<Collider2D>() != null) {
			thing.GetComponent<Collider2D> ().enabled = true;
		}
	}
}
