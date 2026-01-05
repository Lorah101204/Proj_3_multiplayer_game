using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Serializable]
    class SoundPair
    {
        public SoundID id;
        public AudioClip clip;
    }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Settings")]
    [SerializeField] private float minSfxInterval = 0.08f;
    [SerializeField] private SoundPair[] sounds;

    private readonly Dictionary<SoundID, AudioClip> clips = new();
    private float lastSfxTime;

    private const string SAVE_KEY = "PC_AUDIO_SETTINGS";

    [Serializable]
    class AudioSettings
    {
        public float music = 1f;
        public float sfx = 1f;
        public float volume = 1f;
    }

    AudioSettings settings = new();

    public float MusicVolume
    {
        get => settings.music;
        set
        {
            settings.music = Mathf.Clamp01(value);
            musicSource.volume = settings.music;
        }
    }

    public float SfxVolume
    {
        get => settings.sfx;
        set
        {
            settings.sfx = Mathf.Clamp01(value);
            sfxSource.volume = settings.sfx;
        }
    }

    public float MasterVolume
    {
        get => settings.volume;
        set
        {
            settings.volume = Mathf.Clamp01(value);
            AudioListener.volume = settings.volume;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSettings();
        BuildClipMap();
    }

    private void BuildClipMap()
    {
        clips.Clear();
        foreach (var s in sounds)
            if (!clips.ContainsKey(s.id))
                clips.Add(s.id, s.clip);
    }

    // ----------------- MUSIC -----------------

    public static void PlayMusic(SoundID id, bool loop = true)
    {
        if (!Instance.clips.TryGetValue(id, out var clip)) return;

        var src = Instance.musicSource;
        src.clip = clip;
        src.loop = loop;
        src.Play();
        Debug.Log("Play Music: " + id);
    }

    public static void StopMusic()
    {
        Instance.musicSource.Stop();
    }

    // ----------------- SFX -----------------

    public static void PlaySfx(SoundID id, float volume = 1f)
    {
        if (Instance.settings.sfx == 0f) return;
        if (!Instance.clips.TryGetValue(id, out var clip)) return;

        if (Time.time - Instance.lastSfxTime < Instance.minSfxInterval)
            return;
        Debug.Log("Play SFX: " + id);
        Instance.sfxSource.PlayOneShot(clip, volume);
        Instance.lastSfxTime = Time.time;
    }

    // ----------------- SAVE / LOAD -----------------

    private void OnDisable()
    {
        SaveSettings();
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetString(SAVE_KEY, JsonUtility.ToJson(settings));
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
            settings = JsonUtility.FromJson<AudioSettings>(PlayerPrefs.GetString(SAVE_KEY));

        MusicVolume = settings.music;
        SfxVolume = settings.sfx;
        MasterVolume = settings.volume;
    }
}

