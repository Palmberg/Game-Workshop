using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(AudioSource))]
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
					    PlayConcreteSound(walkVolume, stepLength);
                    else if (hitObj.name.Contains("Ramp"))
					    PlayWoodSound(walkVolume, stepLength);
                    else if (hitObj.name.Contains("Dirt"))
                        PlayDirtSound(walkVolume, stepLength);
                    else if (hitObj.name.Contains("Container"))
                        PlayMetalSound(walkVolume, stepLength);
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
					    PlayConcreteSound(runVolume, stepLength);
                    else if (hitObj.name.Contains("Ramp"))
					    PlayWoodSound(runVolume, stepLength);
                    else if (hitObj.name.Contains("BoxSmall"))
                        PlayWoodSound(runVolume, stepLength);
                    else if (hitObj.name.Contains("Container"))
                        PlayMetalSound(runVolume, stepLength);
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

                    if(hitObj.name.Contains("Ground"))
					    PlayConcreteSound(volume, 0.05f);
                    else if (hitObj.name.Contains("Ramp"))
					    PlayWoodSound(volume, 0.05f);
                    else if (hitObj.name.Contains("BoxSmall"))
                        PlayWoodSound(volume, 0.05f);
                    else if (hitObj.name.Contains("Container"))
                        PlayMetalSound(volume, 0.05f);
                }
			}

            // When bumping into another character
            // TODO: Use the actual tag of AI
            if (bump && collision.relativeVelocity.magnitude > 5f && collision.transform.gameObject.CompareTag("AI")) {
                float volume = Mathf.Max(collision.relativeVelocity.magnitude*0.15f, 1f);
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

        void PlayConcreteSound(float volume, float stepLength)
        {
            AudioClip clip = concrete[Random.Range(0, concrete.Length)];
            audioSrc.pitch = Random.Range(0.9f, 1.1f);
            volume = Random.Range(volume, volume+0.05f);
            audioSrc.PlayOneShot(clip, volume);
            StartCoroutine(WaitForFootSteps(stepLength));
        }

        void PlayWoodSound(float volume, float stepLength)
        {
            AudioClip clip = wood[Random.Range(0, wood.Length)];
            audioSrc.PlayOneShot(clip, volume);
            audioSrc.pitch = Random.Range(0.9f, 1.1f);
            volume = Random.Range(volume, volume+0.05f);
            StartCoroutine(WaitForFootSteps(stepLength));
        }

        void PlayDirtSound(float volume, float stepLength)
        {
            AudioClip clip = dirt[Random.Range(0, dirt.Length)];
            audioSrc.PlayOneShot(clip, volume);
            audioSrc.pitch = Random.Range(0.9f, 1.1f);
            volume = Random.Range(volume, volume+0.05f);
            StartCoroutine(WaitForFootSteps(stepLength));
        }

        void PlayMetalSound(float volume, float stepLength)
        {
            AudioClip clip = metal[Random.Range(0, metal.Length)];
            audioSrc.PlayOneShot(clip, volume);
            audioSrc.pitch = Random.Range(0.9f, 1f);
            volume = Random.Range(volume, volume+0.05f);
            StartCoroutine(WaitForFootSteps(stepLength));
        }

    }
}