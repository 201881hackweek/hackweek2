﻿Shader "Unlit/mask"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Mask ("Culling Mask",2D)= "white" {}
		_Cutoff("Alpha cutoff",Range(0,1)) = 0.1
	}
		SubShader
		{
			Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

			Pass
			{

			Lighting Off
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha


			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			
	
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _Mask;
			fixed _Cutoff;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 texColor1 = tex2D(_MainTex, i.uv);
				fixed4 texColor2 = tex2D(_Mask, i.uv);
		
				return fixed4(texColor1.xyz,texColor1.a-texColor2.a*_Cutoff);
			}
			ENDCG
		}
	}
}
