using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public static List<SONG> allSongs = new List<SONG>();
    public static SONG activeSong = null;

    private float songTransitionSpeed = 2f;
    private bool songSmoothTransitions = true;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        ReadAndPlayMusic("strategymp3",false);
    }
    public void ReadAndPlaySFX(string data)
    {
        AudioClip clip = Resources.Load("Audio/SFX/" + data) as AudioClip;
        if (clip != null)
        {
            PlaySFX(clip);
        }
        else
        {
            Debug.LogError("Clip does not exist - " + data);
        }
    }
    public void ReadAndPlayMusic(string data, bool volume)
    {
        AudioClip clip = Resources.Load("Audio/Music/" + data) as AudioClip;
        if (clip != null)
        {
            PlaySong(clip, volumentLeveling:volume);
        }
        else
        {
            Debug.LogError("Clip does not exist - " + data);
        }
    }
    private void PlaySFX(AudioClip effect, float volume = 1f,float pitch = 1f)
    {
        AudioSource source = CreateNewSource(string.Format("SFX [{0}]",effect.name));
        source.outputAudioMixerGroup = sfx;
        source.clip = effect;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();

        Destroy(source.gameObject, effect.length); 
    }

    private void PlaySong(AudioClip song, float maxVolume = 1f, float pitch = 1f,float startingVolume = 0f, bool playOnStart = true,bool loop = true, bool volumentLeveling = true)
    {
        if (song !=null)
        {
            for (int i = 0; i < allSongs.Count; i++)
            {
                SONG s = allSongs[i];
                if (s.clip == song)
                {
                    activeSong = s;
                    break;
                }
            }
            if (activeSong == null || activeSong.clip!=song)
            {
                activeSong = new SONG(song, music, maxVolume, pitch, startingVolume, playOnStart, loop);

            }
            StopAllCoroutines();
            StartCoroutine(VolumeLeveling());
        }
        else
        {
            activeSong = null;
        }
    }

    IEnumerator VolumeLeveling()
    {
        while (TransitionSongs())
        {
            yield return new WaitForEndOfFrame();
        }
    }
    
    bool TransitionSongs()
    {
        bool anyValueChanged = false;

        float speed = songTransitionSpeed * Time.deltaTime;

        for (int i = allSongs.Count - 1; i >= 0; i--)
        {
            SONG song = allSongs[i];
            if (song == activeSong)
            {
                if (song.volume < song.maxVolume)
                {
                    song.volume = songSmoothTransitions ? Mathf.Lerp(song.volume, song.maxVolume, speed) : Mathf.MoveTowards(song.volume, song.maxVolume, speed);
                    anyValueChanged = true;
                }
            }
            else
            {
                if (song.volume > 0)
                {
                    song.volume = songSmoothTransitions ? Mathf.Lerp(song.volume, 0f, speed) : Mathf.MoveTowards(song.volume, 0f, speed);
                    anyValueChanged = true;
                }
                else
                {
                    allSongs.RemoveAt(i);
                    song.DestroySong();
                    continue;
                }
            }

        }

        return anyValueChanged;
    }
    public static AudioSource CreateNewSource(string _name)
    {
        AudioSource newSource = new GameObject(_name).AddComponent<AudioSource>();
        newSource.transform.SetParent(instance.transform);
        return newSource;
    }

    public AudioMixerGroup music;
    public AudioMixerGroup sfx;

    [System.Serializable]
    public class SONG
    {
        public AudioSource source;
        
        public float maxVolume = 1f;
        public SONG(AudioClip clip,AudioMixerGroup mg, float maxVolume, float pitch , float startingVolume,bool playOnStart,bool loop)
        {
            source = CreateNewSource(string.Format("SFX [{0}]", clip.name));
            source.clip = clip;
            source.volume = startingVolume;
            this.maxVolume = maxVolume;
            source.pitch = pitch;
            source.loop = loop;
            source.outputAudioMixerGroup = mg;
            allSongs.Add(this);
            if (playOnStart)
            {
                source.Play();
            }
        }
        public AudioClip clip { get {return source.clip;} set { source.clip = value; } }
        public float volume { get { return source.volume; } set { source.volume = value; } }
        public float pitch { get { return source.pitch; } set { source.pitch = value; } }
        public void Play()
        {
            source.Play();
        }
        public void Stop()
        {
            source.Stop();
        }
        public void Pause()
        {
            source.Pause();
        }
        public void UnPause()
        {
            source.UnPause();
        }
        public void DestroySong()
        {
            AudioManager.allSongs.Remove(this);
            DestroyImmediate(source.gameObject);
        }
    }
}

