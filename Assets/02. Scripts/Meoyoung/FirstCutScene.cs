using UnityEngine;

public class FirstCutScene : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.PlayBGM(BGM.bgm_FirstCutScene);
    }
}
