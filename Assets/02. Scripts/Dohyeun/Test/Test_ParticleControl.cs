using System.Collections;
using UnityEngine;

public class Test_ParticleControl : MonoBehaviour
{
    void Start()
    {
        Test();
    }
    void Test()
    {
        StartCoroutine(CoTest());
    }
    IEnumerator CoTest()
    {
        yield return new WaitForSeconds(1f);
        EffectManager.instance.ShowEffect("Lootbeam_Ice", new Vector2(4f, -1f), 1f);
        EffectManager.instance.ShowEffect("Lootbeam_Shock_Blue", new Vector2(1f, -1f), 2f);
        EffectManager.instance.ShowEffect("Lootbeam_Shock_Yellow", new Vector2(-1f, -1f), 3f);
        EffectManager.instance.ShowEffect("Lootbeams_Silver", new Vector2(-4f, -1f), 4f);
    }

}
