using UnityEngine;

public abstract class Enemy : CharacterBase
{
    public abstract void AttackPlayer();

    public virtual void OnDeath()
    {
        base.OnDeath();
    }

    public virtual void GetDamage(int damage)
    {
        base.GetDamage(damage);
    }
}
