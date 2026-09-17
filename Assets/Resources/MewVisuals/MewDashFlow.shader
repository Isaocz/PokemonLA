Shader "Mew/DashInteriorFlow"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "CanUseSpriteAtlas"="True" }
        Cull Off Lighting Off ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; fixed4 color:COLOR; };
            struct v2f { float4 vertex:SV_POSITION; float2 uv:TEXCOORD0; fixed4 color:COLOR; float worldY:TEXCOORD1; };
            sampler2D _MainTex;
            fixed4 _Color;
            v2f vert(appdata v)
            {
                v2f o; o.vertex=UnityObjectToClipPos(v.vertex); o.uv=v.uv; o.color=v.color*_Color;
                o.worldY=v.vertex.y; return o;
            }
            fixed4 frag(v2f i):SV_Target
            {
                fixed4 sprite=tex2D(_MainTex,i.uv);
                float wave=pow(saturate(0.5+0.5*sin(i.uv.y*18-_Time.y*7)),8);
                fixed3 tint=lerp(i.color.rgb, fixed3(0.7,0.94,1),wave*0.8);
                return fixed4(sprite.rgb*tint, sprite.a*i.color.a*(0.65+wave*0.65));
            }
            ENDCG
        }
    }
}