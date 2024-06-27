Shader "Custom/HalfTone"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _bgColor ("BG Color", Color) = (1,1,1,1)
        _dotColor ("Dot Color", Color) = (0,0,0,1)
        _range("range",Range(0,30)) = 1
    }
    SubShader
    {
        Pass
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

            sampler2D _MainTex;
            fixed4 _bgColor, _dotColor;
            float _range;

            v2f vert(appData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i):SV_TARGET
            {
                fixed4 col;
                float2 center = (0.5,0.5);
                float2 scaledUV = float2(i.uv.x,i.uv.y)*_range;
                // scaledUV = frac(scaledUV);
                float2 fracScaledUV = frac(scaledUV);
                
                float t = distance(center,fracScaledUV);

                // float stepCounter = 0.5 * ((floor(scaledUV)+1)/_range);

                float stepCounter = 0.1 + (0.4 * ((floor(scaledUV)+1)/_range));

                col = lerp(_bgColor, _dotColor,step(stepCounter,t));
                return tex2D(_MainTex,i.uv) * col;
            }
            ENDCG
        }
    }
}