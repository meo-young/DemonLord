using UnityEngine;

public class EndingCredit : MonoBehaviour
{
    void Start()
    {
        EndingCreditUI.instance.Open();
        AudioManager.instance.PlayBGM(BGM.bgm_ending);
    }

}
