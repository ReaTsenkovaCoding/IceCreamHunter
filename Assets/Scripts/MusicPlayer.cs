using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource musicSource;

    void Awake()
    {
        musicSource = GetComponent<AudioSource>();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

}
