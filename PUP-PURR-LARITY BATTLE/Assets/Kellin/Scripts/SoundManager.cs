using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    [Header("Sound Effects")]
    public AudioSource m_soundEffectSource;
    public AudioClip[] m_refocusClips;
    public AudioClip m_shootClip;

    [Header("Background Music")]
    public AudioSource m_musicSource;
    public AudioClip[] m_musicClips;

    public void PlayRefocusSoundEffect(int soundEffect)
    {
        m_soundEffectSource.PlayOneShot(m_refocusClips[soundEffect]);
    }

    public void PlayBackgroundMusic(int music)
    {
        m_soundEffectSource.PlayOneShot(m_refocusClips[music]);
    }

    public void PlayShootSound()
    {
        m_soundEffectSource.PlayOneShot(m_shootClip);
    }
}
