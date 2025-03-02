using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class PartyInfoUI : MonoBehaviour
{
    public static PartyInfoUI instance;

    public GameObject characterInfoPrefab;
    

    private void Awake()
    {
        instance = this;
    }

    public void AddPartyInfo(CharacterBase character, int index)
    {
        // 캐릭터 프리팹 생성
        GameObject infoObject = Instantiate(characterInfoPrefab, transform);

        // UI 최신화
        UpdatePartyInfo(character, index);
    }

    public void UpdatePartyInfo(CharacterBase character, int index)
    {
        // index번째 자식 오브젝트 가져오기
        Transform childTransform = transform.GetChild(index);
        GameObject infoObject = childTransform.gameObject;
        Debug.Log("infoObject : " + infoObject.name);

        // 프리팹의 TMP_Text 컴포넌트 가져오기
        TMP_Text[] texts = infoObject.GetComponentsInChildren<TMP_Text>();

        // Text 컴포넌트 초기화
        texts[0].text = character.characterName;

        if(character.isAlive)
        {
            Debug.Log("생존 : " + character.characterName);
            texts[1].text = "생존";
            texts[1].color = Color.green;
        }
        else
        {
            Debug.Log("사망 : " + character.characterName);
            texts[1].text = "사망";
            texts[1].color = Color.red;
        }
    }

    public void DestroyPartyInfo()
    {
        // 파티 정보 UI 초기화
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }


    /// <summary>
    /// 모든 파티원의 정보를 최신화
    /// </summary>
    public void RefreshPartyInfo()
    { 
        List<CharacterBase> members = PartyManager.instance.GetPartyMembers();
        for (int i = 0; i < members.Count; i++)
        {
            Debug.Log("파티 정보 최신화 : " + members[i].characterName);
            UpdatePartyInfo(members[i], i);
        }
    }
}
