Shader "Custom/Effect/ContinueousRipple"
{
    Properties
    {
        _ColorOne ("Ripple Color", Color) = (1, 1, 1, 1)
        _ColorTwo ("BG Color", Color) = (0, 0, 0, 1)
        _Density ("Density", Range(1, 10)) = 5
    }
    SubShader
    {
        pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            fixed4 _ColorOne, _ColorTwo;
            float  _Density;

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

                float distanceFromCenter = distance((0.5, 0.5), input.uv);

                float time = _Time.y / 4;
                float colorVal = abs(sin((time - (distanceFromCenter * _Density)) * 7));
                fixed4 displayColor = lerp(_ColorOne, _ColorTwo, colorVal);

                return displayColor;
            }
            ENDCG
        }
    }
}