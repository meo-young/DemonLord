using UnityEngine;
using static Constant;

public class HealWarrant : MonoBehaviour, IWarrant
{

    public void UseWarrant()
    {
        Debug.Log("힐 권능 사용");

        // 체력이 가장 낮은 캐릭터를 healAmount만큼 회복
        PartyManager.instance.GetWorstHealthCharacter().Heal(HEAL_WARRANT_HEAL_AMOUNT);
    }
}
