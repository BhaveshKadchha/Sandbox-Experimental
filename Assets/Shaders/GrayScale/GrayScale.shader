Shader "Custom/GrayScale/Basic"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
    }
    SubShader
    {
        pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"
            #include "Assets/Shaders/Includes/ColorHelper.cginc"

            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;

            struct vertexInput {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vertexOutput {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            vertexOutput vert (vertexInput input) {
                vertexOutput output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = input.uv;
                return output;
            }

            float4 frag(vertexOutput input) : SV_TARGET {

                float4 textureColor = tex2D(_MainTex, input.uv);
                float4 displayColor = RGBtoGrayScale(textureColor);
                return displayColor;
            }
            ENDCG
        }
    }
}