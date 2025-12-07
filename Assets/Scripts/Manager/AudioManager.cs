// using UnityEngine;
// using DesignPattern;
// using System.Collections;
// using System;

// public enum SoundID
// {
//     None = 0
// }

// public class AudioManager : Singleton<AudioManager>
// {
//     [SerializeField] AudioSource audioManager;
//     [SerializeField] AudioSource audioEffect;
//     [Serializable]
//     class SoundIDClip
//     {
//         [HideInInspector] public string name;
//         public SoundID soundID;
//         public AudioClip audioClip;
//     }

//     [SerializeField] SoundIDClip[] m_Sounds;
//     readonly Dictionary<SoundID, AudioClip> m_Clips = new();
//     const string k_AudioSettings = "AudioSettings";
//     AudioSettings m_AudioSettings = new();



// }