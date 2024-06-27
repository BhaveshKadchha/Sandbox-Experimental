Shader "Custom/Noise"
{
    Properties
    {
        mainColor("Color",Color)=(0,0,0,1)
        noiseColor("Noise Color",Color) = (1,1,1,1)
        noise("Noise",float) = 1
        mainTexture("Texture",2D) = "white"{}
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "CustomMethods.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D mainTexture;
            fixed4 mainColor,noiseColor;
            float noise;

            float4 frag(v2f i):SV_TARGET
            {
                fixed4 col;
                float2 scaledUV = i.uv;

                // This Line Make The  Noise
                // Remove _Time.y if not needed noise to move.
                float val = frac(sin(dot(scaledUV.xy ,float2(32.9898,78.233))) * (43758 + _Time.y));
                // float val = Noise(scaledUV) * _Time.y;

                col = lerp(mainColor,noiseColor,val);

                return tex2D(mainTexture,scaledUV) *col;
            }
            ENDCG
        }
    }
}