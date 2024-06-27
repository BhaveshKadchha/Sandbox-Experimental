#define PI              3.14159265359f
#define Epsilon         1e-10

float Noise(float2 co)
{
    const float a = 12.9898;
    const float b = 78.233;
    const float c = 43758.5453;
    float dt= dot(co.xy ,float2(a,b));
    float sn= fmod(dt,3.14);
    return frac(sin(dt) * c);
}

float2 RotateTo(float2 thing, float angle)
{
    angle = angle * (PI/180);
    float2 localThing = thing;
    // This Line Decide the Point of rotation.
    // localThing.xy -=0.5;
    float s = sin ( angle );
    float c = cos ( angle );
    float2x2 rotationMatrix = float2x2( c, -s, s, c);
    rotationMatrix *=0.5;
    rotationMatrix +=0.5;
    rotationMatrix = rotationMatrix * 2-1;
    localThing.xy = mul ( localThing.xy, rotationMatrix);
    localThing.xy += 0.5;
    return localThing;
}

float2 RotateContinuosly(float2 thing, float speed)
{
    float2 localThing = thing;
    // This Line Decide the Point of rotation.
    // localThing.xy -=0.5;
    float s = sin ( speed * _Time.y );
    float c = cos ( speed * _Time.y );
    float2x2 rotationMatrix = float2x2( c, -s, s, c);
    rotationMatrix *=0.5;
    rotationMatrix +=0.5;
    rotationMatrix = rotationMatrix * 2-1;
    localThing.xy = mul ( localThing.xy, rotationMatrix);
    localThing.xy += 0.5;
    return localThing;
}

float ConvertDegreeToRadian(float angle)
{
    return angle * (PI/180);
}

float ConvertRadianToDegree(float angle)
{
    return angle * (180/PI);
}

float3 RGBtoHSL(float3 RGB)
{
    float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0/3.0) : float4(RGB.gb, 0.0, -1.0/3.0);
    float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
    float C = Q.x - min(Q.w, Q.y);
    float H = abs((Q.w - Q.y) / (6 * C + Epsilon) + Q.z);
    float3 HCV = float3(H, C, Q.x);
    float L = HCV.z - HCV.y * 0.5;
    float S = HCV.y / (1 - abs(L * 2 - 1) + Epsilon);
    return float3(HCV.x, S, L);
}

float4 RGBtoGrayScale(float4 color)
{
    float val = ((0.299 * color.r) + (0.587 * color.g) + (0.114 * color.b));
    return float4(val.xxx,color.a);
}