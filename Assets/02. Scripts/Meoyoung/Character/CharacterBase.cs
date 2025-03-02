using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

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
    public int currentHealth;           // 현재 체력


    protected virtual void Start()
    {
        // 체력 초기화
        currentHealth = maxHealth;

        // 생존 여부 초기화
        isAlive = true;
    }


    /// <summary>
    /// 현재 캐릭터에게 damage 만큼 현재 체력을 감소소
    /// </summary>
    /// <param name="damage">적용할 데미지</param>
    public virtual void GetDamage(int damage)
    {
        // 데미지 적용
        currentHealth -= damage;

        // 체력 UI 최신화
        InitUI();
        
        // 체력이 0이하일 경우 사망 처리
        if(currentHealth <= 0)
        {
            OnDeath();
        }
    }


    /// <summary>
    /// 현재 캐릭터가 target 캐릭터에게 공격. 추상 클래스인데 설계 잘못함.
    /// </summary>
    /// <param name="target">공격할 대상</param>
    public virtual void Attack(CharacterBase target)
    {

    }


    /// <summary>
    /// 최대 체력을 초과하지 않는 범위에서 현재 체력을 amount 만큼 회복복
    /// </summary>
    /// <param name="amount">회복할 체력</param>
    public virtual void Heal(int amount)
    {
        // amount 만큼 체력 회복
        currentHealth += amount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }



    /// <summary>
    /// 캐릭터 부활시 호출. 쓸 일이 있을지 모르겠음음
    /// </summary>
    public void OnRebirth()
    {
        // 체력 초기화
        currentHealth = maxHealth;

        // 체력 UI 활성화
        transform.GetChild(0).gameObject.SetActive(true);

        // 체력 UI 최신화
        InitUI();
    }



    /// <summary>
    /// 체력 UI에 현재 체력을 반영하여여 최신화
    /// </summary>
    public void InitUI()
    {
        // 체력 UI 최신화
        TMP_Text text = GetComponentInChildren<TMP_Text>();
        text.text = $"{maxHealth} / {currentHealth}";

        Slider slider = GetComponentInChildren<Slider>();
        slider.value = ((float)currentHealth / (float)maxHealth);
    }



    /// <summary>
    /// 캐릭터 사망시 호출. 리소스를 묘비로 변경경
    /// </summary>
    protected virtual void OnDeath()
    {
        Debug.Log($"{characterName}이(가) 죽었습니다.");
        isAlive = false;

        // 캐릭터 이미지 묘비로 변경
        GetComponent<SpriteRenderer>().sprite = GameManager.instance.tombstoneSprite;

        // 체력 UI 비활성화
        transform.GetChild(0).gameObject.SetActive(false);
    }



    /// <summary>
    /// 상성일 경우 데미지 증가
    /// </summary>
    /// <param name="target">상성을 확인할 target 캐릭터</param>
    /// <param name="damage">데미지</param>
    protected void Applycompatibility(CharacterBase target, ref int damage)
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

    /// <summary>
    /// 주사위 타입에 따라 데미지 변화
    /// </summary>
    /// <param name="damage">데미지</param>
    protected void ApplyDiceResult(ref int damage)
    {
        // 주사위 타입에 따라 데미지 변화
        switch(GameManager.instance.GetCurrentDiceResult())
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

        // 주사위 눈 초기화
        GameManager.instance.SetCurrentDiceResult(DiceType.None);
    }

    /// <summary>
    /// 같은 직업이 있다면 추가 데미지 적용
    /// </summary>
    /// <param name="damage">데미지</param>
    protected void ApplySameCharacterTypeDamage(ref int damage)
    {
        // 같은 직업이 있다면 추가 데미지 적용
        if(PartyManager.instance.IsSameCharacterTypeInParty(characterType))
        {
            damage = (int)(damage * 1.5f);
        }
    }

    /// <summary>
    /// 도적이 파티에 존재할 경우 추가 데미지
    /// </summary>
    /// <param name="damage">데미지</param>
    protected void ApplyAssassinDamage(ref int damage)
    {
        // 도적이 파티에 존재할 경우 추가 데미지
        if(PartyManager.instance.IsSameCharacterTypeInParty(CharacterType.Assassin))
        {
            damage = (int)(damage * 1.5f);
        }
    }

    /// <summary>
    /// 공격 애니메이션 재생
    /// </summary>
    protected IEnumerator SetAttackState()
    {
        Debug.Log("공격 애니메이션 재생");
        GetComponent<Animator>().SetBool("Attack", true);
        
        yield return new WaitForSeconds(0.5f);
        
        GetComponent<Animator>().SetBool("Attack", false);
    }

    /// <summary>
    /// 캐릭터 데이터로 초기화하는 메서드 추가
    /// </summary>
    /// <param name="data">캐릭터 데이터</param>
    public virtual void SetCharacterData(CharacterData data)
    {
        characterName = data.characterName;
        characterType = data.characterType;
        maxHealth = data.maxHealth;
        attackPower = data.attackPower;
        currentHealth = maxHealth;
        isAlive = true;

        // 체력 UI 프리팹 생성
        GameObject healthUI = Instantiate(GameManager.instance.hpUIPrefab, transform);

        // 체력 최신화
        InitUI();
    }
}