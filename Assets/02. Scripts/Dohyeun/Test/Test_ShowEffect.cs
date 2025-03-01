using System.Collections;
using UnityEngine;

public class Test_ShowEffect : MonoBehaviour
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
        EffectManager.instance.ShowEffect("TestParticleEffect01", new Vector2(-7f, 3.5f));
        EffectManager.instance.ShowEffect("TestParticleEffect01", new Vector2(7f, -3.5f));
        yield return new WaitForSeconds(1f);
        EffectManager.instance.ShowEffect("TestParticleEffect01", new Vector2(-7f, -3.5f));
        EffectManager.instance.ShowEffect("TestParticleEffect01", new Vector2(7f, 3.5f));
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 5; i++)
        {
            EffectManager.instance.ShowEffect("TestParticleEffect01", new Vector2(Random.Range(-7f, 7f), Random.Range(-3.5f, 3.5f)));
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        EffectManager.instance.ShowEffect("TestGrassEffect", new Vector2(-7f, 3.5f));
        EffectManager.instance.ShowEffect("TestGrassEffect", new Vector2(7f, -3.5f));
        yield return new WaitForSeconds(1f);
        EffectManager.instance.ShowEffect("TestGrassEffect", new Vector2(-7f, -3.5f));
        EffectManager.instance.ShowEffect("TestGrassEffect", new Vector2(7f, 3.5f));
    }
}
