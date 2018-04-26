Shader "Custom/CellShaderPalletSwap"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_OverlayTex ("Color Mask", 2D) = "red" {}
		_Color1 ("Main", Color) = (1,0,0,1)
		_Color2 ("Secondary", Color) = (0,1,0,1)
		_Color3 ("Trim", Color) = (0,0,1,1)
		_Smooth ("Smoothing", Range(0,1)) = 0.55
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf CelShadingForward
		#pragma target 3.0

		half _Smooth;
		half4 LightingCelShadingForward(SurfaceOutput s, half3 lightDir, half atten)
		{
			half NdotL = dot(s.Normal, lightDir);

			NdotL = smoothstep(0, _Smooth, NdotL);

			half4 c;

			c.rgb = s.Albedo * _LightColor0.rgb * clamp(NdotL * atten, 0.3, 1);
			c.a = s.Alpha;

			return c;
		}

		sampler2D _MainTex;
		sampler2D _OverlayTex;
		fixed4 _Color1;
		fixed4 _Color2;
		fixed4 _Color3;

		sampler2D _NormalMap;


		struct Input
		{
			float2 uv_MainTex;
			float2 uv_OverlayTex;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			float4 c;
			float4 newC;
			float4 mask;

			c = tex2D(_MainTex, IN.uv_MainTex);
			mask = tex2D(_OverlayTex, IN.uv_OverlayTex);

			newC = _Color1;
			newC = lerp(newC, _Color2, mask.g);
			newC = lerp(newC, _Color3, mask.b);

			c = lerp (c, newC, 1 - c.a);

		 	o.Albedo = c;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
