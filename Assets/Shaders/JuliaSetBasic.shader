Shader "Fractal/JuliaSet/Basic"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Area("Area", vector) = (0,0,4,4)
        _Iteration("Max Iteration", range(4,1000)) = 255
        _VarZ("Real Number", range(-2,2)) = 0
        _VarC("Complex Number", range(-2,2)) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Iteration, _Aspect, _VarZ, _VarC;
            float4 _Area;

            fixed4 frag (v2f i) : SV_Target
            {
                // Mandelbrot
                // float2 c = _Area.xy + (i.uv - 0.5) * _Area.zw;
                // float2 v = 0;

                // Julia
                float2 c = float2(_VarZ, _VarC);
                float2 z = _Area.xy + (i.uv - 0.5) * _Area.zw;

                float m = 0;
                const float r = 5;

                for(int iter = 0; iter < _Iteration; iter++)
                {
                    z = float2(z.x * z.x - z.y * z.y, z.x * z.y * 2) + c;

                    if(dot(z,z) < (r*r) - 1)
                        m++;

                    z = clamp(z, -r, r);
                }

                if(m == _Iteration) return 0;

                float4 color;
                float dist = length(z);
                float fract = (dist - r)/r*r - r;
                color = float4(sin(m/4),sin(m/5),sin(m/7),1)/4 + 0.75;
                return color;
            }
            ENDCG
        }
    }
}
