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
        EffectManager.instance.ShowEffect(EffectType.Damage_Melee, new Vector2(-4f, -1f), 0.5f);
        EffectManager.instance.ShowEffect(EffectType.Damage_Range, new Vector2(0f, -1f), 0.5f);
        EffectManager.instance.ShowEffect(EffectType.Damage_Magic, new Vector2(4f, -1f), 0.5f);
        yield return new WaitForSeconds(2f);
        EffectManager.instance.ShowEffect("Energe_Melee", new Vector2(-4f, -1f), 1f);
        EffectManager.instance.ShowEffect("Energe_Range", new Vector2(0f, -1f), 3f);
        EffectManager.instance.ShowEffect("Energe_Magic", new Vector2(4f, -1f), 5f);
    }

}
