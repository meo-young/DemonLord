using UnityEngine;
using System.Collections;
using System;
using TMPro;
using UnityEngine.UI;

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


    [Header("주사위 결과 텍스트")]
    public TMP_Text diceResultText;

    [Header("주사위 버튼")]
    public Button diceButton;

    [Header("주사위 굴리는 애니메이션")]
    public Animator diceAnimator;

    private int diceResult;
    private Action diceResultEvent;

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



    /// <summary>
    /// 주사위를 굴리고 DiceType으로로 결과를 반환
    /// </summary>
    /// <returns>주사위 결과</returns>
    public DiceType Roll()
    {
        // 랜덤으로 주사위를 굴림
        diceResult = UnityEngine.Random.Range(2, 13);
        GameManager.instance.currentDiceResultInt = diceResult;

        // 특성에 따라 주사위 결과에 영향을 주는 핸디캡 적용
        int handicap = GameManager.instance.damageHandicap;

        // 결과에 따라 주사위 타입을 반환
        if(diceResult <= 4 - handicap)
        {
            return DiceType.Bad;
        }
        else if(diceResult <= 10 - handicap)
        {
            return DiceType.Normal;
        }
        else
        {
            return DiceType.Good;
        }
    }


    /// <summary>
    /// 주사위 굴린 후 호출할 이벤트 등록
    /// </summary>
    /// <param name="diceResultEvent">등록할 이벤트</param>
    public void AddDiceResultEvent(Action diceResultEvent)
    {
        // 이미 이벤트가 있으면 제거
        if(this.diceResultEvent != null)
        {
            this.diceResultEvent = null;
        }

        // 주사위 버튼 활성화
        diceButton.interactable = true;

        this.diceResultEvent += diceResultEvent;
    }



    /// <summary>
    /// 주사위 결과 텍스트 초기화
    /// </summary>
    public void InitDiceBtn()
    {
        diceResultText.rectTransform.localScale = Vector3.zero;
        diceResultText.text = "";

        diceButton.interactable = false;
    }



    /// <summary>
    /// 주사위를 굴리는 애니메이션 실행
    /// </summary>
    public void RollWithAnimation()
    {
        // 주사위 버튼 비활성화
        diceButton.enabled = false;

        // 선택지 UI 비활성화
        SelectionUI.instance.InitSelectionUI();

        // 권능 버튼 비활성화
        GameManager.instance.SetWarrantButtonDeactive();

        Roll();
        StartCoroutine(RollDice());
    }



    /// <summary>
    /// 주사위 굴리는 애니메이션 실행
    /// 1.5초 후 주사위 결과 텍스트 표시
    /// 0.5초 후 등록된 이벤트 호출
    /// </summary>
    private IEnumerator RollDice()
    {
        // 주사위 굴리는 애니메이션 실행
        diceAnimator.SetBool("Roll", true);

        yield return new WaitForSeconds(1.5f);

        // 주사위 결과 텍스트 표시
        SetDiceResultText(diceResult);

        // 주사위 굴리는 애니메이션 종료
        diceAnimator.SetBool("Roll", false);

        yield return new WaitForSeconds(0.5f);

        // 등록된 이벤트 호출
        diceResultEvent?.Invoke();

        // 주사위 버튼 활성화
        diceButton.enabled = true;
    }



    /// <summary>
    /// 주사위 결과 텍스트 설정
    /// </summary>
    /// <param name="diceResult">주사위 결과</param>
    public void SetDiceResultText(int diceResult)
    {
        diceResultText.rectTransform.localScale = Vector3.one;
        diceResultText.text = diceResult.ToString();
    }
}
