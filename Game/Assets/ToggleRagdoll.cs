using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ToggleRagdoll : MonoBehaviour {
		
		//Skeleton variables
		private Collider[] rigColliders;
		private Rigidbody[] rigRigidbodies;
		
		//Variable to keep track of reset position
		private Vector3 restartPos;
		
		//Player variables
		private Animator animator;
		private Rigidbody playerRigid;
		private Collider playerCollider;
		private Transform playerTransform;
		private ThirdPersonCharacter thirdPersonCharacterScript;
		private ThirdPersonUserControl thirdPersonUserControlScript;
		
		//Public GameObject reference to the player object
		public GameObject player;
		
		//Bool to check if ragdoll mode is enabled
		bool ragdollEnabled = false;
		
		//Bool to check if user has let go of button
		bool buttonPressed = false;
		
		void Start () {
			
			//Find all colliders and rigidbodies in the skeleton rig on start
			rigColliders = GetComponentsInChildren<Collider>();
			rigRigidbodies = GetComponentsInChildren<Rigidbody>();

			//Find the transform, animator, rigidbody and collider of the player
			playerTransform = player.transform;
			animator = player.GetComponent<Animator>();
			playerRigid = player.GetComponent<Rigidbody>();
			playerCollider = player.GetComponent<Collider>();
			
			//Find the controller scripts
			thirdPersonCharacterScript = player.GetComponent<ThirdPersonCharacter>();
			thirdPersonUserControlScript = player.GetComponent<ThirdPersonUserControl>();
			
			//Save the initial position of the player
			checkPoint();
		}
		
		public void checkPoint(){

			//Save the current position to use when resetting the position
			restartPos = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z);
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
					
					//Write something here
				}
				else {
					
					//Write something here
				}
			}
			
			//If user has let go of the T button
			if (input == false){
				
				buttonPressed = false;
			}
		}
		
		public void RagdollON () {
			
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
			playerCollider.enabled = false;
			thirdPersonCharacterScript.enabled = false;
			thirdPersonUserControlScript.enabled = false;

			//Remember that ragdoll mode is enabled.
			ragdollEnabled = true;
			
			//Update points
			//GameObject.Find("gameManager").SendMessage("addScore", -50);
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
			playerCollider.enabled = true;
			thirdPersonCharacterScript.enabled = true;
			thirdPersonUserControlScript.enabled = true;
			
			//Reset player position
			playerTransform.position = restartPos;
			
			//Remember that ragdoll mode is disabled.
			ragdollEnabled = false;
		}
	}
}
