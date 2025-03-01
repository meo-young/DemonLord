using UnityEngine;
using UnityEngine.UI;

public class BattleFormation : MonoBehaviour
{
    public static BattleFormation instance;

    Animator anim;

    private void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
    }

    public void StartBattleFormation()
    {
        anim.SetBool("NoBattle", false);
        anim.SetBool("Start", true);
    }

    public void EndBattleFormation()
    {
        anim.SetBool("End", true);
        anim.SetBool("Start", false);
    }

    public void NoBattleFormation()
    {
        anim.SetBool("NoBattle", true);
        anim.SetBool("End", false);
    }
}
