using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioListener listener;
    public AudioSource music;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the AudioManager object alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeAudio(AudioClip song)
    {
        music.clip = song;
        music.Play();
    }
}
