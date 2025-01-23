Shader "Custom/Experemential/Fluid"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _Speed ("Speed", float) = 1
        _Aspect ("Aspect", float) = 1
        _InvAmp ("Inv Amp", float) = 10
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

            sampler2D _MainTex;

            float4 _MainTex_ST, _MainTex_TexelSize;
            float _MousePosition, _MinDist, _Speed, _Aspect, _InvAmp;

            struct vertexInput {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct vertexOutput {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            vertexOutput vert (vertexInput input) {

                vertexOutput output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                if(output.uv.y!=0)
                    output.vertex.y += (sin((_Time.y * _Speed) + output.uv.x + input.vertex.x) / _InvAmp) 
                                            + abs(sin(_Time.y * 1) / _InvAmp);
                output.color = input.color;
                return output;
            }

            fixed4 frag (vertexOutput input) : SV_Target {

                fixed4 displayColor = tex2D(_MainTex, input.uv) * input.color;
                return lerp(displayColor, 1, smoothstep(.9, 1, input.uv.y));
            }
            ENDCG
        }
    }
}