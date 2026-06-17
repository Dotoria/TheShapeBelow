Shader "Custom/CharacterSprite"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _Corruption ("Corruption", Range(0,1)) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Cull Off
        Lighting Off
        ZWrite Off

        Blend SrcAlpha One

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _MainColor;
            float _Corruption;

            sampler2D _MainTex;

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            float Random(float2 p)
            {
                return frac(sin(dot(p,float2(12.9898,78.233))) * 43758.5453123);
            }

            float Noise(float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);

                float a = Random(i);
                float b = Random(i + float2(1,0));
                float c = Random(i + float2(0,1));
                float d = Random(i + float2(1,1));

                float2 u = f*f*(3.0-2.0*f);

                return lerp(
                    lerp(a,b,u.x),
                    lerp(c,d,u.x),
                    u.y
                );
            }

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                const float NoiseScale = 8.0;
                const float NoiseStrength = 0.15;
                const float EdgeSoftness = 0.05;

                const float GlowStrength = 2.5;
                const float GlowAlpha = 1;

                const float PulseSpeed = 2.5;
                const float PulseAmount = 0.25;

                float2 center = float2(0.5,0.5);
                float dist = distance(i.uv, center);
                float n = Noise(i.uv * NoiseScale);
                float corruptionValue = dist + n * NoiseStrength;
                float threshold = (1.0 - _Corruption) * 0.7;

                float mask = smoothstep
                (
                    threshold - EdgeSoftness,
                    threshold + EdgeSoftness,
                    corruptionValue
                );

                fixed3 stainColor = lerp
                (
                    _MainColor.rgb,
                    float3(0,0,0),
                    mask
                );

                float pulse = sin(_Time.y * PulseSpeed) * 0.5 + 0.5;
                float pulsePower = 1.0 + pulse * PulseAmount;
                float glowMask = 1.0 - mask;

                fixed3 glow = _MainColor.rgb * GlowStrength * pulsePower * glowMask;
                fixed3 finalColor = stainColor + glow;

                return fixed4(finalColor, GlowAlpha);
            }

            ENDCG
        }
    }
}