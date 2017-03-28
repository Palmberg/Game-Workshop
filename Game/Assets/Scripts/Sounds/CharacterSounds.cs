using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(ThirdPersonCharacter))]
    public class CharacterSounds : MonoBehaviour
    {
        AudioSource audioSrc;
        [Header("Steps")]
        public float walkVolume = 0.7f;
        public float runVolume = 1f;
        public float landVolMultiplier = 0.5f;
        public AudioClip[] concrete;
        public AudioClip[] wood;
        public AudioClip[] dirt;
        public AudioClip[] metal;

        AudioSource voiceAudioSrc;
        [Header("Voice")]
        public float voiceVolume = 1f;
        public AudioClip[] jumpSounds;
        public AudioClip fallSound;
        public AudioClip[] bumpSounds;

        ThirdPersonCharacter character;
        bool step = true;
        const float origAudioStepLengthWalk = 0.3f;
        const float origAudioStepLengthRun = 0.25f;

        bool falling = false;
        bool bump = true;

		void Start() {
			audioSrc = gameObject.AddComponent<AudioSource>();
            audioSrc.spatialBlend = 1f; // Make it a 3D source
            voiceAudioSrc = gameObject.AddComponent<AudioSource>();
            voiceAudioSrc.spatialBlend = 1f; // Make it a 3D source
            character = gameObject.GetComponent<ThirdPersonCharacter>();
		}

        void FixedUpdate() {
            if(!falling && character.Rigidbody.velocity.y < -18f) {
                falling = true;
                PlayFallingSound();
            } else if(character.Rigidbody.velocity.y > -6f) {
                falling = false;
            }
        }

		void PlayWalkSound() {
            float stepLength = origAudioStepLengthWalk/(character.MoveSpeedMultiplier*character.AnimSpeedMultiplier);
			RaycastHit hit;
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hit, character.GroundCheckDistance))
			{
				GameObject hitObj = hit.transform.gameObject;

				if (character.IsGrounded && step)
				{
                    if(hitObj.name.Contains("Ground"))
                        playWalkSound(concrete, walkVolume, stepLength);
                    else if (hitObj.name.Contains("Ramp"))
					    playWalkSound(wood, walkVolume, stepLength);
                    else if (hitObj.name.Contains("BoxSmall"))
                        playWalkSound(wood, walkVolume, stepLength);
                    else if (hitObj.name.Contains("Container"))
                        playWalkSound(metal, walkVolume, stepLength);
                }
			}
		}

        void PlayRunSound() {
            float stepLength = origAudioStepLengthRun/(character.MoveSpeedMultiplier*character.AnimSpeedMultiplier);
			RaycastHit hit;
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hit, character.GroundCheckDistance))
			{
                GameObject hitObj = hit.transform.gameObject;

                if (character.IsGrounded && step)
				{
                    if(hitObj.name.Contains("Ground"))
                        playWalkSound(concrete, runVolume, stepLength);
                    else if (hitObj.name.Contains("Ramp"))
					    playWalkSound(wood, runVolume, stepLength);
                    else if (hitObj.name.Contains("BoxSmall"))
                        playWalkSound(wood, runVolume, stepLength);
                    else if (hitObj.name.Contains("Container"))
                        playWalkSound(metal, runVolume, stepLength);
                }
            }
        }

		void OnCollisionEnter(Collision collision) {
            // When landing from a jump/fall
			if (collision.relativeVelocity.y > 3f) {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hit, character.GroundCheckDistance))
                {
                    GameObject hitObj = hit.transform.gameObject;
                    float volume = Mathf.Max(collision.relativeVelocity.y*landVolMultiplier, 1.5f);
                    float stepLength = 0.05f;

                    if(hitObj.name.Contains("Ground"))
                        playWalkSound(concrete, volume, stepLength);
                    else if (hitObj.name.Contains("Ramp"))
					    playWalkSound(wood, volume, stepLength);
                    else if (hitObj.name.Contains("BoxSmall"))
                        playWalkSound(wood, volume, stepLength);
                    else if (hitObj.name.Contains("Container"))
                        playWalkSound(metal, volume, stepLength);
                }
			}

            // When bumping into another character
            if (bump && collision.relativeVelocity.magnitude > 4f && (collision.transform.gameObject.CompareTag("AI") || collision.transform.gameObject.CompareTag("Player"))) {
                float volume = Mathf.Max(collision.relativeVelocity.magnitude*0.3f, 1.2f);
                PlayBumpSound(volume);
            }
		}

        IEnumerator WaitForFootSteps(float stepLength)
        {
            step = false;
            yield return new WaitForSeconds(stepLength);
            step = true;
        }

        IEnumerator WaitForBump(float time)
        {
            bump = false;
            yield return new WaitForSeconds(time);
            bump = true;
        }

        public void PlayJumpSound() {
            AudioClip clip = jumpSounds[Random.Range(0, jumpSounds.Length)];
            voiceAudioSrc.pitch = Random.Range(0.9f, 1.1f);
            float volume = Random.Range(voiceVolume, voiceVolume+0.1f);
            voiceAudioSrc.PlayOneShot(clip, volume);
        }

        void PlayFallingSound() {
            voiceAudioSrc.pitch = 1f;
            float volume = Random.Range(voiceVolume, voiceVolume+0.1f);
            voiceAudioSrc.PlayOneShot(fallSound, volume);
        }

        void PlayBumpSound(float volume) {
            AudioClip clip = bumpSounds[Random.Range(0, bumpSounds.Length)];
            voiceAudioSrc.pitch = Random.Range(0.9f, 1.1f);
            voiceAudioSrc.PlayOneShot(clip, volume);
            StartCoroutine(WaitForBump(0.1f));
        }

        void playWalkSound(AudioClip[] clips, float volume, float stepLength) {
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            audioSrc.pitch = Random.Range(0.9f, 1f);
            volume = Random.Range(volume, volume+0.05f);
            audioSrc.PlayOneShot(clip, volume);
            StartCoroutine(WaitForFootSteps(stepLength));
        }

    }
}