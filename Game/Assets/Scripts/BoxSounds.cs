using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BoxSounds : MonoBehaviour {

	AudioSource audioSrc;
	public AudioClip[] collisionSounds;
	[Space(10)]
	public AudioClip slideSound;
	public float slideSoundVolMultiplier = 1.2f;

	float speed;
	Rigidbody rb;

	// Use this for initialization
	void Start () {
		audioSrc = gameObject.AddComponent<AudioSource>();
		audioSrc.spatialBlend = 1f; // Make it a 3D source
		rb = gameObject.GetComponent<Rigidbody>();
	}

	void FixedUpdate() {
		speed = rb.velocity.magnitude;
	}

	void OnCollisionStay(Collision collision) {
		if (!audioSrc.isPlaying && speed >= 0.1f) // && collision.gameObject.tag == "Ground"
		{
            audioSrc.pitch = Random.Range(0.9f, 1.1f);
			audioSrc.volume = speed*slideSoundVolMultiplier;
			audioSrc.clip = slideSound;
			audioSrc.Play();
		}
		else if (audioSrc.isPlaying && speed < 0.1f) // && collision.gameObject.tag == "Ground"
		{
			audioSrc.Pause();
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.relativeVelocity.magnitude > 1.5f) {
			float volume = Mathf.Max(collision.relativeVelocity.y/10f, 1.2f);
			PlayCollisionSound(volume, 0.05f);
		}
	}

	void OnCollisionExit(Collision collision)
	{
		if (audioSrc.isPlaying) // && collision.gameObject.tag == "Ground"
		{
			audioSrc.Pause();
		}
	}

	void PlayCollisionSound(float volume, float waitLength)
	{
		AudioClip clip = collisionSounds[Random.Range(0, collisionSounds.Length)];
		audioSrc.pitch = Random.Range(0.9f, 1.1f);
		volume = Random.Range(volume, volume+0.1f);
		audioSrc.PlayOneShot(clip, volume);
	}
}
