Shader "Unlit/SpriteParticle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
            #include "../noiseutils.cginc"

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
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float draw_cross(float2 uv){
                float th = .45;
                float s = .3;
                float cross = step(th, uv.x) * step(uv.x, 1-th) + step(th, uv.y) * step(uv.y, 1-th);
                cross *= step(s, uv.y) * step(uv.y, 1-s);
                cross *= step(s, uv.x) * step(uv.x, 1-s);
                cross = saturate(cross);

                return cross;
            }

            float4 draw_layer(float2 uv, float o, float t, float size){
                t += o;
                uv -= .5;
                uv += .5;
                uv = frac(uv + o);
                uv.y -= t;
                uv.x += sin(t) * .1;
                uv = frac(uv);
                
                float2 guv = uv * size;
                float2 fuv = frac(guv);
                float2 iuv = floor(guv);

                float r = hash21(iuv);
                fuv = frac(fuv);
                fuv -= .5;
                fuv *= lerp(1.5, 2.5, r);
                fuv += .5;

                return tex2D(_MainTex, fuv) * step(r, .1);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = _Time.y * .5;
                float2 uv = i.uv;
                float s = 2;
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                float4 c = draw_layer(uv, 1.58742, t, 3 * s);
                c += draw_layer(uv, 12.666, t * .9, 2 * s);
                c += draw_layer(uv, 584.666, t * .8, 1 * s);
                c = saturate(c);
                return i.color * c;
            }
            ENDCG
        }
    }
}
