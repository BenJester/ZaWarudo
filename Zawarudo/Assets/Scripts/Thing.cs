using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing : MonoBehaviour {
	public bool independent;
	public bool displayed;

	public float lerp;

	public Sprite unmarked;
	public Sprite marked;

	public float originalX;

	void Start() {
		if (GetComponent<BoxCollider2D> () != null)
			originalX = GetComponent<BoxCollider2D> ().size.x;
	}

	void Update() {
		float left = Camera.main.WorldToViewportPoint(transform.position - new Vector3(GetComponent<SpriteRenderer>().size.x, GetComponent<SpriteRenderer>().size.y,0f)).x;
		float right = Camera.main.WorldToViewportPoint(transform.position + new Vector3(GetComponent<SpriteRenderer>().size.x, GetComponent<SpriteRenderer>().size.y, 0f)).x;
		//Debug.Log (Mathf.Clamp01 ((currentIndex -1f - left) / (right - left)));
		lerp = Mathf.Clamp01 ((World.Instance.currentIndex - 1f - left) / (right - left));
	}
}
