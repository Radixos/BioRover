Shader "Scanner/WorldScanning/Matrix" {
	Properties {
		[Header(Surface)]
		_BaseMap                        ("Base Map", 2D) = "white" {}
		_BaseColor                      ("Base Color", Color) = (1, 1, 1, 1)
		[Normal][NoScaleOffset]_BumpMap ("Normal Map", 2D) = "bump" {}

		[Header(Matrix)]
		_CharScale    ("Character Scale", Range(0.01, 0.4)) = 0.2
		_CharDarkness ("Character Darkness", Range(0.1, 32)) = 30

		[Header(Scan)]
		_LightSweepVector ("Light Sweep Vector", Vector) = (0, 0, 1, 0)
		_LightSweepAmp ("Light Sweep Amp", Float) = 1
		_LightSweepExp ("Light Sweep Exp", Float) = 10
		_LightSweepInterval ("Light Sweep Interval", Float) = 20
		_LightSweepSpeed ("Light Sweep Speed", Float) = 10
		_LightSweepColor ("Light Sweep Color", Color) = (1, 1, 1, 1)
		_LightSweepAddColor ("Light Sweep Add Color", Color) = (0, 0, 0, 0)
	}
	SubShader {
		Tags { "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True" }

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
			#include "../Utils.cginc"

			// -------------------------------------
			// Universal Render Pipeline keywords
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile ALS_DIRECTIONAL

			TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
			TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap);

			// matrix effect /////////////////////////////////////////////////////////////////////////////////////////////////////////
			sampler2D _GlobalNoise, _GlobalFont;
			half _CharScale, _CharDarkness;

			float text (float2 coord)
			{
				float2 uv = frac(coord / 16.0);
				uv = uv * 0.7 + 0.1;

				float2 block = floor(coord / 16.0);
				float2 rand = tex2D(_GlobalNoise, block / 512.0).xy;   // 512 is the white noise texture width
				rand = floor(rand * 16.0);
				uv += rand;
				uv *= 0.0625;   // 0~16 normalize to 0~1
				uv.x = -uv.x;
				return tex2D(_GlobalFont, uv).r;
			}

			#define dropLength 512
			float3 rain (float2 coord)
			{
				coord.x = floor(coord.x / 16.0);
				float offset = sin(coord.x * 15.0);
				float speed = cos(coord.x * 3.0) * 0.15 + 0.35;
				float y = frac((coord.y / dropLength) + _Time.y * speed + offset);
				return float3(0.1, 1.0, 0.35) / (y * _CharDarkness);
			}
			float3 MatrixEffect (float2 coord)
			{
				float3 c = rain(coord * dropLength *_CharScale);
				return text(coord * dropLength * _CharScale) * c;
			}
			// matrix effect /////////////////////////////////////////////////////////////////////////////////////////////////////////

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
				half4 sweep = LightSweepColor(float4(surfaceData.positionWS, 1.0));

				half4 col = half4(0, 0, 0, 1);
				float3 colFront = MatrixEffect(surfaceData.positionWS.xy + sin(surfaceData.positionWS.zz));
				float3 colSide  = MatrixEffect(surfaceData.positionWS.zy + sin(surfaceData.positionWS.xx));
				float3 colTop   = MatrixEffect(surfaceData.positionWS.xz + sin(surfaceData.positionWS.yy));

				float3 bw = pow(normalize(abs(surfaceData.normalWS)), 10.0);   // blend weight
				bw /= (bw.x + bw.y + bw.z);
				col.xyz = colFront * bw.z + colSide * bw.x + colTop * bw.y;

				half3 rgb = surfaceData.diffuse * lightingData.light.color * lightingData.NdotL;
				rgb += (col.rgb * sweep.r);
				rgb = saturate(rgb);
				half a = surfaceData.alpha;
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