Shader "Unlit/CheckerAnimatedTex"
{
    
    Properties
    {
        [PerRendererData]_MainTex ("Texture", 2D) = "white" {}
        _Size("Size", Int) = 2
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

            int _Size;
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

            fixed4 frag (v2f i) : SV_Target
            {
                float bord = step(abs(i.uv.y - .5), .4);
                float t = _Time.y;
                float2 uv = frac(i.worldPos.xy * _Size);
                uv.x = frac(uv.x + t);
                float a = xor(step(uv.y, .5), step(.5, uv.x));
                fixed3 ch = i.color * lerp(.33, .75, a);
                float2 tuv = frac(uv * 2);// smoothstep(.01, .99, frac(uv * 2));
                fixed4 tx = tex2D(_MainTex, tuv);
                fixed4 col = fixed4(0, 0, 0, 1);
                col.rgb = lerp(ch, tx.rgb * i.color.rgb * .25, tx.a * a);
                col.rgb *= bord;
                return col;
            }
            ENDCG
        }
    }
}
