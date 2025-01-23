Shader "Custom/Effect/HalfTone"
{
    Properties
    {
        _BgColor ("BG Color", Color) = (1, 1, 1, 1)
        _DotsColor ("Dots Color", Color) = (0, 0, 0, 1)
        _Density ("Density", Integer) = 1
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            fixed4 _BgColor, _DotsColor;
            float _Density;

            struct vertexInput {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vertexOutput {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            vertexOutput vert(vertexInput input) {
                vertexOutput output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = input.uv;
                return output;
            }

            float4 frag(vertexOutput input) : SV_TARGET {

                float2 scaledUV = float2(input.uv.x, input.uv.y) * _Density;
                float2 fracScaledUV = frac(scaledUV);
                float distFromCenter = distance((0.5, 0.5), fracScaledUV);
                float stepCounter = 0.1 + (0.4 * ((floor(scaledUV) + 1) / _Density));

                fixed4 displayColor = lerp(_DotsColor, _BgColor, step(stepCounter, distFromCenter));
                
                return displayColor;
            }
            ENDCG
        }
    }
}