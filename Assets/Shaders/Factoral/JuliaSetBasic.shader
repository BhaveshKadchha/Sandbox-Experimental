Shader "Custom/Fractal/Julia/Basic"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _Area ("Area", Vector) = (0,0,4,4)
        _Iteration ("Max Iteration", Range(4,1000)) = 255
        _VarZ ("Real Number", Range(-2,2)) = 0
        _VarC ("Complex Number", Range(-2,2)) = 0
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Iteration, _VarZ, _VarC;
            float4 _Area;

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

                float2 c = float2(_VarZ, _VarC);
                float2 z = _Area.xy + (input.uv - 0.5) * _Area.zw;

                float m = 0;
                const float r = 5;

                for(int iter = 0; iter < _Iteration; iter++)
                {
                    z = float2(z.x * z.x - z.y * z.y, z.x * z.y * 2) + c;
                    if(dot(z, z) < (r * r) - 1) m++;
                    z = clamp(z, -r, r);
                }

                if(m == _Iteration) return 0;

                float dist = length(z);
                float fract = (dist - r) / r * r - r;
                float4 displayColor;
                displayColor = float4(sin(m / 4), sin(m / 5), sin(m / 7), 1) / 4 + 0.75;
                
                return displayColor;
            }
            ENDCG
        }
    }
}