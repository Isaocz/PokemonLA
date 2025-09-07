Shader "Custom/MaskTerastellize"
{
    Properties
    {
        _Color("Color", Color) = (1, 0.1334855, 0, 0)
        NoiceScale("NoiceScale", Float) = 8
        _MaterialPriority("Material Priority", Float) = 0
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
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS
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
            float3 positionWS;
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
            float3 ObjectSpacePosition;
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
            float3 interp0 : TEXCOORD0;
            float4 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
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
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyzw =  input.texCoord0;
            output.interp2.xyzw =  input.color;
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
            output.positionWS = input.interp0.xyz;
            output.texCoord0 = input.interp1.xyzw;
            output.color = input.interp2.xyzw;
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
        float4 _Color;
        float NoiceScale;
        float _MaterialPriority;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        float4 _MainTex_TexelSize;

            // Graph Functions
            
        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }


        inline float2 Unity_Voronoi_RandomVector_float (float2 UV, float offset)
        {
            float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
            UV = frac(sin(mul(UV, m)));
            return float2(sin(UV.y*+offset)*0.5+0.5, cos(UV.x*offset)*0.5+0.5);
        }

        void Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
        {
            float2 g = floor(UV * CellDensity);
            float2 f = frac(UV * CellDensity);
            float t = 8.0;
            float3 res = float3(8.0, 0.0, 0.0);

            for(int y=-1; y<=1; y++)
            {
                for(int x=-1; x<=1; x++)
                {
                    float2 lattice = float2(x,y);
                    float2 offset = Unity_Voronoi_RandomVector_float(lattice + g, AngleOffset);
                    float d = distance(lattice + offset, f);

                    if(d < res.x)
                    {
                        res = float3(d, offset.x, offset.y);
                        Out = res.x;
                        Cells = res.y;
                    }
                }
            }
        }

        void Unity_Minimum_float4(float4 A, float4 B, out float4 Out)
        {
            Out = min(A, B);
        };

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
            UnityTexture2D _Property_7e0568682b694b94b3c7b46652f504c9_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7e0568682b694b94b3c7b46652f504c9_Out_0.tex, _Property_7e0568682b694b94b3c7b46652f504c9_Out_0.samplerstate, IN.uv0.xy);
            float _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_R_4 = _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_RGBA_0.r;
            float _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_G_5 = _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_RGBA_0.g;
            float _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_B_6 = _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_RGBA_0.b;
            float _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_A_7 = _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_RGBA_0.a;
            float4 _Multiply_d1a2a4538d674527b14e73a84d5a1750_Out_2;
            Unity_Multiply_float(_SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_RGBA_0, (_SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_A_7.xxxx), _Multiply_d1a2a4538d674527b14e73a84d5a1750_Out_2);
            float _Split_462243507eef4e78af3e066d4324a4dc_R_1 = _Multiply_d1a2a4538d674527b14e73a84d5a1750_Out_2[0];
            float _Split_462243507eef4e78af3e066d4324a4dc_G_2 = _Multiply_d1a2a4538d674527b14e73a84d5a1750_Out_2[1];
            float _Split_462243507eef4e78af3e066d4324a4dc_B_3 = _Multiply_d1a2a4538d674527b14e73a84d5a1750_Out_2[2];
            float _Split_462243507eef4e78af3e066d4324a4dc_A_4 = _Multiply_d1a2a4538d674527b14e73a84d5a1750_Out_2[3];
            float4 _Combine_c3aa83b360924c8c98a6e50c640712e6_RGBA_4;
            float3 _Combine_c3aa83b360924c8c98a6e50c640712e6_RGB_5;
            float2 _Combine_c3aa83b360924c8c98a6e50c640712e6_RG_6;
            Unity_Combine_float(_Split_462243507eef4e78af3e066d4324a4dc_R_1, _Split_462243507eef4e78af3e066d4324a4dc_G_2, _Split_462243507eef4e78af3e066d4324a4dc_B_3, _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_A_7, _Combine_c3aa83b360924c8c98a6e50c640712e6_RGBA_4, _Combine_c3aa83b360924c8c98a6e50c640712e6_RGB_5, _Combine_c3aa83b360924c8c98a6e50c640712e6_RG_6);
            float4 _Multiply_c35645d407d04aac9f45b24e3fbf05a6_Out_2;
            Unity_Multiply_float(_Combine_c3aa83b360924c8c98a6e50c640712e6_RGBA_4, float4(0.8, 0.8, 0.8, 1), _Multiply_c35645d407d04aac9f45b24e3fbf05a6_Out_2);
            float4 _Multiply_e6d45a2c8d98449fb4c3ed805dc4bf75_Out_2;
            Unity_Multiply_float(_Combine_c3aa83b360924c8c98a6e50c640712e6_RGBA_4, float4(2, 2, 2, 1), _Multiply_e6d45a2c8d98449fb4c3ed805dc4bf75_Out_2);
            float2 _TilingAndOffset_4b3f48d444ae4d9196bb7e40a155af07_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (IN.TimeParameters.x.xx), _TilingAndOffset_4b3f48d444ae4d9196bb7e40a155af07_Out_3);
            float _Property_e776e71a32a641b191bc3ee140bb9db4_Out_0 = NoiceScale;
            float _Voronoi_2557a40482d74d2699105fc2b7355383_Out_3;
            float _Voronoi_2557a40482d74d2699105fc2b7355383_Cells_4;
            Unity_Voronoi_float((IN.ObjectSpacePosition.xy), (_TilingAndOffset_4b3f48d444ae4d9196bb7e40a155af07_Out_3).x, _Property_e776e71a32a641b191bc3ee140bb9db4_Out_0, _Voronoi_2557a40482d74d2699105fc2b7355383_Out_3, _Voronoi_2557a40482d74d2699105fc2b7355383_Cells_4);
            float4 _Property_cacc09d737454a5bb3932cb288a7453a_Out_0 = _Color;
            float4 _Multiply_bd6ef9eb7ed943059c5ce0759c62bafb_Out_2;
            Unity_Multiply_float((_Voronoi_2557a40482d74d2699105fc2b7355383_Cells_4.xxxx), _Property_cacc09d737454a5bb3932cb288a7453a_Out_0, _Multiply_bd6ef9eb7ed943059c5ce0759c62bafb_Out_2);
            float4 _Multiply_a6361d93b5d140d5b6f4b62294c65706_Out_2;
            Unity_Multiply_float(_Multiply_e6d45a2c8d98449fb4c3ed805dc4bf75_Out_2, _Multiply_bd6ef9eb7ed943059c5ce0759c62bafb_Out_2, _Multiply_a6361d93b5d140d5b6f4b62294c65706_Out_2);
            float4 _Minimum_8c1c1737e2274c4e8464b800b861762b_Out_2;
            Unity_Minimum_float4(_Multiply_a6361d93b5d140d5b6f4b62294c65706_Out_2, float4(2, 2, 2, 1), _Minimum_8c1c1737e2274c4e8464b800b861762b_Out_2);
            float4 _Add_15313ae3ed44402bb6518c2c0f8cb3f8_Out_2;
            Unity_Add_float4(_Multiply_c35645d407d04aac9f45b24e3fbf05a6_Out_2, _Minimum_8c1c1737e2274c4e8464b800b861762b_Out_2, _Add_15313ae3ed44402bb6518c2c0f8cb3f8_Out_2);
            surface.BaseColor = (_Add_15313ae3ed44402bb6518c2c0f8cb3f8_Out_2.xyz);
            surface.Alpha = _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_A_7;
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





            output.ObjectSpacePosition =         TransformWorldToObject(input.positionWS);
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
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS
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
            float3 positionWS;
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
            float3 ObjectSpacePosition;
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
            float3 interp0 : TEXCOORD0;
            float4 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
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
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyzw =  input.texCoord0;
            output.interp2.xyzw =  input.color;
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
            output.positionWS = input.interp0.xyz;
            output.texCoord0 = input.interp1.xyzw;
            output.color = input.interp2.xyzw;
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
        float4 _Color;
        float NoiceScale;
        float _MaterialPriority;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        float4 _MainTex_TexelSize;

            // Graph Functions
            
        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }


        inline float2 Unity_Voronoi_RandomVector_float (float2 UV, float offset)
        {
            float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
            UV = frac(sin(mul(UV, m)));
            return float2(sin(UV.y*+offset)*0.5+0.5, cos(UV.x*offset)*0.5+0.5);
        }

        void Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
        {
            float2 g = floor(UV * CellDensity);
            float2 f = frac(UV * CellDensity);
            float t = 8.0;
            float3 res = float3(8.0, 0.0, 0.0);

            for(int y=-1; y<=1; y++)
            {
                for(int x=-1; x<=1; x++)
                {
                    float2 lattice = float2(x,y);
                    float2 offset = Unity_Voronoi_RandomVector_float(lattice + g, AngleOffset);
                    float d = distance(lattice + offset, f);

                    if(d < res.x)
                    {
                        res = float3(d, offset.x, offset.y);
                        Out = res.x;
                        Cells = res.y;
                    }
                }
            }
        }

        void Unity_Minimum_float4(float4 A, float4 B, out float4 Out)
        {
            Out = min(A, B);
        };

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
            UnityTexture2D _Property_7e0568682b694b94b3c7b46652f504c9_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7e0568682b694b94b3c7b46652f504c9_Out_0.tex, _Property_7e0568682b694b94b3c7b46652f504c9_Out_0.samplerstate, IN.uv0.xy);
            float _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_R_4 = _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_RGBA_0.r;
            float _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_G_5 = _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_RGBA_0.g;
            float _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_B_6 = _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_RGBA_0.b;
            float _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_A_7 = _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_RGBA_0.a;
            float4 _Multiply_d1a2a4538d674527b14e73a84d5a1750_Out_2;
            Unity_Multiply_float(_SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_RGBA_0, (_SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_A_7.xxxx), _Multiply_d1a2a4538d674527b14e73a84d5a1750_Out_2);
            float _Split_462243507eef4e78af3e066d4324a4dc_R_1 = _Multiply_d1a2a4538d674527b14e73a84d5a1750_Out_2[0];
            float _Split_462243507eef4e78af3e066d4324a4dc_G_2 = _Multiply_d1a2a4538d674527b14e73a84d5a1750_Out_2[1];
            float _Split_462243507eef4e78af3e066d4324a4dc_B_3 = _Multiply_d1a2a4538d674527b14e73a84d5a1750_Out_2[2];
            float _Split_462243507eef4e78af3e066d4324a4dc_A_4 = _Multiply_d1a2a4538d674527b14e73a84d5a1750_Out_2[3];
            float4 _Combine_c3aa83b360924c8c98a6e50c640712e6_RGBA_4;
            float3 _Combine_c3aa83b360924c8c98a6e50c640712e6_RGB_5;
            float2 _Combine_c3aa83b360924c8c98a6e50c640712e6_RG_6;
            Unity_Combine_float(_Split_462243507eef4e78af3e066d4324a4dc_R_1, _Split_462243507eef4e78af3e066d4324a4dc_G_2, _Split_462243507eef4e78af3e066d4324a4dc_B_3, _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_A_7, _Combine_c3aa83b360924c8c98a6e50c640712e6_RGBA_4, _Combine_c3aa83b360924c8c98a6e50c640712e6_RGB_5, _Combine_c3aa83b360924c8c98a6e50c640712e6_RG_6);
            float4 _Multiply_c35645d407d04aac9f45b24e3fbf05a6_Out_2;
            Unity_Multiply_float(_Combine_c3aa83b360924c8c98a6e50c640712e6_RGBA_4, float4(0.8, 0.8, 0.8, 1), _Multiply_c35645d407d04aac9f45b24e3fbf05a6_Out_2);
            float4 _Multiply_e6d45a2c8d98449fb4c3ed805dc4bf75_Out_2;
            Unity_Multiply_float(_Combine_c3aa83b360924c8c98a6e50c640712e6_RGBA_4, float4(2, 2, 2, 1), _Multiply_e6d45a2c8d98449fb4c3ed805dc4bf75_Out_2);
            float2 _TilingAndOffset_4b3f48d444ae4d9196bb7e40a155af07_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (IN.TimeParameters.x.xx), _TilingAndOffset_4b3f48d444ae4d9196bb7e40a155af07_Out_3);
            float _Property_e776e71a32a641b191bc3ee140bb9db4_Out_0 = NoiceScale;
            float _Voronoi_2557a40482d74d2699105fc2b7355383_Out_3;
            float _Voronoi_2557a40482d74d2699105fc2b7355383_Cells_4;
            Unity_Voronoi_float((IN.ObjectSpacePosition.xy), (_TilingAndOffset_4b3f48d444ae4d9196bb7e40a155af07_Out_3).x, _Property_e776e71a32a641b191bc3ee140bb9db4_Out_0, _Voronoi_2557a40482d74d2699105fc2b7355383_Out_3, _Voronoi_2557a40482d74d2699105fc2b7355383_Cells_4);
            float4 _Property_cacc09d737454a5bb3932cb288a7453a_Out_0 = _Color;
            float4 _Multiply_bd6ef9eb7ed943059c5ce0759c62bafb_Out_2;
            Unity_Multiply_float((_Voronoi_2557a40482d74d2699105fc2b7355383_Cells_4.xxxx), _Property_cacc09d737454a5bb3932cb288a7453a_Out_0, _Multiply_bd6ef9eb7ed943059c5ce0759c62bafb_Out_2);
            float4 _Multiply_a6361d93b5d140d5b6f4b62294c65706_Out_2;
            Unity_Multiply_float(_Multiply_e6d45a2c8d98449fb4c3ed805dc4bf75_Out_2, _Multiply_bd6ef9eb7ed943059c5ce0759c62bafb_Out_2, _Multiply_a6361d93b5d140d5b6f4b62294c65706_Out_2);
            float4 _Minimum_8c1c1737e2274c4e8464b800b861762b_Out_2;
            Unity_Minimum_float4(_Multiply_a6361d93b5d140d5b6f4b62294c65706_Out_2, float4(2, 2, 2, 1), _Minimum_8c1c1737e2274c4e8464b800b861762b_Out_2);
            float4 _Add_15313ae3ed44402bb6518c2c0f8cb3f8_Out_2;
            Unity_Add_float4(_Multiply_c35645d407d04aac9f45b24e3fbf05a6_Out_2, _Minimum_8c1c1737e2274c4e8464b800b861762b_Out_2, _Add_15313ae3ed44402bb6518c2c0f8cb3f8_Out_2);
            surface.BaseColor = (_Add_15313ae3ed44402bb6518c2c0f8cb3f8_Out_2.xyz);
            surface.Alpha = _SampleTexture2D_31f8580f52c24c3db1424e5cf3d92437_A_7;
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





            output.ObjectSpacePosition =         TransformWorldToObject(input.positionWS);
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
