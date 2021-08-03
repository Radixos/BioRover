Shader "Scanner/WorldScanning/Triangles Flow" {
	Properties {
		[Header(Surface)]
		_BaseMap                        ("Base Map", 2D) = "white" {}
		_BaseColor                      ("Base Color", Color) = (1, 1, 1, 1)
		[Normal][NoScaleOffset]_BumpMap ("Normal Map", 2D) = "bump" {}
		[Header(TrianglesFlow)]
		_Color          ("Main Color", Color) = (1, 1, 1, 1)
		_Speed          ("Speed", Range(0.1, 6)) = 1
		_TrianglesScale ("Triangles Scale", Range(1, 32)) = 1
		_RangeScale     ("Range Scale", Range(0.1, 20)) = 1
		_Center         ("Center", Vector) = (0.0, -1.0, 3.0, 1.0)
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

			TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
			TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap);

			half4 _Color, _Center;
			half _Speed, _TrianglesScale, _RangeScale;

			float r (float n)  { return frac(abs(sin(n*55.753)*367.34)); }
			float r (float2 n) { return r(dot(n, float2(2.46, -1.21))); }
			float3 SmallTrianglesColor (float3 pos)
			{
				float a = (radians(60.0));
				float zoom = 0.125;
				float2 c = (pos.xy + float2(0.0, pos.z)) * float2(sin(a), 1.0) / _TrianglesScale;
				c = ((c + float2(c.y, 0.0)*cos(a)) / zoom) + float2(floor((c.x - c.y*cos(a)) / zoom*4.0) / 4.0, 0.0);
				float type = (r(floor(c*4.0))*0.2 + r(floor(c*2.0))*0.3 + r(floor(c))*0.5);
				type += 0.3 * sin(_Time.y * 5.0 * type);

				float l = min(min((1.0 - (2.0 * abs(frac((c.x - c.y)*4.0) - 0.5))),
					(1.0 - (2.0 * abs(frac(c.y * 4.0) - 0.5)))),
					(1.0 - (2.0 * abs(frac(c.x * 4.0) - 0.5))));
				l = smoothstep(0.06, 0.04, l);

				return lerp(type, l, 0.3);
			}
			float3 LargeTrianglesColor (float3 pos)
			{
				float a = (radians(60.0));
				float zoom = 0.5;
				float2 c = (pos.xy + float2(0.0, pos.z)) * float2(sin(a), 1.0) / _TrianglesScale;
				c = ((c + float2(c.y, 0.0)*cos(a)) / zoom) + float2(floor((c.x - c.y*cos(a)) / zoom*4.0) / 4.0, 0.0);

				float l = min(min((1.0 - (2.0 * abs(frac((c.x - c.y)*4.0) - 0.5))),
					(1.0 - (2.0 * abs(frac(c.y * 4.0) - 0.5)))),
					(1.0 - (2.0 * abs(frac(c.x * 4.0) - 0.5))));
				l = smoothstep(0.03, 0.02, l);

				return lerp(0.01, l, 1.0);
			}
			float3 TriFlow (in float3 pos, in float3 midPos)
			{
				float d = length(pos.xz - midPos.xz) + pos.y - midPos.y;
				d /= _RangeScale;
				float border = fmod(_Time.y * _Speed, 5.0);

				float3 c1 = LargeTrianglesColor(pos);
				float3 c2 = SmallTrianglesColor(pos);

				float3 c = smoothstep(border - 0.2, border, d);
				c += c1;
				c += c2 * smoothstep(border - 0.4, border - 0.6, d);
				c *= smoothstep(border, border - 0.05, d);
				c *= smoothstep(border - 3.0, border - 0.5, d);
				c *= smoothstep(5.0, 4.0, border);
				return c * _Color.rgb;
			}

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
				half3 scan = TriFlow(surfaceData.positionWS, _Center.xyz);

				half3 rgb = surfaceData.diffuse * lightingData.light.color * lightingData.NdotL;
				rgb += scan;
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