using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{

	[RequireComponent(typeof (ThirdPersonCharacter))]
	[RequireComponent(typeof (ThirdPersonUserControl))]

	public class ToggleRagdollHjalp : MonoBehaviour {
		
		//Skeleton variables
		private Collider[] rigColliders;
		private Rigidbody[] rigRigidbodies;
		
		//Player variables
		private Animator animator;
		private Transform startPos;
		private Rigidbody playerRigid;
		private ThirdPersonCharacter thirdPersonCharacterScript;
		private ThirdPersonUserControl thirdPersonUserControlScript;
		
		public GameObject player;
		
		//Bool to check if ragdoll mode is enabled
		bool ragdollEnabled = false;
		
		//Bool to check if user has let go of button
		bool buttonPressed = false;
		
		void Start () {
			
			//Find all colliders and rigidbodies in the skeleton rig on start
			rigColliders = GetComponentsInChildren<Collider>();
			rigRigidbodies = GetComponentsInChildren<Rigidbody>();

			//Find the animator of the player
			animator = player.GetComponent<Animator>();
			playerRigid = player.GetComponent<Rigidbody>();
			
			//Find the controller scripts
			thirdPersonCharacterScript = player.GetComponent<ThirdPersonCharacter>();
			thirdPersonUserControlScript = player.GetComponent<ThirdPersonUserControl>();
			
			//Save the initial position of the player
			startPos = player.transform;
			
			//RagdollOFF();
		}
		
		void Update () {
			
			//Check if the T key has been presset
			bool input = Input.GetKey(KeyCode.T);
			
			//If user has pressed down T button, and didn't do it the previous frame 
			if(input == true && buttonPressed == false){
				
				//Note that T has been pressed
				buttonPressed = true;
				
				//Check ragdoll status, and enable accordingly
				if(ragdollEnabled == true){
					
					RagdollOFF();
				}
				else {
					
					RagdollON();
				}
			}
			
			//If user has let go of the T button
			if (input == false){
				
				buttonPressed = false;
			}
		}
		
		void RagdollON () {
			
			//Enable all colliders in the skeleton
			foreach (Collider col in rigColliders){
				
				col.enabled = true;
			}

			//Enable all rigidbodies in the skeleton
			foreach (Rigidbody rigid in rigRigidbodies){
				
				rigid.useGravity = true;
			}
			
			//Disable character animation and character movement
			animator.enabled = false;
			playerRigid.velocity = new Vector3(0, 0, 0);
			playerRigid.useGravity = false;
			thirdPersonCharacterScript.enabled = false;
			thirdPersonUserControlScript.enabled = false;

			//Remember that ragdoll mode is enabled.
			ragdollEnabled = true;
		}
		
		void RagdollOFF(){
			
			//Disable all colliders in the skeleton
			foreach (Collider col in rigColliders){
				
				col.enabled = false;
			}
			
			//Disable all rigidbodies in the skeleton
			foreach (Rigidbody rigid in rigRigidbodies){
				
				rigid.useGravity = false;
			}
			
			//Enable character animation and character movement
			animator.enabled = true;
			playerRigid.useGravity = true;
			thirdPersonCharacterScript.enabled = true;
			thirdPersonUserControlScript.enabled = true;
			
			//Reset player position and rotation
			player.transform.position = startPos.position;
			player.transform.rotation = startPos.rotation;
			
			//Remember that ragdoll mode is disabled.
			ragdollEnabled = false;
		}
	}

}
