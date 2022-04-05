// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/LightShader"
{
Properties
{
_MainTex ("Base (RGB)", 2D) = "white" {}
_OutlineColor ("Outline Color", Color) = (1,1,1,1)
_OutLineSpread ("Outline Spread", Range(0,0.03)) = 0.007
_AlphaReplace ("AlphaReplace", Range(0.01,1)) = 0.5
_Amplitude ("Extrusion Amplitude", float) = 1
_Color ("Tint", Color) = (1,1,1,1)
[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
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
Fog { Mode Off }
Blend SrcAlpha OneMinusSrcAlpha

Pass
{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile DUMMY PIXELSNAP_ON
#include "UnityCG.cginc"

struct appdata_t
{
float4 vertex : POSITION;
float4 color : COLOR;
float2 texcoord : TEXCOORD0;
};

struct v2f
{
float4 vertex : SV_POSITION;
fixed4 color : COLOR;
half2 texcoord : TEXCOORD0;
};

fixed4 _Color;

v2f vert(appdata_t IN)
{
v2f OUT;
OUT.vertex = UnityObjectToClipPos(IN.vertex);
OUT.texcoord = IN.texcoord;
OUT.color = IN.color * _Color;
#ifdef PIXELSNAP_ON
OUT.vertex = UnityPixelSnap (OUT.vertex);

#endif

return OUT;
}

sampler2D _MainTex;
float _OutLineSpread;
float _AlphaReplace;
fixed4 _OutlineColor;

struct Input
{
float2 uv_MainTex;
};


fixed4 frag(v2f IN) : COLOR
{
fixed4 mainColor = (tex2D(_MainTex, IN.texcoord+float2(_OutLineSpread,_OutLineSpread)) + tex2D(_MainTex, IN.texcoord-float2(_OutLineSpread,_OutLineSpread)));
mainColor += (tex2D(_MainTex, IN.texcoord+float2(-_OutLineSpread,_OutLineSpread)) + tex2D(_MainTex, IN.texcoord-float2(-_OutLineSpread,_OutLineSpread)));

if(mainColor.a>0.01)
{
    mainColor.r=_OutlineColor.r;
    mainColor.g=_OutlineColor.g;
    mainColor.b=_OutlineColor.b;
    mainColor.a = _OutlineColor.a;
}

mainColor *= _OutlineColor;

fixed4 addcolor = tex2D(_MainTex, IN.texcoord) * IN.color;

if(addcolor.a >_AlphaReplace)
{
   mainColor = addcolor;
}
if(addcolor.a >0 &&addcolor.a <0.5 )
{
    addcolor = fixed4(1,1,1,1);
    mainColor *= addcolor;
}
return mainColor;
}


            
ENDCG

}
}

Fallback Off
}
 