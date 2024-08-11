Shader "Custom/MyOutLineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Thickness ("Thickness", float) = 0.02
        _Color ("Color", Color) = (0,0,0,1) 
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType"="Transparent"
        }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        LOD 100

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Thickness;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //OutlineOffsetUp
                float2 offsetUpUV = i.uv + float2(0, -_Thickness);
                fixed4 outCol = float4(1,1,1,1) * tex2D(_MainTex, offsetUpUV);

                //OutlineOffsetDown
                float2 offsetDownUV = i.uv + float2(0, _Thickness);
                outCol += float4(1,1,1,1) * tex2D(_MainTex, offsetDownUV);

                //OutlineOffsetLeft
                float2 offsetLeftUV = i.uv + float2(_Thickness, 0);
                outCol += float4(1,1,1,1) * tex2D(_MainTex, offsetLeftUV);

                //OutlineOffsetRight
                float2 offsetRighttUV = i.uv + float2(-_Thickness, 0);
                outCol += float4(1,1,1,1) * tex2D(_MainTex, offsetRighttUV);

                outCol.a = clamp(outCol.a,0,1);
                

                //MainTex
                fixed4 texCol = tex2D(_MainTex, i.uv);

                //HollowOutOutline
                outCol.a -= texCol.a;
                float3 outCol2 = outCol.rgb;
                outCol2 *= float3(outCol.a,outCol.a,outCol.a);

                //OutlineColor
                outCol2 *= _Color;

                //CutInCol
                float3 inCol = texCol.rgb;
                inCol *= float3(texCol.a,texCol.a,texCol.a);

                return float4(inCol + outCol2,outCol.a + texCol.a);
            }
            ENDCG
        }
    }
}
