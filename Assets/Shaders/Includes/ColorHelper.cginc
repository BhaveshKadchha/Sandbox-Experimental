#define Epsilon         1e-10

float3 RGBtoHSL(float3 RGB) {
    float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0 / 3.0) : float4(RGB.gb, 0.0, -1.0 / 3.0);
    float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
    float C = Q.x - min(Q.w, Q.y);
    float H = abs((Q.w - Q.y) / (6 * C + Epsilon) + Q.z);
    float3 HCV = float3(H, C, Q.x);
    float L = HCV.z - HCV.y * 0.5;
    float S = HCV.y / (1 - abs(L * 2 - 1) + Epsilon);
    return float3(HCV.x, S, L);
}

float4 RGBtoGrayScale(float4 color) {
    float val = ((0.299 * color.r) + (0.587 * color.g) + (0.114 * color.b));
    return float4(val.xxx, color.a);
}

float4 ColorGradient(float2 pos, float4 color1, float4 color2, float4 color3, float4 color4)
{
    float t = abs(sin(pos.x));

    if (t >= 0 && t < 0.25) return lerp(color1, color2, t);
    else if (t >= 0.25 && t < 0.5) return lerp(color2, color3, t);
    else if (t >= 0.5 && t < 0.75) return lerp(color3, color4, t);
    else return lerp(color4, color1, t);
}