Shader "Custom/GrayScale/RedShade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _ColorThreshold ("Color Threshold", Range(1, 10)) = 5
        _ColorRange ("Color Range", Range(0, 1)) = 0.8
        
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
            float _ColorRange, _ColorThreshold, _XMin, _XMax, _YMin, _YMax;

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

                float4 textureColor = tex2D(_MainTex, input.uv);

                float isRed = step(_ColorRange, textureColor.x) + 
                                step(textureColor.x, textureColor.y * _ColorThreshold) +
                                step(textureColor.x, textureColor.z * _ColorThreshold);

                isRed /= 3;
                isRed = step(isRed, 0.5);

                float isInRange = IsBetween(input.uv.x, _XMin, _XMax) * IsBetween(input.uv.y, _YMin, _YMax);
                isRed = isRed * isInRange;

                float4 displayColor = lerp(RGBtoGrayScale(textureColor), textureColor, isRed);

                return displayColor;
            }
            ENDCG
        }
    }
}