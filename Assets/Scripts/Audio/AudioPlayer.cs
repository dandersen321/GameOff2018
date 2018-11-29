using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

    public static AudioPlayer Instance { get; private set; }

    public List<Sound> sounds;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.playOnAwake = sound.playOnAwake;
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    // Use this for initialization
    void Start () {
        
	}

    public void PlayAudio(string soundName)
    {
        Sound sound = sounds.Find(obj => obj.name == soundName);

        if (sound == null)
        {
            Debug.Log("Warning: Could not find sound " + soundName);
            return;
        }

        if (!sound.source.isPlaying)
            sound.source.Play();
    }

    public void PlayClipAtPoint(string soundName, Vector3 position)
    {
        Sound sound = sounds.Find(obj => obj.name == soundName);

        if (sound == null)
        {
            Debug.Log("Warning: Could not find sound " + soundName);
            return;
        }

        AudioSource.PlayClipAtPoint(sound.clip, position, sound.volume);
    }

    public void PlaySoundAttachToGameObject(string soundName, GameObject gameObj)
    {
        Sound sound = sounds.Find(obj => obj.name == soundName);

        if (sound == null)
        {
            Debug.Log("Warning: Could not find sound " + soundName);
            return;
        }

        AudioSource source = gameObj.AddComponent<AudioSource>();

        source.clip = sound.clip;
        source.volume = sound.volume;
        source.pitch = sound.pitch;

        source.Play();
    }
}
