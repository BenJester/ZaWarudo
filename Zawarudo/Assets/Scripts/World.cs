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
	AudioSource audio;
	public AudioClip switchSound;
	public AudioClip lockSound;
	public AudioClip unlockSound;
	public int independentCount = 0;

	public float targetIndex;
	public float currentIndex;
	public float switchDuration;

	public GameObject mask1;
	public GameObject mask2;

	public int currentWorld = 1;
	private GameObject world1;
	private GameObject world2;

	void Start () {
		audio = GetComponent<AudioSource> ();
		if (World._instance == null) {
			World._instance = this;
		} else {
			Debug.LogError ("cannot have two worlds");
		}

		world1 = GameObject.Find ("World1");
		world2 = GameObject.Find ("World2");
		currentWorld = 1;
		/*
		if (currentWorld == 1){
			HideWorld (world2);
			ShowWorld (world1);

		} else {
			HideWorld (world1);
			ShowWorld (world2);
		}
		*/
		RightClick ();
		RightClick ();

	}
	public IEnumerator PlayScaleAnimation(Thing thing) {
		if (thing.gameObject.GetComponent<OneTimeTranslation> () != null) {
			thing.gameObject.GetComponent<OneTimeTranslation> ().continuing = true;
			yield return new WaitForSeconds (0.26f);
			thing.gameObject.GetComponent<OneTimeTranslation> ().reinitialize ();
		}

		yield return null;
	}
	void LeftClick() {
		RaycastHit2D hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector3.forward, Mathf.Infinity);

		if (hit && hit.collider.gameObject.GetComponent<Thing>() != null && hit.collider.gameObject.GetComponent<Thing>().displayed){
			var thing = hit.collider.gameObject.GetComponent<Thing> ();
			if (thing.independent) {
				audio.PlayOneShot (unlockSound);
				independentCount --;
				StartCoroutine (PlayScaleAnimation (thing));

			} else if (independentCount < 2){
				audio.PlayOneShot (lockSound);
				StartCoroutine (PlayScaleAnimation (thing));
				independentCount ++;
			}  else if (independentCount >= 2) {
				return;
			}
			thing.independent = !thing.independent;

			//RefreshWorld ();
		}
	}

	public void RightClick () {
		if (currentWorld == 1) {
			targetIndex = 2;
		} else if (currentWorld == 2) {
			targetIndex = 1;
		}
		audio.PlayOneShot (switchSound);

		SwitchWorld ();
	}

	void Update () {
		if (Input.GetMouseButtonDown(0)){
			LeftClick ();
		}

		if (Input.GetMouseButtonDown (1)) {
			RightClick ();
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
			StartCoroutine (Restart ());
		}
	}

	public void R() {
		StartCoroutine (Restart ());
	}

	public IEnumerator Restart() {
		if (currentWorld == 1) {
			RightClick ();
			yield return new WaitForSeconds (switchDuration);
		}
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);

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
