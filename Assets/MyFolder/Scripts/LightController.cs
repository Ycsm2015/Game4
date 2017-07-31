using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {
	public Transform SunLightTrasform;
	public Transform MoonLightTrasform;

	public float yRot = 30f;
	//private float initialYRot = ;
	// Use this for initialization
	void Start () {
//		GameObject GameManager = GameObject.Find ("GameManager");
	}
	
	// Update is called once per frame
	void Update () {
		SunLightTrasform.Rotate (new Vector3 (0,yRot, 0) * Time.deltaTime,Space.Self);
	}

	public void applyLightSpeed(float DayTime){
		yRot = 360 / DayTime;
	}
}
