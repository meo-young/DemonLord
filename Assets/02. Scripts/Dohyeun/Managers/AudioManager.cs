using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum BGM // 네이밍 = 파일명
{
    bgm_test_get_ready,
    bgm_test_moonside_howl,
    COUNT,
}
public enum SFX // 네이밍 = 파일명
{
    sfx_test_coin,
    sfx_test_click,
    COUNT,
}
public class AudioManager : MonoBehaviour
{

    #region 싱글톤 공통
    public static AudioManager instance;

    private bool isDestroyOnLoad = false;
    private bool isInitialized = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (!isDestroyOnLoad)
            {
                if (transform.parent)
                    DontDestroyOnLoad(transform.parent);
                else
                    DontDestroyOnLoad(this);
            }
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        if (!isInitialized)
            instance.Initialize();
        isInitialized = true;
    }
    #endregion
    private void Initialize()
    {
        // 게임 시작 시, 데이터 불러오기
        LoadBGMPlayer();
        LoadSFXPlayer();
        foreach (AudioSource audioSource in bgmPlayer.Values)
            audioSource.volume = 0f;
        foreach (AudioSource audioSource in sfxPlayer.Values)
            audioSource.volume = masterVolume;
    }

    // Resources 내 데이터 경로
    private string resourcesPath = Path.Combine(SubUtils.BASE_PATH,SubUtils.AUDIO_PATH);

    // 게임오브젝트의 Parents
    public Transform BGMTrs;
    public Transform SFXTrs;

    // 데이터 관리
    private Dictionary<BGM, AudioSource> bgmPlayer = new Dictionary<BGM, AudioSource>();
    private Dictionary<SFX, AudioSource> sfxPlayer = new Dictionary<SFX, AudioSource>();
    private AudioSource currentBGMSource;

    public float masterVolume = 0.8f;
    //public float bgmMaxVolume = 1f;
    //public float sfxMaxVolume = 1f;

    private void LoadBGMPlayer()
    {
        for (int i = 0; i < (int)BGM.COUNT; i++)
        {
            var audioName = ((BGM)i).ToString();
            var pathStr = Path.Combine(resourcesPath, audioName);
            var audioClip = Resources.Load(pathStr, typeof(AudioClip)) as AudioClip;
            if (!audioClip)
            {
                Debug.Log($"{audioName} 오디오클립 없음.");
                continue;
            }

            var newGO = new GameObject(audioName);
            var newAudioSource = newGO.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;
            newAudioSource.loop = true;
            newAudioSource.playOnAwake = false;
            newGO.transform.parent = BGMTrs;

            bgmPlayer[(BGM)i] = newAudioSource;
        }
    }
    private void LoadSFXPlayer()
    {
        for (int i = 0; i < (int)SFX.COUNT; i++)
        {
            var audioName = ((SFX)i).ToString();
            var pathStr = Path.Combine(resourcesPath, audioName);
            var audioClip = Resources.Load(pathStr, typeof(AudioClip)) as AudioClip;
            if (!audioClip)
            {
                Debug.Log($"{audioName} 오디오클립 없음.");
                continue;
            }

            var newGO = new GameObject(audioName);
            var newAudioSource = newGO.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;
            newAudioSource.loop = false;
            newAudioSource.playOnAwake = false;
            newGO.transform.parent = SFXTrs;

            sfxPlayer[(SFX)i] = newAudioSource;
        }
    }
    
    // BGM 관리 메서드들
    public void PlayBGM(BGM bgm, bool isFade = false, float fadeSec = 1f)
    {
        StartCoroutine(CoPlayBGM(bgm, isFade, fadeSec));
    }
    IEnumerator CoPlayBGM(BGM bgm, bool isFade, float fadeSec)
    {
        // 현재 BGM 종료
        if (currentBGMSource)
        {
            if(isFade) yield return CoAudioFadeOut(currentBGMSource, fadeSec);
            currentBGMSource.Stop();
            currentBGMSource = null;
        }
        // 새 BGM 재생
        if (!bgmPlayer.ContainsKey(bgm))
        {
            Debug.LogError($"로드 된 오디오클립 목록에 {bgm} 없음");
        }
        else
        {
            currentBGMSource = bgmPlayer[bgm];
            currentBGMSource.Play();
            if (isFade) yield return CoAudioFadeIn(currentBGMSource, fadeSec);
            currentBGMSource.volume = masterVolume;
        }
    }
    public void PauseBGM()
    {
        if (currentBGMSource) currentBGMSource.Pause();
    }
    public void ResumeBGM()
    {
        if (currentBGMSource) currentBGMSource.UnPause();
    }
    public void StopBGM()
    {
        if (currentBGMSource) currentBGMSource.Stop();
    }
    // SFX 관리 메서드들
    public void PlaySFX(SFX sfx)
    {
        if (!sfxPlayer.ContainsKey(sfx))
        {
            Debug.LogError($"클립 이름이 잘못됨. {sfx}");
            return;
        }
        sfxPlayer[sfx].PlayOneShot(sfxPlayer[sfx].clip);
    }

    // TODO: Mute/UnMute 시 Fade 적용에 대해
    public void Mute() 
    {
        foreach (var audioSourceItem in bgmPlayer)
        {
            audioSourceItem.Value.volume = 0f;
        }
        foreach (var audioSourceItem in sfxPlayer)
        {
            audioSourceItem.Value.volume = 0f;
        }
    }
    public void UnMute()
    {
        foreach (var audioSourceItem in bgmPlayer)
        {
            audioSourceItem.Value.volume = masterVolume;
        }
        foreach (var audioSourceItem in sfxPlayer)
        {
            audioSourceItem.Value.volume = masterVolume;
        }
    }

    // Volume을 sec초에 걸쳐 0으로 수렴 
    private IEnumerator CoAudioFadeOut(AudioSource audioSource, float sec)
    {
        if (audioSource == null) yield break;

        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        // 볼륨 0에 수렴
        while (elapsedTime < sec)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / sec);
            yield return null;
        }
        audioSource.volume = 0f; 
    }

    // Volume을 sec초에 걸쳐 masterVolume으로 수렴
    private IEnumerator CoAudioFadeIn(AudioSource audioSource, float sec)
    {
        if (audioSource == null) yield break;

        float startVolume = audioSource.volume; // 현재 볼륨 저장
        float elapsedTime = 0f;

        while (elapsedTime < sec)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, masterVolume, elapsedTime / sec);
            yield return null;
        }

        audioSource.volume = masterVolume; // 마지막 볼륨 1 보정
    }

}