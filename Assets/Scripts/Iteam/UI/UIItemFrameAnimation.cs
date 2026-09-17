using UnityEngine;
using UnityEngine.UI;

/// <summary>Swaps only the icon sprite; does not animate layout, tint or raycast settings.</summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Image))]
public sealed class UIItemFrameAnimation : MonoBehaviour
{
    private Image image;
    private Sprite staticSprite;
    private Sprite[] frames;
    private float fps, elapsed, phase;
    private int displayedFrame = -1;

    public void Bind(Sprite original, PassiveIconAnimationSet.Entry animation)
    {
        image = GetComponent<Image>();
        staticSprite = original;
        frames = animation != null ? animation.frames : null;
        fps = animation != null ? animation.framesPerSecond : 0f;
        phase = animation != null ? (animation.itemId % 4) * 0.125f : 0f;
        Restart();
    }

    private void OnEnable() { Restart(); }

    private void Restart()
    {
        elapsed = phase;
        displayedFrame = -1;
        if (isActiveAndEnabled) ApplyFrame();
        else RestoreStatic();
    }

    private void Update()
    {
        if (frames == null || frames.Length == 0 || fps <= 0f) return;
        // The inventory pauses gameplay; playback must continue independently.
        elapsed = (elapsed + Time.unscaledDeltaTime) % (frames.Length / fps);
        ApplyFrame();
    }

    private void ApplyFrame()
    {
        if (image == null) return;
        if (frames == null || frames.Length == 0 || fps <= 0f) { RestoreStatic(); return; }
        int frame = Mathf.FloorToInt(elapsed * fps) % frames.Length;
        if (frame == displayedFrame) return;
        displayedFrame = frame;
        image.sprite = frames[frame] != null ? frames[frame] : staticSprite;
    }

    private void RestoreStatic()
    {
        if (image != null) image.sprite = staticSprite;
        displayedFrame = -1;
    }

    private void OnDisable() { RestoreStatic(); }
    private void OnDestroy() { RestoreStatic(); }
}
