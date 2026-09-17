using UnityEngine;
using UnityEngine.UI;

/// <summary>Resolution-independent UI geometry; no screen texture, shader lookup or world-space overlay.</summary>
[RequireComponent(typeof(CanvasRenderer))]
public sealed class MewMercyGraphic : MaskableGraphic
{
    public MewMercyGraphic()
    {
        useLegacyMeshGeneration = false;
    }
    private const int Segments = 160;
    private Vector2 focus;
    private float elapsed;
    private bool blackout;
    private static readonly Color Ink = new Color(0.025f, 0.015f, 0.055f);
    private static readonly Color Violet = new Color(0.58f, 0.35f, 1f);
    private static readonly Color Pink = new Color(1f, 0.63f, 0.88f);
    private static readonly Color Pearl = new Color(0.92f, 0.87f, 1f);

    public void SetFrame(Vector2 center, float time, bool showBlackout)
    {
        focus = center;
        elapsed = time;
        blackout = showBlackout;
        SetVerticesDirty();
    }

    private static float Smooth(float value) => Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(value));
    private static Color Alpha(Color color, float alpha) { color.a = Mathf.Clamp01(alpha); return color; }

    protected override void OnPopulateMesh(VertexHelper mesh)
    {
        mesh.Clear();
        Rect bounds = rectTransform.rect;
        if (bounds.width <= 0f || bounds.height <= 0f) return;
        float unit = Mathf.Min(bounds.width, bounds.height);
        float far = 0f;
        for (int x = 0; x < 2; x++)
            for (int y = 0; y < 2; y++)
                far = Mathf.Max(far, Vector2.Distance(focus, new Vector2(x == 0 ? bounds.xMin : bounds.xMax,
                    y == 0 ? bounds.yMin : bounds.yMax)));
        far += unit * 0.15f;
        float shelter = unit * 0.145f;
        float closing = Smooth(elapsed / 1.25f);
        float awaken = Smooth((elapsed - 1.12f) / 0.55f);
        float release = Smooth((elapsed - 1.8f) / 1.8f);
        float goodbye = 1f - Smooth((elapsed - 3.65f) / 0.7f);
        float radius = elapsed < 1.8f ? Mathf.Lerp(far, shelter, closing) : Mathf.Lerp(shelter, far, release);
        float darkness = blackout ? Smooth(elapsed / 0.55f) * (1f - Smooth((elapsed - 3.0f) / 0.65f)) : 0f;

        // The hole remains clear around the fainting player. Three soft bands replace the old stretched bitmap.
        Annulus(mesh, radius * 0.84f, radius, Alpha(Ink, 0f), Alpha(Ink, 0.72f * darkness));
        Annulus(mesh, radius, radius + unit * 0.09f, Alpha(Ink, 0.72f * darkness), Alpha(Ink, 0.97f * darkness));
        Annulus(mesh, radius + unit * 0.09f, far + unit, Alpha(Ink, 0.97f * darkness), Alpha(Ink, 0.97f * darkness));

        float barrier = awaken * (1f - Smooth((elapsed - 3.25f) / 0.5f));
        float shieldRadius = elapsed < 1.8f ? shelter * Mathf.Lerp(0.88f, 1f, awaken) : radius;
        float pulse = 1f + 0.018f * Mathf.Sin((elapsed - 1.12f) * 15f) * (1f - release);
        shieldRadius *= pulse;
        // A wide dim halo, narrow pink rim and pearl core create depth without a white screen flash.
        GlowRing(mesh, shieldRadius, unit * 0.022f, Violet, barrier * 0.28f);
        GlowRing(mesh, shieldRadius, unit * 0.008f, Pink, barrier * 0.65f);
        GlowRing(mesh, shieldRadius, unit * 0.0018f, Pearl, barrier * 0.9f);
        GlowRing(mesh, shieldRadius * 0.94f, unit * 0.002f, Violet, barrier * 0.42f);
        float echo = Smooth((elapsed - 2f) / 0.35f) * (1f - Smooth((elapsed - 3.5f) / 0.6f));
        GlowRing(mesh, Mathf.Lerp(shelter, far, Smooth((elapsed - 2.02f) / 1.9f)), unit * 0.009f, Pink, echo * 0.2f);

        // Keep a quiet, localized protective aura after the large wave has left the screen.
        float aura = awaken * goodbye;
        Annulus(mesh, shelter * 0.2f, shelter * 0.8f, Alpha(Violet, 0f), Alpha(Violet, aura * 0.065f));
        Annulus(mesh, shelter * 0.8f, shelter * 1.15f, Alpha(Violet, aura * 0.065f), Alpha(Violet, 0f));
        GlowRing(mesh, shelter * 0.82f, unit * 0.003f, Pink, aura * 0.25f);

        for (int i = 0; i < 28; i++)
        {
            float seed = Mathf.Repeat(i * 0.6180339f, 1f);
            float angle = i * 2.399963f + elapsed * (i % 2 == 0 ? 0.10f : -0.08f);
            float travel = Smooth((elapsed - 1.3f - seed * 0.38f) / 2.1f);
            float distance = shelter * (0.87f + seed * 0.25f) + unit * travel * (0.08f + seed * 0.2f);
            Vector2 point = focus + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
            float twinkle = 0.7f + 0.3f * Mathf.Sin(elapsed * 5f + i);
            float opacity = Smooth((elapsed - 1.22f - seed * 0.3f) / 0.5f) * goodbye * (1f - travel * 0.75f);
            Star(mesh, point, unit * (0.002f + seed * 0.0025f) * twinkle, Alpha(Color.Lerp(Pink, Pearl, seed), opacity * 0.75f));
        }
    }

    private void GlowRing(VertexHelper mesh, float radius, float width, Color tint, float opacity)
    {
        if (opacity <= 0f) return;
        Annulus(mesh, Mathf.Max(0f, radius - width), radius, Alpha(tint, 0f), Alpha(tint, opacity));
        Annulus(mesh, radius, radius + width, Alpha(tint, opacity), Alpha(tint, 0f));
    }

    private void Annulus(VertexHelper mesh, float inner, float outer, Color inside, Color outside)
    {
        if (outer <= inner || (inside.a <= 0f && outside.a <= 0f)) return;
        int start = mesh.currentVertCount;
        for (int i = 0; i <= Segments; i++)
        {
            float angle = i * Mathf.PI * 2f / Segments;
            Vector2 ray = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            mesh.AddVert(focus + ray * inner, inside, Vector2.zero);
            mesh.AddVert(focus + ray * outer, outside, Vector2.zero);
            if (i == Segments) continue;
            int index = start + i * 2;
            mesh.AddTriangle(index, index + 1, index + 2);
            mesh.AddTriangle(index + 1, index + 3, index + 2);
        }
    }

    private static void Star(VertexHelper mesh, Vector2 point, float size, Color tint)
    {
        int start = mesh.currentVertCount;
        mesh.AddVert(point, tint, Vector2.zero);
        for (int i = 0; i < 8; i++)
        {
            float angle = i * Mathf.PI / 4f;
            float length = i % 2 == 0 ? size * 2f : size * 0.38f;
            mesh.AddVert(point + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * length, Alpha(tint, 0f), Vector2.zero);
            mesh.AddTriangle(start, start + i + 1, start + (i + 1) % 8 + 1);
        }
    }
}
