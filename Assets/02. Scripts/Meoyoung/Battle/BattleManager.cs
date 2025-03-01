using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    private Queue<Action> eventQueue = new Queue<Action>();
    private bool isProcessingQueue = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start() {
        StartBattle();
    }

    public void StartBattle()
    {
        // 전투 대형으로 이동
        BattleFormation.instance.StartBattleFormation();
        ActiveBattle();
    }

    public void ActiveBattle()
    {
        // @TOdo Start Text 애니메이션
        // 

        SelectionUI.instance.SetSelectionUIAlpha(1.0f, true);
    }

    public void AddEventToQueue(Action action)
    {
        // 이벤트 큐에 이벤트 추가
        eventQueue.Enqueue(action);
    }

        
    public void StartEventProcess()
    {
        // 이벤트 큐가 비어있지 않고 이벤트 실행이 진행중이지 않으면 이벤트 실행
        if (!isProcessingQueue && eventQueue.Count > 0)
        {
            StartCoroutine(ProcessEventQueue());
        }
    }

    private IEnumerator ProcessEventQueue()
    {
        isProcessingQueue = true;
        
        while (eventQueue.Count > 0)
        {
            System.Action currentEvent = eventQueue.Dequeue();
            currentEvent?.Invoke();
            yield return new WaitForSeconds(2f);
        }

        Debug.Log("끝");
        isProcessingQueue = false;
    }
}
