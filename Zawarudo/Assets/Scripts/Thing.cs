using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing : MonoBehaviour {
	public bool independent;
	public bool displayed;

	public float lerp;

	public Sprite unmarked;
	public Sprite marked;
	public GameObject whiteMark;
	public GameObject blackMark;
	public GameObject mark;
	public SpriteRenderer sprite;
	public float originalX;

	void Start() {
		whiteMark = GameObject.Find ("White Mark");
		blackMark = GameObject.Find ("Black Mark");
		sprite = GetComponent<SpriteRenderer> ();
		if (transform.parent.name == "World1") {
			mark = Instantiate (whiteMark);
			mark.transform.SetParent (transform);
			mark.transform.localPosition = Vector3.back;
		} else {
			mark = Instantiate (blackMark);
			mark.transform.SetParent (transform);
			mark.transform.localPosition = Vector3.back;
		}
		if (GetComponent<BoxCollider2D> () != null) {
			originalX = GetComponent<SpriteRenderer> ().size.x;
			GetComponent<BoxCollider2D> ().size = GetComponent<SpriteRenderer> ().size;
		}
	}

	void Update() {
		float left = Camera.main.WorldToViewportPoint(transform.position - new Vector3(GetComponent<SpriteRenderer>().size.x, GetComponent<SpriteRenderer>().size.y,0f)).x;
		float right = Camera.main.WorldToViewportPoint(transform.position + new Vector3(GetComponent<SpriteRenderer>().size.x, GetComponent<SpriteRenderer>().size.y, 0f)).x;
		//Debug.Log (Mathf.Clamp01 ((currentIndex -1f - left) / (right - left)));
		lerp = Mathf.Clamp01 ((World.Instance.currentIndex - 1f - left) / (right - left));


		if (independent) {
			sprite.maskInteraction = SpriteMaskInteraction.None;
			mark.SetActive (true);
		}
		else {
			sprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
			mark.SetActive (false);

		}
	}
}
