// RGB to HSV (H:0..1, S:0..1, V:>=0) - preserves V > 1
float3 RGBtoHSV_HDR_float(float3 c , out float3 outputhsv )
{
    float maxc = max(c.r, max(c.g, c.b));
    float minc = min(c.r, min(c.g, c.b));
    float d = maxc - minc;
    float h = 0.0;
    if (d > 1e-6)
    {
        if (maxc == c.r) h = (c.g - c.b) / d;
        else if (maxc == c.g) h = 2.0 + (c.b - c.r) / d;
        else h = 4.0 + (c.r - c.g) / d;
        h = h / 6.0;
        if (h < 0.0) h += 1.0;
    }
    float s = (maxc > 0.0) ? (d / maxc) : 0.0;
    float v = maxc; // preserve HDR

    outputhsv = float3(h, s, v);
    return float3(h, s, v);

}
