Shader "Custom/Grid"
{
    properties{
        _MainTex("Main Texture", 2D) = "white" {}
        _col1("color 1",Color) = (1,1,1,1)
        _col2("color 2",Color) = (1,1,1,1)
        _range("range",Range(0,10)) = 1
        _rotationSpeed("Rot Speed",float) = 1
        // _rotateTo("Rot Angle",float) = 0
    }

    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #include "unityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag
            #include "CustomMethods.cginc"

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

            sampler2D _MainTex;
            fixed4 _col1,_col2;
            float _range,_rotationSpeed,_rotateTo,_animationSpeed;

            v2f vert(appData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i):SV_TARGET
            {
                fixed4 col;

                // ------------------ OLD WAY ---------------------- //

                // float2 center = (0.5,0.5);
                // float2 scaledUV = float2(i.uv.x,i.uv.y)*_range;

                // scaledUV = RotateContinuosly(scaledUV, _rotationSpeed);
                // scaledUV = RotateTo(scaledUV, _rotateTo);
                // scaledUV = frac(scaledUV);

                // float t = frac((step(0.5,scaledUV.y) + step(0.5,scaledUV.x))/2);
                // col = lerp(_col1, _col2,step(0.5,t));
                // float t = abs(scaledUV.x - 0.5) + abs(scaledUV.y - 0.5);

                // float t = distance(center,scaledUV) + 0.1;
                // col = lerp(_col1, _col2,t);

                // ------------------ OLD END ---------------------- //


                // ------------------ NEW WAY -------------------- //

                // float2 scaledUV = float2(i.uv.x - 0.5, i.uv.y - 0.5) * _range;
                float2 scaledUV = float2(i.uv.x - 0.5, i.uv.y - 0.5) * _range * sin(_Time.y);

                scaledUV = RotateContinuosly(scaledUV, _rotationSpeed);
                // scaledUV = RotateTo(scaledUV, _rotateTo);

                float t = (abs(floor(scaledUV.x) + floor(scaledUV.y)))%2;
                col = lerp(_col1,_col2,t);

                // ------------------ NEW END ---------------------- //

                return tex2D(_MainTex, i.uv) * col;
            }
            ENDCG
        }
    }
}