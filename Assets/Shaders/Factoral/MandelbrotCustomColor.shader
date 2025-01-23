Shader "Custom/Fractal/Mandelbrot/CustomColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _Area ("Area", Vector) = (0, 0, 4, 4)
        _Angle ("Angle", Range(-3.1415, 3.1415)) = 0
        _MaxIter ("Max Iterations", Range(4, 1000)) = 255
        _Color ("Color", Range(0, 1)) = 0.5
        _Repeat ("Repeat", Float) = 1
        _Speed ("Speed", Float) = 1

        _Color1 ("Color One", Color) = (1,1,1,1)
        _Color2 ("Color Two", Color) = (1,1,1,1)
        _Color3 ("Color Three", Color) = (1,1,1,1)
        _Color4 ("Color Four", Color) = (1,1,1,1)
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Assets/Shaders/Includes/RotationHelper.cginc"
            #include "Assets/Shaders/Includes/ColorHelper.cginc"

            sampler2D _MainTex;
            float _Angle, _MaxIter, _Color, _Repeat, _Speed;
            float4 _Area, _Color1, _Color2, _Color3, _Color4;

            struct vertexInput {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vertexOutput {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            vertexOutput vert (vertexInput input) {
                vertexOutput output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = input.uv;
                return output;
            }

            fixed4 frag (vertexOutput input) : SV_Target {

                float2 c = _Area.xy + (input.uv - 0.5) * _Area.zw;
                c = Rotate(c, _Area.xy, _Angle);

                float r = 20;            // Escape Radius.
                float r2 = r * r;
                float2 z, zPrevious;
                float iter;

                for(iter = 0; iter < _MaxIter; iter++)
                {
                    zPrevious = z;
                    z = float2(z.x * z.x - z.y * z.y, 2 * z.x * z.y) + c;
                    if(length(z) > r) break;
                }

                if(iter >= _MaxIter) return 0;

                float dist = length(z);
                float fracIter = (dist - r) / (r2 - r);     // Linear Interpolation
                fracIter = log2(log(dist) / log(r));        // Double Exponential Interpolation
                iter -= fracIter;
                float m = sqrt(iter / _MaxIter);

                float4 displayColor = sin(float4(.3, .45, .65, 1) * m * 20) * 0.5 + 0.5;
                float position = float2(m * _Repeat + _Time.y * _Speed, _Color);
                displayColor = ColorGradient(position, _Color1, _Color2, _Color3, _Color4);
                
                return displayColor;
            }
            ENDCG
        }
    }
}