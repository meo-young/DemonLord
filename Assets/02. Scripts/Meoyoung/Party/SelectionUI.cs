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


    public void ShowSelectionUI()
    {
        transform.localScale = Vector3.one;
    }


    public void HideSelectionUI()
    {
        transform.localScale = Vector3.zero;
    }



    /// <summary>
    /// 존재하지 않는 캐릭터 타입에 대해선 UI 회색처리
    /// 존재한 캐릭터 타입에 대해선 UI 빨간색, 투명도 0.1f로 설정
    /// </summary>
    public void InitSelectionUI()
    {
        DeactivateSelectionUI();

        List<CharacterBase> partyMembers = PartyManager.instance.GetAlivePartyMembers();

        foreach(CharacterBase member in partyMembers)
        {
            Image image = GetImageByCharacterType(member.characterType);
            if(image != null)
            {
                Color color = Color.red;
                color.a = 0.1f;
                image.color = color;
            }

            Button button = image?.GetComponent<Button>();
            if(button != null)
            {
                button.enabled = false;
            }
        }
    }



    /// <summary>
    /// 존재하고 살아있는 캐릭터 타입에 대한 UI만 활성화
    /// </summary>
    public void SetActiveSelectionUI()
    {
        // 살아있는 파티 멤버 반환
        List<CharacterBase> partyMembers = PartyManager.instance.GetAlivePartyMembers();

        // 해당되는 UI만 활성화
        foreach(CharacterBase member in partyMembers)
        {
            Image image = GetImageByCharacterType(member.characterType);
            Button button = image?.GetComponent<Button>();

            if(image != null)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
            }
            if(button != null)
            {
                button.enabled = true;
            }
        }
    }



    /// <summary>
    /// 선택지 UI 모두 비활성화
    /// </summary>
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



    /// <summary>
    /// 공격 버튼 클릭 이벤트
    /// </summary>
    /// <param name="characterTypeIndex"> 공격을 진행할 캐릭터 인덱스</param>
    public void OnClickAttackBtn(int characterTypeIndex)
    {
        // int를 CharacterType enum으로 변환
        Human human = PartyManager.instance.GetCharacterByType((CharacterType)characterTypeIndex) as Human;
        
        // 파티 공격 이벤트 추가
        BattleManager.instance.AddEventToQueue(
            () => human.AttackEnemy(GameManager.instance.enemy)
            );

        // 적 공격 이벤트 추가
        BattleManager.instance.AddEventToQueue(
            () => GameManager.instance.enemy.AttackPlayer()
            );

        // 이벤트 큐 처리
        BattleManager.instance.StartEventProcess();
    }



    /// <summary>
    /// 캐릭터 타입에 따른 이미지 반환
    /// </summary>
    /// <param name="type"> 캐릭터 타입</param>
    /// <returns> 캐릭터 타입에 따른 이미지</returns>
    private Image GetImageByCharacterType(CharacterType type)
    {
        int childIndex = type switch
        {
            CharacterType.Warrior => 0,
            CharacterType.Ranger => 1,
            CharacterType.Wizard => 2,
            _ => -1
        };

        return childIndex >= 0 ? transform.GetChild(childIndex).GetComponent<Image>() : null;
    }
}
