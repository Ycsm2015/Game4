using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSetting : MonoBehaviour {
	public float TimeTravelSpeed;
	public float currentDayTime;

	public float SecPerDay = 60f;

	//private float currentTime = 0f;

	private LightController lightController;
	// Use this for initialization
	void Start () {
		lightController = GameObject.Find ("Light").GetComponent<LightController> ();
		lightController.applyLightSpeed (SecPerDay);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void confirmChange(){
		lightController.applyLightSpeed (SecPerDay);
	}
}
