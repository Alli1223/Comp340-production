Shader "Toon/Basic Outline" {
	Properties {
		_Color ("Main Color", Color) = (.5,.5,.5,1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline", Range (.002, 0.10)) = .005
		_OutlineWidth("Outline Width", Range(.01, 0.9)) = .9
		_MainTex ("Base (RGB)", 2D) = "white" { }
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	
	struct appdata {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f {
		float4 pos : SV_POSITION;
		UNITY_FOG_COORDS(0)
		fixed4 color : COLOR;
	};
	
	uniform float _Outline;
	uniform float _OutlineWidth;
	uniform float4 _OutlineColor;
	
	v2f vert(appdata v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);

		float3 norm   = normalize(mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal));
		float2 offset = TransformViewToProjection(norm.xy);

		#ifdef UNITY_Z_0_FAR_FROM_CLIPSPACE //to handle recent standard asset package on older version of unity (before 5.5)
			o.pos.xy + offset * UNITY_Z_0_FAR_FROM_CLIPSPACE(o.pos.z) * _Outline;

			// Remove the missing corners
			o.pos.xy -= _Outline * norm.xy;
			//Offset the height of the outline by the outline width
			o.pos.y += -o.pos.z * _OutlineWidth;
			
		#else
			o.pos.xy += offset * o.pos.z * _Outline;
			
			
		#endif
		o.color = _OutlineColor;
		UNITY_TRANSFER_FOG(o,o.pos);
		return o;
	}
	ENDCG

	SubShader {
		Tags { "RenderType"="Opaque" }
		UsePass "Toon/Basic/BASE"
		Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			ZWrite On
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			fixed4 frag(v2f i) : SV_Target
			{
				UNITY_APPLY_FOG(i.fogCoord, i.color);
				return i.color;
			}
			ENDCG
		}
	}
	
	Fallback "Toon/Basic"
}
