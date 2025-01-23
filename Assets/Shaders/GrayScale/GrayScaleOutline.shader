Shader "Custom/GrayScale/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }

        _XMin ("X Min", Range(0, 0.5)) = 0
        _XMax ("X Max", Range(0.5, 1)) = 1
        _YMin ("Y Min", Range(0, 0.5)) = 0
        _YMax ("Y Max", Range(0.5, 1)) = 1
    }
    SubShader
    {
        pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"
            #include "Assets/Shaders/Includes/ColorHelper.cginc"
            #include "Assets/Shaders/Includes/CustomHelper.cginc"

            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float _XMin, _XMax, _YMin, _YMax;

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

                float isInRange = IsBetween(input.uv.x, _XMin, _XMax) * IsBetween(input.uv.y, _YMin, _YMax);
                float4 displayColor = lerp(RGBtoGrayScale(textureColor), textureColor, isInRange);
                
                return displayColor;
            }
            ENDCG
        }
    }
}