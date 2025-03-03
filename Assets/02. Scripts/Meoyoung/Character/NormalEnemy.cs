using UnityEngine;

public class NormalEnemy : Enemy
{
    public override void AttackPlayer()
    {
        if(isAlive == false) return;

        // 기본 데미지 선언
        int damage = attackPower;

        // 주사위 굴리기
        GameManager.instance.RollDice();

        // 파티 멤버 중 상성이 안 좋은 캐릭터 반환
        CharacterBase target = PartyManager.instance.GetReverseCounterTypeCharacter(characterType);

        // 같은 타입의 캐릭터 탐색
        if(target == null)
        {
            Debug.Log("같은 타입의 캐릭터 탐색");
            target = PartyManager.instance.GetSameTypeCharacter(characterType);
        }

        // 상성인 캐릭터 탐색
        if(target == null)  
        {
            Debug.Log("상성인 캐릭터 탐색");
            target = PartyManager.instance.GetCounterTypeCharacter(characterType);
        }

        // 모든 캐릭터가 사망한 경우 플레이어 공격
        if(target == null)
        {
            Debug.Log("모든 캐릭터가 사망한 경우 플레이어 공격");
            target = GameManager.instance.player;
        }

        switch(characterType)
        {
            case CharacterType.Warrior:
                VFXManager.instance.ShowEffect(EffectType.Damage_Melee, target.transform.position);
                break;
            case CharacterType.Ranger:
                VFXManager.instance.ShowEffect(EffectType.Damage_Range, target.transform.position);
                break;
            case CharacterType.Wizard:
                VFXManager.instance.ShowEffect(EffectType.Damage_Magic, target.transform.position);
                break;
        }

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

        Debug.Log("적 사망");

        GameObject healthUI = transform.GetChild(0).gameObject;
        Destroy(healthUI);

        // 전투 승리
        BattleManager.instance.BattleWin();
    }

    public override void GetDamage(int damage)
    {
        // 공격 이펙트 출력
        switch(characterType)
        {
            case CharacterType.Warrior:
                VFXManager.instance.ShowEffect(EffectType.Energe_Melee, transform.position);
                break;
            case CharacterType.Ranger:
                VFXManager.instance.ShowEffect(EffectType.Energe_Range, transform.position);
                break;
            case CharacterType.Wizard:
                VFXManager.instance.ShowEffect(EffectType.Energe_Magic, transform.position);
                break;
        }

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

}
