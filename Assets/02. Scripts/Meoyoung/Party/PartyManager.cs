using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PartyManager : MonoBehaviour
{
    public static PartyManager instance;

    [SerializeField] private List<CharacterBase> partyMembers;  // 현재 파티 멤버

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 현재 파티에 캐릭터 추가
    public void AddMember(CharacterBase member)
    {
        partyMembers.Add(member);
        PartyInfoUI.instance.AddPartyInfo(member, partyMembers.Count - 1);
    }

    // 파티 멤버 반환
    public List<CharacterBase> GetPartyMembers()
    {
        return partyMembers;
    }

    // 특정 직업 캐릭터 반환
    public CharacterBase GetCharacterByType(CharacterType type)
    {
        return partyMembers.Find(member => member.characterType == type);
    }

    // 파티 멤버 수 반환
    public int GetPartyMemberNum()
    {
        return partyMembers.Count;
    }

    // 생존 중인 파티원 중 체력 상태가 가장 나쁜 캐릭터 반환
    public CharacterBase GetWorstHealthCharacter()
    {
        return partyMembers.Where(member => member.isAlive).OrderBy(member => member.currentHealth).First();
    }

    // 동일한 직업군이 있는지 반환
    public bool IsSameCharacterTypeInParty(CharacterType characterType)
    {
        return partyMembers.Any(member => member.characterType == characterType);
    }
    
}
