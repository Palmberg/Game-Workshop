using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspenseTrigger : MonoBehaviour {

	AmbientSoundsAndMusic musicScript;

	void Start () {
		musicScript = transform.parent.Find("AmbientSoundsAndMusic").GetComponent<AmbientSoundsAndMusic>();
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player")
			musicScript.PlaySuspense();
	}
}
