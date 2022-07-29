Shader "Unlit/SpriteRepeat"
{
     Properties
    {
        [PerRendererData]_MainTex ("Texture", 2D) = "white" {}
        _Size("Size", Float) = 2
        _Speed("Speed", Range(.5, 10)) = 2
        [PerRendererData]_StencilComp ("Stencil Comparison", Float) = 8
		[PerRendererData]_Stencil ("Stencil ID", Float) = 0
		[PerRendererData]_StencilOp ("Stencil Operation", Float) = 0
		[PerRendererData]_StencilWriteMask ("Stencil Write Mask", Float) = 255
		[PerRendererData]_StencilReadMask ("Stencil Read Mask", Float) = 255
		[PerRendererData]_ColorMask ("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		ZWrite Off
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		Lighting Off

		Stencil
         {
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _Size;
            float _Speed;
            sampler2D _MainTex;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float4 color : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            int xor(int a, int b){
                return a == b ? 0 : 1;
            }

            float4 frag (v2f i) : SV_Target
            {
                float t = _Time.y * _Speed;
                float2 uv = i.uv;
                uv.x = frac(uv.x * _Size + t);
                float4 col = tex2D(_MainTex, uv) * i.color;
                return col;
            }
            ENDCG
        }
    }
}
