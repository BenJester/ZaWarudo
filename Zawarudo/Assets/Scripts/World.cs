using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour {
	
	private static World _instance;
	public static World Instance { 
		get { 	
			return _instance; 
		}
	}
	public float targetIndex;
	public float currentIndex;
	public float switchDuration;

	public GameObject mask1;
	public GameObject mask2;

	public int currentWorld = 1;
	private GameObject world1;
	private GameObject world2;

	void Start () {
		if (World._instance == null) {
			World._instance = this;
		} else {
			Debug.LogError ("cannot have two worlds");
		}

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
				var thing = hit.collider.gameObject.GetComponent<Thing> ();
				var sprite = hit.collider.gameObject.GetComponent<SpriteRenderer> ();
				thing.independent = !thing.independent;

				if (thing.independent) {
					sprite.maskInteraction = SpriteMaskInteraction.None;
					sprite.sprite = thing.marked;
				}
				else {
					sprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
					sprite.sprite = thing.unmarked;
				}

				//RefreshWorld ();
			}
		}

		if (Input.GetMouseButtonDown (1)) {
			if (currentWorld == 1) {
				targetIndex = 2;
			} else if (currentWorld == 2) {
				targetIndex = 1;
			}
			SwitchWorld ();
		}

		if (currentIndex < targetIndex) {
			//currentIndex = Mathf.Clamp(Mathf.Lerp (targetIndex == 1 ? 2 : 1, targetIndex, easeInOutQuad(currentIndex - 1f)), 1f, 2f);
			currentIndex = Mathf.Clamp(currentIndex + (Time.deltaTime / switchDuration), 1f, 2f);
		} else if (currentIndex > targetIndex) {
			//currentIndex = Mathf.Clamp(Mathf.Lerp (targetIndex == 1 ? 2 : 1, targetIndex, easeInOutQuad(currentIndex - 1f)), 1f, 2f);

			currentIndex = Mathf.Clamp(currentIndex - (Time.deltaTime / switchDuration), 1f, 2f);
		}
		Vector3 newMaskPos = Camera.main.ViewportToWorldPoint (new Vector3 (currentIndex - 1f, 0.5f, 0f));
		mask1.transform.position = new Vector3 (newMaskPos.x, newMaskPos.y, 2f) + Vector3.left * mask1.GetComponent<SpriteMask>().bounds.size.x / 2f; 
		mask2.transform.position = new Vector3 (newMaskPos.x, newMaskPos.y, 2f) + Vector3.right * mask2.GetComponent<SpriteMask>().bounds.size.x / 2f; 

		HandleCollider ();

		if (Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}

	void HandleCollider() {
		for (int i = 0; i < world2.transform.childCount; i ++) {
			GameObject child = world2.transform.GetChild (i).gameObject;
			if (child.GetComponent<Thing> () != null) {
				float x = child.GetComponent<Thing> ().originalX;

				var box = child.GetComponent<BoxCollider2D> ();
				if (!child.GetComponent<Thing>().independent) {
					if (box != null) {
						float lerp = child.GetComponent<Thing>().lerp;
						box.size = new Vector2((1 - lerp) * x, box.size.y);
						box.offset = new Vector2(lerp * x / 2f, 0f);
					}
				} else {
					box.size = new Vector2 (x, box.size.y);
					box.offset = Vector2.zero;
				}
			}

		}
		for (int i = 0; i < world1.transform.childCount; i ++) {
			GameObject child = world1.transform.GetChild (i).gameObject;
			if (child.GetComponent<Thing> () != null) {
				var box = child.GetComponent<BoxCollider2D> ();
				float x = child.GetComponent<Thing> ().originalX;

				if (!child.GetComponent<Thing>().independent) {
					if (box != null) {
						float lerp = child.GetComponent<Thing>().lerp;

						box.size = new Vector2(lerp * x, box.size.y);
						box.offset = new Vector2(- (1 - lerp) * x / 2f, 0f);
					}
				} else {
					box.size = new Vector2 (x, box.size.y);
					box.offset = Vector2.zero;
				}
			}
			}
	}

	float CalculateSize (GameObject g) {
		// return 0 to 1 based on currentIndex, pos and size

		float left = Camera.main.WorldToViewportPoint(g.transform.position - new Vector3(g.GetComponent<SpriteRenderer>().size.x, g.GetComponent<SpriteRenderer>().size.y,0f)).x;
		float right = Camera.main.WorldToViewportPoint(g.transform.position + new Vector3(g.GetComponent<SpriteRenderer>().size.x, g.GetComponent<SpriteRenderer>().size.y, 0f)).x;
		//Debug.Log (Mathf.Clamp01 ((currentIndex -1f - left) / (right - left)));
		return Mathf.Clamp01 ((currentIndex - 1f - left) / (right - left));
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

		// check for overlapping

		if (currentWorld == 1){
			currentWorld = 2;
			//HideWorld (world1);
			ShowWorld (world2);

		} else {
			
			currentWorld = 1;
			//HideWorld (world2);
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

	public float easeInOutQuad(float t)
	{
		return t < .5 ? 2 * t * t : -1 + (4 - 2 * t) * t;
	}
}
