using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dir {
	right = 1,
	left = -1,
	up = 2,
	down = -2,
	none = 0
}
public class MovingBlock : MonoBehaviour {
	
	public Dir direction;
	public float speed;

	Rigidbody2D body;

	void Start () {
		body = GetComponent<Rigidbody2D> ();
	}
	
	void Update () {
		if (direction == Dir.left) {
			body.velocity = new Vector2 (-speed, 0f);
			body.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
		} else if (direction == Dir.right) {
			body.velocity = new Vector2 (speed, 0f);
			body.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

		} else if (direction == Dir.up) {
			body.velocity = new Vector2 (0, speed);
			body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

		} else if (direction == Dir.down) {
			body.velocity = new Vector2 (0, -speed);
			body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

		} else if (direction == Dir.none) {
			body.velocity = Vector2.zero;
		}
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		if (collision.gameObject.tag == "Wall")
			direction = (Dir) (- (int) direction);
	}
}
