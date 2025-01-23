#define PI              3.14159265359f

float ConvertDegreeToRadian(float angle) {
    return angle * (PI / 180);
}

float ConvertRadianToDegree(float angle) {
    return angle * (180 / PI);
}

float Noise(float2 co, float time = 0) {
    const float a = 32.9898;
    const float b = 78.233;
    const float c = 43758.5453;
    
    float dotProduct = dot(co.xy, float2(a, b));
    float sinWave = sin(dotProduct);
    sinWave *= (c + time);
    return frac(sinWave);
}

int IsBetween(float value, float minValue, float maxValue) {
    return step(minValue, value) * step(value, maxValue);
}

float FlipOneZero(float value) {
    return 1 - value;
}

/*
int IsBetweenFloat2(float2 value, float2 minValue, float2 maxValue) {
    return (step(minValue.x, value.x) * step(value.x, maxValue.x)) 
            * (step(minValue.y, value.y) * step(value.y, maxValue.y));
}
*/