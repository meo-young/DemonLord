using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class AbilityWarrantUI : MonoBehaviour
{
    public static AbilityWarrantUI instance;

    public Image card_1;
    public Image card_2;
    public Image card_3;
    public Image card_4;

    public Sprite focusCardFirst;
    public Sprite focusCardSecond;
    public Sprite backCardFirst;
    public Sprite backCardSecond;

    private WarrantUI warrantUI;
    private AbilityUI abilityUI;
    private Queue<Action> eventQueue = new Queue<Action>(); // 이벤트 처리 순서를 저장하는 큐

    private void Awake()
    {
        instance = this;
        
        warrantUI = FindFirstObjectByType<WarrantUI>();
        abilityUI = FindFirstObjectByType<AbilityUI>();
    }



    /// <summary>
    /// 1번째 이벤트 : 특성 UI 비활성화, 권능 UI 활성화
    /// 2번째 이벤트 : 권능 UI 비활성화, 캔버스 비활성화, 동료 모집 UI 활성화
    /// </summary>
    private void Start()
    {
        eventQueue.Enqueue(
            () => {
                
                card_1.sprite = backCardFirst;
                card_2.sprite = backCardSecond;
                card_3.sprite = focusCardFirst;
                card_4.sprite = focusCardSecond;

                
                abilityUI.transform.localScale = Vector3.zero;
                warrantUI.transform.localScale = Vector3.one;
            }
        );

        eventQueue.Enqueue(
            () => {
                warrantUI.transform.localScale = Vector3.zero;
                this.gameObject.SetActive(false);
                PartyRecruitUI.instance.ShowRecruitPanel();
            }
        );
    }



    /// <summary>
    /// 이벤트 큐에서 이벤트를 1개씩 꺼내는 함수수
    /// </summary>
    public void ProcessEventQueue()
    {
        if(eventQueue.Count > 0)
        {
            System.Action currentEvent = eventQueue.Dequeue();
            currentEvent?.Invoke();
        }
    }

}
