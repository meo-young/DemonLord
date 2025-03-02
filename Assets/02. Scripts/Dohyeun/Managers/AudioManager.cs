using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum BGM // 네이밍 = 파일명
{
    bgm_ending,
    bgm_gameover,
    bgm_lobby,
    bgm_opening,
    bgm_test_get_ready,
    bgm_test_moonside_howl,
    COUNT,
}
public enum SFX // 네이밍 = 파일명
{
    sfx_attack_magic,
    sfx_attack_melee,
    sfx_attack_range,
    sfx_clear,
    sfx_critical,
    sfx_damage_demon,
    sfx_damage_magic,
    sfx_damage_melee,
    sfx_damage_range,
    sfx_dodge,
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
    // BGM, SFX, 사운드 상태 로드
    private void Initialize()
    {
        LoadAudioStatus();
        LoadBGMPlayer();
        LoadSFXPlayer();
        foreach (AudioSource audioSource in bgmPlayer.Values)
            audioSource.volume = 0f;
        foreach (AudioSource audioSource in sfxPlayer.Values)
            audioSource.volume = MasterVolume;
        if (IsMute) Mute();
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

    public float MasterVolume = 0.8f;
    //public float bgmMaxVolume = 1f;
    //public float sfxMaxVolume = 1f;
    public bool IsMute = false;

    // 게임 시작 시, 이전 저장했던 볼륨상태와 음소거상태를 로드
    private void LoadAudioStatus()
    {
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.8f);
        IsMute = PlayerPrefs.GetInt("IsMute", 0) == 1 ? true : false;
    }
    // BGM과 SFX 를 불러오기
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
    // 볼륨 세부 조정
    public void ChangeMaxVolume(float volume)
    {
        MasterVolume = volume;

        currentBGMSource.volume = volume;
        foreach (var audioSourceItem in sfxPlayer)
        {
            audioSourceItem.Value.volume = volume;
        }
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
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
            currentBGMSource.volume = 0f;
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
            currentBGMSource.volume = MasterVolume;
        }
    }
    // 일시정지
    public void PauseBGM(bool isFade = false, float fadeSec = 1f) => StartCoroutine(CoPauseBGM(isFade, fadeSec));
    IEnumerator CoPauseBGM(bool isFade, float fadeSec)
    {
        if (currentBGMSource)
        {
            if (isFade) yield return CoAudioFadeOut(currentBGMSource, fadeSec);
            currentBGMSource.volume = 0f;
            currentBGMSource.Pause();
        }
    }
    // 재개
    public void ResumeBGM(bool isFade = false, float fadeSec = 1f) => StartCoroutine(CoResumeBGM(isFade, fadeSec));
    IEnumerator CoResumeBGM(bool isFade, float fadeSec)
    {
        if (currentBGMSource)
        {
            if (isFade) yield return CoAudioFadeIn(currentBGMSource, fadeSec);
            currentBGMSource.volume = MasterVolume;
            currentBGMSource.UnPause();
        }
    }
    // 정지
    public void StopBGM(bool isFade = false, float fadeSec = 1f) => StartCoroutine(CoStopBGM(isFade, fadeSec));
    IEnumerator CoStopBGM(bool isFade, float fadeSec)
    {
        if (currentBGMSource)
        {
            if (isFade) yield return CoAudioFadeOut(currentBGMSource, fadeSec);
            currentBGMSource.volume = 0f;
            currentBGMSource.Stop();
        }
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

    // Mute, UnMute에 Fade 가능
    public void Mute(bool isFade = false, float fadeSec = 1f) 
    {
        foreach (var audioSourceItem in bgmPlayer)
        {
            StartCoroutine(CoMute(audioSourceItem.Value, isFade, fadeSec));
        }
        foreach (var audioSourceItem in sfxPlayer)
        {
            StartCoroutine(CoMute(audioSourceItem.Value, isFade, fadeSec));
        }
    }
    IEnumerator CoMute(AudioSource audioSource, bool isFade, float fadeSec)
    {
        if (audioSource)
        {
            if (isFade) yield return CoAudioFadeOut(audioSource, fadeSec);
            audioSource.volume = 0f;
        }
    }
    public void UnMute(bool isFade = false, float fadeSec = 1f)
    {
        foreach (var audioSourceItem in bgmPlayer)
        {
            StartCoroutine(CoUnMute(audioSourceItem.Value, isFade, fadeSec));
        }
        foreach (var audioSourceItem in sfxPlayer)
        {
            StartCoroutine(CoUnMute(audioSourceItem.Value, isFade, fadeSec));
        }
    }
    IEnumerator CoUnMute(AudioSource audioSource, bool isFade, float fadeSec)
    {
        if (audioSource)
        {
            if (isFade) yield return CoAudioFadeIn(audioSource, fadeSec);
            audioSource.volume = MasterVolume;
        }
    }
    // Volume을 sec초에 걸쳐 0으로 수렴 
    private IEnumerator CoAudioFadeOut(AudioSource audioSource, float sec)
    {
        yield return CoChangeVolume(audioSource, sec, audioSource.volume, 0f);
    }
    // Volume을 sec초에 걸쳐 masterVolume으로 수렴
    private IEnumerator CoAudioFadeIn(AudioSource audioSource, float sec)
    {
        yield return CoChangeVolume(audioSource, sec, audioSource.volume, MasterVolume);
    }
    IEnumerator CoChangeVolume(AudioSource audioSource, float sec, float startVolume, float endVolume)
    {
        if (audioSource == null) yield break;

        float elapsedTime = 0f;
        while (elapsedTime < sec)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, elapsedTime / sec);
            yield return null;
        }
        audioSource.volume = endVolume;
    }
}