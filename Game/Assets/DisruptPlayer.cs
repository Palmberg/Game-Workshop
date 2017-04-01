using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{

	public class DisruptPlayer : MonoBehaviour {

		private ToggleRagdollHjalp script;
		private Transform skeleton;

		void OnTriggerEnter(Collider other) {

			skeleton = other.gameObject.transform.Find("EthanSkeleton");

			if(skeleton != null){
			
				//Try to access the ragdoll script component of the gameobject that collided.
				script = skeleton.GetComponent<ToggleRagdollHjalp>();

				if(script != null){

					//If there is a ragdoll script, call it's ragdoll function
					script.RagdollON();
					
					//GameObject.Find("gameManager").SendMessage("addScore", -50);

				}
			}
		}
	}
}
