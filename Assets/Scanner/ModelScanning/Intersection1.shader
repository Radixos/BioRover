Shader "Scanner/ModelScanning/Intersection1" {
	Properties {
		[HDR]_MainColor         ("Main", Color) = (1, 1, 1, 0.25)
		_MainTex                ("Main Texture", 2D) = "white" {}
		_TextureScroll          ("Texture Scroll", Vector) = (0, 0, 0, 0)

		[Header(Rim)]
		[Toggle(RIM)]_RIM  ("Rim", Float) = 1
		_RimPower          ("Rim Power", Range(1, 10)) = 2
		_RimIntensity      ("Rim Intensity", Range(1, 6)) = 2

		[Header(Flow)]
		[Toggle(FLOW)]_FLOW        ("Flow Enable", Float) = 0
		[NoScaleOffset]_HexEdgeTex ("Hex Edge", 2D) = "white" {}
		[HDR]_HexColor             ("Hex Color", Color) = (1, 1, 1, 1)
		[HDR]_HexEdgeColor         ("Hex Edge Color", Color) = (0, 0, 0, 0)

		[Header(Intersection)]
		_IntersectionMax        ("Intersection Max", Float) = 1
		_IntersectionDamper     ("Intersection Damper", Float) = 0.3
		[HDR]_IntersectionColor ("Intersection Color", Color) = (1, 1, 1, 1)
	}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off Cull Off
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature RIM
			#pragma shader_feature FLOW
			#pragma target 3.0
			#include "UnityCG.cginc"

#if UNITY_VERSION < 540
			#define COMPUTESCREENPOS ComputeScreenPos
#else
			#define COMPUTESCREENPOS ComputeNonStereoScreenPos
#endif
			sampler2D _CameraDepthTexture, _MainTex;
			fixed4 _MainColor, _IntersectionColor, _RimColor, _TextureScroll;
			half _IntersectionMax, _IntersectionDamper, _RimPower, _RimIntensity;
			float4 _MainTex_ST;

			sampler2D _HexEdgeTex;
			half4 _HexColor, _HexEdgeColor;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 scrpos : TEXCOORD1;
				float fresnel : TEXCOORD2;
				float4 objpos : TEXCOORD3;
			};
			v2f vert (appdata_base v)
			{
				float4 wp = mul(unity_ObjectToWorld, v.vertex);
				float3 vdir = normalize(ObjSpaceViewDir(v.vertex));

				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.scrpos = COMPUTESCREENPOS(o.pos);
				o.scrpos.z = lerp(o.pos.w, mul(UNITY_MATRIX_V, wp).z, unity_OrthoParams.w);
				o.fresnel = 1.0 - saturate(dot(vdir, v.normal));
				o.objpos = v.vertex;
				return o;
			}
			fixed4 frag (v2f input, fixed facing : VFACE) : SV_Target
			{
				float sceneZ = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(input.scrpos));
				float perpectiveZ = LinearEyeDepth(sceneZ);
#if defined(UNITY_REVERSED_Z)
				sceneZ = 1 - sceneZ;
#endif
				float orthoZ = sceneZ * (_ProjectionParams.y - _ProjectionParams.z) - _ProjectionParams.y;
				sceneZ = lerp(perpectiveZ, orthoZ, unity_OrthoParams.w);
				float dist = sqrt(pow(sceneZ - input.scrpos.z, 2));

				half mask = max(0, sign(_IntersectionMax - dist));
				mask *= 1.0 - dist / _IntersectionMax * _IntersectionDamper;

				// unused 'iscol' code anymore...
//				fixed4 iscol = tex2D(_MainTex, input.uv + _TextureScroll.xy * _Time.x);
//				mask *= iscol.a * _IntersectionColor.a;

#if FLOW
				float horizontalDist = abs(input.objpos.x);
				float verticalDist = abs(input.objpos.y);

				half4 pulseTex = tex2D(_MainTex, input.uv);
				half4 pulseTerm = pulseTex * _HexColor * abs(sin(_Time.y * 1.0 - horizontalDist * 10.0 + pulseTex.r * 0.5));
				
				float flow = max(sin((horizontalDist + verticalDist)*50-_Time.y*6), 0.0);
				half4 hexEdgeTex = tex2D(_HexEdgeTex, input.uv);
				half4 hexEdgeTerm = hexEdgeTex * _HexEdgeColor * flow;
				
				half4 col = half4(hexEdgeTerm.rgb + pulseTerm.rgb, 1.0);
				col.a *= _MainColor.a;   // align to non-flow alpha logic
				col.rgb = lerp(col.rgb, _IntersectionColor.rgb, mask);
#else
				half4 col = tex2D(_MainTex, input.uv + _TextureScroll.xy * _Time.x);
				col *= _MainColor * (1.0 - mask);
				col += _IntersectionColor * mask;
#endif

#if RIM
				float f = facing > 0 ? pow(input.fresnel, _RimPower) * _RimIntensity : 1.0;
				col.a *= f;
#endif

				col.a = max(mask, col.a);
				return col;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Color"
}
