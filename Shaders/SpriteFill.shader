// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SpriteFill"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
    }
        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }
            Cull Off
            Lighting Off
            ZWrite On
            ZTest Off
            Fog { Mode Off }
            Blend One OneMinusSrcAlpha

            Pass
            {
                // Используется в дальнейшем
                Stencil
                {
                    WriteMask 7
                    Ref 6
                    Pass Replace
                }

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float4 color : COLOR;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float4 color : COLOR;
                };

                sampler2D _MainTex;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    #ifdef PIXELSNAP_ON
                    v.vertex = UnityPixelSnap(v.vertex);
                    #endif
                    o.uv = v.uv;
                    o.color = v.color;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target0
                {
                    fixed4 color = tex2D(_MainTex, i.uv) * i.color;

                    if (color.a == 0)
                        discard;

                    return i.color * color.a;
                }
                ENDCG
            }
        }
}