using UnityEngine;

public class Player : Human
{
    // @TODO : 권능, 특성 추가

    protected override void Start()
    {
        base.Start();

        Debug.Log($"{characterName} 캐릭터 생성");

        // 파티에 플레이어 추가
        PartyManager.instance.AddMember(this);

        // Test
        //SelectionUI.instance.InitSelectionUI();
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        Debug.Log("게임 종료 해라");
        
    }
}
