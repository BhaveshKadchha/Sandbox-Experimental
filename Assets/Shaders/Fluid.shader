Shader "Fluid"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed("Speed", float) = 1
        _Aspect("Aspect", float) = 1

        _InvAmp("Inv Amp", float) = 10


    }
    SubShader
    {
        Tags{ "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha 

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
                float4 color:COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color: COLOR;
            };

            sampler2D _MainTex;

            float4 _MainTex_ST;
            float _MousePosition;
            float _MinDist;
            float _Speed;
            float _InvAmp;
            float _Aspect;

            float4 _MainTex_TexelSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                if(o.uv.y!=0)
                {
                    o.vertex.y+=(sin((_Time.y * _Speed) + o.uv.x+v.vertex.x)/_InvAmp)+ abs(sin(_Time.y*1    )/_InvAmp);
                }
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                return lerp(col, 1, smoothstep(.9, 1, i.uv.y));
            }
            ENDCG
        }
    }
}


