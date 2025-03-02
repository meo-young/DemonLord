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

    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private BattleFormation battleFormation;
    [SerializeField] private GameObject bossEnemy;

    public SpriteRenderer monsterSpriteRenderer;

    private bool isBattle = false;
    private bool isBoss = false;

    private void Awake()
    {
        instance = this;
    }



    private void Start() 
    {
        // 스테이지 데이터 큐 초기화
        stageDataQueue = new Queue<StageData>(stageDataList);
    }

    /// <summary>
    /// 전투 스테이지로 넘어갈 때 호출. FadeOut 연출 후 다음 스테이지로 넘어감
    /// </summary>
    public void PassOverBattle()
    {
        FadeManager.instance.FadeOut(
            () =>
            {
                SelectionUI.instance.HideSelectionUI();
                monsterSpriteRenderer.sprite = null;
                BattleManager.instance.StartBattle();
            }
        );
    }


    /// <summary>
    /// 새로운 스테이지 시작 시 호출
    /// </summary>
    public void StartBattle()
    {
        // 뒷배경 리소스 설정
        GameManager.instance.SetBackground(GameManager.instance.battleBackground);

        // 주사위 결과 텍스트 초기화
        Dice.instance.InitDiceBtn();

        // Fade In 연출로 전투 시작
        FadeManager.instance.FadeIn();

        // 다음 스테이지 데이터 처리
        ProcessNextStageData();
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

        if(isBoss)
        {
            isBoss = false;

            // 적 공격 이벤트 추가
            AddEventToQueue(
                () => {
                    GameManager.instance.boss.AttackPlayer();
                    GameManager.instance.SetInactiveWarrant();
                }
            );

            // 이벤트 큐 처리
            StartEventProcess();
            
            return;
        }
        else
        {
            // 선택지 UI 활성화
            SelectionUI.instance.InitSelectionUI();
            SelectionUI.instance.SetActiveSelectionUI();
        }

        GameManager.instance.SetWarrantButtonInteractable();

        // 주사위 결과 텍스트 초기화
        Dice.instance.InitDiceBtn();
    }



    /// <summary>
    /// 전투 승리 시 호출
    /// </summary>
    public void BattleWin()
    {
        // 배틀 여부 비활성화
        isBattle = false;
        GameManager.instance.SetWarrantButtonDeactive();

        // 승리 화면 표시
        //BattleWinUI.instance.ShowWinUI();

        // 캐릭터 앞으로 이동
        battleFormation.EndBattleFormation();
    }


    public void BattleLose()
    {
        // 배틀 여부 비활성화
        isBattle = false;
        GameManager.instance.SetWarrantButtonDeactive();

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
        // 모집 여부 체크
        if(currentStage.isRecruit)
        {
            if(PartyManager.instance.GetPartyMemberNum() >= 3)
            {
                // 다음 스테이지 데이터 처리
                ProcessNextStageData();
                Debug.Log("모든 동료를 모집했습니다.");
            }
            else
            {
                PartyRecruitUI.instance.ShowRecruitPanel();
            }
            return;
        }

        // 아이템 모집 여부 체크
        if(currentStage.isItemRecruit)
        {
            ThiefItem.instance.ShowThiefItem();
            return;
        }


        // 랜덤 확률 계산
        int randomValue = UnityEngine.Random.Range(0, 100);
        int probabilitySum = 0;
        
        // 근접 몬스터 확률 체크
        if (randomValue < (probabilitySum += currentStage.meleeMonsterProbability))
        {
            Debug.Log($"근접 몬스터 등장! (확률: {currentStage.meleeMonsterProbability}%)");
            AudioManager.instance.PlayBGM(BGM.bgm_NormalField);
            enemySpawner.SpawnEnemy(CharacterType.Warrior);
        }
        // 원거리 몬스터 확률 체크
        else if (randomValue < (probabilitySum += currentStage.rangedMonsterProbability))
        {
            Debug.Log($"원거리 몬스터 등장! (확률: {currentStage.rangedMonsterProbability}%)");
            AudioManager.instance.PlayBGM(BGM.bgm_NormalField);
            enemySpawner.SpawnEnemy(CharacterType.Ranger);
        }
        // 마법 몬스터 확률 체크
        else if (randomValue < (probabilitySum += currentStage.magicMonsterProbability))
        {
            Debug.Log($"마법 몬스터 등장! (확률: {currentStage.magicMonsterProbability}%)");
            AudioManager.instance.PlayBGM(BGM.bgm_NormalField);
            enemySpawner.SpawnEnemy(CharacterType.Wizard);
        }
        // 함정 확률 체크
        else if (randomValue < (probabilitySum += currentStage.trapProbability))
        {
            Debug.Log($"함정 발견! (확률: {currentStage.trapProbability}%)");
            AudioManager.instance.PlayBGM(BGM.bgm_Trap);
            TrapUI.instance.ShowTrapUI();
            return;
        }
        // NPC 확률 체크
        else if (randomValue < (probabilitySum += currentStage.npcProbability))
        {

            Debug.Log($"NPC 조우! (확률: {currentStage.npcProbability}%)");
            AudioManager.instance.PlayBGM(BGM.bgm_item);
            NPCSelectionUI.instance.ShowNPCSelectionUI();
            return;
        }


        // 보스 등장 여부 체크
        if(currentStage.isBoss)
        {
            isBoss = true;
            
            bossEnemy.SetActive(true);
            enemySpawner.DestroySelf();
            GameManager.instance.enemy = bossEnemy.GetComponent<BossEnemy>();
            GameManager.instance.enemy.InitUI();
            SituationUI.instance.SetSituationText("마왕을 만났습니다.");
            GameManager.instance.SetBackground(GameManager.instance.bossBackground);
            AudioManager.instance.PlayBGM(BGM.bgm_Boss);
        }


        // 배틀 여부 활성화
        isBattle = true;

        // 선택지 UI 표시
        SelectionUI.instance.ShowSelectionUI();
    
        // 애니메이션 초기화 용.. 야메 ..
        battleFormation.gameObject.SetActive(false);
        battleFormation.gameObject.SetActive(true);

        // 대형을 모두 갖춘 경우 선택지 UI 활성화
        battleFormation.StartBattleFormation();
    }
}
