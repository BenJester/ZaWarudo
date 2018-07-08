using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeStamp {
	public float timeStamp;
	public Vector3 location;
	public Vector3 scale;

	public OneTimeStamp(float t, Vector3 l, Vector3 s)
	{
		timeStamp = t;
		location = l;
		scale = s;
	}
}



public class OneTimeTranslation : MonoBehaviour {
	public bool linear = false;
	public int keyFrameNum = 4;
	public Vector3[] posArr = new Vector3[4];
	public Vector3[] scaleArr = new Vector3[4];
	public float[] keyFrameTimeArr = new float[4];
	public bool continuing = true;

	public List<OneTimeStamp> posDict;

	private float lerpTime;
	private float currentLerpTime;
	private float perc = 0f;
	private Vector3 initialPos;
	private Vector3 startPos;
	private Vector3 endPos;
	private Vector3 startScale;
	private Vector3 initialScale;
	private Vector3 endScale;
	private int index = 0;


	// Use this for initialization
	void OnEnable () {
		posDict = new List<OneTimeStamp>();
		for (int i = 0; i < keyFrameNum; i++)
		{

			OneTimeStamp t = new OneTimeStamp(keyFrameTimeArr[i], posArr[i], scaleArr[i]);
			posDict.Add(t);
		}
		initialPos = transform.position;
		startPos = transform.position;
		endPos = transform.position + posDict[0].location;
		startScale = transform.localScale;
		initialScale = transform.localScale;
		endScale = posDict[0].scale;
		lerpTime = posDict[0].timeStamp;
	}

	public void reinitialize ()
	{
		continuing = true;
		transform.localScale = initialScale;
		//transform.position = initialPos;
	}

	// Update is called once per frame
	void Update () {
		//Set to a new translation when finished
		if (continuing) {
			if ((index) == posDict.Count) {
				continuing = false;
				index = 0;
				return;
			}
			if (perc == 1f) {
				currentLerpTime = 0f;
				index += 1;

				if ((index) == posDict.Count) {
					continuing = false;
					index = 0;
					return;
				}
				startScale = transform.localScale;
				endScale = posDict [index].scale;
				startPos = transform.position;
				endPos = transform.position + posDict [index].location;
				lerpTime = posDict [index].timeStamp;
			} 


			//increment timer once per frame, calculate percentage
			currentLerpTime += Time.deltaTime;
			if (currentLerpTime > lerpTime) {
				currentLerpTime = lerpTime;
			}
			perc = currentLerpTime / lerpTime;

			//Actually move the object
			if (!linear) {
				transform.position = Vector3.Lerp (startPos, endPos, easeInOutQuad (perc));
				transform.localScale = Vector3.Lerp (startScale, endScale, easeInOutQuad (perc));
			} else {
				
			}

		}
	}
	public float easeInOutQuad(float t)
	{
		return t < .5 ? 2 * t * t : -1 + (4 - 2 * t) * t;
	}
}

