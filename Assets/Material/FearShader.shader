Shader "FearCustom/MyShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        // ����͸����
        Blend SrcAlpha OneMinusSrcAlpha
        // ������Ⱦ����
        Tags { "Queue"="Transparent" "RenderType"="Opaque" }

        // ---------------------------��ͨ��һ��---------------------------
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            // ������ɫ��ȡ
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uv.y = o.uv.y;
                return o;
            }

            //ͨ��c#��ȡ�����������ݹ���
            sampler2D _PlayerTex;

            // ƬԪ��ɫ��ȡ
            fixed4 frag (v2f i) : SV_Target
            {
                // ����������������
                fixed4 col = tex2D(_PlayerTex, i.uv);
                // ������step����if
                // �� ͸����ֵ����1ʱ, �ͳ��ֺ�ɫ(��Ӱ��)
                col.r -= 0.120;
                col.g -= 0.220;
                col.b -= 0.120;
                //col.a = 1 - step(0,col.a);
                return col;
            }
            ENDCG
        }
    }
}
