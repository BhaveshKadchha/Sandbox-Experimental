Shader "Custom/Border"
{
    Properties
    {
        _InternalColor ("Internal Color", Color) = (1,1,1,1)
        _BorderColor ("Border Color", Color) = (0,0,0,1)
        _BorderLength("Border Length",Range(0,0.4)) = 0.1
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        pass
        {
            CGPROGRAM
            #include "unityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag

            struct appData
            {
                float4 vertex : POSITION;
                float2 uv:TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv:TEXCOORD0;
            };

            v2f vert(appData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _BorderLength;
            fixed4 _InternalColor, _BorderColor;

            float4 frag(v2f i):SV_TARGET
            {
                fixed4 col;
                float checker = step(0.2,step(1-_BorderLength,i.uv.x) + step(1-_BorderLength,i.uv.y) + step(-_BorderLength,-i.uv.x) + step(-_BorderLength,-i.uv.y));

                // float val = (abs(i.uv.x - 0.5) + abs(i.uv.y - 0.5)) * step(0.2,checker);
                float val = (abs(i.uv.x - 0.5) + abs(i.uv.y - 0.5)) * abs(sin(_Time.y)) * checker;

                col = lerp(_InternalColor, _BorderColor, val);
                return tex2D(_MainTex,i.uv) * col;
            }
            ENDCG
        }
    }
}