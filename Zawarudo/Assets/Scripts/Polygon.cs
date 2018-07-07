using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon : MonoBehaviour {

	void Start () {
		if (GetComponent<PolygonCollider2D> () == null) {
			var poly = gameObject.AddComponent<PolygonCollider2D> ();
			poly.sharedMaterial = (PhysicsMaterial2D)Resources.Load("Materials/WallPhy");
		}
	}
	

}
