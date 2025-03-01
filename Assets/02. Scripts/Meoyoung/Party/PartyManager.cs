using UnityEngine;
using System.Collections.Generic;

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

    public List<CharacterBase> GetPartyMembers()
    {
        return partyMembers;
    }
    
}
