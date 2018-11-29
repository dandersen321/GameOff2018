using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

    public static AudioPlayer Instance { get; private set; }

    public List<Sound> sounds;

    private string voiceLastPlayed;
    private string steplastPlayed;

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

    public void PlayeRandomVoice()
    {
        List<string> soundNames = new List<string> { "Voice1", "Voice2", "Voice3", "Voice4" };

        if (voiceLastPlayed != null)
            soundNames.Remove(voiceLastPlayed);

        string soundName = soundNames[(int)Random.Range(0, soundNames.Count)];
        voiceLastPlayed = soundName;

        PlayAudio(soundName);
    }

    public void PlayRandomFootStep()
    {
        List<string> soundNames = new List<string> { "Step1", "Step2", "Step3", "Step4", "Step5" };

        if (steplastPlayed != null)
            soundNames.Remove(steplastPlayed);

        string soundName = soundNames[(int)Random.Range(0, soundNames.Count)];
        steplastPlayed = soundName;

        PlayAudio(soundName);

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
