using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSounds : MonoBehaviour {

	AudioSource audioSrc;

	[Header("Wing flaps")]
	public float wingVolume = 1.3f;
	public AudioClip[] wingSounds;

	void Start () {
		audioSrc = gameObject.AddComponent<AudioSource>();
		audioSrc.spatialBlend = 1f; // Make it a 3D source
	}

	void PlayWingSound() {
		AudioClip clip = wingSounds[Random.Range(0, wingSounds.Length)];
		audioSrc.pitch = Random.Range(0.9f, 1.1f);
		float volume = Random.Range(wingVolume, wingVolume+0.1f);
		audioSrc.PlayOneShot(clip, volume);
	}
}
