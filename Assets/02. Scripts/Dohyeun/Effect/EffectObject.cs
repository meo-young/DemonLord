using System;
using UnityEngine;
using UnityEngine.VFX;

public class EffectObject : MonoBehaviour
{
    // 애니메이션 또는 파티클 시스템으로 효과 표현. 단일 게임오브젝트
    private float timer = 0f;
    private ParticleSystem particleSystem;
    private Animator animator;
    private VisualEffect visualEffect;

    // 한 사이클만 실행하고 없앨지
    [NonSerialized] public bool IsShowOneTime;
    public void Initialize()
    {
        if (TryGetComponent(out visualEffect))
        {
            visualEffect.Play();
        }
        if (IsShowOneTime)
        {
            if (TryGetComponent(out particleSystem))
            {
                timer = particleSystem.time;
            }
            if (TryGetComponent(out animator))
            {
                timer = animator.GetCurrentAnimatorStateInfo(0).length;
            }
            if (TryGetComponent(out visualEffect))
            {
                timer = 0.2f; // 하드코딩
            }
            Invoke(nameof(StopEffect), timer);
        }
    }
    public void StopEffect()
    {
        if (particleSystem) particleSystem.Stop();
        if (visualEffect) visualEffect.Stop();
        Destroy(gameObject, 3f);
    }
}
