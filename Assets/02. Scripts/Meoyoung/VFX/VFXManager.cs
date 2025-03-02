using UnityEngine;
using AYellowpaper.SerializedCollections;


public class VFXManager : MonoBehaviour
{
    public static VFXManager instance;

    public SerializedDictionary<EffectType, GameObject> effectPrefabs = new();   // 이펙트 프리펩

    private void Awake()
    {
        instance = this;
    }
    public void ShowEffect(EffectType effectType, Vector3 position)
    {
        Instantiate(effectPrefabs[effectType], position, Quaternion.identity);
    }

}
