using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; set; }

    [Header("Audio Components")]
    [SerializeField] private AudioSource AudioSource_BGM; // BGM을 재생할 오디오 소스 컴포넌트

    [Header("Default BGM Settings")]
    [SerializeField] private AudioClip _defaultSailingBgm; // 바다 항해 시 재생할 기본 배경음악

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (AudioSource_BGM == null || clip == null) return;

        if (AudioSource_BGM.clip == clip && AudioSource_BGM.isPlaying == true) return;

        AudioSource_BGM.clip = clip;
        AudioSource_BGM.loop = true; // 반복 재생 
        AudioSource_BGM.Play();
    }

    public void PlayDefaultSailingBGM()
    {
        // 인스펙터에 등록해둔 기본 항해 BGM을 PlayBGM 메서드를 통해 재생
        if (_defaultSailingBgm != null)
        {
            PlayBGM(_defaultSailingBgm);
        }
        else
        {
            Debug.LogWarning("[SoundManager] 기본 항해 BGM이 할당되지 않았습니다.");
        }
    }

}
