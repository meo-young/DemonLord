using UnityEngine;

public class FixResolution : MonoBehaviour
{
    [SerializeField] private int setWidth = 800;
    [SerializeField] private int setHeight = 600;
    [SerializeField] private bool isFullScreen = true;

    private void Start()
    {
        SetResolution();
    }

    public void SetResolution()
    {
        //해상도를 설정값에 따라 변경
        //3번째 파라미터는 풀스크린 모드를 설정 > true : 풀스크린, false : 창모드
        Screen.SetResolution(setWidth, setHeight, isFullScreen);
    }
}
