using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

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

    private void Start() {
        InitSelectionUI();
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
                Image image = transform.GetChild(0).GetComponent<Image>();
                image.color = Color.red;
            }
            // 파티에 원거리가 존재할 경우 원거리 공격 선택 활성화
            else if(member.characterType == CharacterType.Ranger)
            {
                Image image = transform.GetChild(1).GetComponent<Image>();
                image.color = Color.red;
            }
            // 파티에 마법사가 존재할 경우 마법 공격 선택 활성화
            else if(member.characterType == CharacterType.Wizard)
            {
                Image image = transform.GetChild(2).GetComponent<Image>();
                image.color = Color.red;
            }
        }
    }


    public void SetSelectionUIAlpha(float alpha, bool isActive)
    {
        List<CharacterBase> partyMembers = PartyManager.instance.GetPartyMembers();

        // 파티 멤버의 직업에 따라 해당 UI만 투명도를 alpha로 설정
        foreach(CharacterBase member in partyMembers)
        {
            Image image = null;
            Button button = null;
            if(member.characterType == CharacterType.Warrior)
            {
                image = transform.GetChild(0).GetComponent<Image>();
                button = transform.GetChild(0).GetComponent<Button>();
            }
            else if(member.characterType == CharacterType.Ranger)
            {
                image = transform.GetChild(1).GetComponent<Image>();
                button = transform.GetChild(1).GetComponent<Button>();
            }
            else if(member.characterType == CharacterType.Wizard)
            {
                image = transform.GetChild(2).GetComponent<Image>();
                button = transform.GetChild(2).GetComponent<Button>();
            }

            if(image != null)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            }
            if(button != null)
            {
                button.enabled = isActive;
            }
        }
    }


    public void DeactivateSelectionUI()
    {
        // 선택지 UI 색상을 회색으로 변경하고 버튼 비활성화
        foreach(Transform child in transform)
        {
            Image image = child.GetComponent<Image>();
            Button button = child.GetComponent<Button>();
            if(image != null)
            {
                image.color = Color.gray;
            }
            if(button != null) 
            {
                button.enabled = false;
            }
        }
    }

    public void OnClickMeleeAttack()
    {
        // 근거리 공격
        Debug.Log("근거리 공격");
        PartyManager.instance.GetCharacterByType(CharacterType.Warrior).Attack(GameManager.instance.enemy);
        
    }

    public void OnClickRangedAttack()
    {
        // 원거리 공격
        Debug.Log("원거리 공격");
        PartyManager.instance.GetCharacterByType(CharacterType.Ranger).Attack(GameManager.instance.enemy);
    }

    public void OnClickMagicAttack()
    {
        // 마법 공격
        Debug.Log("마법 공격");
        PartyManager.instance.GetCharacterByType(CharacterType.Wizard).Attack(GameManager.instance.enemy);
    }
}
