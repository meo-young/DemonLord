using UnityEngine;

public class Test_OpenSelectAWUI : MonoBehaviour
{
    void Start()
    {
        Invoke("OpenUI", 3f);
    }
    void OpenUI()
    {
        SelectAbilityWarrantUI.instance.Open();
    }
}
