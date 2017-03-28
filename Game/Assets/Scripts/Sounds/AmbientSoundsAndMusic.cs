﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundsAndMusic : MonoBehaviour {

	[Header("Ambient")]
	public AudioClip ambientSound;
	[Range(0.01f,1f)]
	public float ambientVolume = 0.35f;

	[Header("Music")]
	public AudioClip music;
	[Range(0.01f,1f)]
	public float musicVolume = 0.4f;

	[Header("Suspense")]
	public AudioClip suspenseSound;
	[Range(0.01f,1f)]
	public float suspenseVolume = 0.5f;

	[Header("Fail")]
	public AudioClip failSound;
	[Range(0.01f,1f)]
	public float failVolume = 0.5f;

	[Header("Win")]
	public AudioClip winSound;
	[Range(0.01f,1f)]
	public float winVolume = 0.7f;

	AudioSource ambientSrc;
	AudioSource musicSrc;
	AudioSource suspenseSrc;
	AudioSource failSrc;
	AudioSource winSrc;

	void Start () {
		ambientSrc = gameObject.AddComponent<AudioSource>();
		musicSrc = gameObject.AddComponent<AudioSource>();
		suspenseSrc = gameObject.AddComponent<AudioSource>();
		failSrc = gameObject.AddComponent<AudioSource>();
		winSrc = gameObject.AddComponent<AudioSource>();

		ambientSrc.clip = ambientSound;
		ambientSrc.volume = ambientVolume;
		ambientSrc.loop = true;
		ambientSrc.time = Random.Range(0,ambientSrc.clip.length);
		ambientSrc.Play();

		musicSrc.volume = musicVolume;
		musicSrc.loop = true;

		suspenseSrc.clip = suspenseSound;
		suspenseSrc.volume = suspenseVolume;

		failSrc.clip = failSound;
		failSrc.volume = failVolume;

		winSrc.clip = winSound;
		winSrc.volume = winVolume;
	}

	public void PlayFirstSong() {
		if(!musicSrc.isPlaying || musicSrc.clip != music) {
			musicSrc.clip = music;
			musicSrc.Play();
		}
	}

	IEnumerator WaitForSuspense(float suspenseLength) {
		musicSrc.volume = 0.1f;
		yield return new WaitForSeconds(suspenseLength);
		musicSrc.volume = musicVolume;
	}

	public void PlaySuspense() {
		suspenseSrc.Play();
		StartCoroutine(WaitForSuspense(suspenseSrc.clip.length-0.02f));
	}

	public void PlayFail() {
		failSrc.Play();
	}

	public void PlayWin() {
		musicSrc.Stop();
		winSrc.Play();
	}
}
