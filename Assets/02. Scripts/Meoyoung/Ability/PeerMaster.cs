using UnityEngine;

public class PeerMaster : MonoBehaviour, IAbility
{
    public void UseAbility()
    {
        Debug.Log("PeerMaster 특성 사용");
        GameManager.instance.peerHandicap = 2;
    }

}
