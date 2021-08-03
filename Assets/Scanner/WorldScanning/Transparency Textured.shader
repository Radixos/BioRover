Shader "Scanner/WorldScanning/Transparency Textured" {
	Properties {
		[Header(Surface)]
		_BaseMap            ("Base Map", 2D) = "white" {}
		_BaseColor          ("Base Color", Color) = (1, 1, 1, 1)
		[Header(Scan)]
		_LightSweepVector   ("Light Sweep Vector", Vector) = (0, 0, 1, 0)
		_LightSweepAmp      ("Light Sweep Amp", Float) = 1
		_LightSweepExp      ("Light Sweep Exp", Float) = 10
		_LightSweepInterval ("Light Sweep Interval", Float) = 20
		_LightSweepSpeed    ("Light Sweep Speed", Float) = 10
		_LightSweepColor    ("Light Sweep Color", Color) = (1, 0, 0, 0)
		_LightSweepAddColor ("Light Sweep Add Color", Color) = (0, 0, 0, 0)	
	}
	SubShader {
		Tags { "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True" "RenderType" = "Transparent" "Queue" = "Transparent" }
		ZWrite Off
//		Blend SrcAlpha One
		Blend SrcAlpha OneMinusSrcAlpha

		HLSLINCLUDE
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		float4 _BaseMap_ST;
		half4 _BaseColor;
		ENDHLSL

		Pass {
			Tags { "LightMode" = "UniversalForward" }

			HLSLPROGRAM
			#pragma vertex SurfaceVertex
			#pragma fragment SurfaceFragment
			#include "Utils.cginc"

			// -------------------------------------
			// Universal Render Pipeline keywords
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile ALS_DIRECTIONAL ALS_SPHERICAL

			TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);

			void SurfaceFunction (Varyings IN, out SurfaceData surfaceData)
			{
				surfaceData = (SurfaceData)0;
				float2 uv = TRANSFORM_TEX(IN.uv, _BaseMap);

				half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv) * _BaseColor;
				surfaceData.diffuse = baseColor.rgb;
				surfaceData.alpha = baseColor.a;
				surfaceData.positionWS = IN.positionWS;
			}
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			half4 LightingFunction (SurfaceData surfaceData, LightingData lightingData)
			{
				float4 sonar = LightSweepColor(float4(surfaceData.positionWS, 1.0));
				half3 c = surfaceData.diffuse.rgb * _LightSweepAmp;
				return half4(c, sonar.a);
			}
			ENDHLSL
		}
		UsePass "Universal Render Pipeline/Lit/ShadowCaster"
		UsePass "Universal Render Pipeline/Lit/DepthOnly"
		UsePass "Universal Render Pipeline/Lit/Meta"
	}
	FallBack Off
}