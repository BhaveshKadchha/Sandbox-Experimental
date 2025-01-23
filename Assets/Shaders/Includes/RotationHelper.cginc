#define PI              3.14159265359f

float2 Rotate(float2 value, float2 pivot, float angle)
{
    float s = sin(angle);
    float c = cos(angle);
    value -= pivot;
    value = float2(value.x * c - value.y * s, value.x * s + value.y * c);
    value += pivot;
    return value;
}

float2 RotateTo(float2 thing, float angle) {
    angle = angle * (PI / 180);
    float2 localThing = thing;
    float s = sin(angle);
    float c = cos(angle);
    float2x2 rotationMatrix = float2x2(c, -s, s, c);
    rotationMatrix *= 0.5;
    rotationMatrix += 0.5;
    rotationMatrix = rotationMatrix * 2 - 1;
    localThing.xy = mul(localThing.xy, rotationMatrix);
    localThing.xy += 0.5;
    return localThing;
}

float2 RotateContinuosly(float2 thing, float speed) {
    float2 localThing = thing;
    float s = sin(speed * _Time.y);
    float c = cos(speed * _Time.y);
    float2x2 rotationMatrix = float2x2(c, -s, s, c);
    rotationMatrix *= 0.5;
    rotationMatrix += 0.5;
    rotationMatrix = rotationMatrix * 2 - 1;
    localThing.xy = mul(localThing.xy, rotationMatrix);
    localThing.xy += 0.5;
    return localThing;
}