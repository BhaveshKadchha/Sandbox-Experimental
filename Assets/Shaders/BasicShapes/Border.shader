Shader "Custom/Effect/Border"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _InternalColor ("Internal Color", Color) = (1, 1, 1, 1)
        _BorderColor ("Border Color", Color) = (0, 0, 0, 1)
        _BorderLength("Border Length", Range(0, 0.4)) = 0.1
    }
    SubShader
    {
        pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float _BorderLength;
            fixed4 _InternalColor, _BorderColor;

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

            float4 frag(vertexOutput input):SV_TARGET {

                float checker = step(0.2, step(1 - _BorderLength, input.uv.x) + step(1 - _BorderLength, input.uv.y)
                                + step(-_BorderLength, -input.uv.x) + step(-_BorderLength, -input.uv.y));

                float val = (abs(input.uv.x - 0.5) + abs(input.uv.y - 0.5)) * abs(sin(_Time.y)) * checker;
                fixed4 displayColor = lerp(_InternalColor, _BorderColor, val);

                return tex2D(_MainTex, input.uv) * displayColor;
            }
            ENDCG
        }
    }
}