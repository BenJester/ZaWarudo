using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	Rigidbody2D body2d;
	public LayerMask groundLayer;
	public bool faceRight;
	SpriteRenderer bodySprite;
	public float moveSpeed;
	float acc = 1f;
	private ContactFilter2D contactfilter;
	AudioSource audio;
	public AudioClip jumpSound;
	bool wasGrounded;
	Animator animator;
	bool dead;

	void Start () {
		animator = GetComponent<Animator> ();
		audio = GetComponent<AudioSource> ();
		bodySprite = GetComponent<SpriteRenderer> ();
		body2d = GetComponent<Rigidbody2D> ();
		contactfilter.SetLayerMask (groundLayer);
		acc = 1f;
	}

	void Update () {
		if (transform.position.y < -25f && !dead) {
			StartCoroutine(World.Instance.Restart ());
			dead = true;
		}
		if (Input.GetKeyDown ("w") && IsGrounded ()) {
			//body2d.AddForce(new Vector2(0, 4900), ForceMode2D.Impulse);
			body2d.velocity = new Vector2(body2d.velocity.x, 22f);
			//audioSource.PlayOneShot (jumpSound);
			//StartCoroutine (jumpCoolDown (0.15f));
			audio.PlayOneShot(jumpSound);
		}

		if (Input.GetKeyDown ("d")) {
			faceRight = true;
			//bodySprite.flipX = false;
			accelerate ();
		}

		if (Input.GetKeyDown ("a")) {
			faceRight = false;
			//bodySprite.flipX = true;
			accelerate ();

		}

		if (Input.GetKeyUp ("d")) {
			if (Input.GetKey ("a")) {
				faceRight = false;
				//bodySprite.flipX = true;
				accelerate ();
			} else {
				decelerate ();
			}
		}

		if (Input.GetKeyUp ("a")) {
			if (Input.GetKey ("d")) {
				faceRight = true;
				//bodySprite.flipX = false;
				accelerate ();
				//body2d.velocity = new Vector2 (moveSpeed, body2d.velocity.y);
			} else {
				decelerate ();
			}
		}

		if (Input.GetKey ("a") && !Input.GetKey ("d")) {
			faceRight = false;
			//bodySprite.flipX = true;
			accelerate ();
			//body2d.velocity = new Vector2 (-moveSpeed, body2d.velocity.y);
		}

		if (Input.GetKey ("d") && !Input.GetKey ("a")) {
			faceRight = true;
			//bodySprite.flipX = false;
			accelerate ();
			//body2d.velocity = new Vector2 (moveSpeed, body2d.velocity.y);
		}
		wasGrounded = IsGrounded ();

	}
	void accelerate() {
		if (IsGrounded() || (!body2d.IsTouching(contactfilter)))
		{
			if (faceRight)
			{
				//body2d.velocity = new Vector2(moveSpeed, body2d.velocity.y);
				body2d.velocity = new Vector2 (body2d.velocity.x + acc > moveSpeed ? moveSpeed : body2d.velocity.x + acc, body2d.velocity.y);
			}
			else {
				//body2d.velocity = new Vector2(-moveSpeed, body2d.velocity.y);
				body2d.velocity = new Vector2 (body2d.velocity.x - acc < -moveSpeed ? -moveSpeed : body2d.velocity.x - acc, body2d.velocity.y);
			}
		}

	}

	void decelerate() {
		//if (!noGravity)
		body2d.velocity = new Vector2 (0, body2d.velocity.y);
		//body2d.velocity = new Vector2 (body2d.velocity.x + acc > moveSpeed ? moveSpeed : body2d.velocity.x + acc, body2d.velocity.y);
	}
	public bool IsGrounded() {
		Vector2 position = transform.position;
		Vector2 positionLeft = transform.position + new Vector3(bodySprite.bounds.size.x/2.0f,0,0);
		Vector2 positionRight = transform.position - new Vector3(bodySprite.bounds.size.x/2.0f,0,0);
		Vector2 direction = Vector2.down;
		float distance = 30f;

		RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
		RaycastHit2D hit2 = Physics2D.Raycast(positionLeft, direction, distance, groundLayer);
		RaycastHit2D hit3 = Physics2D.Raycast(positionRight, direction, distance, groundLayer);
		if (hit.collider != null || hit2.collider != null || hit3.collider != null) {
			return body2d.IsTouching (contactfilter);
		}

		return false;
	}
}
