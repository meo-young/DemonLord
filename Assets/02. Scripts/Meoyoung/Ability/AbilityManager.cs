using UnityEngine;
using AYellowpaper.SerializedCollections;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;

    public SerializedDictionary<AbilityType, string> abilityDescriptions = new();   // 특성 설명

    private void Awake() 
    {
        instance = this;
    }



    /// <summary>
    /// 특성 설명 반환
    /// </summary>
    /// <param name="abilityType"> 특성 타입 </param>
    public string GetAbilityDescription(AbilityType abilityType)
    {
        return abilityDescriptions[abilityType];
    }
}
