// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/FogOfWar" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_FogRadius("FogRadius", Float) = 1.0
		_FogMaxRadius("FogMaxRadius", Float) = 0.5
		_PlayerPos("PlayerPos", Vector) = (0,0,0,1)

	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		Cull off

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
//#pragma surface surf Standard fullforwardshadows

#pragma surface surf Lambert vertex:vert alpha:blend

		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		float _FogRadius;
		float _FogMaxRadius;
		float4 _PlayerPos;


		struct Input 
		{
			float2 uv_MainTex;
			float2 location;
		};

		void vert(inout appdata_full vertexData, out Input outData)
		{
			float4 pos = UnityObjectToClipPos(vertexData.vertex);
			float4 posWorld = mul(unity_ObjectToWorld, vertexData.vertex);
			outData.uv_MainTex = vertexData.texcoord;
			outData.location = posWorld.xz;
		}
		float powerForPos(float4 pos, float2 nearVertex)
		{
			float atten = (_FogRadius * length(pos.xy - nearVertex.xy));
			return atten / _FogMaxRadius;
		}
		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 baseColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			float alpha = (1.0 * powerForPos(_PlayerPos, IN.location));
			o.Albedo = baseColor.rgb;
			o.Alpha = alpha;
		}

		

	// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
	// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
	// #pragma instancing_options assumeuniformscaling
	UNITY_INSTANCING_CBUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

	ENDCG
	}
		FallBack "Diffuse"
}
