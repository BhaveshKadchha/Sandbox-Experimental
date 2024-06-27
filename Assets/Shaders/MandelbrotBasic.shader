Shader "Fractal/Mandelbrot/Basic"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Area ("Area", vector) = (0,0,4,4)
        _Angle("Angle",range(-3.1415,3.1415)) = 0
        _MaxIter("Max Iterations",range(4,1000)) = 255
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
            float4 _Area;
            float _Angle, _MaxIter;

            float2 rotation(float2 p,float2 pivot, float angle)
            {
                float s = sin(angle);
                float c = cos(angle);
                p -=pivot;
                p = float2(p.x*c - p.y*s, p.x*s + p.y*c);
                p+=pivot;
                return p;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 c = _Area.xy + (i.uv - 0.5) * _Area.zw;

                c= rotation(c,_Area.xy,_Angle);
                float2 z;
                float iter;

                for(iter = 0; iter < _MaxIter; iter++)
                {
                    z = float2(z.x * z.x - z.y * z.y, 2 * z.x * z.y) + c;            // Mandelbrot
                    if(length(z)>2) break;

                    // z = float2(cos(z.x)*cosh(z.y), -sin(z.x)*sinh(z.y)) + c;          // Cos Mandelbrot
                    // if(length(z)>100) break;
                }

                if(iter >= _MaxIter) return 0;

                float m = sqrt(iter / _MaxIter);
                return m;
            }
            ENDCG
        }
    }
}
