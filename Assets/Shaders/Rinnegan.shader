Shader "Custom/Rinnegan"
{
    Properties
    {
        _col1 ("Circle Color", Color) = (1,1,1,1)
        _col2 ("BG Color", Color) = (0,0,0,1)
        _MainTex ("Texture", 2D) = "white" {}
        _lineCount("Line Count",Range(1,10)) = 5
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
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
            fixed4 _col1, _col2;
            float  _lineCount;

            float4 frag(v2f i):SV_TARGET
            {
                fixed4 col;
                fixed2 center = (0.5, 0.5);
                float dist = distance(center,i.uv);


                // RINNEGAN EFFECT
                float t = _Time.y/4;
                float pointOuter = abs( sin((t - (dist * _lineCount)) * 7));
                col = lerp(_col1, _col2, pointOuter);
                return tex2D(_MainTex,i.uv) * col;
            }
            ENDCG
        }
    }
}