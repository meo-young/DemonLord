using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("시작 클릭 시 넘어갈 씬 이름")]
    [SerializeField] private string startSceneName;

    void Start()
    {
        AudioManager.instance.PlayBGM(BGM.bgm_Title);
    }

    public void OnStartButtonClick()
    {
        // 페이드 아웃 후 시작 씬으로 이동
        FadeManager.instance.FadeOut(onComplete :
            () => {
                SceneManager.LoadScene(startSceneName);
            }
        );
    }

    public void OnExitButtonClick()
    {
        // 페이드 아웃 후 게임 종료
        FadeManager.instance.FadeOut(onComplete :
            () => {
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
            }
        );
    }

    public void OnCreditButtonClick()
    {
        // @To-do 크레딧 화면 표시
        Debug.Log("크레딧 화면 표시");
    }
}
