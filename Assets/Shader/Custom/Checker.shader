Shader "Unlit/Checker"
{
    Properties
    {
        _Size("Size", Int) = 2
        _Color1("Color 1", Color) = (1, 1, 1, 1)
        _Color2("Color 2", Color) = (0, 0, 0, 1)
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            int _Size;
            float4 _Color1;
            float4 _Color2;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            int xor(int a, int b){
                return a == b ? 0 : 1;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = frac(i.uv * _Size);
                float a = xor(step(uv.y, .5), step(.5, uv.x));
                fixed4 col = lerp(_Color1, _Color2, a);
                return col;
            }
            ENDCG
        }
    }
}
