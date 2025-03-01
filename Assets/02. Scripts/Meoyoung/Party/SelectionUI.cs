using UnityEngine;
using System.Collections.Generic;

public class SelectionUI : MonoBehaviour
{
    public static SelectionUI instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void InitSelectionUI()
    {
        DeactivateSelectionUI();

        // 파티 멤버 가져오기
        List<CharacterBase> partyMembers = PartyManager.instance.GetPartyMembers();

        foreach(CharacterBase member in partyMembers)
        {
            // 파티에 근거리가 존재할 경우 근거리 공격 선택 활성화
            if(member.characterType == CharacterType.Warrior)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            // 파티에 원거리가 존재할 경우 원거리 공격 선택 활성화
            else if(member.characterType == CharacterType.Ranger)
            {
                transform.GetChild(1).gameObject.SetActive(true);
            }
            // 파티에 마법사가 존재할 경우 마법 공격 선택 활성화
            else if(member.characterType == CharacterType.Wizard)
            {
                transform.GetChild(2).gameObject.SetActive(true);
            }
        }
    }

    public void DeactivateSelectionUI()
    {
        // 선택지 UI 모두 비활성화
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
