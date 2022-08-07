Shader "Unlit/Dice"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DetailTex ("Detail", 2D) = "white" {}
        _ValueTex ("Value", 2D) = "white" {}
        [HDR]_Color("Color", Color) = (1,1,1,1)
        _Brightness("Brightness", Float) = 2
        _OuterBrightness("_OuterBrightness", Float) = 2
        _Alpha("Alpha", Range(0, 1)) = 1
        _Thickness("Thickness", Range(0, 0.05)) = .01
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
            sampler2D _DetailTex;
            sampler2D _ValueTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Brightness;
            float _Thickness;
            float _OuterBrightness;
            float _Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 det = tex2D(_DetailTex, i.uv);
                col += det * det.a * _Color * _OuterBrightness * _Alpha;
                float2 uv = smoothstep(.15, .85, 1-i.uv);
                fixed4 val = tex2D(_ValueTex, uv) * _Color;

                uv = smoothstep(.15-_Thickness, .85+_Thickness, 1-i.uv);
                float outer = tex2D(_ValueTex, uv).a - val.a * .5;
                val += outer * _Color * _Brightness;
                return lerp(col, val, val.a * _Alpha);
            }
            ENDCG
        }
    }
}
