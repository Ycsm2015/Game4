using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour {

	public  Slider healthBar;
	public  Slider moraleBar;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		healthBar.transform.LookAt (Camera.main.transform);
		moraleBar.transform.LookAt (Camera.main.transform);
	}

	public void setHealth(float health){
		healthBar.value = health;
	}

	public void setMorale(float morale){
		moraleBar.value = morale;
	}
}
