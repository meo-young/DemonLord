using UnityEngine;

public class Mouse : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼을 눌렀을 때
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // 2D 게임에서는 z값을 0으로 설정
            VFXManager.instance.ShowEffect(EffectType.Mouse_Click, mousePosition);
        }
    }
}
