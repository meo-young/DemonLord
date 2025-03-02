using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("카드 리소스")]
    public Sprite cardBack;
    public Sprite cardFront;

    [Header("뒷배경 리소스")]
    public Sprite battleBackground;
    public Sprite bossBackground;
    public Image backgroundImage;

    [Header("현재 주사위 결과")]
    [SerializeField] private DiceType currentDiceResult;
    public int currentDiceResultInt;

    [Header("주사위 버튼")]
    public Button diceButton;

    [Header("현재 권능")]
    [SerializeField] private TMP_Text warrantText;
    [SerializeField] private Button warrantButton;
    public bool isWarrantUsed = false; // 권능 사용 여부
    public bool isWarrantActive = false; // 권능 활성화 여부


    [Header("현재 특성")]
    [SerializeField] private IAbility currentAbility;
    [SerializeField] private TMP_Text abilityText;
    public int trapHandicap = 0;
    public int peerHandicap = 0;
    public bool isDamageHandicap = false;

    [Header("HP UI 프리팹")]
    public GameObject hpUIPrefab;

    [Header("묘비 이미지")]
    public Sprite tombstoneSprite;

    [Header("도적의 가호 아이템")]
    public bool isThiefItem = false;
    public TMP_Text thiefItemText;


    public Enemy enemy; // 현재 전투 중인 적 캐릭터
    public Player player; // 현재 플레이 중인 플레이어
    public BossEnemy boss; // 현재 전투 중인 보스 캐릭터
    
    private IWarrant currentWarrant;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // 변수 초기화
        currentDiceResult = DiceType.None;
        currentWarrant = null;
    }


#region 주사위
    // 주사위의 결과를 설정
    public void SetCurrentDiceResult(DiceType result)
    {
        currentDiceResult = result;
    }

    // 현재 주사위 결과 반환
    public DiceType GetCurrentDiceResult()
    {
        return currentDiceResult;
    }

    // 주사위 굴리기
    public void RollDice()
    {
        if(IsDiceRolled())
        {
            return;
        }
        else
        {
            currentDiceResult = Dice.instance.Roll();
        }
    }

    // 주사위 굴리기 여부 반환
    public bool IsDiceRolled()
    {
        return currentDiceResult != DiceType.None;
    }
#endregion

#region 권능
    // 현재 권능 설정
    public void SetCurrentWarrant(WarrantType warrantType)
    {
        // 기존 권능이 있다면 return
        if (currentWarrant != null)
        {
            return;
        }

        // 권능 타입에 따른 권능 추가
        switch (warrantType)
        {
            case WarrantType.Dice:
                currentWarrant = gameObject.AddComponent<DiceWarrant>();
                break;
            case WarrantType.Heal:
                currentWarrant = gameObject.AddComponent<HealWarrant>();
                break;
            case WarrantType.Random:
                currentWarrant = gameObject.AddComponent<RandomWarrant>();
                break;
        }

        warrantText.text = WarrantManager.instance.GetWarrantDescription(warrantType);
    }

    // 현재 권능 반환
    public IWarrant GetCurrentWarrant()
    {
        return currentWarrant;
    }

    // 권능 사용
    public void UseWarrant()
    {
        currentWarrant.UseWarrant();
    }   

    // 권능 사용 여부 설정
    public void SetActiveWarrant()
    {
        isWarrantUsed = true;
        isWarrantActive = true;
        warrantButton.interactable = false;
    }

    // 권능 사용 여부 해제
    public void SetInactiveWarrant()
    {
        isWarrantUsed = false;
        isWarrantActive = false;
        warrantButton.interactable = true;
    }

    // 권능 버튼 비활성화
    public void SetWarrantButtonDeactive()
    {
        warrantButton.interactable = false;
    }

    // 권능 버튼 활성화
    public void SetWarrantButtonActive()
    {
        warrantButton.interactable = true;
    }

    // 권능 버튼 비활성화 여부 설정
    public void SetWarrantButtonInteractable()
    {
        if(isWarrantUsed)
        {
            SetWarrantButtonDeactive();
        }
        else
        {
            SetWarrantButtonActive();
        }
    }

#endregion

#region 특성
    // 현재 특성 설정
    public void SetCurrentAbility(AbilityType abilityType)
    {
        // 기존 특성이 있다면 return
        if (currentAbility != null)
        {
            return;
        }

        // 특성 타입에 따른 특성 추가
        switch (abilityType)
        {
            case AbilityType.BattleMaster:
                currentAbility = gameObject.AddComponent<BattleMaster>();
                break;
            case AbilityType.PeerMaster:
                currentAbility = gameObject.AddComponent<PeerMaster>();
                break;
            case AbilityType.TrapMaster:
                currentAbility = gameObject.AddComponent<TrapMaster>();
                break;
        }

        abilityText.text = AbilityManager.instance.GetAbilityDescription(abilityType);

        currentAbility.UseAbility();
    }
#endregion

#region 도적의 가호 아이템
    // 도적의 가호 아이템 활성화
    public void GetThiefItem()
    {
        isThiefItem = true;
        thiefItemText.text = "활성화";
        Image img = thiefItemText.transform.parent.GetComponent<Image>();
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
    }
#endregion

#region 보스 이벤트 
    //@TODO : 보스 이벤트 처리
#endregion

#region 뒷배경 리소스
    // 뒷배경 리소스 설정
    public void SetBackground(Sprite background)
    {
        backgroundImage.sprite = background;
    }
#endregion
}