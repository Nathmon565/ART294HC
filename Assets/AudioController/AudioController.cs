using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Contains a list of sounds which can be instantiated and played</summary>
public class AudioController : MonoBehaviour {
	public List<Sound> sounds;

	void Awake() {
		foreach (Sound s in sounds) {
			s.source = gameObject.AddComponent<AudioSource>();

			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
			s.source.spatialBlend = s.spacialBlend;
			s.source.rolloffMode = s.audioRolloffMode;
			s.source.bypassEffects = s.bypassEffects;
			s.source.outputAudioMixerGroup = s.audioMixerGroup;
			s.source.playOnAwake = s.playOnAwake;
		}
	}

	///<summary>Returns a Sound from the list of sounds</summary>
	///<param name="sound">The string of the sound name to return</param>
	public Sound GetSound(string sound) {
		List<Sound> soundsT = sounds;
		foreach (Sound s in soundsT) {
			if (s.name == sound) {
				return s;
			}
		}
		Debug.LogError("Sound name '" + sound + "' not found when trying to find it. Returning null.");
		return null;
	}

	///<summary>Create and play a sound based on its reference</summary>
	///<param name='sound'>The sound reference to be played</param>
	///<param name='parent'>The transform parent the sound should be attached to</param>
	///<param name='position'>The position offset for the sound to start with</param>
	///<param name='localSpace'>Whether the position coordinates are in local or global space</param>
	///<param name='deleteAfterPlay'>Whether the object should be deleted immediately after finishing playing a sound</param>
	///<param name='pitchVariance'>The percent pitch variance in either direction</param>
	public AudioSource PlaySound(Sound sound, Transform parent = null, Vector3 position = default(Vector3), bool localSpace = true, bool deleteAfterPlay = true, float pitchVariance = 0.1f) {
		GameObject g = new GameObject(sound.name);
		Transform t = g.transform;
		AudioSource s = g.AddComponent<AudioSource>();
		int c = Random.Range(-1, sound.alternates.ToArray().Length);
		s.clip = c<0?sound.clip:sound.alternates[c];
		s.volume = sound.volume;
		s.pitch = sound.pitch;
		s.loop = sound.loop;
		s.spatialBlend = sound.spacialBlend;
		s.rolloffMode = sound.audioRolloffMode;
		s.bypassEffects = sound.bypassEffects;
		s.outputAudioMixerGroup = sound.audioMixerGroup;
		s.playOnAwake = sound.playOnAwake;

		s.pitch = 1 + Random.Range(-pitchVariance, pitchVariance);
		s.Play();
		if(deleteAfterPlay) { DeleteAfterPlayingSounds(s); }

		t.parent = parent;
		if(localSpace) { t.localPosition = position; }
		else { t.position = position; }
		return s;
	}

	///<summary>Create and play a sound based on its name</summary>
	///<param name='name'>The name of the sound to be played</param>
	///<param name='parent'>The transform parent the sound should be attached to</param>
	///<param name='position'>The position offset for the sound to start with</param>
	///<param name='localSpace'>Whether the position coordinates are in local or global space</param>
	///<param name='deleteAfterPlay'>Whether the object should be deleted immediately after finishing playing a sound</param>
	///<param name='pitchVariance'>The percent pitch variance in either direction</param>
	public AudioSource PlaySound(string name, Transform parent = null, Vector3 position = default(Vector3), bool localSpace = true, bool deleteAfterPlay = true, float pitchVariance = 0.1f) {
		return PlaySound(GetSound(name), parent, position, localSpace, deleteAfterPlay, pitchVariance);
	}

	///<summary>Create and play a sound based on its name</summary>
	///<param name='name'>The name of the sound to be played</param>
	///<param name='pitchVariance'>The percent pitch variance in either direction</param>
	///<param name='parent'>The transform parent the sound should be attached to</param>
	///<param name='position'>The position offset for the sound to start with</param>
	///<param name='localSpace'>Whether the position coordinates are in local or global space</param>
	///<param name='deleteAfterPlay'>Whether the object should be deleted immediately after finishing playing a sound</param>
	public AudioSource PlaySound(string name, float pitchVariance, Transform parent = null, Vector3 position = default(Vector3), bool localSpace = true, bool deleteAfterPlay = true) {
		return PlaySound(GetSound(name), parent, position, localSpace, deleteAfterPlay, pitchVariance);
	}

	///<summary>Delete a sound immediately once it is no longer playing</summary>
	///<param name='sound'>The reference to the audio source</param>
	public void DeleteAfterPlayingSounds(AudioSource sound) {
		StartCoroutine(DeleteAfterPlayingSoundsCoroutine(sound));
	}

	private IEnumerator DeleteAfterPlayingSoundsCoroutine(AudioSource sound) {
		yield return new WaitUntil(() => !sound.isPlaying);
		Destroy(sound.gameObject);
	}
}
