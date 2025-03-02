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

    [Header("하단 동료모집 패널")]
    [SerializeField] private GameObject bottomRecruitPanel;

    [Header("캐릭터 이미지")]
    [SerializeField] private GameObject[] characterImages;
    [Header("캐릭터 데이터")]
    [SerializeField] private CharacterData[] characterDatas;

    [Header("모집하기 버튼")]
    [SerializeField] private Button recruitBtn;
    [Header("모집 텍스트")]
    [SerializeField] private TMP_Text recruitResultText;
    [SerializeField] private TMP_Text recruitResultSubText;

    [Header("추가되는 위치")]
    [SerializeField] private GameObject[] addPositions;

    private int currentCharacterIndex = 0;
    private int recruitCount = 0;        // 현재 모집 횟수



    /// <summary>
    /// 캐릭터 이미지 선택 시, 모집하기 버튼 활성화 및 클릭된 캐릭터 이미지 활성화
    /// </summary>
    /// <param name="index"> 클릭된 캐릭터 이미지 인덱스 </param>
    public void OnClickCharacterBtn(int index)
    {
        // 모든 캐릭터 이미지 비활성화
        DeactivateCharacterImages();

        // 버튼 활성화
        recruitBtn.enabled = true;

        // 클릭된 캐릭터 인덱스 저장
        currentCharacterIndex = index;

        // 클릭된 캐릭터 이미지 활성화
        characterImages[index].transform.localScale = Vector3.one;

        Dice.instance.AddDiceResultEvent(OnClickRecruitBtn);
    }



    /// <summary>
    /// 모집하기 버튼 클릭 시, 모집 처리
    /// </summary>
    public void OnClickRecruitBtn()
    {
        // 클릭된 캐릭터 이미지 비활성화
        characterImages[currentCharacterIndex].transform.localScale = Vector3.zero;

        // 모집 결과 확인
        GetRecruitResult();

        SelectionBtnDeactivate();

        SelectionUI.instance.InitSelectionUI();
        
        // 모집 횟수 증가
        recruitCount++;

        // 버튼 초기화
        InitBtn();
    }



    /// <summary>
    /// 동료모집 활성화
    /// </summary>
    public void ShowRecruitPanel()
    {
        AudioManager.instance.PlayBGM(BGM.bgm_Recruit);

        // 모집 횟수 초기화
        recruitCount = 0;

        // 선택지 버튼 활성화
        SelectionBtnActivate();

        // 모집 패널 활성화
        transform.localScale = Vector3.one;
        bottomRecruitPanel.transform.localScale = Vector3.one;

        // 공격 선택지 패널 비활성화
        SelectionUI.instance.transform.localScale = Vector3.zero;

        // 버튼 초기화
        InitBtn();
    }



    /// <summary>
    /// 동료모집 비활성화
    /// </summary>
    public void HideRecruitPanel()
    {
        // 모집 패널 비활성화
        transform.localScale = Vector3.zero;
        bottomRecruitPanel.transform.localScale = Vector3.zero;
    }

    


    /// <summary>
    /// 버튼 초기화
    /// </summary>
    private void InitBtn()
    {
        // 항상 1명씩만 모집
        int remainingRecruits = 1 - recruitCount;
        SituationUI.instance.SetSituationText($"남은 동료 모집 횟수 {remainingRecruits}명");

        // 파티 멤버 가져오기
        List<CharacterBase> partyMembers = PartyManager.instance.GetPartyMembers();

        foreach(CharacterBase member in partyMembers)
        {
            if(member.characterType == CharacterType.Ranger)
            {
                transform.GetChild(0).transform.localScale = Vector3.zero;
            }
            // 파티에 마법사가 존재할 경우 마법 모집 비활성화
            else if(member.characterType == CharacterType.Wizard)
            {
                transform.GetChild(1).transform.localScale = Vector3.zero;
            }
        }

        // 버튼 비활성화
        DeactivateCharacterImages();
        recruitBtn.interactable = false;
    }



    /// <summary>
    /// 캐릭터 이미지 비활성화
    /// </summary>
    private void DeactivateCharacterImages()
    {
        foreach(GameObject image in characterImages)
        {
            image.transform.localScale = Vector3.zero;
        }
    }


    /// <summary>
    /// 선택지 버튼 비활성화
    /// </summary>
    public void SelectionBtnDeactivate()
    {
        int childCount = transform.childCount;
        for(int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }
    
    /// <summary>
    /// 선택지 버튼 활성화
    /// </summary>
    private void SelectionBtnActivate()
    {
        int childCount = transform.childCount;
        for(int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).GetComponent<Button>().interactable = true;
        }
    }


    /// <summary>
    /// 모집 성공 텍스트 활성화
    /// </summary>
    private void SuccessRecruit()
    {   
        // 동료 모집
        Recruit();

        recruitResultText.gameObject.SetActive(false);
        recruitResultSubText.gameObject.SetActive(false);

        // 모집 성공 텍스트 활성화
        recruitResultText.gameObject.SetActive(true);
        recruitResultText.text = "성공 !";

        // 모집 성공 텍스트 활성화
        recruitResultSubText.gameObject.SetActive(true);
        recruitResultSubText.text = "동료 모집에 성공했습니다 !";
    }


    /// <summary>
    /// 모집 실패 텍스트 활성화
    /// </summary>
    private void FailRecruit()
    {
        recruitResultText.gameObject.SetActive(false);
        recruitResultSubText.gameObject.SetActive(false);

        // 모집 실패 텍스트 활성화
        recruitResultText.gameObject.SetActive(true);
        recruitResultText.text = "실패 !";

        // 모집 실패 텍스트 활성화
        recruitResultSubText.gameObject.SetActive(true);
        recruitResultSubText.text = "동료 모집에 실패했습니다.";
    }



    /// <summary>
    /// 동료 모집
    /// </summary>
    private void Recruit()
    {
        if (currentCharacterIndex >= 0 && currentCharacterIndex < characterDatas.Length)
        {
            CharacterData selectedData = characterDatas[currentCharacterIndex];
            int currentPartySize = PartyManager.instance.GetPartyMemberNum();
            
            // 현재 파티 크기를 인덱스로 사용하여 위치 지정
            GameObject positionObject = addPositions[currentPartySize - 1];
            
            // CharacterBase 컴포넌트 추가 및 데이터 설정
            Human newCharacter = positionObject.AddComponent<Human>();
            newCharacter.SetCharacterData(selectedData);
            
            // 스프라이트 이미지 설정
            SpriteRenderer spriteRenderer = positionObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = positionObject.AddComponent<SpriteRenderer>();
            }
            spriteRenderer.sprite = selectedData.characterSprite;
            
            // 파티에 멤버 추가
            PartyManager.instance.AddMember(newCharacter);
        }
    }




    /// <summary>
    /// 모집 결과 확인
    /// </summary>
    private void GetRecruitResult()
    {
        int diceValue = GameManager.instance.currentDiceResultInt;
        int partySize = PartyManager.instance.GetPartyMemberNum();

        int peerHandicap = GameManager.instance.peerHandicap;

        bool isSuccess = false;

        Debug.Log("Party Size : " + partySize);
        Debug.Log("Dice Value : " + diceValue);
        Debug.Log("Peer Handicap : " + peerHandicap);

        switch (partySize)
        {
            case 1:
                isSuccess = diceValue >= 7 - peerHandicap;
                break;
            case 2:
                isSuccess = diceValue >= 9 - peerHandicap;
                break;
            case 3:
                isSuccess = diceValue >= 10 - peerHandicap;
                break;
        }

        recruitResultText.gameObject.SetActive(true);
        recruitResultSubText.gameObject.SetActive(true);

        if (isSuccess)
        {
            AudioManager.instance.PlaySFX(SFX.sfx_Recruit_Success);
            SuccessRecruit();
            SituationUI.instance.SetSituationText("동료 모집에 성공했습니다 !");
        }
        else
        {
            AudioManager.instance.PlaySFX(SFX.sfx_Recruit_Fail);
            FailRecruit();
            SituationUI.instance.SetSituationText("동료 모집에 실패했습니다.");
        }
    }

}
