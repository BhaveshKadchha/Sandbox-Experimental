Shader "Custom/Noise/Basic"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _ColorOne ("Color One", Color) = (0,0,0,1)
        _ColorTwo ("Color Two", Color) = (1,1,1,1)
        _Noice ("Noice Distortion", Float) = 0
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Assets/Shaders/Includes/CustomHelper.cginc"

            sampler2D _MainTex;
            float _Noice;
            fixed4 _ColorOne, _ColorTwo;
            
            struct vertexInput {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vertexOutput {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            vertexOutput vert(vertexInput input) {
                vertexOutput output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = input.uv;
                return output;
            }

            float4 frag(vertexOutput input) : SV_TARGET {

                float2 scaledUV = input.uv;
                float noiceDistortion  = Noise(scaledUV, _Noice);
                fixed4 displayColor = lerp(_ColorOne, _ColorTwo, noiceDistortion);

                return tex2D(_MainTex, scaledUV) * displayColor ;
            }
            ENDCG
        }
    }
}