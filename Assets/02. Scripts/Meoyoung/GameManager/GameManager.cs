using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("현재 주사위 결과")]
    [SerializeField] private DiceType currentDiceResult;
    public int currentDiceResultInt;

    [Header("주사위 버튼")]
    public Button diceButton;

    [Header("현재 권능")]
    [SerializeField] private IWarrant currentWarrant;

    [Header("현재 특성")]
    [SerializeField] private IAbility currentAbility;

    [Header("HP UI 프리팹")]
    public GameObject hpUIPrefab;

    [Header("묘비 이미지")]
    public Sprite tombstoneSprite;

    public Enemy enemy; // 현재 전투 중인 적 캐릭터

    public Player player; // 현재 플레이 중인 플레이어


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

        Debug.Log("현재 권능: " + currentWarrant.GetType().Name);
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
        
        
    }
#endregion
}