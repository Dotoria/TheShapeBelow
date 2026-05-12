Shader "Custom/SmoothBackground"
{
    Properties
    {
        _ColorA ("Color A", Color) = (0.027, 0.067, 0.114, 1)
        _ColorB ("Color B", Color) = (0.063, 0.235, 0.369, 1)
        _ColorC ("Color C", Color) = (0.416, 0.0, 0.722, 1)

        _Speed ("Speed", Float) = 0.25
        _Scale ("Scale", Float) = 2.0
        _Strength ("Blend Strength", Range(0, 1)) = 0.65

        _DownFlowSpeed ("Down Flow Speed", Float) = 0.08
        _PlayerFlowStrength ("Player Flow Strength", Float) = 0.15
        _FlowOffset ("Flow Offset", Vector) = (0, 0, 0, 0)

        _MistStrength ("Aqua Mist Strength", Range(0, 1)) = 0.25
        _VignetteStrength ("Vignette Strength", Range(0, 1)) = 0.45
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

            float _Speed;
            float _Scale;
            float _Strength;

            float _DownFlowSpeed;
            float _PlayerFlowStrength;
            float4 _FlowOffset;

            float _MistStrength;
            float _VignetteStrength;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float time = _Time.y * _Speed;

                // 기본적으로 아래로 흐르는 느낌.
                // UV y를 시간에 따라 올리면 화면상 패턴이 아래로 내려가는 것처럼 보임.
                float2 flowUv = i.uv;
                flowUv.y += _Time.y * _DownFlowSpeed;

                // 플레이어 이동 반대 방향으로 배경 흐름 추가
                flowUv += _FlowOffset.xy * _PlayerFlowStrength;

                float wave1 = sin((flowUv.x * _Scale + time) * 3.14159) * 0.5 + 0.5;
                float wave2 = sin((flowUv.y * _Scale - time * 0.8) * 3.14159) * 0.5 + 0.5;
                float wave3 = sin(((flowUv.x + flowUv.y) * _Scale + time * 0.6) * 3.14159) * 0.5 + 0.5;

                // 우주 남색 -> 심해 블루 -> 보라 에너지
                fixed3 colorAB = lerp(_ColorA.rgb, _ColorB.rgb, wave1);
                fixed3 colorABC = lerp(colorAB, _ColorC.rgb, wave2 * _Strength);

                // 청록 안개 느낌
                fixed3 aquaMist = fixed3(0.12, 0.82, 0.90);
                colorABC = lerp(colorABC, colorABC + aquaMist * 0.25, wave3 * _MistStrength);

                // 중심부는 살짝 밝게, 가장자리는 어둡게
                float2 center = i.uv - 0.5;
                float vignette = 1.0 - saturate(length(center) * 1.35);
                fixed3 finalColor = colorABC;
                finalColor *= lerp(1.0 - _VignetteStrength, 1.0, vignette);

                // 아주 약한 심해/우주 광택
                finalColor += wave3 * 0.035;

                return fixed4(finalColor, 1);
            }
            ENDCG
        }
    }
}