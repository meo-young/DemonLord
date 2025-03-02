using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Monologue : MonoBehaviour
{
    public string sceneName;

    private void Start()
    {
        AudioManager.instance.PlaySFX(SFX.sfx_dead);
        Invoke("NextScene", 4f);
    }

    public void NextScene()
    {
        FadeManager.instance.FadeOut(
            () =>
            {
                SceneManager.LoadScene(sceneName);
            }
        );
    }

}
