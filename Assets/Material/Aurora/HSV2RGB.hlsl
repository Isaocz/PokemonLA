// HSV to RGB (expects H in 0..1, S 0..1, V >= 0)
float3 HSVtoRGB_HDR_float(float3 hsv , out float3 outputrgb)
{
    float h = hsv.x * 6.0; // sector 0..6
    float s = hsv.y;
    float v = hsv.z;

    float i = floor(h);
    float f = h - i;
    float p = v * (1.0 - s);
    float q = v * (1.0 - s * f);
    float t = v * (1.0 - s * (1.0 - f));

    int ii = (int)i % 6;
    if (ii == 0) return float3(v, t, p);
    if (ii == 1) return float3(q, v, p);
    if (ii == 2) return float3(p, v, t);
    if (ii == 3) return float3(p, q, v);
    if (ii == 4) return float3(t, p, v);

    outputrgb = float3(v, p, q);
    return outputrgb;
}