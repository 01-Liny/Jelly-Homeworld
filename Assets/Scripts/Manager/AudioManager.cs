using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> m_PlayAudioClips;
    public AudioClip m_ProducerAudioClip;
    public AudioClip m_ThemeAudioClip;

    private AudioSource m_MusicAudioSource;

    private AudioSource m_ButtonPressSFX;
    private AudioSource m_UpgradeSFX;

    private AudioClip m_SelectedClip;
    private bool isPlayBGM = false;
    private bool isOnPlayMode = false;
    private int lastMusicIndex = -1;

    private bool isFadeOut = false;
    private bool isFadeIn = false;
    
    private void Awake()
    {
        m_MusicAudioSource= transform.FindChild("Music").GetComponent<AudioSource>();

        m_ButtonPressSFX = transform.FindChild("Effects/Pop").GetComponent<AudioSource>();
        m_UpgradeSFX= transform.FindChild("Effects/Upgrade").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(isFadeOut)
        {
            m_MusicAudioSource.volume -= 2f * Time.deltaTime;
            if (m_MusicAudioSource.volume < 0.4)
            {
                isFadeOut = false;
                isFadeIn = true;
            }               
        }
        else if(isFadeIn)
        {
            m_MusicAudioSource.volume += 2f * Time.deltaTime;
            if (m_MusicAudioSource.volume > 0.8f)
            {
                m_MusicAudioSource.volume = 1;
                isFadeIn = false;
            }
        }

        if (isPlayBGM)
        {
            if(isOnPlayMode)
            {
                if (m_MusicAudioSource.isPlaying == false)
                {
                    isFadeOut = true;
                    int musicIndex = Random.Range(0, m_PlayAudioClips.Count);
                    //随机到和上一次一样的曲子时，自动下一首 保证音乐不重复
                    if (musicIndex == lastMusicIndex)
                    {
                        musicIndex = (musicIndex + 1) % m_PlayAudioClips.Count;
                    }
                    m_MusicAudioSource.clip = m_PlayAudioClips[musicIndex];
                    m_MusicAudioSource.Play();

                    lastMusicIndex = musicIndex;
                }
            }
            else
            {
                if(m_MusicAudioSource.isPlaying == false)
                {
                    //m_MusicAudioSource.clip = m_SelectedClip;
                    m_MusicAudioSource.Play();
                }
            }
        }
    }

    public void StartBGM()
    {
        isPlayBGM = true;
    }

    public void StopBGM()
    {
        isPlayBGM = false;
    }

    public void StartPlayMusic()
    {
        if(isOnPlayMode==false)
        {
            m_MusicAudioSource.Stop();
            isOnPlayMode = true;
        }
        
    }

    public void StopPlayMusic()
    {
        isOnPlayMode = false;
    }

    public void StartProducerMusic()
    {
        if(m_MusicAudioSource.clip != m_ProducerAudioClip)
        {
            m_MusicAudioSource.clip = m_ProducerAudioClip;
            isFadeOut = true;
            m_MusicAudioSource.Play();
        }
    }
	
    public void StartThemeMusic()
    {
        //m_MusicAudioSource.Stop();
        if(m_MusicAudioSource.clip != m_ThemeAudioClip)
        {
            m_MusicAudioSource.clip = m_ThemeAudioClip;
            isFadeOut = true;
            m_MusicAudioSource.Play();
        }
    }

    public void PlayPop()
    {
        //m_ButtonPressSFX.Stop();
        m_ButtonPressSFX.Play();
    }

    public void PlayUpgrade()
    {
        //m_UpgradeSFX.Stop();
        m_UpgradeSFX.Play();
    }

}
