using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
///<summary>A class for containing an audio clip in a non-gameobject oriented fashion</summary>
public class Sound {
	public string name;
	public AudioClip clip;
	///<summary>Alternate sounds that have a chance to play instead when the regular clip plays</summary>
	public List<AudioClip> alternates;

	[Range(0f, 1f)]
	public float volume = 1f;
	[Range(0.1f, 3f)]
	public float pitch = 1f;
	[Range(0f, 1f)]
	public float spacialBlend = 1f;
	public bool loop = false;
	public AudioRolloffMode audioRolloffMode = AudioRolloffMode.Linear;
	public bool bypassEffects = false;
	public AudioMixerGroup audioMixerGroup;
	public bool playOnAwake = false;

	[HideInInspector]
	public AudioSource source;
}