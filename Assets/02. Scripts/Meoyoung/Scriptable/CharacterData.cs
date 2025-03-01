using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Character/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("캐릭터 기본 정보")]
    public string characterName;         // 캐릭터 이름
    public CharacterType characterType;  // 캐릭터 직업

    [Header("캐릭터 스탯")]
    public int maxHealth;               // 최대 체력
    public int attackPower;             // 공격력

    [Header("캐릭터 이미지")]
    public Sprite characterSprite;
}