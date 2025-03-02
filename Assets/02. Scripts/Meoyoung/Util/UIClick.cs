using UnityEngine;

public class UIClick : MonoBehaviour
{
    public void OnClick()
    {
        AudioManager.instance.PlaySFX(SFX.sfx_UI_Click);
    }
}
