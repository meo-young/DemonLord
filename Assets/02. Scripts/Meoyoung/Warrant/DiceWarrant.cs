using UnityEngine;

public class DiceWarrant : MonoBehaviour, IWarrant
{
    public void UseWarrant()
    {
        Debug.Log("주사위 권능 사용");
        
        // 주사위를 가장 좋은 결과로 설정
        GameManager.instance.currentDiceResultInt = 12;
        Dice.instance.SetDiceResultText(GameManager.instance.currentDiceResultInt);
    }
}
