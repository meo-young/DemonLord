using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    [Header("스테이지 데이터")]
    [SerializeField] private List<StageData> stageDataList;

    private Queue<StageData> stageDataQueue = new Queue<StageData>(); // 스테이지 데이터 큐
    private Queue<Action> eventQueue = new Queue<Action>(); // 이벤트 처리 순서를 저장하는 큐
    private bool isProcessingQueue = false; // 이벤트 큐 처리 여부

    private EnemySpawner enemySpawner;
    private BattleFormation battleFormation;

    private bool isBattle = false;

    private void Awake()
    {
        instance = this;

        enemySpawner = GetComponent<EnemySpawner>();
        battleFormation = FindFirstObjectByType<BattleFormation>();
    }



    private void Start() {
        // 스테이지 데이터 큐 초기화
        stageDataQueue = new Queue<StageData>(stageDataList);
        
        StartBattle();
    }


    /// <summary>
    /// 새로운 스테이지 시작 시 호출
    /// </summary>
    public void StartBattle()
    {
        // 애니메이션 초기화 용.. 야메 ..
        battleFormation.gameObject.SetActive(false);
        battleFormation.gameObject.SetActive(true);

        // Fade In 연출로 전투 시작
        FadeManager.instance.FadeIn();

        // 다음 스테이지 데이터 처리
        ProcessNextStageData();

        // 대형을 모두 갖춘 경우 선택지 UI 활성화
        battleFormation.StartBattleFormation();

        // 배틀 여부 활성화
        isBattle = true;
    }



    /// <summary>
    /// 선택지를 클릭해서 공격이 시작된 경우 호출출
    /// </summary>
    public void DeactiveBattle()
    {
        // 선택지 UI 비활성화
        SelectionUI.instance.InitSelectionUI();
    }



    /// <summary>
    /// 플레이어 턴이 돌아왔을 때 호출
    /// </summary>
    public void ActiveBattle()
    {
        // 배틀 중이 아니면 return
        if(isBattle == false) return;

        // 선택지 UI 활성화
        SelectionUI.instance.InitSelectionUI();
        SelectionUI.instance.SetActiveSelectionUI();
    }



    /// <summary>
    /// 전투 승리 시 호출
    /// </summary>
    public void BattleWin()
    {
        // 배틀 여부 비활성화
        isBattle = false;

        // 승리 화면 표시
        //BattleWinUI.instance.ShowWinUI();

        // 캐릭터 앞으로 이동
        battleFormation.EndBattleFormation();
    }


    public void BattleLose()
    {
        // 배틀 여부 비활성화
        isBattle = false;


        // 패배 화면 표시
        //BattleLoseUI.instance.ShowLoseUI();
        DeactiveBattle();
    }



    /// <summary>
    /// 이벤트 큐에 이벤트 추가
    /// </summary>
    /// <param name="action"> 추가할 이벤트</param>
    public void AddEventToQueue(Action action)
    {
        // 이벤트 큐에 이벤트 추가
        eventQueue.Enqueue(action);
    }



    /// <summary>
    /// 이벤트 큐에 이벤트가 존재한다면 처리 시작
    /// </summary>
    public void StartEventProcess()
    {
        // 이벤트 큐가 비어있지 않고 이벤트 실행이 진행중이지 않으면 이벤트 실행
        if (!isProcessingQueue && eventQueue.Count > 0)
        {
            StartCoroutine(ProcessEventQueue());
        }
    }



    /// <summary>
    /// 다음 스테이지 데이터 처리
    /// </summary>
    private void ProcessNextStageData()
    {
        if (stageDataQueue.Count > 0)
        {
            // 현재 스테이지 정보 가져오기
            StageData currentStage = stageDataQueue.Dequeue();

            // 스테이지 정보 UI 설정
            StageInfoUI.instance.SetStageInfo(currentStage.stageNumber);

            // 스테이지 이벤트 체크
            CheckStageEvent(currentStage);
        }
        else
        {
            Debug.Log("모든 스테이지 완료");
        }
    }



    /// <summary>
    /// 이벤트 큐에서 1초 간격으로 이벤트 처리
    /// </summary>
    private IEnumerator ProcessEventQueue()
    {
        isProcessingQueue = true;

        DeactiveBattle();
        
        while (eventQueue.Count > 0)
        {
            yield return new WaitForSeconds(1f);
            System.Action currentEvent = eventQueue.Dequeue();
            currentEvent?.Invoke();
        }

        yield return new WaitForSeconds(1f);

        ActiveBattle();

        Debug.Log("끝");
        isProcessingQueue = false;
    }



    private void CheckStageEvent(StageData currentStage)
    {
        // 랜덤 확률 계산
        int randomValue = UnityEngine.Random.Range(0, 100);
        int probabilitySum = 0;
        
        // 근접 몬스터 확률 체크
        if (randomValue < (probabilitySum += currentStage.meleeMonsterProbability))
        {
            Debug.Log($"근접 몬스터 등장! (확률: {currentStage.meleeMonsterProbability}%)");
            enemySpawner.SpawnEnemy(CharacterType.Warrior);
        }
        // 원거리 몬스터 확률 체크
        else if (randomValue < (probabilitySum += currentStage.rangedMonsterProbability))
        {
            Debug.Log($"원거리 몬스터 등장! (확률: {currentStage.rangedMonsterProbability}%)");
            enemySpawner.SpawnEnemy(CharacterType.Ranger);
        }
        // 마법 몬스터 확률 체크
        else if (randomValue < (probabilitySum += currentStage.magicMonsterProbability))
        {
            Debug.Log($"마법 몬스터 등장! (확률: {currentStage.magicMonsterProbability}%)");
            enemySpawner.SpawnEnemy(CharacterType.Wizard);
        }
        // 함정 확률 체크
        else if (randomValue < (probabilitySum += currentStage.trapProbability))
        {
            Debug.Log($"함정 발견! (확률: {currentStage.trapProbability}%)");
        }
        // NPC 확률 체크
        else if (randomValue < (probabilitySum += currentStage.npcProbability))
        {
            Debug.Log($"NPC 조우! (확률: {currentStage.npcProbability}%)");
        }

        // 동료 모집 체크
        if (currentStage.isRecruit)
        {
            Debug.Log("동료 모집 가능한 스테이지입니다!");
        }

        // 아이템 획득 체크
        if (currentStage.isItemRecruit)
        {
            Debug.Log("아이템 획득 가능한 스테이지입니다!");
        }
    }
}
