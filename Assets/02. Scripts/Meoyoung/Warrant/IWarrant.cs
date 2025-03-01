using UnityEngine;

public enum WarrantType
{
    Dice,
    Heal,
    Random
}

public interface IWarrant
{
    void UseWarrant();
}
