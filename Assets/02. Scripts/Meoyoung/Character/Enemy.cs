using UnityEngine;

public class Enemy : CharacterBase
{
    public virtual void AttackPlayer()
    {
    
    }

    public virtual void OnDeath()
    {
        base.OnDeath();
    }

    public virtual void GetDamage(int damage)
    {
        base.GetDamage(damage);
    }
}
