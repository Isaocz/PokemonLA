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
        // 开启透明度
        Blend SrcAlpha OneMinusSrcAlpha
        // 设置渲染队列
        Tags { "Queue"="Transparent" "RenderType"="Opaque" }

        // ---------------------------【通道一】---------------------------
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
            // 顶点着色获取
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uv.y = o.uv.y;
                return o;
            }

            //通过c#获取到人物纹理传递过来
            sampler2D _PlayerTex;

            // 片元着色获取
            fixed4 frag (v2f i) : SV_Target
            {
                // 采样传过来的纹理
                fixed4 col = tex2D(_PlayerTex, i.uv);
                // 这里用step代替if
                // 当 透明度值大于1时, 就呈现黑色(即影子)
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
