using UnityEngine;

public class EffectObject : MonoBehaviour
{
    // 애니메이션 또는 파티클 시스템으로 효과 표현. 단일 게임오브젝트
    private float timer = 0f;
    private ParticleSystem particleSystem;
    private Animator animator;

    private void OnEnable()
    {
        if (TryGetComponent(out particleSystem))
        {
            timer = particleSystem.time + 5f;
        }
        if (TryGetComponent(out animator))
        {
            timer = animator.GetCurrentAnimatorStateInfo(0).length;
        }
        Destroy(gameObject, timer);
    }

}
