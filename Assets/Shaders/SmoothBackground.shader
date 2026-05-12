Shader "Custom/SmoothBackground"
{
    Properties
    {
        _ColorA ("Deep Space", Color) = (0.027, 0.067, 0.114, 1)
        _ColorB ("Deep Sea Blue", Color) = (0.063, 0.235, 0.369, 1)
        _ColorC ("Violet Energy", Color) = (0.416, 0.0, 0.722, 1)
        _MistColor ("Aqua Mist", Color) = (0.12, 0.82, 0.90, 1)

        _Speed ("Color Speed", Float) = 0.15
        _Scale ("Noise Scale", Float) = 3.0
        _Strength ("Violet Strength", Range(0, 1)) = 0.55

        _DownFlowSpeed ("Down Flow Speed", Float) = 0.18
        _PlayerFlowStrength ("Player Flow Strength", Float) = 0.45
        _FlowOffset ("Flow Offset", Vector) = (0, 0, 0, 0)

        _WarpStrength ("Warp Strength", Range(0, 1)) = 0.22
        _MistStrength ("Mist Strength", Range(0, 1)) = 0.35
        _VignetteStrength ("Vignette Strength", Range(0, 1)) = 0.5
        _Brightness ("Brightness", Range(0, 2)) = 1.0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Background"
            "RenderType"="Opaque"
        }

        Cull Off
        ZWrite Off
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _ColorA;
            fixed4 _ColorB;
            fixed4 _ColorC;
            fixed4 _MistColor;

            float _Speed;
            float _Scale;
            float _Strength;

            float _DownFlowSpeed;
            float _PlayerFlowStrength;
            float4 _FlowOffset;

            float _WarpStrength;
            float _MistStrength;
            float _VignetteStrength;
            float _Brightness;

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

            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }

            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);

                float a = hash(i);
                float b = hash(i + float2(1.0, 0.0));
                float c = hash(i + float2(0.0, 1.0));
                float d = hash(i + float2(1.0, 1.0));

                float2 u = f * f * (3.0 - 2.0 * f);

                return lerp(a, b, u.x)
                     + (c - a) * u.y * (1.0 - u.x)
                     + (d - b) * u.x * u.y;
            }

            float fbm(float2 p)
            {
                float value = 0.0;
                float amplitude = 0.5;

                value += amplitude * noise(p);
                p *= 2.02;
                amplitude *= 0.5;

                value += amplitude * noise(p);
                p *= 2.03;
                amplitude *= 0.5;

                value += amplitude * noise(p);
                p *= 2.01;
                amplitude *= 0.5;

                value += amplitude * noise(p);

                return value;
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
                float t = _Time.y;

                float2 uv = i.uv;

                float2 flowUv = uv;
                flowUv.y += t * _DownFlowSpeed;

                float2 playerFlow = float2(_FlowOffset.x, -_FlowOffset.y);
                flowUv += playerFlow * _PlayerFlowStrength;

                float2 warpUv = flowUv * _Scale;
                float warpX = fbm(warpUv + float2(0.0, t * 0.12));
                float warpY = fbm(warpUv + float2(8.3, -t * 0.10));

                float2 warp = float2(warpX - 0.5, warpY - 0.5) * _WarpStrength;
                warp += _FlowOffset.xy * 0.25;

                float2 finalUv = flowUv + warp;

                float n1 = fbm(finalUv * _Scale + float2(0.0, -t * _Speed));
                float n2 = fbm(finalUv * (_Scale * 1.8) + float2(4.0, -t * _Speed * 1.4));
                float n3 = fbm(finalUv * (_Scale * 0.65) + float2(-3.0, -t * _Speed * 0.6));

                fixed3 deepBase = lerp(_ColorA.rgb, _ColorB.rgb, n1);
                fixed3 violetLayer = lerp(deepBase, _ColorC.rgb, n2 * _Strength);

                fixed3 mist = _MistColor.rgb * n3 * _MistStrength;
                fixed3 finalColor = violetLayer + mist;

                float verticalDepth = smoothstep(0.0, 1.0, uv.y);
                finalColor = lerp(finalColor * 0.75, finalColor, verticalDepth);

                float2 center = uv - 0.5;
                float vignette = 1.0 - saturate(length(center) * 1.35);
                finalColor *= lerp(1.0 - _VignetteStrength, 1.0, vignette);

                finalColor *= _Brightness;

                return fixed4(finalColor, 1.0);
            }
            ENDCG
        }
    }
}