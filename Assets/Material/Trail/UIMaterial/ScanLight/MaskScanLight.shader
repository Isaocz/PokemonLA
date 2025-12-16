Shader "Custom/MaskScanLight"
{
    Properties
    {
        [NoScaleOffset]_ScanlineTex("_ScanlineTex", 2D) = "white" {}
        Speed("Speed", Float) = -1
        _Color("Color", Color) = (1, 0, 0, 1)
        Till("Till", Vector) = (1, 0, 0, 0)
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}


        //MASK SUPPORT ADD
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        //MASK SUPPORT END

       
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
        }
        
        //MASK SUPPORT ADD
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp] 
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]
        //MASK SUPPORT END

        Pass
        {
            Name "Sprite Unlit"
            Tags
            {
                "LightMode" = "Universal2D"
            }

            // Render State
            Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITEUNLIT
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float4 texCoord0;
            float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float4 uv0;
            float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float4 interp0 : TEXCOORD0;
            float4 interp1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            output.interp1.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            output.color = input.interp1.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _ScanlineTex_TexelSize;
        float Speed;
        float4 _Color;
        float2 Till;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        float4 _MainTex_TexelSize;
        TEXTURE2D(_ScanlineTex);
        SAMPLER(sampler_ScanlineTex);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_Lerp_float(float A, float B, float T, out float Out)
        {
            Out = lerp(A, B, T);
        }

        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }

        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_57343e0bf5e848f391a9a82285d598dd_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_RGBA_0 = SAMPLE_TEXTURE2D(_Property_57343e0bf5e848f391a9a82285d598dd_Out_0.tex, _Property_57343e0bf5e848f391a9a82285d598dd_Out_0.samplerstate, IN.uv0.xy);
            float _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_R_4 = _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_RGBA_0.r;
            float _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_G_5 = _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_RGBA_0.g;
            float _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_B_6 = _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_RGBA_0.b;
            float _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_A_7 = _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_RGBA_0.a;
            UnityTexture2D _Property_16a8b777907e4d068ff0b1d7091edd5d_Out_0 = UnityBuildTexture2DStructNoScale(_ScanlineTex);
            float2 _Property_99075bcf3cc54f27ac6a28f5baea643a_Out_0 = Till;
            float _Property_f7b4fdf7643c42249fef1cd0506fbff3_Out_0 = Speed;
            float _Multiply_12c0db3c13f2429e904d8cc9207cd9d9_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Property_f7b4fdf7643c42249fef1cd0506fbff3_Out_0, _Multiply_12c0db3c13f2429e904d8cc9207cd9d9_Out_2);
            float2 _TilingAndOffset_37af45223b914dd5b8d5d808307020e4_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99075bcf3cc54f27ac6a28f5baea643a_Out_0, (_Multiply_12c0db3c13f2429e904d8cc9207cd9d9_Out_2.xx), _TilingAndOffset_37af45223b914dd5b8d5d808307020e4_Out_3);
            float4 _SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_RGBA_0 = SAMPLE_TEXTURE2D(_Property_16a8b777907e4d068ff0b1d7091edd5d_Out_0.tex, _Property_16a8b777907e4d068ff0b1d7091edd5d_Out_0.samplerstate, _TilingAndOffset_37af45223b914dd5b8d5d808307020e4_Out_3);
            float _SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_R_4 = _SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_RGBA_0.r;
            float _SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_G_5 = _SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_RGBA_0.g;
            float _SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_B_6 = _SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_RGBA_0.b;
            float _SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_A_7 = _SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_RGBA_0.a;
            float4 _Property_8342d789bf65438e897cda488517f29d_Out_0 = _Color;
            float4 _Multiply_d334279329414155852942c249cf3029_Out_2;
            Unity_Multiply_float(_SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_RGBA_0, _Property_8342d789bf65438e897cda488517f29d_Out_0, _Multiply_d334279329414155852942c249cf3029_Out_2);
            float _Split_3bfb1dc84d3d44bb96be19a806e180c3_R_1 = _Multiply_d334279329414155852942c249cf3029_Out_2[0];
            float _Split_3bfb1dc84d3d44bb96be19a806e180c3_G_2 = _Multiply_d334279329414155852942c249cf3029_Out_2[1];
            float _Split_3bfb1dc84d3d44bb96be19a806e180c3_B_3 = _Multiply_d334279329414155852942c249cf3029_Out_2[2];
            float _Split_3bfb1dc84d3d44bb96be19a806e180c3_A_4 = _Multiply_d334279329414155852942c249cf3029_Out_2[3];
            float _Subtract_5295b2d5565546d3822e2139ba56f925_Out_2;
            Unity_Subtract_float(_Split_3bfb1dc84d3d44bb96be19a806e180c3_A_4, 0, _Subtract_5295b2d5565546d3822e2139ba56f925_Out_2);
            float4 _UV_c54048b08a4d4c49b82124d1a78835e3_Out_0 = IN.uv0;
            float _Split_27c67389782f4ae3bc48a4c99e2f87f3_R_1 = _UV_c54048b08a4d4c49b82124d1a78835e3_Out_0[0];
            float _Split_27c67389782f4ae3bc48a4c99e2f87f3_G_2 = _UV_c54048b08a4d4c49b82124d1a78835e3_Out_0[1];
            float _Split_27c67389782f4ae3bc48a4c99e2f87f3_B_3 = _UV_c54048b08a4d4c49b82124d1a78835e3_Out_0[2];
            float _Split_27c67389782f4ae3bc48a4c99e2f87f3_A_4 = _UV_c54048b08a4d4c49b82124d1a78835e3_Out_0[3];
            float _Lerp_500efe376af34fb58fe3ef778f7f5e75_Out_3;
            Unity_Lerp_float(1, 0, _Split_27c67389782f4ae3bc48a4c99e2f87f3_R_1, _Lerp_500efe376af34fb58fe3ef778f7f5e75_Out_3);
            float _Step_2c30b3a7954a46a396554f1f0568c56b_Out_2;
            Unity_Step_float(0.5, _Split_27c67389782f4ae3bc48a4c99e2f87f3_R_1, _Step_2c30b3a7954a46a396554f1f0568c56b_Out_2);
            float _Multiply_096c78988639462b997e3d6dcc2f19d0_Out_2;
            Unity_Multiply_float(_Lerp_500efe376af34fb58fe3ef778f7f5e75_Out_3, _Step_2c30b3a7954a46a396554f1f0568c56b_Out_2, _Multiply_096c78988639462b997e3d6dcc2f19d0_Out_2);
            float _Multiply_06ed808ffc0e4aafb931a657a15c2c93_Out_2;
            Unity_Multiply_float(_Subtract_5295b2d5565546d3822e2139ba56f925_Out_2, _Multiply_096c78988639462b997e3d6dcc2f19d0_Out_2, _Multiply_06ed808ffc0e4aafb931a657a15c2c93_Out_2);
            float _OneMinus_a67d5c84e2034b0a9ef07d3cd81a17a9_Out_1;
            Unity_OneMinus_float(_Step_2c30b3a7954a46a396554f1f0568c56b_Out_2, _OneMinus_a67d5c84e2034b0a9ef07d3cd81a17a9_Out_1);
            float _OneMinus_941626a56bd94db6ad5274d40df5129e_Out_1;
            Unity_OneMinus_float(_Lerp_500efe376af34fb58fe3ef778f7f5e75_Out_3, _OneMinus_941626a56bd94db6ad5274d40df5129e_Out_1);
            float _Multiply_4bf25f9218614e919944e0d440bebfc8_Out_2;
            Unity_Multiply_float(_OneMinus_a67d5c84e2034b0a9ef07d3cd81a17a9_Out_1, _OneMinus_941626a56bd94db6ad5274d40df5129e_Out_1, _Multiply_4bf25f9218614e919944e0d440bebfc8_Out_2);
            UnityTexture2D _Property_920863fb2f8d48eb8d6d24ec0e2846c9_Out_0 = UnityBuildTexture2DStructNoScale(_ScanlineTex);
            float _Multiply_db27487869c441f9876564481a0e628f_Out_2;
            Unity_Multiply_float(_Property_f7b4fdf7643c42249fef1cd0506fbff3_Out_0, -1, _Multiply_db27487869c441f9876564481a0e628f_Out_2);
            float _Multiply_7ecff73097a9445784bcb5006e64eacf_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Multiply_db27487869c441f9876564481a0e628f_Out_2, _Multiply_7ecff73097a9445784bcb5006e64eacf_Out_2);
            float2 _TilingAndOffset_6a6fb99ffacf415e95878d6d7d010524_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99075bcf3cc54f27ac6a28f5baea643a_Out_0, (_Multiply_7ecff73097a9445784bcb5006e64eacf_Out_2.xx), _TilingAndOffset_6a6fb99ffacf415e95878d6d7d010524_Out_3);
            float4 _SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_RGBA_0 = SAMPLE_TEXTURE2D(_Property_920863fb2f8d48eb8d6d24ec0e2846c9_Out_0.tex, _Property_920863fb2f8d48eb8d6d24ec0e2846c9_Out_0.samplerstate, _TilingAndOffset_6a6fb99ffacf415e95878d6d7d010524_Out_3);
            float _SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_R_4 = _SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_RGBA_0.r;
            float _SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_G_5 = _SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_RGBA_0.g;
            float _SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_B_6 = _SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_RGBA_0.b;
            float _SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_A_7 = _SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_RGBA_0.a;
            float4 _Property_86f463425fb84feb9f112889c3a0cc1d_Out_0 = _Color;
            float4 _Multiply_2a87bfd81b3945418b90f525c93fdd06_Out_2;
            Unity_Multiply_float(_SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_RGBA_0, _Property_86f463425fb84feb9f112889c3a0cc1d_Out_0, _Multiply_2a87bfd81b3945418b90f525c93fdd06_Out_2);
            float _Split_c9d545b6cd364cc6b492077dc31b90b8_R_1 = _Multiply_2a87bfd81b3945418b90f525c93fdd06_Out_2[0];
            float _Split_c9d545b6cd364cc6b492077dc31b90b8_G_2 = _Multiply_2a87bfd81b3945418b90f525c93fdd06_Out_2[1];
            float _Split_c9d545b6cd364cc6b492077dc31b90b8_B_3 = _Multiply_2a87bfd81b3945418b90f525c93fdd06_Out_2[2];
            float _Split_c9d545b6cd364cc6b492077dc31b90b8_A_4 = _Multiply_2a87bfd81b3945418b90f525c93fdd06_Out_2[3];
            float _Subtract_a51d72f8445a4a7f8a75c0bde6ed5910_Out_2;
            Unity_Subtract_float(_Split_c9d545b6cd364cc6b492077dc31b90b8_A_4, 0, _Subtract_a51d72f8445a4a7f8a75c0bde6ed5910_Out_2);
            float _Multiply_4ffd8ec344ff4e2999c1a725b5498266_Out_2;
            Unity_Multiply_float(_Multiply_4bf25f9218614e919944e0d440bebfc8_Out_2, _Subtract_a51d72f8445a4a7f8a75c0bde6ed5910_Out_2, _Multiply_4ffd8ec344ff4e2999c1a725b5498266_Out_2);
            float _Add_16b87ac96a914d4695c76ec54ca57329_Out_2;
            Unity_Add_float(_Multiply_06ed808ffc0e4aafb931a657a15c2c93_Out_2, _Multiply_4ffd8ec344ff4e2999c1a725b5498266_Out_2, _Add_16b87ac96a914d4695c76ec54ca57329_Out_2);
            float4 _Property_86ab76f83ce2430aad32841c12edbab2_Out_0 = _Color;
            float4 _Multiply_62eb165440d94d3f9ebbcc3613d6c8fe_Out_2;
            Unity_Multiply_float((_Add_16b87ac96a914d4695c76ec54ca57329_Out_2.xxxx), _Property_86ab76f83ce2430aad32841c12edbab2_Out_0, _Multiply_62eb165440d94d3f9ebbcc3613d6c8fe_Out_2);
            float4 _Add_2c81de5cffdf43fb9c8b5d817f35123b_Out_2;
            Unity_Add_float4(_SampleTexture2D_27fb62511bf94c3485449d5e3667571a_RGBA_0, _Multiply_62eb165440d94d3f9ebbcc3613d6c8fe_Out_2, _Add_2c81de5cffdf43fb9c8b5d817f35123b_Out_2);
            surface.BaseColor = (_Add_2c81de5cffdf43fb9c8b5d817f35123b_Out_2.xyz);
            surface.Alpha = _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_A_7;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





            output.uv0 =                         input.texCoord0;
            output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "Sprite Unlit"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

            // Render State
            Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITEFORWARD
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float4 texCoord0;
            float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float4 uv0;
            float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float4 interp0 : TEXCOORD0;
            float4 interp1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            output.interp1.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            output.color = input.interp1.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _ScanlineTex_TexelSize;
        float Speed;
        float4 _Color;
        float2 Till;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        float4 _MainTex_TexelSize;
        TEXTURE2D(_ScanlineTex);
        SAMPLER(sampler_ScanlineTex);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_Lerp_float(float A, float B, float T, out float Out)
        {
            Out = lerp(A, B, T);
        }

        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }

        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_57343e0bf5e848f391a9a82285d598dd_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_RGBA_0 = SAMPLE_TEXTURE2D(_Property_57343e0bf5e848f391a9a82285d598dd_Out_0.tex, _Property_57343e0bf5e848f391a9a82285d598dd_Out_0.samplerstate, IN.uv0.xy);
            float _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_R_4 = _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_RGBA_0.r;
            float _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_G_5 = _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_RGBA_0.g;
            float _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_B_6 = _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_RGBA_0.b;
            float _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_A_7 = _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_RGBA_0.a;
            UnityTexture2D _Property_16a8b777907e4d068ff0b1d7091edd5d_Out_0 = UnityBuildTexture2DStructNoScale(_ScanlineTex);
            float2 _Property_99075bcf3cc54f27ac6a28f5baea643a_Out_0 = Till;
            float _Property_f7b4fdf7643c42249fef1cd0506fbff3_Out_0 = Speed;
            float _Multiply_12c0db3c13f2429e904d8cc9207cd9d9_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Property_f7b4fdf7643c42249fef1cd0506fbff3_Out_0, _Multiply_12c0db3c13f2429e904d8cc9207cd9d9_Out_2);
            float2 _TilingAndOffset_37af45223b914dd5b8d5d808307020e4_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99075bcf3cc54f27ac6a28f5baea643a_Out_0, (_Multiply_12c0db3c13f2429e904d8cc9207cd9d9_Out_2.xx), _TilingAndOffset_37af45223b914dd5b8d5d808307020e4_Out_3);
            float4 _SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_RGBA_0 = SAMPLE_TEXTURE2D(_Property_16a8b777907e4d068ff0b1d7091edd5d_Out_0.tex, _Property_16a8b777907e4d068ff0b1d7091edd5d_Out_0.samplerstate, _TilingAndOffset_37af45223b914dd5b8d5d808307020e4_Out_3);
            float _SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_R_4 = _SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_RGBA_0.r;
            float _SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_G_5 = _SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_RGBA_0.g;
            float _SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_B_6 = _SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_RGBA_0.b;
            float _SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_A_7 = _SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_RGBA_0.a;
            float4 _Property_8342d789bf65438e897cda488517f29d_Out_0 = _Color;
            float4 _Multiply_d334279329414155852942c249cf3029_Out_2;
            Unity_Multiply_float(_SampleTexture2D_a8dfc8a708e44c4d8f4abb0e0f923666_RGBA_0, _Property_8342d789bf65438e897cda488517f29d_Out_0, _Multiply_d334279329414155852942c249cf3029_Out_2);
            float _Split_3bfb1dc84d3d44bb96be19a806e180c3_R_1 = _Multiply_d334279329414155852942c249cf3029_Out_2[0];
            float _Split_3bfb1dc84d3d44bb96be19a806e180c3_G_2 = _Multiply_d334279329414155852942c249cf3029_Out_2[1];
            float _Split_3bfb1dc84d3d44bb96be19a806e180c3_B_3 = _Multiply_d334279329414155852942c249cf3029_Out_2[2];
            float _Split_3bfb1dc84d3d44bb96be19a806e180c3_A_4 = _Multiply_d334279329414155852942c249cf3029_Out_2[3];
            float _Subtract_5295b2d5565546d3822e2139ba56f925_Out_2;
            Unity_Subtract_float(_Split_3bfb1dc84d3d44bb96be19a806e180c3_A_4, 0, _Subtract_5295b2d5565546d3822e2139ba56f925_Out_2);
            float4 _UV_c54048b08a4d4c49b82124d1a78835e3_Out_0 = IN.uv0;
            float _Split_27c67389782f4ae3bc48a4c99e2f87f3_R_1 = _UV_c54048b08a4d4c49b82124d1a78835e3_Out_0[0];
            float _Split_27c67389782f4ae3bc48a4c99e2f87f3_G_2 = _UV_c54048b08a4d4c49b82124d1a78835e3_Out_0[1];
            float _Split_27c67389782f4ae3bc48a4c99e2f87f3_B_3 = _UV_c54048b08a4d4c49b82124d1a78835e3_Out_0[2];
            float _Split_27c67389782f4ae3bc48a4c99e2f87f3_A_4 = _UV_c54048b08a4d4c49b82124d1a78835e3_Out_0[3];
            float _Lerp_500efe376af34fb58fe3ef778f7f5e75_Out_3;
            Unity_Lerp_float(1, 0, _Split_27c67389782f4ae3bc48a4c99e2f87f3_R_1, _Lerp_500efe376af34fb58fe3ef778f7f5e75_Out_3);
            float _Step_2c30b3a7954a46a396554f1f0568c56b_Out_2;
            Unity_Step_float(0.5, _Split_27c67389782f4ae3bc48a4c99e2f87f3_R_1, _Step_2c30b3a7954a46a396554f1f0568c56b_Out_2);
            float _Multiply_096c78988639462b997e3d6dcc2f19d0_Out_2;
            Unity_Multiply_float(_Lerp_500efe376af34fb58fe3ef778f7f5e75_Out_3, _Step_2c30b3a7954a46a396554f1f0568c56b_Out_2, _Multiply_096c78988639462b997e3d6dcc2f19d0_Out_2);
            float _Multiply_06ed808ffc0e4aafb931a657a15c2c93_Out_2;
            Unity_Multiply_float(_Subtract_5295b2d5565546d3822e2139ba56f925_Out_2, _Multiply_096c78988639462b997e3d6dcc2f19d0_Out_2, _Multiply_06ed808ffc0e4aafb931a657a15c2c93_Out_2);
            float _OneMinus_a67d5c84e2034b0a9ef07d3cd81a17a9_Out_1;
            Unity_OneMinus_float(_Step_2c30b3a7954a46a396554f1f0568c56b_Out_2, _OneMinus_a67d5c84e2034b0a9ef07d3cd81a17a9_Out_1);
            float _OneMinus_941626a56bd94db6ad5274d40df5129e_Out_1;
            Unity_OneMinus_float(_Lerp_500efe376af34fb58fe3ef778f7f5e75_Out_3, _OneMinus_941626a56bd94db6ad5274d40df5129e_Out_1);
            float _Multiply_4bf25f9218614e919944e0d440bebfc8_Out_2;
            Unity_Multiply_float(_OneMinus_a67d5c84e2034b0a9ef07d3cd81a17a9_Out_1, _OneMinus_941626a56bd94db6ad5274d40df5129e_Out_1, _Multiply_4bf25f9218614e919944e0d440bebfc8_Out_2);
            UnityTexture2D _Property_920863fb2f8d48eb8d6d24ec0e2846c9_Out_0 = UnityBuildTexture2DStructNoScale(_ScanlineTex);
            float _Multiply_db27487869c441f9876564481a0e628f_Out_2;
            Unity_Multiply_float(_Property_f7b4fdf7643c42249fef1cd0506fbff3_Out_0, -1, _Multiply_db27487869c441f9876564481a0e628f_Out_2);
            float _Multiply_7ecff73097a9445784bcb5006e64eacf_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Multiply_db27487869c441f9876564481a0e628f_Out_2, _Multiply_7ecff73097a9445784bcb5006e64eacf_Out_2);
            float2 _TilingAndOffset_6a6fb99ffacf415e95878d6d7d010524_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99075bcf3cc54f27ac6a28f5baea643a_Out_0, (_Multiply_7ecff73097a9445784bcb5006e64eacf_Out_2.xx), _TilingAndOffset_6a6fb99ffacf415e95878d6d7d010524_Out_3);
            float4 _SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_RGBA_0 = SAMPLE_TEXTURE2D(_Property_920863fb2f8d48eb8d6d24ec0e2846c9_Out_0.tex, _Property_920863fb2f8d48eb8d6d24ec0e2846c9_Out_0.samplerstate, _TilingAndOffset_6a6fb99ffacf415e95878d6d7d010524_Out_3);
            float _SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_R_4 = _SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_RGBA_0.r;
            float _SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_G_5 = _SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_RGBA_0.g;
            float _SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_B_6 = _SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_RGBA_0.b;
            float _SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_A_7 = _SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_RGBA_0.a;
            float4 _Property_86f463425fb84feb9f112889c3a0cc1d_Out_0 = _Color;
            float4 _Multiply_2a87bfd81b3945418b90f525c93fdd06_Out_2;
            Unity_Multiply_float(_SampleTexture2D_d7ad3d5b61b4481eb5e5396bed16affc_RGBA_0, _Property_86f463425fb84feb9f112889c3a0cc1d_Out_0, _Multiply_2a87bfd81b3945418b90f525c93fdd06_Out_2);
            float _Split_c9d545b6cd364cc6b492077dc31b90b8_R_1 = _Multiply_2a87bfd81b3945418b90f525c93fdd06_Out_2[0];
            float _Split_c9d545b6cd364cc6b492077dc31b90b8_G_2 = _Multiply_2a87bfd81b3945418b90f525c93fdd06_Out_2[1];
            float _Split_c9d545b6cd364cc6b492077dc31b90b8_B_3 = _Multiply_2a87bfd81b3945418b90f525c93fdd06_Out_2[2];
            float _Split_c9d545b6cd364cc6b492077dc31b90b8_A_4 = _Multiply_2a87bfd81b3945418b90f525c93fdd06_Out_2[3];
            float _Subtract_a51d72f8445a4a7f8a75c0bde6ed5910_Out_2;
            Unity_Subtract_float(_Split_c9d545b6cd364cc6b492077dc31b90b8_A_4, 0, _Subtract_a51d72f8445a4a7f8a75c0bde6ed5910_Out_2);
            float _Multiply_4ffd8ec344ff4e2999c1a725b5498266_Out_2;
            Unity_Multiply_float(_Multiply_4bf25f9218614e919944e0d440bebfc8_Out_2, _Subtract_a51d72f8445a4a7f8a75c0bde6ed5910_Out_2, _Multiply_4ffd8ec344ff4e2999c1a725b5498266_Out_2);
            float _Add_16b87ac96a914d4695c76ec54ca57329_Out_2;
            Unity_Add_float(_Multiply_06ed808ffc0e4aafb931a657a15c2c93_Out_2, _Multiply_4ffd8ec344ff4e2999c1a725b5498266_Out_2, _Add_16b87ac96a914d4695c76ec54ca57329_Out_2);
            float4 _Property_86ab76f83ce2430aad32841c12edbab2_Out_0 = _Color;
            float4 _Multiply_62eb165440d94d3f9ebbcc3613d6c8fe_Out_2;
            Unity_Multiply_float((_Add_16b87ac96a914d4695c76ec54ca57329_Out_2.xxxx), _Property_86ab76f83ce2430aad32841c12edbab2_Out_0, _Multiply_62eb165440d94d3f9ebbcc3613d6c8fe_Out_2);
            float4 _Add_2c81de5cffdf43fb9c8b5d817f35123b_Out_2;
            Unity_Add_float4(_SampleTexture2D_27fb62511bf94c3485449d5e3667571a_RGBA_0, _Multiply_62eb165440d94d3f9ebbcc3613d6c8fe_Out_2, _Add_2c81de5cffdf43fb9c8b5d817f35123b_Out_2);
            surface.BaseColor = (_Add_2c81de5cffdf43fb9c8b5d817f35123b_Out_2.xyz);
            surface.Alpha = _SampleTexture2D_27fb62511bf94c3485449d5e3667571a_A_7;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





            output.uv0 =                         input.texCoord0;
            output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"

            ENDHLSL
        }
    }
    FallBack "Hidden/Shader Graph/FallbackError"
}