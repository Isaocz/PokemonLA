Shader "Mew/Visual/Star Projectile Prismatic"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _RainbowSpeed ("Rainbow Speed", Range(-3,3)) = 0.28
        _RainbowScale ("Rainbow Scale", Range(0,8)) = 1.25
        _Saturation ("Saturation", Range(0,1)) = 0.78
        _Value ("Rainbow Value", Range(0,4)) = 1.25
        _CoreStrength ("White Core Strength", Range(0,5)) = 1.8
        _GlowWidth ("Glow Width (Texels)", Range(0.5,5)) = 1.5
        _GlowStrength ("Glow Strength", Range(0,4)) = 0.72
        _Opacity ("Opacity", Range(0,1)) = 0.96
        [HideInInspector] _RendererColor ("Renderer Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
        }
        Cull Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        float4 _MainTex_TexelSize;
        half4 _RendererColor;

        CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            half4 _Color;
            half _RainbowSpeed;
            half _RainbowScale;
            half _Saturation;
            half _Value;
            half _CoreStrength;
            half _GlowWidth;
            half _GlowStrength;
            half _Opacity;
        CBUFFER_END

        struct Attributes
        {
            float4 positionOS : POSITION;
            float2 uv : TEXCOORD0;
            half4 color : COLOR;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct Varyings
        {
            float4 positionHCS : SV_POSITION;
            float2 uv : TEXCOORD0;
            half4 color : COLOR;
            UNITY_VERTEX_OUTPUT_STEREO
        };

        Varyings Vert(Attributes input)
        {
            Varyings output = (Varyings)0;
            UNITY_SETUP_INSTANCE_ID(input);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
            output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
            output.uv = input.uv * _MainTex_ST.xy + _MainTex_ST.zw;
            output.color = input.color * _Color * _RendererColor;
            return output;
        }

        half3 HueToRGB(half h)
        {
            half3 p = abs(frac(h + half3(0.0h, 0.6666667h, 0.3333333h)) * 6.0h - 3.0h);
            return saturate(p - 1.0h);
        }

        half3 HSV(half h, half s, half v)
        {
            return lerp(half3(1,1,1), HueToRGB(h), s) * v;
        }

        half SampleAlpha(float2 uv)
        {
            return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv).a;
        }

        half4 Frag(Varyings input) : SV_Target
        {
            half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
            float2 px = _MainTex_TexelSize.xy * _GlowWidth;
            half nearby = 0;
            nearby = max(nearby, SampleAlpha(input.uv + float2( px.x, 0)));
            nearby = max(nearby, SampleAlpha(input.uv + float2(-px.x, 0)));
            nearby = max(nearby, SampleAlpha(input.uv + float2(0,  px.y)));
            nearby = max(nearby, SampleAlpha(input.uv + float2(0, -px.y)));
            nearby = max(nearby, SampleAlpha(input.uv + float2( px.x,  px.y)));
            nearby = max(nearby, SampleAlpha(input.uv + float2(-px.x,  px.y)));
            nearby = max(nearby, SampleAlpha(input.uv + float2( px.x, -px.y)));
            nearby = max(nearby, SampleAlpha(input.uv + float2(-px.x, -px.y)));

            half glowMask = saturate(nearby - tex.a);
            float2 centered = input.uv - 0.5;
            half angle = atan2(centered.y, centered.x) * 0.15915494h;
            half hue = frac(_Time.y * _RainbowSpeed + angle + length(centered) * _RainbowScale);
            half3 prism = HSV(hue, _Saturation, _Value);

            half alpha = tex.a * input.color.a * _Opacity;
            half whiteCore = pow(saturate(tex.a), 2.2h) * _CoreStrength;
            half3 body = tex.rgb * input.color.rgb * prism * (_Value * 0.72h);
            half3 core = half3(1,1,1) * whiteCore;
            half haloAlpha = glowMask * input.color.a * 0.72h;
            half3 halo = prism * glowMask * _GlowStrength;

            half outAlpha = saturate(alpha + haloAlpha);
            half3 premultiplied = (body + core) * alpha + halo;
            return half4(premultiplied, outAlpha);
        }
        ENDHLSL

        Pass
        {
            Name "Universal2D"
            Tags { "LightMode"="Universal2D" }
            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing
            ENDHLSL
        }

        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode"="UniversalForward" }
            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing
            ENDHLSL
        }
    }
}
