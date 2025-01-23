Shader "Custom/Effect/Ripple"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _RippleColor ("Ripple Color", Color) = (1, 1, 1, 1)
        _RippleTravelDist ("Ripple Travel Length", Range(0.1, 1)) = 0.5
        _RippleWidth ("Ripple Width", Range(0.01, 0.1)) = 0.1
        _RippleSpeed ("Ripple Speed", Range(0.1, 1)) = 0.5
    }
    SubShader
    {
        pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

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

            sampler2D _MainTex;
            fixed4 _RippleColor;
            float _RippleWidth;
            float _RippleTravelDist;
            float _RippleSpeed;
            fixed4 whiteColor = (1, 1, 1, 1);

            float4 frag(vertexOutput input) : SV_TARGET {

                fixed2 center = (0.5, 0.5);
                float dist = distance(center, input.uv);

                float outerRadius = frac(_Time.y * _RippleSpeed);
                float innerRadius = outerRadius - (_RippleWidth * outerRadius);
                float isRippleOutput = frac((step(_RippleTravelDist * outerRadius, dist) + step(_RippleTravelDist * innerRadius, dist)) / 2);

                fixed4 rippleOutput = lerp(_RippleColor, whiteColor, isRippleOutput);
                return tex2D(_MainTex, input.uv) * rippleOutput;
            }
            ENDCG
        }
    }
}