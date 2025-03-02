using UnityEngine;
using UnityEngine.UI;
using System;

public class BattleFormation : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
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
