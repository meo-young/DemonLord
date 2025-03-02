using UnityEngine;

public class Human : CharacterBase
{
    public void AttackEnemy(Enemy target)  
    {

        // 기본 데미지 선언
        int damage = attackPower;

        // 상성 적용
        Applycompatibility(target, ref damage);

        // 도적이 파티에 존재할 경우 추가 데미지
        ApplyAssassinDamage(ref damage);

        // 주사위 타입 적용
        ApplyDiceResult(ref damage);

        Debug.Log($"{characterName}의 공격 ! {target.characterName}에게 {damage}의 데미지 적용");

        // 공격 애니메이션 재생
        StartCoroutine(SetAttackState());

        // 데미지 적용
        target.GetDamage(damage);
    }

    protected override void OnDeath()
    {
        base.OnDeath();        
    }

}
