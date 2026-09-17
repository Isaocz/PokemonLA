Shader "Custom/ArenaBoundary2D"
{
    Properties
    {
        _Color ("Color", Color) = (0.7, 0.95, 1.0, 1)
        _Center ("Center", Vector) = (0, 0, 0, 0)
        _PlayerPos ("Player Position", Vector) = (0, 0, 0, 0)

        _Radius ("Radius", Float) = 15
        _BaseWidth ("Base Width", Float) = 0.12
        _WidthAmp ("Width Floating Amount", Float) = 0.05
        _Softness ("Edge Softness", Float) = 0.08

        _GlowWidth ("Glow Width", Float) = 0.8
        _GlowStrength ("Glow Strength", Float) = 1.5

        _RevealRadius ("Local Reveal Radius", Float) = 4
        _ShowDistance ("Show Distance", Float) = 5

        _NoiseFreq ("Noise Frequency", Float) = 16
        _FlowSpeed ("Flow Speed", Float) = 2
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        Blend SrcAlpha One
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float4 _Center;
            float4 _PlayerPos;

            float _Radius;
            float _BaseWidth;
            float _WidthAmp;
            float _Softness;

            float _GlowWidth;
            float _GlowStrength;

            float _RevealRadius;
            float _ShowDistance;

            float _NoiseFreq;
            float _FlowSpeed;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float hash(float n)
            {
                return frac(sin(n) * 43758.5453123);
            }

            float noise1D(float x)
            {
                float i = floor(x);
                float f = frac(x);
                float u = f * f * (3.0 - 2.0 * f);

                return lerp(hash(i), hash(i + 1.0), u);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 world = i.worldPos.xy;
                float2 center = _Center.xy;
                float2 player = _PlayerPos.xy;

                float2 p = world - center;
                float distToCenter = length(p);

                float angle = atan2(p.y, p.x);

                // 边框宽度的浮动
                float wave = sin(angle * 10.0 + _Time.y * _FlowSpeed);
                float n = noise1D(angle * _NoiseFreq + _Time.y * _FlowSpeed);

                float width = _BaseWidth;
                width += wave * _WidthAmp;
                width += (n - 0.5) * _WidthAmp;
                width = max(width, 0.02);

                // 到圆形边框的距离
                float ringDist = abs(distToCenter - _Radius);

                // 主边框
                float core = 1.0 - smoothstep(width, width + _Softness, ringDist);

                // 外发光
                float glow = 1.0 - smoothstep(width, width + _GlowWidth, ringDist);

                // 玩家是否靠近边框
                float playerToRing = abs(length(player - center) - _Radius);
                float nearBoundary = 1.0 - smoothstep(_ShowDistance * 0.35, _ShowDistance, playerToRing);

                // 只显示玩家附近的一段圆弧
                float distToPlayer = length(world - player);
                float localReveal = 1.0 - smoothstep(_RevealRadius * 0.45, _RevealRadius, distToPlayer);

                // 光线轻微闪烁
                float flicker = 0.8 + 0.2 * sin(_Time.y * 5.0 + angle * 20.0);

                float alpha = (core + glow * 0.35 * _GlowStrength);
                alpha *= nearBoundary;
                alpha *= localReveal;
                alpha *= flicker;
                alpha *= _Color.a;

                float3 col = _Color.rgb;
                col *= core * 1.5 + glow * _GlowStrength;

                return fixed4(col, saturate(alpha));
            }
            ENDCG
        }
    }
}