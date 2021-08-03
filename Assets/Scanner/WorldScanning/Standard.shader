Shader "Scanner/WorldScanning/Standard" {
	Properties {
		[Header(Surface)]
		_BaseMap                        ("Base Map", 2D) = "white" {}
		_BaseColor                      ("Base Color", Color) = (1, 1, 1, 1)
		_Smoothness                     ("Smoothness", Range(0, 1)) = 0
		_Metallic                       ("Metallic", Range(0, 1)) = 0
		[HDR]_Emission                  ("Emission", Color) = (0, 0, 0, 1)
		[Normal][NoScaleOffset]_BumpMap ("Normal Map", 2D) = "bump" {}
		[Header(Scan)]
		_LightSweepVector   ("Light Sweep Vector", Vector) = (0, 0, 1, 0)
		_LightSweepAmp      ("Light Sweep Amp", Range(0, 2)) = 1
		_LightSweepExp      ("Light Sweep Exp", Range(0, 32)) = 10
		_LightSweepInterval ("Light Sweep Interval", Range(0, 32)) = 20
		_LightSweepSpeed    ("Light Sweep Speed", Range(0, 16)) = 10
		_LightSweepColor    ("Light Sweep Color", Color) = (1, 0, 0, 0)
		_LightSweepAddColor ("Light Sweep Add Color", Color) = (0, 0, 0, 0)
	}
	SubShader {
		Tags { "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True" }

		HLSLINCLUDE
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		float4 _BaseMap_ST;
		half4 _BaseColor, _Emission;
		half _Metallic, _Smoothness;
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

			void SurfaceFunction (Varyings IN, out SurfaceData surfaceData)
			{
				surfaceData = (SurfaceData)0;
				float2 uv = TRANSFORM_TEX(IN.uv, _BaseMap);

				half3 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv).rgb * _BaseColor.rgb;
				surfaceData.diffuse = baseColor.rgb;//ComputeDiffuseColor(baseColor.rgb, _Metallic);
				surfaceData.reflectance = ComputeFresnel0(baseColor.rgb, _Metallic, 0.25 * 0.16);
				surfaceData.ao = 1.0;
				surfaceData.perceptualRoughness = 1.0 - _Smoothness;
				surfaceData.normalWS = GetPerPixelNormalScaled(TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), uv, IN.normalWS, IN.tangentWS, 1.0);
				surfaceData.emission = _Emission.rgb;
				surfaceData.alpha = 1.0;
				surfaceData.positionWS = IN.positionWS;
			}
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			half4 LightingFunction (SurfaceData surfaceData, LightingData lightingData)
			{
				half perceptualRoughness = max(surfaceData.perceptualRoughness, 0.089);
				half roughness = PerceptualRoughnessToRoughness(perceptualRoughness);

				half3 environmentReflection = lightingData.environmentReflections;
				environmentReflection *= EnvironmentBRDF(surfaceData.reflectance, roughness, lightingData.NdotV);

				half3 environmentLighting = lightingData.environmentLighting * surfaceData.diffuse;
				half3 diffuse = surfaceData.diffuse;

				half DV = DV_SmithJointGGX(lightingData.NdotH, lightingData.NdotL, lightingData.NdotV, roughness);

				half3 F = F_Schlick(surfaceData.reflectance, lightingData.LdotH);
				half3 specular = DV * F;
				half3 finalColor = (diffuse + specular) * lightingData.light.color * lightingData.NdotL;
				finalColor += environmentReflection + environmentLighting + surfaceData.emission;
				finalColor += LightSweepColor(float4(surfaceData.positionWS, 1.0)).rgb;
				return half4(finalColor * lightingData.light.shadowAttenuation, surfaceData.alpha);
			}
			ENDHLSL
		}
		UsePass "Universal Render Pipeline/Lit/ShadowCaster"
		UsePass "Universal Render Pipeline/Lit/DepthOnly"
		UsePass "Universal Render Pipeline/Lit/Meta"
	}
	FallBack Off
}