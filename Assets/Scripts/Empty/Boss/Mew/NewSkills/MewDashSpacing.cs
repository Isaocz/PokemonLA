/// <summary>Distance accumulator independent of frame duration and movement easing.</summary>
public struct MewDashSpacing
{
    private readonly float spacing;
    private float untilNext;

    public MewDashSpacing(float spacing)
    {
        this.spacing = System.Math.Max(0.25f, spacing);
        untilNext = this.spacing;
    }

    // Call until false for each travelled segment; offsets are measured from that segment's start.
    public bool TryTake(float length, ref float consumed, out float offset)
    {
        float remaining = System.Math.Max(0f, length - consumed);
        if (remaining + 0.00001f >= untilNext)
        {
            consumed += untilNext;
            offset = System.Math.Min(length, consumed);
            untilNext = spacing;
            return true;
        }
        untilNext -= remaining;
        offset = 0f;
        return false;
    }
}
