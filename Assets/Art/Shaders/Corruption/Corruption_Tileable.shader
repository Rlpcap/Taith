// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Corruption_Tileable"
{
	Properties
	{
		_CorruptedTex("CorruptedTex", 2D) = "white" {}
		_SpeadAmount("SpeadAmount", Range( 0 , 1)) = 0.6030607
		_DistortionAmmount("DistortionAmmount", Range( 0 , 1)) = 0
		_ScrollSpeed("ScrollSpeed", Range( 0 , 1)) = 0
		_CorruptedColorDark("Corrupted Color Dark", Color) = (0.05899113,0.02024741,0.1226415,0)
		_CorruptedColorLight("Corrupted Color Light", Color) = (0,0,0,0)
		_enemyPos("enemyPos", Vector) = (0,0,0,0)
		_radius("radius", Float) = 0
		_FallOff("Fall Off", Float) = 0
		_DistortionMap("DistortionMap", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_Noise("Noise", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#define ASE_TEXTURE_PARAMS(textureName) textureName

		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _CorruptedTex;
		uniform float4 _CorruptedTex_ST;
		uniform float4 _CorruptedColorDark;
		uniform float4 _CorruptedColorLight;
		uniform sampler2D _Mask;
		uniform sampler2D _DistortionMap;
		uniform float4 _DistortionMap_ST;
		uniform float _DistortionAmmount;
		uniform float _ScrollSpeed;
		uniform float _SpeadAmount;
		uniform float3 _enemyPos;
		uniform float _radius;
		uniform float _FallOff;
		uniform sampler2D _Noise;
		uniform float4 _Noise_ST;


		inline float4 TriplanarSamplingSF( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tiling * worldPos.zy * float2( nsign.x, 1.0 ) ) );
			yNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tiling * worldPos.xz * float2( nsign.y, 1.0 ) ) );
			zNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tiling * worldPos.xy * float2( -nsign.z, 1.0 ) ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float2 uv_CorruptedTex = i.uv_texcoord * _CorruptedTex_ST.xy + _CorruptedTex_ST.zw;
			float4 MainTex92 = tex2D( _CorruptedTex, uv_CorruptedTex );
			float2 uv_DistortionMap = i.uv_texcoord * _DistortionMap_ST.xy + _DistortionMap_ST.zw;
			float2 panner13 = ( ( _Time.y * _ScrollSpeed ) * float2( 0,-1 ) + float2( 0,0 ));
			float2 uv_TexCoord16 = i.uv_texcoord + panner13;
			float4 lerpResult32 = lerp( _CorruptedColorDark , _CorruptedColorLight , tex2D( _Mask, ( ( (UnpackNormal( tex2D( _DistortionMap, uv_DistortionMap ) )).xy * _DistortionAmmount ) + uv_TexCoord16 ) ));
			float4 temp_cast_0 = (3.0).xxxx;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float4 triplanar91 = TriplanarSamplingSF( _Mask, ase_worldPos, ase_worldNormal, 1.0, float2( 0.05,0.05 ), 1.0, 0 );
			float4 temp_cast_1 = (_SpeadAmount).xxxx;
			float4 temp_output_34_0 = step( triplanar91 , temp_cast_1 );
			float4 temp_cast_2 = (_SpeadAmount).xxxx;
			float4 temp_cast_3 = (( _SpeadAmount / 1.1 )).xxxx;
			float4 lerpResult42 = lerp( ( MainTex92 * float4( 0.1226415,0.1226415,0.1226415,0 ) ) , ( pow( lerpResult32 , temp_cast_0 ) * 3.0 ) , ( temp_output_34_0 + ( temp_output_34_0 - step( triplanar91 , temp_cast_3 ) ) ));
			float4 Corruptionfold87 = lerpResult42;
			float4 color56 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float Mask44 = saturate( pow( ( distance( _enemyPos , ase_worldPos ) / _radius ) , _FallOff ) );
			float2 uv_Noise = i.uv_texcoord * _Noise_ST.xy + _Noise_ST.zw;
			float4 Noise72 = tex2D( _Noise, uv_Noise );
			float4 temp_cast_5 = (Mask44).xxxx;
			float4 lerpResult62 = lerp( Corruptionfold87 , MainTex92 , ( color56 * step( ( ( ( 1.0 - Mask44 ) * Noise72 ) - temp_cast_5 ) , float4( 0,0,0,0 ) ) ));
			o.Albedo = lerpResult62.rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;0;1920;1019;1836.538;-1358.113;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;3;-4559.251,-635.9263;Inherit;False;1862.961;907.1392;Distortion;13;31;26;23;17;16;15;14;13;9;8;6;5;4;Base Distortion;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;5;-4440.351,47.01272;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-4509.251,156.2128;Inherit;False;Property;_ScrollSpeed;ScrollSpeed;3;0;Create;True;0;0;False;0;False;0;0.142;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;6;-4328.692,-482.5042;Inherit;True;Property;_DistortionMap;DistortionMap;9;0;Create;True;0;0;False;0;False;None;45b7d34a07e91d14f8fd42ca2b072fe0;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.Vector3Node;64;-3635.784,2591.927;Inherit;False;Property;_enemyPos;enemyPos;6;0;Create;False;0;0;False;0;False;0,0,0;169.1328,20.05385,59.59611;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;63;-3603.612,2783.324;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-4168.651,97.7127;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;8;-4007.336,-413.6114;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DistanceOpNode;66;-3355.902,2697.373;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-3214.644,2799.142;Inherit;False;Property;_radius;radius;7;0;Create;True;0;0;False;0;False;0;70;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;68;-3037.944,2697.442;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-2958.158,2805.527;Inherit;False;Property;_FallOff;Fall Off;8;0;Create;True;0;0;False;0;False;0;5.67;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;15;-3662.114,-405.7611;Inherit;True;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;13;-3915.08,48.33791;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-3710.983,-168.131;Inherit;False;Property;_DistortionAmmount;DistortionAmmount;2;0;Create;True;0;0;False;0;False;0;0.848;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;18;-2514.183,42.97451;Inherit;False;1640.536;721.7322;Comment;8;41;37;34;33;27;24;22;91;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-3694.675,2.621988;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;69;-2802.158,2696.527;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-3374.984,-244.131;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;21;-2491.606,-1307.719;Inherit;False;1187.951;569.4151;Colors;6;39;36;35;32;30;29;Colors;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;26;-3351.087,-517.3755;Inherit;True;Property;_Mask;Mask;10;0;Create;True;0;0;False;0;False;None;ed200a308c3598c489313a6dd109b644;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-3249.676,-54.37799;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-2407.274,450.3712;Inherit;False;Property;_SpeadAmount;SpeadAmount;1;0;Create;True;0;0;False;0;False;0.6030607;0.489;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-2330.638,605.1334;Inherit;False;Constant;_DivideAmount;DivideAmount;7;0;Create;True;0;0;False;0;False;1.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;70;-2597.019,2695.932;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;44;-2411.719,2689.492;Inherit;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;71;-3017.045,2315.859;Inherit;True;Property;_Noise;Noise;11;0;Create;True;0;0;False;0;False;-1;None;759a45d2aa606de469cde71db0d2136c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;38;-4335.319,-1447.781;Inherit;True;Property;_CorruptedTex;CorruptedTex;0;0;Create;True;0;0;False;0;False;-1;None;e2fe0e995778c22449542f306491aa32;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TriplanarNode;91;-2492.124,128.5579;Inherit;True;Spherical;World;False;Top Texture 2;_TopTexture2;white;-1;None;Mid Texture 2;_MidTexture2;white;-1;None;Bot Texture 2;_BotTexture2;white;-1;None;Triplanar Sampler;False;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;0.05,0.05;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;30;-2434.64,-1257.719;Inherit;False;Property;_CorruptedColorDark;Corrupted Color Dark;4;0;Create;True;0;0;False;0;False;0.05899113,0.02024741,0.1226415,0;0.05899113,0.02024741,0.1226415,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;27;-2008.983,511.7068;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;29;-2441.606,-1050.469;Inherit;False;Property;_CorruptedColorLight;Corrupted Color Light;5;0;Create;True;0;0;False;0;False;0,0,0,0;0.4766821,0.4466002,0.490566,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;31;-2811.193,-411.1618;Inherit;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;33;-1688.663,347.5425;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;32;-2054.914,-1082.301;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-1989.503,-853.3041;Inherit;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;45;-2533.014,2933.677;Inherit;False;44;Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;72;-2593.553,2350.039;Inherit;False;Noise;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;34;-1891.592,92.97451;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0.2;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;92;-3791.708,-1419.566;Inherit;False;MainTex;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;47;-2308.016,2916.376;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;46;-2522.536,3021.263;Inherit;False;72;Noise;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;93;-1653.399,-479.0963;Inherit;False;92;MainTex;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;36;-1783.635,-1199.476;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;37;-1388.362,234.0958;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;48;-2298.602,3166.328;Inherit;False;44;Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-2122.385,2935.868;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-1325.867,-492.7641;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.1226415,0.1226415,0.1226415,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-1538.656,-1061.3;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;41;-1108.646,130.5409;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;42;-762.1824,-272.8715;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;53;-1833.748,2995.398;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;54;-1541.213,2997.322;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;56;-1539.361,2784.51;Inherit;False;Constant;_EdgeColor;EdgeColor;10;0;Create;True;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;87;-451.8762,-248.5321;Inherit;False;Corruptionfold;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-1228.628,2848.1;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;88;-1335.177,1685.679;Inherit;False;87;Corruptionfold;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;94;-1262.569,1850.954;Inherit;False;92;MainTex;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;62;-821.3864,1828.715;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-460.4896,1849.004;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Corruption_Tileable;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;5;0
WireConnection;9;1;4;0
WireConnection;8;0;6;0
WireConnection;66;0;64;0
WireConnection;66;1;63;0
WireConnection;68;0;66;0
WireConnection;68;1;65;0
WireConnection;15;0;8;0
WireConnection;13;1;9;0
WireConnection;16;1;13;0
WireConnection;69;0;68;0
WireConnection;69;1;67;0
WireConnection;17;0;15;0
WireConnection;17;1;14;0
WireConnection;23;0;17;0
WireConnection;23;1;16;0
WireConnection;70;0;69;0
WireConnection;44;0;70;0
WireConnection;91;0;26;0
WireConnection;27;0;22;0
WireConnection;27;1;24;0
WireConnection;31;0;26;0
WireConnection;31;1;23;0
WireConnection;33;0;91;0
WireConnection;33;1;27;0
WireConnection;32;0;30;0
WireConnection;32;1;29;0
WireConnection;32;2;31;0
WireConnection;72;0;71;0
WireConnection;34;0;91;0
WireConnection;34;1;22;0
WireConnection;92;0;38;0
WireConnection;47;0;45;0
WireConnection;36;0;32;0
WireConnection;36;1;35;0
WireConnection;37;0;34;0
WireConnection;37;1;33;0
WireConnection;49;0;47;0
WireConnection;49;1;46;0
WireConnection;40;0;93;0
WireConnection;39;0;36;0
WireConnection;39;1;35;0
WireConnection;41;0;34;0
WireConnection;41;1;37;0
WireConnection;42;0;40;0
WireConnection;42;1;39;0
WireConnection;42;2;41;0
WireConnection;53;0;49;0
WireConnection;53;1;48;0
WireConnection;54;0;53;0
WireConnection;87;0;42;0
WireConnection;60;0;56;0
WireConnection;60;1;54;0
WireConnection;62;0;88;0
WireConnection;62;1;94;0
WireConnection;62;2;60;0
WireConnection;0;0;62;0
ASEEND*/
//CHKSM=8490B4675C7205AF3C7FD6AD068745C0DE29C114