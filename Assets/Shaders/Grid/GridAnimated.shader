Shader "Custom/Grid/Animated"
{
    Properties{
        _MainTex ("Main Texture", 2D) = "white" { }
        _Col1 ("Color One", Color) = (0, 0, 0, 1)
        _Col2 ("Color Two", Color) = (1, 1, 1, 1)
        _GridLength ("Grid Lenght", Range(0, 10)) = 1
        _RotationSpeed ("Rotation Speed", Float) = 0
        _ScaleSpeed ("Scale Speed", Float) = 1
    }

    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"
            #include "Assets/Shaders/Includes/RotationHelper.cginc"
            
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            fixed4 _Col1,_Col2;
            float _GridLength, _RotationSpeed, _ScaleSpeed;

            struct vertexInput {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vertexOutput {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            vertexOutput vert (vertexInput input) {
                vertexOutput output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = input.uv;
                return output;
            }

            float4 frag(vertexOutput input) : SV_TARGET {

                fixed4 displayColor;
                float2 scaledUV = float2(input.uv.x - 0.5, input.uv.y - 0.5) * _GridLength * sin(_Time.y * _ScaleSpeed);
                scaledUV = RotateContinuosly(scaledUV, _RotationSpeed);
                float t = (abs(floor(scaledUV.x) + floor(scaledUV.y))) % 2;
                displayColor = lerp(_Col1, _Col2, t);

                return tex2D(_MainTex, input.uv) * displayColor ;
            }
            ENDCG
        }
    }
}