using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class PartyRecruitUI : MonoBehaviour
{
    public static PartyRecruitUI instance;

    private void Awake()
    {
        instance = this;
    }   

    [Header("캐릭터 이미지")]
    [SerializeField] private GameObject[] characterImages;
    [Header("캐릭터 데이터")]
    [SerializeField] private CharacterData[] characterDatas;

    [Header("모집하기 버튼")]
    [SerializeField] private Button recruitBtn;
    [Header("모집 텍스트")]
    [SerializeField] private TMP_Text recruitResultText;
    [SerializeField] private TMP_Text recruitResultSubText;

    private int currentCharacterIndex = 0;
    private bool isFirstRecruit = true;  // 초기 모집 여부
    private int recruitCount = 0;        // 현재 모집 횟수

    private void Start() 
    {
        DeactivateCharacterImages();
        ShowRecruitPanel();
        isFirstRecruit = true;   // 시작할 때 초기 모집으로 설정
        recruitCount = 0;        // 모집 횟수 초기화
    }

    public void OnClickCharacterBtn(int index)
    {
        // 모든 캐릭터 이미지 비활성화
        DeactivateCharacterImages();

        // 버튼 활성화
        SetRecruitButtonTransparency(1f); // 시작 시 버튼 투명도 설정
        recruitBtn.enabled = true;

        // 클릭된 캐릭터 인덱스 저장
        currentCharacterIndex = index;

        // 클릭된 캐릭터 이미지 활성화
        characterImages[index].transform.localScale = Vector3.one;
    }

    public void OnClickRecruitBtn()
    {
        if (currentCharacterIndex >= 0 && currentCharacterIndex < characterDatas.Length)
        {
            CharacterData selectedData = characterDatas[currentCharacterIndex];
            
            // 새로운 GameObject 생성
            GameObject characterObject = new GameObject(selectedData.characterName);
            
            // CharacterBase 컴포넌트 추가 및 데이터 설정
            CharacterBase newCharacter = characterObject.AddComponent<CharacterBase>();
            newCharacter.SetCharacterData(selectedData);
            
            // 파티에 멤버 추가
            PartyManager.instance.AddMember(newCharacter);
        }

        // 클릭된 캐릭터 이미지 비활성화
        characterImages[currentCharacterIndex].transform.localScale = Vector3.zero;

        IsSuccessRecruit();
        
        // 모집 횟수 증가
        recruitCount++;

        // 초기 모집시 2명, 이후 1명씩 모집
        if ((isFirstRecruit && recruitCount >= 2) || (!isFirstRecruit && recruitCount >= 1))
        {
            HideRecruitPanel();
            recruitCount = 0;    // 모집 횟수 초기화
            isFirstRecruit = false;  // 초기 모집 완료 표시
        }

        // 버튼 초기화
        InitBtn();
    }

    public void ShowRecruitPanel()
    {
        // 모집 패널 활성화
        transform.localScale = Vector3.one;

        // 공격 선택지 패널 비활성화
        SelectionUI.instance.transform.localScale = Vector3.zero;

        InitBtn();
    }

    public void HideRecruitPanel()
    {
        // 모집 패널 비활성화
        transform.localScale = Vector3.zero;

        // 공격 선택지 패널 활성화
        SelectionUI.instance.transform.localScale = Vector3.one;
    }

    


    private void InitBtn()
    {
        // 초기 모집일 때는 2명, 이후에는 1명씩 모집
        int remainingRecruits = isFirstRecruit ? 2 - recruitCount : 1 - recruitCount;
        SituationUI.instance.SetSituationText($"남은 동료 모집 횟수 {remainingRecruits}명");

        // 파티 멤버 가져오기 (용사 제외)
        List<CharacterBase> partyMembers = PartyManager.instance.GetPartyMembers().Where(member => member.characterName != "용사").ToList();

        foreach(CharacterBase member in partyMembers)
        {
            // 파티에 근거리가 존재할 경우 근거리 공격 선택 활성화
            if(member.characterType == CharacterType.Warrior)
            {
                transform.GetChild(0).transform.localScale = Vector3.zero;
            }
            // 파티에 원거리가 존재할 경우 원거리 공격 선택 활성화
            else if(member.characterType == CharacterType.Ranger)
            {
                transform.GetChild(1).transform.localScale = Vector3.zero;
            }
            // 파티에 마법사가 존재할 경우 마법 공격 선택 활성화
            else if(member.characterType == CharacterType.Wizard)
            {
                transform.GetChild(2).transform.localScale = Vector3.zero;
            }
        }

        // 버튼 비활성화
        SetRecruitButtonTransparency(0.2f);
        recruitBtn.enabled = false;
    }

    private void DeactivateCharacterImages()
    {
        foreach(GameObject image in characterImages)
        {
            image.transform.localScale = Vector3.zero;
        }
    }

    // 버튼 투명도 설정 함수 추가
    private void SetRecruitButtonTransparency(float alpha)
    {
        Image buttonImage = recruitBtn.GetComponent<Image>();
        if (buttonImage != null)
        {
            Color color = buttonImage.color;
            color.a = alpha; // 알파값 설정 (0.6f = 60% 투명도)
            buttonImage.color = color;
        }
    }

    private void IsSuccessRecruit()
    {
        recruitResultText.gameObject.SetActive(false);
        recruitResultSubText.gameObject.SetActive(false);

        // 모집 성공 텍스트 활성화
        recruitResultText.gameObject.SetActive(true);
        recruitResultText.text = "성공 !";

        // 모집 성공 텍스트 활성화
        recruitResultSubText.gameObject.SetActive(true);
        recruitResultSubText.text = "동료 모집에 성공했습니다 !";
    }

}
