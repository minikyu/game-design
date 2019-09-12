Shader "Unlit/VFShader"
{
    Properties

    {
		_MainTex ("Texture", 2D) = "white" {}
		_MainColor("Main Color", Color) = (1,1,1,1)
		_Shininess("Shininess",Range(10,200)) = 10
		_Specular("Specular Color", Color) = (1,1,1,1)//光源颜色
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
			#pragma shader_feature USE_SPECULAR
			#pragma shader_feature TEX_AND_LIGHT

            #include "UnityCG.cginc"
			#include  "UnityStandardBRDF.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;

            };

            struct v2f
            {			
                float4 vertex : SV_POSITION;
				float3 normal : TEXCOORD1;
				float2 uv : TEXCOORD0;
				float3 worldPos : TEXCOORD2;

            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _MainColor;
			half _Shininess;
			float4 _Specular;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				//漫反射
				float3 lightDir = _WorldSpaceLightPos0.xyz;
				float3 lightColor = _LightColor0.rgb;
				float3 diffuse = tex2D(_MainTex, i.uv).rgb * lightColor * DotClamped(lightDir, i.normal);
				//Blinn Phong
				float3 normalDir = normalize(i.normal);
				float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
				float3 halfVector = normalize(lightDir + viewDir);
				float3 specular = float3(0, 0, 0);
				
				#if USE_SPECULAR
					specular = _LightColor0.rgb * _Specular.rgb * pow(max(dot(normalDir, halfVector), 0),_Shininess);
				#endif

				//环境光
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * tex2D(_MainTex, i.uv).rgb;

				
				#if TEX_AND_LIGHT
				return float4(diffuse+ambient+specular, 1);
				#endif

				
				return float4(i.normal, 1);

            }
            ENDCG
        }
    }
			
	CustomEditor "CustomShaderGUI"

}
