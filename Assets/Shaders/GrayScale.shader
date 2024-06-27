Shader "Custom/GrayScale"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SplashTex ("Texture", 2D) = "white" {}
        _Range("Range Of Colour",Range(0,2)) = 1
        _CheckColor("Pick Color",Color) = (1,1,1,1)
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        pass
        {
            Tags{ "DisableBatching" = "false"}
            CGPROGRAM
            #include "unityCG.cginc"
            #include "CustomMethods.cginc"
            #pragma vertex vert
            #pragma fragment frag

            struct appData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                // float val = (o.vertex.x + o.vertex.y)/10;
                // o.color.xyz = float3(v.vertex.xyz);
                // o.color.w = 1;
                return o;
            }

            sampler2D _MainTex,_SplashTex;
            float4 _CheckColor;
            float _Range;

            float4 frag(v2f i):SV_TARGET
            {
                float4 col = tex2D(_MainTex,i.uv);
                float4 col2 = tex2D(_SplashTex,i.uv);

                // Use color for effect.
                // float distOne = distance(_CheckColor,col);
                // float detectCol = step(distOne,_Range);
                // // float detectCol = step(frac(_Time.y * 0.3),(i.uv.x + i.uv.y)/2);
                // float4 newCol = lerp(RGBtoGrayScale(col),col,detectCol);

                // Use second image for effect.
                float detectCol = step(0.1,col2.a);

                float4 newCol = lerp(RGBtoGrayScale(col),col,detectCol);
                // float4 newCol = lerp(_CheckColor,col,detectCol);

                return newCol;
            }
            ENDCG
        }
    }
}