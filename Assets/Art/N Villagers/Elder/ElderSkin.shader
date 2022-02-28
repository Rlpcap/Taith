// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ElderSkin"
{
	Properties
	{
		_MainText1("MainText", 2D) = "white" {}
		_DissolveAmount2("DissolveAmount", Range( -0.5 , 1.5)) = 0
		_SpeadAmount2("SpeadAmount", Range( 0 , 1)) = 0.6030607
		_CorruptedColorDark2("Corrupted Color Dark", Color) = (0.05899113,0.02024741,0.1226415,0)
		_CorruptedColorLight2("Corrupted Color Light", Color) = (0,0,0,0)
		[Toggle]_ToggleSwitch2("Toggle Switch0", Float) = 1
		_DistortionMap2("DistortionMap", 2D) = "white" {}
		_ScrollSpeed("ScrollSpeed", Range( 0 , 1)) = 0.3058824
		_DistortionAmmount("DistortionAmmount", Range( 0 , 1)) = 0.65
		_Mask2("Mask", 2D) = "white" {}
		_DissolveDirection("DissolveDirection", Vector) = (0,1,0,0)
		_EdgeColor3("EdgeColor", Color) = (0.5813924,0.2198736,0.764151,0)
		_Noise2("Noise", 2D) = "white" {}
		_EdgeThickness("EdgeThickness", Range( 0 , 1.15)) = 1.15
		_NoiseScrollSpeed("NoiseScrollSpeed", Float) = 0.27
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
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

		uniform float4 _CorruptedColorDark2;
		uniform float4 _CorruptedColorLight2;
		uniform sampler2D _Mask2;
		uniform sampler2D _DistortionMap2;
		uniform float4 _DistortionMap2_ST;
		uniform float _DistortionAmmount;
		uniform float _ScrollSpeed;
		uniform float _SpeadAmount2;
		uniform sampler2D _MainText1;
		uniform float4 _MainText1_ST;
		uniform float4 _EdgeColor3;
		uniform float _ToggleSwitch2;
		uniform float3 _DissolveDirection;
		uniform float _DissolveAmount2;
		uniform sampler2D _Noise2;
		uniform float _NoiseScrollSpeed;
		uniform float _EdgeThickness;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;


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
			float2 uv_DistortionMap2 = i.uv_texcoord * _DistortionMap2_ST.xy + _DistortionMap2_ST.zw;
			float2 panner37 = ( ( _Time.y * _ScrollSpeed ) * float2( 0,-1 ) + float2( 0,0 ));
			float2 uv_TexCoord52 = i.uv_texcoord + panner37;
			float4 lerpResult91 = lerp( _CorruptedColorDark2 , _CorruptedColorLight2 , tex2D( _Mask2, ( ( (UnpackNormal( tex2D( _DistortionMap2, uv_DistortionMap2 ) )).xy * _DistortionAmmount ) + uv_TexCoord52 ) ));
			float4 temp_cast_0 = (3.0).xxxx;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float4 triplanar77 = TriplanarSamplingSF( _Mask2, ase_worldPos, ase_worldNormal, 1.0, float2( 0.05,0.05 ), 1.0, 0 );
			float4 temp_cast_1 = (_SpeadAmount2).xxxx;
			float4 temp_output_87_0 = step( triplanar77 , temp_cast_1 );
			float4 temp_cast_2 = (_SpeadAmount2).xxxx;
			float4 temp_cast_3 = (( _SpeadAmount2 / 1.1 )).xxxx;
			float4 lerpResult117 = lerp( float4( 0,0,0,0 ) , ( pow( lerpResult91 , temp_cast_0 ) * 3.0 ) , ( temp_output_87_0 + ( temp_output_87_0 - step( triplanar77 , temp_cast_3 ) ) ));
			float4 Corruptionfold128 = lerpResult117;
			float2 uv_MainText1 = i.uv_texcoord * _MainText1_ST.xy + _MainText1_ST.zw;
			float4 MainTex86 = tex2D( _MainText1, uv_MainText1 );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 normalizeResult44 = normalize( (( _ToggleSwitch2 )?( ase_vertex3Pos ):( ase_worldPos )) );
			float3 normalizeResult41 = normalize( _DissolveDirection );
			float dotResult53 = dot( normalizeResult44 , normalizeResult41 );
			float Gradient75 = ( dotResult53 + (-1.29 + (_DissolveAmount2 - 0.0) * (1.06 - -1.29) / (1.0 - 0.0)) );
			float3 normalizeResult27 = normalize( float3(0.76,1,0) );
			float3 temp_output_62_0 = ( ase_vertex3Pos + ( ( normalizeResult27 * _NoiseScrollSpeed ) * _Time.y ) );
			float4 CorruptNoise84 = tex2D( _Noise2, temp_output_62_0.xy );
			float4 temp_cast_6 = (Gradient75).xxxx;
			float4 temp_cast_7 = (( _EdgeThickness + (0.0 + (0.0 - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) )).xxxx;
			float4 lerpResult136 = lerp( Corruptionfold128 , MainTex86 , ( _EdgeColor3 * step( ( ( ( 1.0 - Gradient75 ) * CorruptNoise84 ) - temp_cast_6 ) , temp_cast_7 ) ));
			o.Albedo = lerpResult136.rgb;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float3 temp_cast_9 = (tex2D( _TextureSample0, uv_TextureSample0 ).a).xxx;
			o.Emission = temp_cast_9;
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
-63;522;1920;1019;3309.071;645.2401;2.67462;True;True
Node;AmplifyShaderEditor.CommentaryNode;2;-5352.914,233.6014;Inherit;False;1971.741;2153.503;;2;8;7;Corruption;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;6;-6878.674,-2902.798;Inherit;False;1862.961;907.1392;Distortion;13;74;61;56;55;52;43;38;37;30;28;21;18;15;Base Distortion;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;7;-5227.185,1260.567;Inherit;False;1698.852;1060.583;;15;75;60;53;51;44;41;40;36;35;32;31;29;26;20;17;Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;8;-5277.329,377.4104;Inherit;False;1794.989;768.6633;;12;137;135;84;73;62;50;49;42;33;27;22;12;Fade;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;20;-5177.185,1481.746;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;18;-6759.774,-2219.858;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-6828.674,-2110.658;Inherit;False;Property;_ScrollSpeed;ScrollSpeed;8;0;Create;True;0;0;False;0;False;0.3058824;0.547;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;15;-6648.115,-2749.375;Inherit;True;Property;_DistortionMap2;DistortionMap;7;0;Create;True;0;0;False;0;False;None;45b7d34a07e91d14f8fd42ca2b072fe0;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.Vector3Node;12;-5227.329,605.1172;Inherit;False;Constant;_NoiseDirection2;NoiseDirection;5;0;Create;True;0;0;False;0;False;0.76,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;17;-5173.58,1310.567;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-6488.074,-2169.158;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-5082.293,1929.561;Inherit;False;Constant;_UpperLimit2;UpperLimit;7;0;Create;True;0;0;False;0;False;1.29;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;27;-4996.037,629.1481;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-5102.671,849.9272;Inherit;False;Property;_NoiseScrollSpeed;NoiseScrollSpeed;15;0;Create;True;0;0;False;0;False;0.27;0.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-5087.698,2014.251;Inherit;False;Constant;_LowerLimit2;LowerLimit;7;0;Create;True;0;0;False;0;False;-1.06;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;30;-6326.759,-2680.482;Inherit;True;Property;_TextureSample2;Texture Sample 0;3;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;29;-4869.06,1371.831;Inherit;False;Property;_ToggleSwitch2;Toggle Switch0;6;0;Create;True;0;0;False;0;False;1;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;32;-4871.913,1669.618;Inherit;False;Property;_DissolveDirection;DissolveDirection;11;0;Create;True;0;0;False;0;False;0,1,0;1,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NegateNode;40;-4824.622,1907.938;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;42;-5107.177,967.0732;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;43;-6031.639,-2435.002;Inherit;False;Property;_DistortionAmmount;DistortionAmmount;9;0;Create;True;0;0;False;0;False;0.65;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;44;-4571.748,1391.652;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;41;-4576.023,1695.382;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NegateNode;36;-4840.838,2016.053;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-5056.951,2178.297;Inherit;False;Property;_DissolveAmount2;DissolveAmount;1;0;Create;True;0;0;False;0;False;0;0.83;-0.5;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-4818.813,692.2272;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PannerNode;37;-6234.503,-2218.533;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;38;-5981.537,-2672.632;Inherit;True;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-5694.407,-2511.002;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;52;-6014.098,-2264.249;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;48;-4833.606,-2223.896;Inherit;False;1640.536;721.7322;Comment;8;113;95;87;85;77;76;64;63;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TFHCRemapNode;51;-4523.706,2014.251;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;53;-4281.652,1518.158;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-4682.142,852.9302;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;49;-5062.571,427.4104;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;61;-5569.099,-2321.249;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;62;-4455.364,555.913;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;65;-4811.029,-3574.59;Inherit;False;1187.951;569.4151;Colors;6;112;93;91;89;70;69;Colors;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-4756.345,-1818.353;Inherit;False;Property;_SpeadAmount2;SpeadAmount;2;0;Create;True;0;0;False;0;False;0.6030607;0.764;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-4650.062,-1661.738;Inherit;False;Constant;_DivideAmount2;DivideAmount;7;0;Create;True;0;0;False;0;False;1.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;56;-5670.51,-2784.247;Inherit;True;Property;_Mask2;Mask;10;0;Create;True;0;0;False;0;False;None;ed200a308c3598c489313a6dd109b644;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleAddOpNode;60;-4060.873,1824.546;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;76;-4328.406,-1755.164;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;68;-1995.425,287.0095;Inherit;False;1513.373;1226.981;;12;134;129;126;119;118;115;111;107;104;100;98;82;Corruption;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;74;-5130.616,-2678.033;Inherit;True;Property;_TextureSample3;Texture Sample 1;2;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;75;-3771.331,1755.511;Inherit;False;Gradient;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;70;-4754.063,-3524.59;Inherit;False;Property;_CorruptedColorDark2;Corrupted Color Dark;3;0;Create;True;0;0;False;0;False;0.05899113,0.02024741,0.1226415,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;69;-4761.029,-3317.34;Inherit;False;Property;_CorruptedColorLight2;Corrupted Color Light;4;0;Create;True;0;0;False;0;False;0,0,0,0;0.2904759,0.116723,0.4056604,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;73;-4061.985,383.037;Inherit;True;Property;_Noise2;Noise;13;0;Create;True;0;0;False;0;False;-1;None;759a45d2aa606de469cde71db0d2136c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TriplanarNode;77;-4811.547,-2138.313;Inherit;True;Spherical;World;False;Top Texture 2;_TopTexture2;white;-1;None;Mid Texture 2;_MidTexture2;white;-1;None;Bot Texture 2;_BotTexture2;white;-1;None;Triplanar Sampler;False;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;0.05,0.05;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;91;-4374.337,-3349.172;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-4308.926,-3120.175;Inherit;False;Constant;_Float2;Float 0;6;0;Create;True;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;84;-3725.341,652.0751;Inherit;False;CorruptNoise;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;87;-4211.016,-2173.896;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0.2;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StepOpNode;85;-4008.086,-1919.329;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;82;-1933.922,346.3115;Inherit;False;75;Gradient;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;98;-1718.501,351.0095;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;95;-3707.785,-2032.776;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PowerNode;93;-4103.059,-3466.347;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;100;-1918.482,441.1585;Inherit;False;84;CorruptNoise;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;112;-3858.08,-3328.171;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;-1552.501,337.0095;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;104;-1496.951,1311.989;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;107;-1945.425,562.9693;Inherit;False;75;Gradient;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;111;-1550.166,1190.53;Inherit;False;Property;_EdgeThickness;EdgeThickness;14;0;Create;True;0;0;False;0;False;1.15;0;0;1.15;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;113;-3428.069,-2136.33;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;78;-6654.742,-3714.652;Inherit;True;Property;_MainText1;MainText;0;0;Create;True;0;0;False;0;False;-1;None;913a141ab29a6e44d8a057986146914a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;117;-3081.606,-2539.743;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;118;-1245.984,1183.003;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;119;-1311.501,485.0093;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;128;-2771.299,-2515.403;Inherit;True;Corruptionfold;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;86;-6111.131,-3686.437;Inherit;False;MainTex;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;129;-1007.922,578.9586;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;126;-1019.886,864.4974;Inherit;False;Property;_EdgeColor3;EdgeColor;12;0;Create;True;0;0;False;0;False;0.5813924,0.2198736,0.764151,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;-717.0525,672.7402;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;133;-558.5411,-276.0019;Inherit;False;128;Corruptionfold;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;139;-687.4098,-29.55731;Inherit;True;86;MainTex;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;135;-4023.338,681.1669;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;137;-4399.563,817.0932;Inherit;False;Property;_NoiseScale;NoiseScale;5;0;Create;True;0;0;False;0;False;1.38,0.01;1.38,0.01;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;110;-3645.29,-2759.635;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.1226415,0.1226415,0.1226415,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;102;-3972.822,-2745.967;Inherit;False;86;MainTex;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;136;-283.3905,-14.82693;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;140;-364.7679,375.683;Inherit;True;Property;_TextureSample0;Texture Sample 0;16;0;Create;True;0;0;False;0;False;-1;None;64f05b46e84f74145962dbe69f40d29b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;ElderSkin;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;28;0;18;0
WireConnection;28;1;21;0
WireConnection;27;0;12;0
WireConnection;30;0;15;0
WireConnection;29;0;17;0
WireConnection;29;1;20;0
WireConnection;40;0;26;0
WireConnection;44;0;29;0
WireConnection;41;0;32;0
WireConnection;36;0;31;0
WireConnection;33;0;27;0
WireConnection;33;1;22;0
WireConnection;37;1;28;0
WireConnection;38;0;30;0
WireConnection;55;0;38;0
WireConnection;55;1;43;0
WireConnection;52;1;37;0
WireConnection;51;0;35;0
WireConnection;51;3;40;0
WireConnection;51;4;36;0
WireConnection;53;0;44;0
WireConnection;53;1;41;0
WireConnection;50;0;33;0
WireConnection;50;1;42;2
WireConnection;61;0;55;0
WireConnection;61;1;52;0
WireConnection;62;0;49;0
WireConnection;62;1;50;0
WireConnection;60;0;53;0
WireConnection;60;1;51;0
WireConnection;76;0;64;0
WireConnection;76;1;63;0
WireConnection;74;0;56;0
WireConnection;74;1;61;0
WireConnection;75;0;60;0
WireConnection;73;1;62;0
WireConnection;77;0;56;0
WireConnection;91;0;70;0
WireConnection;91;1;69;0
WireConnection;91;2;74;0
WireConnection;84;0;73;0
WireConnection;87;0;77;0
WireConnection;87;1;64;0
WireConnection;85;0;77;0
WireConnection;85;1;76;0
WireConnection;98;0;82;0
WireConnection;95;0;87;0
WireConnection;95;1;85;0
WireConnection;93;0;91;0
WireConnection;93;1;89;0
WireConnection;112;0;93;0
WireConnection;112;1;89;0
WireConnection;115;0;98;0
WireConnection;115;1;100;0
WireConnection;113;0;87;0
WireConnection;113;1;95;0
WireConnection;117;1;112;0
WireConnection;117;2;113;0
WireConnection;118;0;111;0
WireConnection;118;1;104;0
WireConnection;119;0;115;0
WireConnection;119;1;107;0
WireConnection;128;0;117;0
WireConnection;86;0;78;0
WireConnection;129;0;119;0
WireConnection;129;1;118;0
WireConnection;134;0;126;0
WireConnection;134;1;129;0
WireConnection;135;0;62;0
WireConnection;135;1;137;0
WireConnection;110;0;102;0
WireConnection;136;0;133;0
WireConnection;136;1;139;0
WireConnection;136;2;134;0
WireConnection;0;0;136;0
WireConnection;0;2;140;4
ASEEND*/
//CHKSM=19132FCA784F4D77CC389A1455E5FF4BD1643FE9