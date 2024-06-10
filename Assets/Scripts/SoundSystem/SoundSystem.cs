using UnityEngine;
public class SoundSystem : ISoundSystem
{
    private AudioSource source;
    public SoundSystem(AudioSource source)
    {
        this.source = source;
    }
    public void Play(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
