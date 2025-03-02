using UnityEngine;
using AYellowpaper.SerializedCollections;

public class WarrantManager : MonoBehaviour
{
    public static WarrantManager instance;

    public SerializedDictionary<WarrantType, string> warrantDescriptions = new();   // 권능 설명

    private void Awake() 
    {
        instance = this;
    }



    /// <summary>
    /// 권능 설명 반환
    /// </summary>
    /// <param name="warrantType"> 권능 타입 </param>
    public string GetWarrantDescription(WarrantType warrantType)
    {
        return warrantDescriptions[warrantType];
    }
}
