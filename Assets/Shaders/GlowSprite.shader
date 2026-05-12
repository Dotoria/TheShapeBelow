Shader "Custom/GlowSprite"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}

        _Color ("Tint", Color) = (1,1,1,1)

        _GlowColor ("Glow Color", Color) = (0.2, 0.9, 1.0, 1)
        _SubGlowColor ("Sub Glow Color", Color) = (0.7, 0.2, 1.0, 1)

        _GlowStrength ("Glow Strength", Range(0, 3)) = 1.0
        _GlowSize ("Glow Size", Range(0, 0.05)) = 0.015
        _PulseSpeed ("Pulse Speed", Float) = 3.0
        _PulseAmount ("Pulse Amount", Range(0, 1)) = 0.4

        _Alpha ("Alpha", Range(0, 1)) = 1.0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            fixed4 _Color;
            fixed4 _GlowColor;
            fixed4 _SubGlowColor;

            float _GlowStrength;
            float _GlowSize;
            float _PulseSpeed;
            float _PulseAmount;
            float _Alpha;

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 baseCol = tex2D(_MainTex, i.uv) * i.color;

                float2 offset = _MainTex_TexelSize.xy * (_GlowSize * 1000.0);

                float alphaAround = 0.0;

                alphaAround += tex2D(_MainTex, i.uv + float2(offset.x, 0)).a;
                alphaAround += tex2D(_MainTex, i.uv + float2(-offset.x, 0)).a;
                alphaAround += tex2D(_MainTex, i.uv + float2(0, offset.y)).a;
                alphaAround += tex2D(_MainTex, i.uv + float2(0, -offset.y)).a;

                alphaAround += tex2D(_MainTex, i.uv + float2(offset.x, offset.y)).a;
                alphaAround += tex2D(_MainTex, i.uv + float2(-offset.x, offset.y)).a;
                alphaAround += tex2D(_MainTex, i.uv + float2(offset.x, -offset.y)).a;
                alphaAround += tex2D(_MainTex, i.uv + float2(-offset.x, -offset.y)).a;

                alphaAround = saturate(alphaAround / 8.0);

                float outlineMask = saturate(alphaAround - baseCol.a);

                float pulse = sin(_Time.y * _PulseSpeed) * 0.5 + 0.5;
                float pulseStrength = lerp(1.0, 1.0 + _PulseAmount, pulse);

                fixed3 mixedGlow = lerp(_GlowColor.rgb, _SubGlowColor.rgb, pulse);

                fixed3 finalColor = baseCol.rgb;

                finalColor += mixedGlow * outlineMask * _GlowStrength * pulseStrength;
                finalColor += mixedGlow * baseCol.a * _GlowStrength * 0.15 * pulseStrength;

                float finalAlpha = max(baseCol.a, outlineMask * _GlowStrength * 0.6);
                finalAlpha *= _Alpha;

                return fixed4(finalColor, finalAlpha);
            }
            ENDCG
        }
    }
}