using System;
using System.Collections;
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
    ParticleSystem[] particleSystems;
    VisualEffect[] visualEffects;
    public void StopEffect()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        visualEffects = GetComponentsInChildren<VisualEffect>();

        foreach (var ps in particleSystems)
            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        foreach (var vfx in visualEffects)
            vfx.Stop();
        StartCoroutine(DestroyWhenParticlesGone());
    }
    private IEnumerator DestroyWhenParticlesGone()
    {
        // 모든 파티클이 완전히 사라질 때까지 대기
        yield return new WaitUntil(() => AreAllParticlesGone());
        Destroy(gameObject);
    }
    private bool AreAllParticlesGone()
    {
        foreach (var ps in particleSystems)
        {
            if (ps.IsAlive(true)) return false; // 아직 살아있는 파티클이 있다면 대기
        }
        foreach (var vfx in visualEffects)
        {
            if (vfx.HasAnySystemAwake()) return false;
        }
        return true; // 모든 파티클이 사라지면.
    }
}
