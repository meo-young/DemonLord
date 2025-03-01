using System.Collections;
using UnityEngine;

public class Test_Sound : MonoBehaviour
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
        AudioManager.instance.PlayBGM(BGM.bgm_test_get_ready);
        yield return new WaitForSeconds(3);
        AudioManager.instance.PlayBGM(BGM.bgm_test_moonside_howl, isFade: true);
        yield return new WaitForSeconds(3);
        AudioManager.instance.PlayBGM(BGM.bgm_test_get_ready, isFade: true);
        yield return new WaitForSeconds(3);
        AudioManager.instance.PlayBGM(BGM.COUNT, isFade: true, fadeSec:5f); // 비활성화

        yield return new WaitForSeconds(6);
        AudioManager.instance.PlaySFX(SFX.sfx_test_click);
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.PlaySFX(SFX.sfx_test_click);
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.PlaySFX(SFX.sfx_test_click);
        yield return new WaitForSeconds(1f);

        AudioManager.instance.PlaySFX(SFX.sfx_test_coin);
        yield return new WaitForSeconds(1);
        AudioManager.instance.PlaySFX(SFX.sfx_test_coin);
        yield return new WaitForSeconds(1);
        AudioManager.instance.PlaySFX(SFX.sfx_test_coin);
        yield return new WaitForSeconds(0.08f);
        AudioManager.instance.PlaySFX(SFX.sfx_test_coin);
        yield return new WaitForSeconds(0.08f);
        AudioManager.instance.PlaySFX(SFX.sfx_test_coin);
        yield return new WaitForSeconds(0.08f);
    }
}
