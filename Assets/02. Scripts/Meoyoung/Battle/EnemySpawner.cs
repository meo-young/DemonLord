using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [Header("설정될 GameObject")]
    [SerializeField] private GameObject enemyGameObject;

    [Header("근접 몬스터 데이터")]
    [SerializeField] private CharacterData meleeEnemyData;

    [Header("원거리 몬스터 데이터")]
    [SerializeField] private CharacterData rangedEnemyData;

    [Header("마법사 몬스터 데이터")]
    [SerializeField] private CharacterData wizardEnemyData;
    
    

    public void SpawnEnemy(CharacterType characterType)
    {
        // 적 캐릭터 데이터 가져오기
        CharacterData enemyData = characterType switch
        {
            CharacterType.Warrior => meleeEnemyData,
            CharacterType.Ranger => rangedEnemyData,
            CharacterType.Wizard => wizardEnemyData,
            _ => meleeEnemyData // 기본값
        };

        // 이미지 초기화
        SpriteRenderer spriteRenderer = enemyGameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = enemyData.characterSprite;
        
        // 적 캐릭터 컴포넌트 초기화
        NormalEnemy enemy = enemyGameObject.GetComponent<NormalEnemy>();
        enemy.SetCharacterData(enemyData);

        // 적 캐릭터 설정
        GameManager.instance.enemy = enemy;
    }
}
