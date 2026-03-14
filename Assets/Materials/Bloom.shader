Shader "Custom/UnlitBloomSimple"
{
    Properties
    {
        _MainTex ("Main Tex", 2D) = "white" {}
        _Color ("Base Color", Color) = (1,1,1,1)
        _GlowColor ("Glow Color", Color) = (1,0.6,0,1)
        _GlowIntensity ("Glow Intensity", Range(0,20)) = 6.0
        _GlowWidth ("Glow Width", Range(0.01,0.5)) = 0.18
        _GlowSoftness ("Glow Softness", Range(0,0.6)) = 0.25
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Cull Off
        ZWrite Off
        Blend One One

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
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _GlowColor;
            float _GlowIntensity;
            float _GlowWidth;
            float _GlowSoftness;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // base texture and color
                fixed4 tex = tex2D(_MainTex, i.uv) * _Color;

                // distance from center across line width (0=center, 1=edge)
                float d = abs(i.uv.y - 0.5) * 2.0;

                // inner bright rim (sharp)
                float rimStart = 1.0 - _GlowWidth - _GlowSoftness;
                float rimEnd = 1.0 - _GlowWidth;
                float rim = smoothstep(rimStart, rimEnd, d);

                // outer soft halo (soft falloff beyond rim)
                float halo = smoothstep(rimStart, 1.0, d) - rim;
                halo = saturate(halo);

                // combined emissive mask
                float glowMask = saturate(rim + halo);

                // emissive contribution (HDR-capable)
                float3 emissive = _GlowColor.rgb * _GlowIntensity * glowMask;

                // keep base visible but reduce where glow is strong so rim stands out
                float3 baseCol = tex.rgb * (1.0 - glowMask * 0.5);

                return fixed4(baseCol + emissive, tex.a);
            }
            ENDCG
        }
    }
    FallBack "Unlit/Color"
}