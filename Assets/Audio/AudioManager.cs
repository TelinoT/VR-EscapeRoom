using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource musicSourceIntro;
    
    public AudioSource musicSourceLoop;

    public AudioClip bgmIntro;
    public AudioClip bgmLoop;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayBackgroundMusic();
    }

    private void PlayBackgroundMusic()
    {
        musicSourceIntro.clip = bgmIntro;
        musicSourceLoop.clip = bgmLoop;

        musicSourceIntro.loop = false;
        musicSourceLoop.loop = true;

        musicSourceIntro.spatialBlend = 0.0f;
        musicSourceLoop.spatialBlend = 0.0f;

        double introDuration = (double)bgmIntro.samples / bgmIntro.frequency;
        
        //double startTime = AudioSettings.dspTime;
        double loopStartTime = AudioSettings.dspTime + introDuration;

        musicSourceIntro.Play();
        musicSourceLoop.PlayScheduled(loopStartTime);
    }
}