using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PartyManager : MonoBehaviour
{
    public static PartyManager instance;

    [SerializeField] private List<CharacterBase> partyMembers;  // 현재 파티 멤버

    private void Awake()
    {
        instance = this;
    }



    /// <summary>
    /// 현재 파티에 캐릭터 추가
    /// </summary>
    /// <param name="member">추가할 캐릭터</param>
    public void AddMember(CharacterBase member)
    {
        partyMembers.Add(member);
        PartyInfoUI.instance.AddPartyInfo(member, partyMembers.Count - 1);
    }



    /// <summary>
    /// 현재 파티 멤버 반환
    /// </summary>
    /// <returns>현재 파티 멤버</returns>
    public List<CharacterBase> GetPartyMembers()
    {
        return partyMembers;
    }



    /// <summary>
    /// 특정 직업 캐릭터 반환
    /// </summary>
    /// <param name="type">찾을 캐릭터 타입</param>
    /// <returns>특정 직업 캐릭터 반환</returns>
    public CharacterBase GetCharacterByType(CharacterType type)
    {
        return partyMembers.Find(member => member.characterType == type);
    }



    /// <summary>
    /// 생존해 있는 파티원들의 체력을 amount 만큼 회복
    /// </summary>
    /// <param name="amount">회복할 체력</param>
    public void HealAlivePartyMembers(int amount)
    {
        AudioManager.instance.PlaySFX(SFX.sfx_Heal);
        foreach (CharacterBase member in GetAlivePartyMembers())
        {
            member.Heal(amount);
        }
    }




    /// <summary>
    /// 생존해 있는 파티원들이 amount 만큼 데미지를 받음
    /// </summary>
    /// <param name="amount">데미지</param>
    public void GetDamageAlivePartyMembers(int amount)
    {
        foreach (CharacterBase member in GetAlivePartyMembers())
        {
            member.GetDamage(amount);
        }
    }



    /// <summary>
    /// 파티 멤버 수 반환
    /// </summary>
    /// <returns>파티 멤버 수 반환</returns>
    public int GetPartyMemberNum()
    {
        return partyMembers.Count;
    }



    /// <summary>
    /// 생존 중인 파티원 중 체력 상태가 가장 나쁜 캐릭터 반환
    /// </summary>
    /// <returns>생존 중인 파티원 중 체력 상태가 가장 나쁜 캐릭터 반환</returns>
    public CharacterBase GetWorstHealthCharacter()
    {
        return partyMembers.Where(member => member.isAlive).OrderBy(member => member.currentHealth).First();
    }



    /// <summary>
    /// 동일한 직업군이 있는지 반환
    /// </summary>
    /// <param name="characterType">찾을 캐릭터 타입</param>
    /// <returns>동일한 직업군 여부 반환</returns>
    public bool IsSameCharacterTypeInParty(CharacterType characterType)
    {
        return partyMembers.Any(member => member.characterType == characterType);
    }



    /// <summary>
    /// 생존해 있는 파티 멤버들만 반환
    /// </summary>
    /// <returns>생존해 있는 파티 멤버들만 반환</returns>
    public List<CharacterBase> GetAlivePartyMembers()
    {
        return partyMembers.Where(member => member.isAlive).ToList();
    }



    /// <summary>
    /// 반대 상성 관계에 있는 캐릭터 반환
    /// </summary>
    /// <param name="type">찾을 캐릭터 타입</param>
    /// <returns>반대 상성 관계에 있는 캐릭터 반환</returns>
    public CharacterBase GetReverseCounterTypeCharacter(CharacterType type)
    {
        CharacterType counterType = type switch
        {
            CharacterType.Warrior => CharacterType.Ranger,
            CharacterType.Ranger => CharacterType.Wizard,
            CharacterType.Wizard => CharacterType.Warrior,
            _ => throw new System.ArgumentException("지원하지 않는 캐릭터 타입입니다.")
        };

        CharacterBase character = GetCharacterByType(counterType);
        return character != null && character.isAlive ? character : null;
    }



    /// <summary>
    /// 같은 타입의 캐릭터 반환
    /// </summary>
    /// <param name="type">찾을 캐릭터 타입</param>
    /// <returns>같은 타입의 캐릭터 반환</returns>
    public CharacterBase GetSameTypeCharacter(CharacterType type)
    {
        return partyMembers.Find(member => member.characterType == type && member.isAlive);
    }



    /// <summary>
    /// 상성 관계에 있는 캐릭터 반환
    /// </summary>
    /// <param name="type">찾을 캐릭터 타입입</param>
    /// <returns>상성 관계에 있는 캐릭터 반환</returns>
    public CharacterBase GetCounterTypeCharacter(CharacterType type)
    {
       CharacterType counterType = type switch
        {
            CharacterType.Warrior => CharacterType.Wizard,
            CharacterType.Ranger => CharacterType.Warrior,
            CharacterType.Wizard => CharacterType.Ranger,
            _ => throw new System.ArgumentException("지원하지 않는 캐릭터 타입입니다.")
        };

        CharacterBase character = GetCharacterByType(counterType);
        return character != null && character.isAlive ? character : null;
    }



    /// <summary>
    /// 파티 멤버들을 부활시킴
    /// </summary>
    public void RebirthPartyMembers()
    {
        foreach(CharacterBase member in partyMembers)
        {
            member.OnRebirth();
        }
    }
}
