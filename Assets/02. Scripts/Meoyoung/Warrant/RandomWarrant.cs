using UnityEngine;
using System.Collections.Generic;
using static Constant;

public class RandomWarrant : MonoBehaviour, IWarrant
{
    [SerializeField] private List<IWarrant> warrants;

    private void Start()
    {
        warrants = new List<IWarrant>();

        // 랜덤 권능 추가
        warrants.Add(new DiceWarrant());
        warrants.Add(new HealWarrant());
    }

    public void UseWarrant()
    {
        Debug.Log("랜덤 권능 사용");

        // 랜덤 확률 계산
        float randomValue = Random.Range(0f, 1f);

        // 모든 권능 사용
        if (randomValue < 0.1f)
        {
            foreach (var warrant in warrants)
            {
                warrant.UseWarrant();
            }
        }
        // 20% 확률로 DiceWarrant 사용
        else if (randomValue < 0.3f)
        {
            warrants[0].UseWarrant();
        }
        // 20% 확률로 HealWarrant 사용
        else if (randomValue < 0.5f)
        {
            warrants[1].UseWarrant();
        }
        // 40% 확률로 return
        else if (randomValue < 0.9f)
        {
            return;
        }
        // 나머지 10% 확률로 파티 전체에게 피해
        else
        {
            foreach (var member in PartyManager.instance.GetPartyMembers())
            {
                member.GetDamage(RANDOM_WARRANT_DAMAGE_AMOUNT);
            }
        }
    }   
}
