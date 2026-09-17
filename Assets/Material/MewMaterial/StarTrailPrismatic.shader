Shader "Mew/Visual/Star Trail Prismatic"
{
    Properties
    {
        _MainTex ("Trail Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _RainbowSpeed ("Rainbow Speed", Range(-3,3)) = 0.22
        _RainbowScale ("Rainbow Repetitions", Range(0,8)) = 1.5
        _Saturation ("Saturation", Range(0,1)) = 0.62
        _Value ("Brightness", Range(0,4)) = 1.12
        _CoreStrength ("Center Core", Range(0,3)) = 0.45
        _Opacity ("Opacity", Range(0,1)) = 0.78
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "IgnoreProjector"="True"
        }
        Cull Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);

        CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            half4 _Color;
            half _RainbowSpeed;
            half _RainbowScale;
            half _Saturation;
            half _Value;
            half _CoreStrength;
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
            output.color = input.color * _Color;
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

        half4 Frag(Varyings input) : SV_Target
        {
            half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
            half hue = frac(input.uv.x * _RainbowScale - _Time.y * _RainbowSpeed + input.uv.y * 0.12h);
            half3 prism = HSV(hue, _Saturation, _Value);
            half center = pow(saturate(1.0h - abs(input.uv.y * 2.0h - 1.0h)), 5.0h);
            half alpha = tex.a * input.color.a * _Opacity;
            half3 rgb = (prism * input.color.rgb + center * _CoreStrength) * alpha;
            return half4(rgb, alpha);
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
