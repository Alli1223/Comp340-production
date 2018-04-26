Shader "Custom/CellShaderPalletSwap-Deffered"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_OverlayTex ("Texture", 2D) = "white" {}
		_Color1 ("Color", Color) = (1,0,0,1)
		_Color2 ("Color", Color) = (0,1,0,1)
		_Color3 ("Color", Color) = (0,0,1,1)
		_Smooth ("Smoothing", Range(0,1)) = 0.55
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM 
		#pragma surface surf Standard
		#pragma target 3.0
		
		// Old forward method
//		half4 LightingCelShadingForward(SurfaceOutput s, half3 lightDir, half atten)
//		{
//			half NdotL = dot(s.Normal, lightDir);
//			//if(NdotL <= 0.0) NdotL = 0;
//			//else NdotL = 1.0;
//
//			//NdotL = 1 + clamp(floor(NdotL), -1, 0);
//			NdotL = smoothstep(0, _Smooth, NdotL);
//
//			half4 c;
//
//			c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 2);
//			c.a = s.Alpha;
//
//			return c;
//		}

		sampler2D _MainTex;
		sampler2D _OverlayTex;
		fixed4 _Color1;
		fixed4 _Color2;
		fixed4 _Color3;
		half _Smooth;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);


			o.Albedo = c;
			//o. = _Smooth;
		}

		ENDCG
	}
}
