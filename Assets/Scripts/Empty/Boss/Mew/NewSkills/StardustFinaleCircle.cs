using UnityEngine;

/// <summary>
/// 「星尘幻想」终幕使用的纯视觉技能圈。
/// 原限制圈解除后，本技能圈从终幕起始半径缓慢收缩到中心，
/// 用画面本身提示玩家中心正在蓄力，不承担碰撞或拖拽功能。
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(LineRenderer))]
public sealed class StardustFinaleCircle : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Material runtimeMaterial;

    private Vector2 center;
    private float startRadius;
    private float endRadius;
    private float shrinkDuration;
    private float lineWidth;
    private int segments;
    private Color baseColor;

    private float elapsed;
    private float fadeElapsed;
    private float fadeDuration;
    private bool initialized;
    private bool fading;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void Initialize(
        Vector2 usedCenter,
        float usedStartRadius,
        float usedEndRadius,
        float usedShrinkDuration,
        float usedLineWidth,
        int usedSegments,
        Color usedColor)
    {
        center = usedCenter;
        startRadius = Mathf.Max(0.1f, usedStartRadius);
        endRadius = Mathf.Clamp(
            usedEndRadius,
            0.1f,
            startRadius);
        shrinkDuration = Mathf.Max(0.1f, usedShrinkDuration);
        lineWidth = Mathf.Max(0.01f, usedLineWidth);
        segments = Mathf.Clamp(usedSegments, 24, 160);
        baseColor = usedColor;

        elapsed = 0f;
        fadeElapsed = 0f;
        fading = false;
        initialized = true;

        transform.position = center;
        ConfigureLineRenderer();
        DrawCircle(startRadius, 1f);
    }

    public void FadeOut(float duration)
    {
        if (!initialized)
        {
            Destroy(gameObject);
            return;
        }

        fading = true;
        fadeElapsed = 0f;
        fadeDuration = Mathf.Max(0.01f, duration);
    }

    private void Update()
    {
        if (!initialized)
        {
            return;
        }

        elapsed += Time.deltaTime;
        float shrinkProgress = Mathf.Clamp01(
            elapsed / shrinkDuration);
        float eased = SmoothStep(shrinkProgress);
        float radius = Mathf.Lerp(
            startRadius,
            endRadius,
            eased);

        float pulse = 0.82f +
                      0.18f *
                      Mathf.Sin(elapsed * 8.5f);
        float alphaMultiplier = Mathf.Clamp01(pulse);

        if (fading)
        {
            fadeElapsed += Time.deltaTime;
            float fadeProgress = Mathf.Clamp01(
                fadeElapsed / fadeDuration);
            alphaMultiplier *= 1f - SmoothStep(fadeProgress);

            if (fadeProgress >= 1f)
            {
                if (lineRenderer != null)
                {
                    lineRenderer.enabled = false;
                }

                initialized = false;
                return;
            }
        }

        DrawCircle(radius, alphaMultiplier);
    }

    private void ConfigureLineRenderer()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = segments;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.numCapVertices = 4;
        lineRenderer.numCornerVertices = 2;
        lineRenderer.sortingOrder = 18;
        lineRenderer.textureMode = LineTextureMode.Stretch;

        Shader shader = Shader.Find("Sprites/Default");
        if (shader != null)
        {
            runtimeMaterial = new Material(shader);
            runtimeMaterial.name =
                "Stardust Finale Circle Runtime Material";
            lineRenderer.material = runtimeMaterial;
        }
    }

    private void DrawCircle(float radius, float alphaMultiplier)
    {
        if (lineRenderer == null)
        {
            return;
        }

        for (int i = 0; i < segments; i++)
        {
            float angle =
                i * Mathf.PI * 2f / segments;
            lineRenderer.SetPosition(
                i,
                new Vector3(
                    Mathf.Cos(angle) * radius,
                    Mathf.Sin(angle) * radius,
                    0f));
        }

        Color usedColor = baseColor;
        usedColor.a *= Mathf.Clamp01(alphaMultiplier);
        lineRenderer.startColor = usedColor;
        lineRenderer.endColor = usedColor;
    }

    private static float SmoothStep(float value)
    {
        value = Mathf.Clamp01(value);
        return value * value * (3f - 2f * value);
    }

    private void OnDestroy()
    {
        if (runtimeMaterial != null)
        {
            Destroy(runtimeMaterial);
            runtimeMaterial = null;
        }
    }
}
