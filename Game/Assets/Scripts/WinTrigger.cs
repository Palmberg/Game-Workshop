using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour {

	AmbientSoundsAndMusic soundScript;

	void Start () {
		soundScript = transform.parent.Find("AmbientSoundsAndMusic").GetComponent<AmbientSoundsAndMusic>();
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player")
			soundScript.PlayWin();
	}
}
