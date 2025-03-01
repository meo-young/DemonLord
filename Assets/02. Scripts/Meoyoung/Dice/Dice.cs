using UnityEngine;

/* 주사위의 결과는 다음과 같은 타입으로 나온다.
* 2~4 : Bad
* 5~10 : Normal
* 11~12 : Good */
public enum DiceType
{
    Bad,
    Normal,
    Good,
    None
}

public class Dice : MonoBehaviour
{
    public static Dice instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public DiceType Roll()
    {
        // 랜덤으로 주사위를 굴림
        int result = Random.Range(2, 13);

        // 결과에 따라 주사위 타입을 반환
        if(result >= 2 && result <= 4)
        {
            return DiceType.Bad;
        }
        else if(result >= 5 && result <= 10)
        {
            return DiceType.Normal;
        }
        else
        {
            return DiceType.Good;
        }
    }
}
