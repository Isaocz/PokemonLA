// 主实现：明确在所有分支中直接返回，不依赖外部未初始化变量
float2 ScreenGridUV_float(float4 screenPosClip, float2 screenSize, float gridPx, float centerSample , out float2 gridUV)
{
    // 计算 clip-space -> 0..1 screenUV
    float2 screenUV = screenPosClip.xy / screenPosClip.w;
    screenUV = saturate(screenUV);

    // 转为屏幕像素坐标
    float2 screenPx = screenUV * screenSize;

    // 防止除以零或负值，直接在每个分支返回值，避免未初始化
    if (gridPx <= 0.0)
    {
        return screenPx / screenSize;
    }

    // 计算格子左上角，centerSample 控制是否采样中心
    float2 grid = floor(screenPx / gridPx) * gridPx;
    if (centerSample > 0.5){
        grid += gridPx * 0.5;}

    gridUV = grid / screenSize;

    return gridUV;
}


