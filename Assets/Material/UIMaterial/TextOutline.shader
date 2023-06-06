Shader "Custom/TextOutline" {
	Properties{
		_MainTex("Font Texture", 2D) = "white" {}
		_Color("Text Color", Color) = (1,1,1,1)
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_StencilComp("Stencil Comparison", Float) = 3.000000
		 _Stencil("Stencil ID", Float) = 1.000000
		 _StencilOp("Stencil Operation", Float) = 0.000000
		 _StencilWriteMask("Stencil Write Mask", Float) = 0.000000
		 _StencilReadMask("Stencil Read Mask", Float) = 1.000000
	}

		SubShader{

			Tags {
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
			}

			Lighting Off Cull Off ZTest Always ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			Stencil {
				Ref[_Stencil]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
				Comp[_StencilComp]
				Pass[_StencilOp]
			}
			//ColorMask[_ColorMask]

			//��һ��Pass��ʵ��Text���ݱ�����ɫ������������_OutlineWidth
			Pass {
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_INSTANCING_ON STEREO_MULTIVIEW_ON
				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					//UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					UNITY_VERTEX_OUTPUT_STEREO
				};

				sampler2D _MainTex;
				uniform float4 _MainTex_ST;
				uniform float4 _MainTex_TexelSize;
				uniform fixed4 _Color;
				uniform fixed4 _OutlineColor;


				v2f vert(appdata_t v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.color = v.color * _Color;
					o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
					return o;
				}
				//ȷ��ÿ��������Χ8�����ص����ꡣ
				static const float2 dirList[9] = {
					float2(-1,-1),float2(0,-1),float2(1,-1),
					float2(-1,0),float2(0,0),float2(1,0),
					float2(-1,1),float2(0,1),float2(1,1)
				};
				//ıȡdirList��dirIndex����λ��͸����ֵ��
				float getDirPosAlpha(float index, float2 xy) {
					float2 curPos = xy;
					float2 dir = dirList[index];
					float2 dirPos = curPos + dir * _MainTex_TexelSize.xy * 0.6;
					return tex2D(_MainTex, dirPos).a;
				};
				//����ÿ�����أ�����ƬԪ����v2f i ����ȡ��������Χ������Ĺ�9�����ؽ���͸���ȵ��ӡ�
				//��ô�ó��Ľ�����Ƿ�͸�������򱻷Ŵ��ˣ��γ��˺ڱߡ�
				float getShadowAlpha(float2 xy) {
					float a = 0;
					float index = 0;
					a += getDirPosAlpha(index, xy);
					a += getDirPosAlpha(index++, xy);
					a += getDirPosAlpha(index++, xy);
					a += getDirPosAlpha(index++, xy);
					a += getDirPosAlpha(index++, xy);
					a += getDirPosAlpha(index++, xy);
					a += getDirPosAlpha(index++, xy);
					a += getDirPosAlpha(index++, xy);
					a += getDirPosAlpha(index++, xy);
					a = clamp(a,0,1);
					return a;
				}


				//������ȾText����ʱ��Text����û�б���Ⱦ��������͸���ģ�Ҳ����͸����aֵ��0��
				//����ֻҪ�������ݵ���������͸����Ϊ0��������չһЩ���ؽ����ܹ��γ����Ч����
				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = _OutlineColor;
					float2 xy = i.texcoord.xy;
					col.a *= getShadowAlpha(xy);
					return col;
				}
				ENDCG
			}
			//�ڶ���Pass��������ȾText���ݡ�

			Pass {
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_INSTANCING_ON STEREO_MULTIVIEW_ON
				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					//UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					UNITY_VERTEX_OUTPUT_STEREO
				};

				sampler2D _MainTex;
				uniform float4 _MainTex_ST;
				uniform float4 _MainTex_TexelSize;
				uniform fixed4 _Color;

				v2f vert(appdata_t v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.color = v.color * _Color;
					step(v.texcoord, v.vertex.xy);
					o.texcoord = TRANSFORM_TEX(v.texcoord.xy,_MainTex);
					return o;
				}
				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = i.color;
					col.a = tex2D(_MainTex, i.texcoord).a;
					return col;
				}
				ENDCG
			}
		}
}
