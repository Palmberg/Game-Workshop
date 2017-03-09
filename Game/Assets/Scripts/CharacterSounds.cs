using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(AudioSource))]
	[RequireComponent(typeof(ThirdPersonCharacter))]
    public class CharacterSounds : MonoBehaviour
    {
        public AudioSource audioSrc;
        public ThirdPersonCharacter character;
        public AudioClip[] concrete;
        public AudioClip[] wood;
        public AudioClip[] dirt;
        public AudioClip[] metal;

        private bool step = true;
		float audioStepLength;
        const float origAudioStepLengthWalk = 0.3f;
        const float origAudioStepLengthRun = 0.25f;

		void Start() {
			audioSrc = gameObject.AddComponent<AudioSource>();
		}

		void PlayWalkSound() {
            float audioStepLengthWalk = origAudioStepLengthWalk/(character.MoveSpeedMultiplier*character.AnimSpeedMultiplier);
			RaycastHit hit;
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hit, character.GroundCheckDistance))
			{
				float forward = character.Animator.GetFloat("Forward");
				bool isWalking = (forward > 0.15f && forward < 0.75f) ? true : false;
				bool isRunning = forward >= 0.75f ? true : false;

				GameObject hitObj = hit.transform.gameObject;
				Vector3 charVelocity = character.Rigidbody.velocity;

				if (character.IsGrounded && step)
				{
                    if(hitObj.name.Contains("Ground"))
					    PlayConcreteSound(0.1f, audioStepLengthWalk);
                    else if (hitObj.name.Contains("Ramp"))
					    PlayWoodSound(0.1f, audioStepLengthWalk);
                    else if (hitObj.tag == "Dirt")
                        PlayDirtSound(0.1f, audioStepLengthWalk);
                    else if (hitObj.name.Contains("Container"))
                        PlayMetalSound(0.1f, audioStepLengthWalk);
                }
			}
		}

        void PlayRunSound() {
            float audioStepLengthRun = origAudioStepLengthRun/(character.MoveSpeedMultiplier*character.AnimSpeedMultiplier);
			RaycastHit hit;
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hit, character.GroundCheckDistance))
			{
                GameObject hitObj = hit.transform.gameObject;
				Vector3 charVelocity = character.Rigidbody.velocity;

                if (character.IsGrounded && step)
				{
                    if(hitObj.name.Contains("Ground"))
					    PlayConcreteSound(0.3f, audioStepLengthRun);
                    else if (hitObj.name.Contains("Ramp"))
					    PlayWoodSound(0.3f, audioStepLengthRun);
                    else if (hitObj.tag == "Dirt")
                        PlayDirtSound(0.3f, audioStepLengthRun);
                    else if (hitObj.name.Contains("Container"))
                        PlayMetalSound(0.3f, audioStepLengthRun);
                }
            }
        }

		void OnCollisionEnter(Collision collision) {
			if (collision.relativeVelocity.y > 3f) {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hit, character.GroundCheckDistance))
                {
                    GameObject hitObj = hit.transform.gameObject;
                    float volume = collision.relativeVelocity.y/12f;

                    if(hitObj.name.Contains("Ground"))
					    PlayConcreteSound(volume, 0.05f);
                    else if (hitObj.name.Contains("Ramp"))
					    PlayWoodSound(volume, 0.05f);
                    else if (hitObj.tag == "Dirt")
                        PlayDirtSound(volume, 0.05f);
                    else if (hitObj.name.Contains("Container"))
                        PlayMetalSound(volume, 0.05f);
                }
			}
			
		}

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
			
        }

        IEnumerator WaitForFootSteps(float stepsLength)
        {
            step = false;
            yield return new WaitForSeconds(stepsLength);
            step = true;
        }

        // CONCRETE
        void PlayConcreteSound(float volume, float stepLength)
        {
            AudioClip clip = concrete[Random.Range(0, concrete.Length)];
            audioSrc.pitch = Random.Range(0.9f, 1.1f);
            volume = Random.Range(volume, volume+0.05f);
            audioSrc.PlayOneShot(clip, volume);
            StartCoroutine(WaitForFootSteps(stepLength));
        }

        // WOOD
        void PlayWoodSound(float volume, float stepLength)
        {
            AudioClip clip = wood[Random.Range(0, concrete.Length)];
            audioSrc.PlayOneShot(clip, volume);
            audioSrc.pitch = Random.Range(0.9f, 1.1f);
            volume = Random.Range(volume, volume+0.05f);
            StartCoroutine(WaitForFootSteps(stepLength));
        }

        // DIRT 
        void PlayDirtSound(float volume, float stepLength)
        {
            AudioClip clip = dirt[Random.Range(0, concrete.Length)];
            audioSrc.PlayOneShot(clip, volume);
            audioSrc.pitch = Random.Range(0.9f, 1.1f);
            volume = Random.Range(volume, volume+0.05f);
            StartCoroutine(WaitForFootSteps(stepLength));
        }

        // METAL
        void PlayMetalSound(float volume, float stepLength)
        {
            AudioClip clip = metal[Random.Range(0, concrete.Length)];
            audioSrc.PlayOneShot(clip, volume);
            audioSrc.pitch = Random.Range(0.9f, 1f);
            volume = Random.Range(volume, volume+0.05f);
            StartCoroutine(WaitForFootSteps(stepLength));
        }

    }
}