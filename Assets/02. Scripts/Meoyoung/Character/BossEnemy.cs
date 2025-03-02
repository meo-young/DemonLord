using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BossEnemy : Enemy
{
    public override void AttackPlayer()
    {
        if(isAlive == false) return;

        // 기본 데미지 선언
        int damage = attackPower;

        // 랜덤 캐릭터 타입 선언
        SetRandomCharacterType();

        // 주사위 굴리기
        GameManager.instance.RollDice();

        // 랜덤으로 타겟 할당
        CharacterBase target = SetRandomTarget();

        // 상성 적용
        Applycompatibility(target, ref damage);

        // 주사위 타입 적용
        ApplyDiceResult(ref damage);

        Debug.Log($"{characterName}의 공격 ! {target.characterName}에게 {damage}의 데미지 적용");

        // 공격 애니메이션 재생
        StartCoroutine(SetAttackState());

        // 데미지 적용
        target.GetDamage(damage);
    }

    public override void OnDeath()
    {
        base.OnDeath();

        Debug.Log("보스 사망");

        GameObject healthUI = transform.GetChild(0).gameObject;
        Destroy(healthUI);

        // 전투 승리
        BattleManager.instance.BattleWin();
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);

        // 데미지 적용
        currentHealth -= damage;

        // 체력 UI 최신화
        InitUI();
        
        // 체력이 0이하일 경우 사망 처리
        if(currentHealth <= 0)
        {
            OnDeath();
        }

        Debug.Log("적 데미지 적용");
    }



    /// <summary>
    /// 랜덤 캐릭터 타입 선언
    /// </summary>
    private void SetRandomCharacterType()
    {
        int randomValue = Random.Range(0, 4);

        switch(randomValue)
        {
            case 0:
            //@TODO : 각 캐릭터 타입에 따른 VFX 출력
                characterType = CharacterType.Warrior;
                break;
            case 1:
                characterType = CharacterType.Ranger;
                break;
            case 2:
                characterType = CharacterType.Wizard;
                break;
        }
    }



    /// <summary>
    /// 플레이어를 제외한 파티원 중 랜덤으로 타겟을 할당
    /// 만약 파티원이 모두 사망한 경우 플레이어를 타겟으로 할당
    /// </summary>
    /// <returns>랜덤 타겟</returns>
    private CharacterBase SetRandomTarget()
    {
        // Warrior를 제외한 살아있는 파티원 목록 가져오기
        List<CharacterBase> availableTargets = PartyManager.instance.GetAlivePartyMembers()
            .Where(member => member.characterType != CharacterType.Warrior)
            .ToList();

        CharacterBase target;
        
        if (availableTargets.Count > 0)
        {
            // 랜덤으로 타겟 선택
            int randomIndex = Random.Range(0, availableTargets.Count);
            target = availableTargets[randomIndex];
        }
        else
        {
            // Warrior 타입 캐릭터 선택
            target = PartyManager.instance.GetCharacterByType(CharacterType.Warrior);
            
            // Warrior도 없다면 플레이어 공격
            if (target == null || !target.isAlive)
            {
                target = GameManager.instance.player;
            }
        }

        return target;
    }
}
