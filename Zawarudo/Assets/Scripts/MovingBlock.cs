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
	public Thing thing;
	Rigidbody2D body;
	Vector3 lastPos;

	void Start () {
		thing = GetComponent<Thing> ();
		body = GetComponent<Rigidbody2D> ();
	}
	
	void Update () {
		lastPos = transform.position;

		if ((thing.lerp > 0 && transform.parent.name == "World1") || (thing.lerp < 1 && transform.parent.name == "World2") || thing.independent) {
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
		} else {
			body.velocity = Vector2.zero;
		}
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		Debug.Log ("las " + lastPos);
		Debug.Log ("this " + transform.position);
		Debug.Log ("~~~");
		if (collision.gameObject.tag == "Wall")
			direction = (Dir) (- (int) direction);
		//else if ((transform.position - lastPos).magnitude < 0.01f) {
		//	direction = (Dir) (- (int) direction);
		//}
	}
}
