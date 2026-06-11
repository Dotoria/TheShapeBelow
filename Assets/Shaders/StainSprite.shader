Shader "Custom/StainSprite"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        _CorruptionColor ("Corruption Color", Color) = (0, 0, 0, 1)

        _Corruption ("Corruption", Range(0,1)) = 0

        _NoiseScale ("Noise Scale", Float) = 8
        _NoiseStrength ("Noise Strength", Range(0,1)) = 0.15
        _EdgeSoftness ("Edge Softness", Range(0.001,0.2)) = 0.05
    }

    SubShader
    {
        Tags
        {
            "Queue"="Geometry"
            "RenderType"="Opaque"
        }

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _MainColor;
            fixed4 _CorruptionColor;

            float _Corruption;
            float _NoiseScale;
            float _NoiseStrength;
            float _EdgeSoftness;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float Random(float2 p)
            {
                return frac(
                    sin(dot(p, float2(12.9898,78.233)))
                    * 43758.5453123
                );
            }

            float Noise(float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);

                float a = Random(i);
                float b = Random(i + float2(1,0));
                float c = Random(i + float2(0,1));
                float d = Random(i + float2(1,1));

                float2 u = f * f * (3.0 - 2.0 * f);

                return lerp(
                    lerp(a, b, u.x),
                    lerp(c, d, u.x),
                    u.y
                );
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);

                float dist = distance(i.uv, center);

                float n = Noise(i.uv * _NoiseScale);

                float corruptionValue =
                    dist + n * _NoiseStrength;

                float threshold =
                    (1.0 - _Corruption) * 0.7;

                float mask =
                    smoothstep(
                        threshold - _EdgeSoftness,
                        threshold + _EdgeSoftness,
                        corruptionValue
                    );

                fixed3 color =
                    lerp(
                        _MainColor.rgb,
                        _CorruptionColor.rgb,
                        mask
                    );

                return fixed4(color, 1);
            }

            ENDCG
        }
    }
}