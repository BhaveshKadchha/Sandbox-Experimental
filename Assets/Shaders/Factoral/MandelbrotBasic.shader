Shader "Custom/Fractal/Mandelbrot/Basic"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _Area ("Area", Vector) = (0, 0, 4, 4)
        _MaxIter ("Max Iterations", Range(4, 1000)) = 255
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
            float4 _Area;
            float _MaxIter;

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
                float2 z;
                float iter;

                for(iter = 0; iter < _MaxIter; iter++)
                {
                    z = float2(z.x * z.x - z.y * z.y, 2 * z.x * z.y) + c;
                    if(length(z) > 2) break;
                }

                if(iter >= _MaxIter) return 0;

                float displayNumber = sqrt(iter / _MaxIter);
                return displayNumber.xxxx;
            }
            ENDCG
        }
    }
}