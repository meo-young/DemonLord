using UnityEngine;
using TMPro;

public class SituationUI : MonoBehaviour
{
    public static SituationUI instance;

    private TMP_Text situationText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        situationText = transform.GetChild(0).GetComponent<TMP_Text>();
    }
    
    public void SetSituationText(string text)
    {
        situationText.text = text;
    }
}
