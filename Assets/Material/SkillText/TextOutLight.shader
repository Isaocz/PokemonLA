Shader "UI/TextOutLight"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _LightColor ("Light Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _Size ("Size", Int) = 1 
	_Strength ("Strength", Float) = 1

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        CGINCLUDE
        #include "UnityCG.cginc"
	struct a2v
	{
	    float4 vertex : POSITION;
	    float4 color : COLOR;
	    float2 texcoord : TEXCOORD0;
	    float2 uv2 : TEXCOORD2;
	    float2 uv3 : TEXCOORD3;
	    float4 tangent : TANGENT;
	};

	struct v2f {
	    float4 vertex : SV_POSITION;
	    float2 texcoord: TEXCOORD0;
	    fixed4 color : COLOR;
	    float4 worldPosition : TEXCOORD1;
	    half4 clipRect : TEXCOORD2;
	};

	float4 _LightColor;
	float _Size;
	float _Strength;

	sampler2D _MainTex;
	half4 _MainTex_TexelSize;
	fixed4 _TextureSampleAdd;
	float4 _ClipRect;
	ENDCG

        Pass {
	    Tags{ "LightMode" = "LightweightForward" }

            CGPROGRAM
	    #pragma multi_compile_instancing
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
	    #include "UnityUI.cginc"

            v2f vert(a2v v)
            {
                v2f OUT;
		OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = v.texcoord;
		OUT.clipRect = half4(v.uv2, v.uv3);
                OUT.color = v.color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = _LightColor;
                float sum = tex2D(_MainTex, IN.texcoord).a;
		int count = 1;
                for (int i = -_Size ; i <= _Size; i ++) {
                    for (int j = -_Size; j <= _Size; j ++) {
			if (abs(i) != abs(j))
                        {
			    float2 tex = IN.texcoord + _MainTex_TexelSize.xy * half2(i, j);
                            sum += tex2D(_MainTex, tex).a * UnityGet2DClipping(tex, IN.clipRect);	//裁切其他字延伸出来的描边
			    count++;
                        }       
                    }
                }
		c.a = sum / count * _Strength;
		c.a = min(1, c.a);
		return c;
            }

            ENDCG
        }

	Pass {
	    Tags{ "LightMode" = "SRPDefaultUnlit" }

            CGPROGRAM
	    #pragma multi_compile_instancing
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
	    #include "UnityUI.cginc"

            v2f vert(a2v v)
            {
                v2f OUT;
		OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = v.texcoord;
		OUT.clipRect = half4(v.uv2, v.uv3);
                OUT.color = v.color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
	        fixed4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
		color.a *= UnityGet2DClipping(IN.texcoord, IN.clipRect);
		return color;
            }

            ENDCG
        }
    } 
}