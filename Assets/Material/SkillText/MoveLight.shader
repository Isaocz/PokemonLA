Shader "Custom/MoveLight"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _LightTex("LightTex",2D) = "white" {}
        _LightColor("LightColor",Color) = (1,1,1,1)
        _LightStrength("LightStrength",Range(1,10)) = 1
        _Speed("Speed",Range(0,10)) = 1
        _RotateAngel("RotateAngel",Range(0,360)) = 0
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
               
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp] 
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]

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
                float2 lightUV : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            sampler2D _LightTex;
            float4 _LightTex_ST;

            float _Speed;
            float4 _LightColor;
            float _LightStrength;
            float _RotateAngel;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.lightUV = TRANSFORM_TEX(v.uv, _LightTex);
                // o.lightUV = v.uv;
                
                return o;
            }



            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                float angel = _RotateAngel * 3.14159265359 / 180;//�Ƕ�ת���ȣ���Ϊ�����Ҳ��û���

                //��תǰ���ı����ġ������Ļ����������½�(0,0)Ϊ����,��ȥ����(0.5,0.5)Ϊ���ģ����������Ҫ����תЧ��
                float2 lightUV = i.lightUV - float2(0.5,0.5);

                //���Ӧ����ת��������UV������ת
                lightUV= float2(lightUV.x * cos(angel) - lightUV.y * sin(angel),lightUV.y * cos(angel) + lightUV.x * sin(angel));
                
                //�ӻ�ƫ��
                lightUV = lightUV + float2(0.5,0.5);

                //����ʱ�䣬��ɨ�⶯����
                lightUV.x = lightUV.x + frac(_Speed * _Time.y);//_Time��4������ float4 Time (t/20, t, t*2, t*3), 
                
                fixed4 lightCol = tex2D(_LightTex, lightUV);
                
                float4 result = col;
                float alpha = step(1,lightCol.a);
                result = lerp(col,col + lightCol * _LightColor * _LightStrength,alpha);
                result.a = col.a;
                return result;

            }
            ENDCG
        }
    }
}