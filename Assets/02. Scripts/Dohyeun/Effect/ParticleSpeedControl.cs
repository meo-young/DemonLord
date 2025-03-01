using UnityEngine;

public class ParticleSpeedControl : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystem.VelocityOverLifetimeModule velocityModule;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();

        velocityModule = ps.velocityOverLifetime;
        velocityModule.enabled = true;

        velocityModule.x = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.EaseInOut(0, ps.startSpeed, 1, 0));
        velocityModule.y = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.EaseInOut(0, ps.startSpeed, 1, 0));
        velocityModule.z = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.EaseInOut(0, ps.startSpeed, 1, 0));
    }
}
