using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Player-centered iris, psychic interception, release wave and a quiet afterglow.</summary>
public sealed class MewMercyEffect : MonoBehaviour
{
    private GameObject overlay;
    private MewMercyGraphic graphic;

    public IEnumerator Play(Transform target, bool showBlackout, System.Action onPsychicRelease = null, float playbackSpeed = 1f)
    {
        Cleanup();
        overlay = new GameObject("Mew psychic rescue", typeof(RectTransform), typeof(Canvas));
        Canvas canvas = overlay.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 32000;
        canvas.pixelPerfect = false;
        GameObject panel = new GameObject("Psychic iris and starlight", typeof(RectTransform), typeof(CanvasRenderer));
        panel.transform.SetParent(overlay.transform, false);
        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        graphic = panel.AddComponent<MewMercyGraphic>();
        graphic.raycastTarget = false;
        Canvas.ForceUpdateCanvases();
        Camera camera = Camera.main;
        float start = showBlackout ? 0f : 1.25f;
        bool released = false;
        for (float t = start; t < 4.65f; t += Time.unscaledDeltaTime * Mathf.Max(0.1f, playbackSpeed))
        {
            if (overlay == null || graphic == null) yield break;
            Vector2 center = Vector2.zero;
            if (camera == null) camera = Camera.main;
            if (target != null && camera != null)
            {
                // Use the rendering camera, including Cinemachine zoom/viewport, and the same UI space for every layer.
                Vector3 screen = camera.WorldToScreenPoint(target.position + Vector3.up * 0.35f);
                if (screen.z > 0f)
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screen, null, out center);
            }
            graphic.SetFrame(center, t, showBlackout);
            if (!released && t >= 1.8f)
            {
                released = true;
                onPsychicRelease?.Invoke();
            }
            yield return null;
        }
        Cleanup();
    }

    public IEnumerator WhiteScreen(bool fadeIn)
    {
        Image white;
        if (fadeIn)
        {
            Cleanup();
            overlay = new GameObject("Mew return whiteout", typeof(RectTransform), typeof(Canvas));
            Canvas canvas = overlay.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 32000;
            GameObject panel = new GameObject("White", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            panel.transform.SetParent(overlay.transform, false);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            white = panel.GetComponent<Image>();
            white.raycastTarget = false;
            white.color = new Color(1f, 1f, 1f, 0f);
        }
        else white = overlay != null ? overlay.GetComponentInChildren<Image>() : null;
        if (white == null) yield break;
        float duration = fadeIn ? 0.65f : 0.9f;
        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            float a = Mathf.SmoothStep(0f, 1f, t / duration);
            white.color = new Color(1f, 1f, 1f, fadeIn ? a : 1f - a);
            yield return null;
        }
        white.color = fadeIn ? Color.white : Color.clear;
        if (fadeIn) yield return null; // Render a fully white frame before moving rooms.
        else Cleanup();
    }

    private void Cleanup()
    {
        if (overlay != null)
        {
            overlay.SetActive(false);
            Destroy(overlay);
        }
        overlay = null;
        graphic = null;
    }
    private void OnDisable() { Cleanup(); }
    private void OnDestroy() { Cleanup(); }
}
