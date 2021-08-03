Shader "Scanner/WorldScanning/Transparency Stripe" {
	Properties {
		[Header(Surface)]
		_BaseMap                        ("Base Map", 2D) = "white" {}
		_BaseColor                      ("Base Color", Color) = (1, 1, 1, 1)
		[Normal][NoScaleOffset]_BumpMap ("Normal Map", 2D) = "bump" {}
		[Header(Scan)]
		_LightSweepVector   ("Light Sweep Vector", Vector) = (0, 0, 1, 0)
		_LightSweepAmp      ("Light Sweep Amp", Float) = 1
		_LightSweepExp      ("Light Sweep Exp", Float) = 10
		_LightSweepInterval ("Light Sweep Interval", Float) = 20
		_LightSweepSpeed    ("Light Sweep Speed", Float) = 10
		_LightSweepColor    ("Light Sweep Color", Color) = (1, 0, 0, 0)
		_LightSweepAddColor ("Light Sweep Add Color", Color) = (0, 0, 0, 0)
		[Header(Stripe)]
		_StripeTex     ("Stripe", 2D) = "white" {}
		_StripeColor   ("Stripe Color", Color) = (1, 0.8, 0, 1)
		_StripeWidth   ("Stripe Width", Float) = 0.1
		_StripeDensity ("Stripe Density", Float) = 5
	}
	SubShader {
		Tags { "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True" "RenderType" = "Transparent" "Queue" = "Transparent" }
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
			TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap);
			sampler2D _StripeTex;
			float4 _StripeColor;
			float _StripeWidth, _StripeDensity;

			void SurfaceFunction (Varyings IN, out SurfaceData surfaceData)
			{
				surfaceData = (SurfaceData)0;
				float2 uv = TRANSFORM_TEX(IN.uv, _BaseMap);

				half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv) * _BaseColor;
				surfaceData.diffuse = baseColor.rgb;
				surfaceData.normalWS = GetPerPixelNormalScaled(TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), uv, IN.normalWS, IN.tangentWS, 1.0);
				surfaceData.alpha = baseColor.a;
				surfaceData.positionWS = IN.positionWS;
			}
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			half4 LightingFunction (SurfaceData surfaceData, LightingData lightingData)
			{
				half3 tc = surfaceData.diffuse.rgb * _LightSweepAmp;
				float4 sonar = LightSweepColor(float4(surfaceData.positionWS, 1.0));

				half3 stripeUvw = surfaceData.positionWS * _StripeDensity;
				float stripex = tex2D(_StripeTex, float2(stripeUvw.x, 1.0 - _StripeWidth)).x;
				float stripey = tex2D(_StripeTex, float2(stripeUvw.y, 1.0 - _StripeWidth)).x;
				float stripez = tex2D(_StripeTex, float2(stripeUvw.z, 1.0 - _StripeWidth)).x;
				float checker = stripex * stripey * stripez;
				half3 rgb = lerp(tc, _StripeColor.rgb, sonar.a) * lightingData.light.color * lightingData.NdotL;
				half a = lerp(surfaceData.alpha, 1.0 - checker, sonar.a);
				return half4(rgb * lightingData.light.shadowAttenuation, a);
			}
			ENDHLSL
		}
		UsePass "Universal Render Pipeline/Lit/ShadowCaster"
		UsePass "Universal Render Pipeline/Lit/DepthOnly"
		UsePass "Universal Render Pipeline/Lit/Meta"
	}
	FallBack Off
}