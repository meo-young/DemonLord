using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("시작 클릭 시 넘어갈 씬 이름")]
    [SerializeField] private string startSceneName;

    public Transform gameDescriptionPanel;

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


    public void OnGameDescriptionButtonClick()
    {
        if(gameDescriptionPanel.localScale == Vector3.zero)
        {
            gameDescriptionPanel.localScale = Vector3.one;
        }
        else
        {
            gameDescriptionPanel.localScale = Vector3.zero;
        }
    }
}
