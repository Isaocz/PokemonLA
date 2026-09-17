Shader "Mew/Visual/Mew Boss Prismatic Outline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Renderer Tint", Color) = (1,1,1,1)
        _CoreTint ("Core Tint", Color) = (1,0.96,1,1)
        _OutlineWidth ("Outline Width (Texels)", Range(0.5,4)) = 1.35
        _OutlineIntensity ("Outline Intensity", Range(0,4)) = 1.35
        _OutlineAlpha ("Outline Alpha", Range(0,1)) = 0.9
        _RainbowSpeed ("Rainbow Speed", Range(-2,2)) = 0.16
        _RainbowScale ("Rainbow Scale", Range(0,8)) = 1.6
        _Saturation ("Rainbow Saturation", Range(0,1)) = 0.72
        _Value ("Rainbow Value", Range(0,3)) = 1.2
        _CoreBoost ("Core Brightness", Range(0,1)) = 0.08
        _PulseAmount ("Pulse Amount", Range(0,0.5)) = 0.06
        _PulseSpeed ("Pulse Speed", Range(0,8)) = 1.4
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
            half4 _CoreTint;
            half _OutlineWidth;
            half _OutlineIntensity;
            half _OutlineAlpha;
            half _RainbowSpeed;
            half _RainbowScale;
            half _Saturation;
            half _Value;
            half _CoreBoost;
            half _PulseAmount;
            half _PulseSpeed;
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
            float2 px = _MainTex_TexelSize.xy * _OutlineWidth;

            half around = 0;
            around = max(around, SampleAlpha(input.uv + float2( px.x, 0)));
            around = max(around, SampleAlpha(input.uv + float2(-px.x, 0)));
            around = max(around, SampleAlpha(input.uv + float2(0,  px.y)));
            around = max(around, SampleAlpha(input.uv + float2(0, -px.y)));
            around = max(around, SampleAlpha(input.uv + float2( px.x,  px.y)));
            around = max(around, SampleAlpha(input.uv + float2(-px.x,  px.y)));
            around = max(around, SampleAlpha(input.uv + float2( px.x, -px.y)));
            around = max(around, SampleAlpha(input.uv + float2(-px.x, -px.y)));

            half outerEdge = saturate(around - tex.a);
            half hue = frac(_Time.y * _RainbowSpeed + input.uv.y * _RainbowScale + input.uv.x * 0.22h);
            half3 prism = HSV(hue, _Saturation, _Value);

            half pulse = 1.0h + sin(_Time.y * _PulseSpeed + input.uv.y * 5.0h) * _PulseAmount;
            half alpha = tex.a * input.color.a;
            half3 core = tex.rgb * input.color.rgb * _CoreTint.rgb * (1.0h + _CoreBoost) * pulse;
            half edgeAlpha = outerEdge * _OutlineAlpha * input.color.a;
            half3 edge = prism * (_OutlineIntensity * edgeAlpha);

            half outAlpha = saturate(alpha + edgeAlpha);
            half3 premultiplied = core * alpha + edge;
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
