using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PartyInfoUI : MonoBehaviour
{
    public static PartyInfoUI instance;

    public GameObject characterInfoPrefab;
    

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

        // 프리팹의 TMP_Text 컴포넌트 가져오기
        TMP_Text[] texts = infoObject.GetComponentsInChildren<TMP_Text>();

        // Text 컴포넌트 초기화
        texts[0].text = character.characterName;
        texts[1].text = $"{character.maxHealth} / {character.currentHealth}";

        if(character.isAlive)
        {
            texts[2].text = "생존";
            texts[2].color = Color.green;
        }
        else
        {
            texts[2].text = "사망";
            texts[2].color = Color.red;
        }

        // 슬라이더 초기화
        Slider slider = infoObject.GetComponentInChildren<Slider>();
        slider.maxValue = character.maxHealth;
        slider.value = (float)(character.maxHealth / character.currentHealth)*100;

        Debug.Log($"슬라이더 초기화: {slider.value}");
    }

    public void DestroyPartyInfo()
    {
        // 파티 정보 UI 초기화
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
