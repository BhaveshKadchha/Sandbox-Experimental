Shader "Custom/Circle"
{
    Properties
    {
        _col1 ("Circle Color", Color) = (1,1,1,1)
        _col2 ("BG Color", Color) = (0,0,0,1)
        _MainTex ("Texture", 2D) = "white" {}
        _circleTravelDist("Circle Travel Length",Range(0.1,1)) = 0.5
        _innerCircle("Inner Circle Length",Range(0.01,0.1)) = 0.1
        _circleExpandSpeed("Expand Speed",Range(0.1,1)) = 0.5
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
            fixed4 _col1,_col2;
            float _innerCircle;
            float _circleTravelDist;
            float _circleExpandSpeed;

            float4 frag(v2f i):SV_TARGET
            {
                fixed4 col;
                fixed2 center = (0.5, 0.5);
                float dist = distance(center,i.uv);

                // float timeRadii = abs(sin(_Time.y));
                float timeRadii = frac(_Time.y * _circleExpandSpeed);

                float innerRadius = timeRadii - (_innerCircle * timeRadii);
                float pointOuter = frac((step(_circleTravelDist * timeRadii, dist) + step(_circleTravelDist * innerRadius, dist)) / 2);

                col = lerp(_col1, _col2, pointOuter);
                return tex2D(_MainTex,i.uv) * col;
            }
            ENDCG
        }
    }
}