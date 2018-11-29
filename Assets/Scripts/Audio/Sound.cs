using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound {

    public string name;
    public AudioClip clip;

    [Range(0, 1)]
    public float volume = 1f;

    [Range(.25f, 3)]
    public float pitch = 1f;

    public bool loop = false;
    public bool playOnAwake = false;

    [HideInInspector]
    public AudioSource source;
}
