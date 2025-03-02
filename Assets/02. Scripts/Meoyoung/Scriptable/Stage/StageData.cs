using UnityEngine;


[CreateAssetMenu(fileName = "StageData", menuName = "Stage/Stage Data")]
public class StageData : ScriptableObject
{
    [Header("스테이지 번호")]
    public string stageNumber;

    [Header("근접 몬스터 조우 확률")]
    public int meleeMonsterProbability;

    [Header("원거리 몬스터 조우 확률")]
    public int rangedMonsterProbability;

    [Header("마법 몬스터 조우 확률")]
    public int magicMonsterProbability;

    [Header("함정 조우 확률")]
    public int trapProbability;

    [Header("NPC 조우 확률")]
    public int npcProbability;

    [Header("동료 모집 여부")]
    public bool isRecruit;

    [Header("아이템 모집 여부")]
    public bool isItemRecruit;

    [Header("보스 등장 여부")]
    public bool isBoss;

}
