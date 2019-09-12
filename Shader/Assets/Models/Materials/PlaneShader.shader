// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/PlaneShader"
{
	Properties
	{
		_MainTint("Diffuse Tint", Color) = (1,1,1,1)
		_ColorA("Terrain Color A", Color) = (1,1,1,1)
		_ColorB("Terrain Color B", Color) = (1,1,1,1)
		_RTexture("Red Channel Texture", 2D) = "white"{}
		_GTexture("Green Channel Texture", 2D) = "white"{}
		_BTexture("Blue Channel Texture", 2D) = "white"{}
		_ATexture("Alpha Channel Texture", 2D) = "white"{}
		_BlendTex("Blend Texture", 2D) = "white"{}
	}

		SubShader
	{
		LOD 200

		Tags
		{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		}

		Pass
		{

			Lighting Off
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Fog{ Mode Off }
			Offset -1, -1

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.0

			#include "UnityCG.cginc"
			float4 _MainTint;
			float4 _ColorA;
			float4 _ColorB;
			sampler2D _RTexture;
			sampler2D _GTexture;
			sampler2D _BTexture;
			sampler2D _ATexture;
			sampler2D _BlendTex;


			struct appdata
			{
				float4 vertex    : POSITION;
				float2 texcoord0 : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float2 texcoord2 : TEXCOORD2;
				float2 texcoord3 : TEXCOORD3;
				float2 texcoord4 : TEXCOORD4;
			};

			struct v2f
			{
				float4 vertex    : POSITION;
				float2 uv_RTexture : TEXCOORD0;
				float2 uv_GTexture : TEXCOORD1;
				float2 uv_BTexture : TEXCOORD2;
				float2 uv_ATexture : TEXCOORD3;
				float2 uv_BlendTex : TEXCOORD4;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv_RTexture = v.texcoord0;
				o.uv_GTexture = v.texcoord1;
				o.uv_BTexture = v.texcoord2;
				o.uv_ATexture = v.texcoord3;
				o.uv_BlendTex = v.texcoord4;
				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
				

				//Get the data from the textures we want to blend  
				float4 rTexData = tex2D(_RTexture, i.uv_RTexture);
				float4 gTexData = tex2D(_GTexture, i.uv_GTexture);
				float4 bTexData = tex2D(_BTexture, i.uv_BTexture);
				float4 aTexData = tex2D(_ATexture, i.uv_ATexture);
				float4 blendData = tex2D(_BlendTex, i.uv_BlendTex);

				float4 finalColor =(0,0,0,1);
				finalColor = lerp(rTexData, gTexData, blendData.g);
				finalColor = lerp(finalColor, bTexData, blendData.b);
				finalColor = lerp(finalColor, aTexData, blendData.a);

				float4 terrainLayers = lerp(_ColorA, _ColorB, blendData.r);
				finalColor *= terrainLayers;
				finalColor = saturate(finalColor);

				return float4(finalColor.rgb * _MainTint.rgb, finalColor.a);
			}
			ENDCG
		}
	}
}