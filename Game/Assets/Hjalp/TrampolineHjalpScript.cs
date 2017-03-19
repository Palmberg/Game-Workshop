using UnityEngine;
using System.Collections;

public class TrampolineHjalpScript : MonoBehaviour {
	
	
	
    void OnTriggerEnter(Collider other) {
		
		if(other.attachedRigidbody == true){
			
			//Create a new vector with space for 3 float values
			Vector3 vel = new Vector3();
			
			//Set the values of the new vector so that it has the same
			//xy direction as the velocity vector of the object that has
			//hit the colider, but set the upwards value to 15
			vel.x = other.attachedRigidbody.velocity.x * 3f;
			vel.y = 15f;
			vel.z = other.attachedRigidbody.velocity.z * 3f;
			
			//Set the velocity of the object that hit the collider
			//to the value that we calculated above
			other.attachedRigidbody.velocity = vel;
			
		}
    }
}