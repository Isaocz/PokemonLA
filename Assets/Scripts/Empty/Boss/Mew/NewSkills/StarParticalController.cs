using UnityEngine;

/// <summary>
/// 根据弹幕追踪状态控制尾迹，并在弹幕渐隐时启用子特效。
/// </summary>
public class StarParticalController : MonoBehaviour
{
    private ParticleSystem particleSystemComponent;
    private BarrageProjectile barrageProjectile;

    public GameObject Child;

    private bool isPlaying;

    public void ResetForReuse()
    {
        isPlaying = false;
        if (particleSystemComponent != null)
            particleSystemComponent.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (Child != null) Child.SetActive(false);
    }

    private void Awake()
    {
        particleSystemComponent = GetComponent<ParticleSystem>();
        barrageProjectile = GetComponent<BarrageProjectile>();

        if (particleSystemComponent != null)
        {
            var main = particleSystemComponent.main;
            main.playOnAwake = false;
            particleSystemComponent.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void OnEnable() { ResetForReuse(); }
    private void OnDisable() { ResetForReuse(); }

    private void Update()
    {
        if (barrageProjectile == null)
        {
            return;
        }

        bool shouldPlay =
            (barrageProjectile.moveBehavior == BarrageProjectile.projectileBehavior.Target ||
             barrageProjectile.moveBehavior == BarrageProjectile.projectileBehavior.CloseTarget) &&
            !barrageProjectile.isTargeting && barrageProjectile.Target != null && barrageProjectile.FadeMode == 0;

        if (particleSystemComponent != null)
        {
            if (shouldPlay && !isPlaying)
            {
                particleSystemComponent.Play();
                isPlaying = true;
            }
            else if (!shouldPlay && (isPlaying || particleSystemComponent.isPlaying || particleSystemComponent.particleCount > 0))
            {
                particleSystemComponent.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                isPlaying = false;
            }
        }

        if (Child != null && barrageProjectile.FadeMode == 2 && !Child.activeSelf)
        {
            Child.SetActive(true);
        }
    }
}
