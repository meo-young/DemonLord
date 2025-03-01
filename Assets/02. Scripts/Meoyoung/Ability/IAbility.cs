using UnityEngine;

// 특성 종류
public enum AbilityType
{
    BattleMaster,
    PeerMaster,
    TrapMaster
}

public interface IAbility
{
    void UseAbility();
}
