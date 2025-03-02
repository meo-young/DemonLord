using UnityEngine;

public class AnimManager : MonoBehaviour
{
    public static AnimManager instance;

    [Header("전투 텍스트 애니메이션")]
    [SerializeField] private GameObject battleCanvas;

    private void Awake()
    {
        instance = this;
    }
    
    public void StartBattleTextAnim()
    {
        battleCanvas.SetActive(true);
    }

    public void EndBattleTextAnim()
    {
       battleCanvas.SetActive(false);
    }
}
