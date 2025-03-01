using UnityEngine;

/* 각 직업은 다음과 같은 특성을 가진다.
* 전사 : 마법사에게 추가 데미지
* 궁수 : 전사에게 추가 데미지
* 마법사 : 궁수에게 추가 데미지
* 도적 : 별도의 특수 능력 */
public enum CharacterType
{
    Warrior,    // 전사
    Ranger,     // 궁수
    Wizard,     // 마법사
    Assassin    // 도적
}

public class CharacterBase : MonoBehaviour
{
    public string characterName;         // 캐릭터 이름 
    public int maxHealth;                // 최대 체력
    public int attackPower;              // 공격력
    public CharacterType characterType;  // 캐릭터 직업
    public bool isAlive;                 // 생존 여부
    public int currentHealth;  // 현재 체력
    private Dice dice;          // 주사위 클래스

    private void Awake() 
    {
        dice = GetComponent<Dice>();
    }

    protected virtual void Start()
    {
        // 체력 초기화
        currentHealth = maxHealth;

        // 생존 여부 초기화
        isAlive = true;
    }

    public virtual void Attack(CharacterBase target)
    {
        // 공격 대상이 존재하지 않으면 종료
        if(target == null) return;

        // 기본 데미지 선언
        int damage = attackPower;

        // 주사위 굴리기
        DiceType diceType = dice.Roll();
        
        // @TODO : 같은 직업이 있다면 추가 데미지 적용
        //if(GameManager.instance.party.Find(character => character.characterType == characterType))

        // 상성 적용
        Applycompatibility(target, ref damage);

        // @TODO : 도적이 파티에 존재할 경우 추가 데미지. 게임 매니저에서 도적 여부를 체크하면 될 것 같다.

        // 주사위 타입 적용
        ApplyDiceResult(diceType, ref damage);

        Debug.Log($"{characterName}의 공격 ! {target.characterName}에게 {damage}의 데미지 적용");

        // 데미지 적용
        target.currentHealth -= damage;

        // 체력이 0이하일 경우 사망 처리
        if(target.currentHealth <= 0)
        {
            target.OnDeath();
        }
    }

    protected virtual void OnDeath()
    {
        Debug.Log($"{characterName}이(가) 죽었습니다.");
        isAlive = false;
    }

    private void Applycompatibility(CharacterBase target, ref int damage)
    {
        // 각 직업별 상성 처리
        switch(characterType)
        {
            case CharacterType.Warrior:
                if(target.characterType == CharacterType.Wizard)
                {
                    Debug.Log($"{characterType} > {target.characterType} 상성 적용");
                    damage = (int)(damage * 1.5f);
                }
                else if(target.characterType == CharacterType.Ranger)
                {
                    Debug.Log($"{characterType} < {target.characterType} 상성 적용");
                    damage = (int)(damage * 0.5f);
                }
                break;
            case CharacterType.Ranger:
                if(target.characterType == CharacterType.Warrior)
                {
                    Debug.Log($"{characterType} > {target.characterType} 상성 적용");
                    damage = (int)(damage * 1.5f);
                }
                else if(target.characterType == CharacterType.Wizard)
                {
                    Debug.Log($"{characterType} < {target.characterType} 상성 적용");
                    damage = (int)(damage * 0.5f);
                }
                break;
            case CharacterType.Wizard:
                if(target.characterType == CharacterType.Ranger)
                {
                    Debug.Log($"{characterType} > {target.characterType} 상성 적용");
                    damage = (int)(damage * 1.5f);
                }
                else if(target.characterType == CharacterType.Warrior)
                {
                    Debug.Log($"{characterType} < {target.characterType} 상성 적용");
                    damage = (int)(damage * 0.5f);
                }
                break;
            case CharacterType.Assassin:
                // @TODO : 도적의 특수 능력 추가
                break;
        }
    }

    private void ApplyDiceResult(DiceType diceType, ref int damage)
    {
        // 주사위 타입에 따라 데미지 변화
        switch(diceType)
        {
            case DiceType.Bad:
                Debug.Log("주사위 망함");
                damage = (int)(damage * 0.5f);
                break;
            case DiceType.Normal:
                Debug.Log("주사위 보통");
                damage = (int)(damage * 1.0f);
                break;
            case DiceType.Good:
                Debug.Log("주사위 좋음");
                damage = (int)(damage * 1.5f);
                break;
        }
    }
}
