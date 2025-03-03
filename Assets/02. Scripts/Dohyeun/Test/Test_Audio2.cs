using System.Collections;
using UnityEngine;

public class Test_Audio2: MonoBehaviour
{
    void Start()
    {
        TestAudios();
    }
    void TestAudios()
    {
        StartCoroutine(CoTestAudios());
    }
    IEnumerator CoTestAudios()
    {
        yield return new WaitForSeconds(1);
        AudioManager.instance.PlayBGM(BGM.bgm_opening, isFade: true);
        yield return new WaitForSeconds(10);
        AudioManager.instance.PlayBGM(BGM.bgm_lobby, isFade: true);
        //yield return new WaitForSeconds(3);
        //AudioManager.instance.PlaySFX(SFX.sfx_attack_magic);
        //yield return new WaitForSeconds(0.2f);
        //AudioManager.instance.PlaySFX(SFX.sfx_attack_magic);
        //yield return new WaitForSeconds(0.2f);
        //AudioManager.instance.PlaySFX(SFX.sfx_attack_magic);
        //yield return new WaitForSeconds(10);
        //AudioManager.instance.PlayBGM(BGM.bgm_lobby, isFade: true);
        //yield return new WaitForSeconds(3);
        //AudioManager.instance.PlaySFX(SFX.sfx_clear);
        //AudioManager.instance.PlayBGM(BGM.COUNT, isFade: true);
        //yield return new WaitForSeconds(4);
        //AudioManager.instance.PlayBGM(BGM.bgm_ending, isFade: true);

    }
}
