using UnityEngine;
using UnityEngine.UI;

public class SoundSettingUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject settingPanel; // 설정 패널 (토글 버튼 클릭 시 활성화/비활성화)
    public Button settingButton;    // 설정 토글 버튼
    public Button settingButton2;    // 설정 열기 버튼
    public Button settingButton3;    // 설정 열기 버튼
    public Toggle muteToggle;       // Mute 체크 버튼
    public Slider volumeSlider;     // 볼륨 조절 슬라이더

    private void Start()
    {
        // UI 초기화
        settingPanel.SetActive(false);

        // 현재 AudioManager 상태 반영
        muteToggle.isOn = AudioManager.instance.IsMute;
        volumeSlider.value = AudioManager.instance.MasterVolume;

        // UI 이벤트 연결
        settingButton.onClick.AddListener(ToggleSettingPanel);
        settingButton2.onClick.AddListener(ToggleSettingPanel);
        settingButton3.onClick.AddListener(ToggleSettingPanel);
        muteToggle.onValueChanged.AddListener(ToggleMute);
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    // 설정 UI 활성화/비활성화
    private void ToggleSettingPanel()
    {
        settingPanel.SetActive(!settingPanel.activeSelf);
    }

    // 음소거 토글
    private void ToggleMute(bool isMuted)
    {
        AudioManager.instance.IsMute = isMuted;
        if (isMuted)
            AudioManager.instance.Mute(true, 0.5f); // 페이드 아웃
        else
            AudioManager.instance.UnMute(true, 0.5f); // 페이드 인
        PlayerPrefs.SetInt("IsMute", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    // 볼륨 변경
    private void ChangeVolume(float volume)
    {
        AudioManager.instance.ChangeMaxVolume(volume);
    }
}
