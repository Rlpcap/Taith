// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Corruption Aura"
{
	Properties
	{
		_CorruptedTex1("CorruptedTex", 2D) = "white" {}
		_SpeadAmount1("SpeadAmount", Range( 0 , 1)) = 0.6030607
		_DistortionAmmount1("DistortionAmmount", Range( 0 , 1)) = 0
		_ScrollSpeed1("ScrollSpeed", Range( 0 , 1)) = 0
		_CorruptedColorDark1("Corrupted Color Dark", Color) = (0.05899113,0.02024741,0.1226415,0)
		_CorruptedColorLight1("Corrupted Color Light", Color) = (0,0,0,0)
		_DistortionMap1("DistortionMap", 2D) = "white" {}
		_Mask1("Mask", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" }
		Cull Off
		AlphaToMask On
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

		uniform sampler2D _CorruptedTex1;
		uniform float4 _CorruptedTex1_ST;
		uniform float4 _CorruptedColorDark1;
		uniform float4 _CorruptedColorLight1;
		uniform sampler2D _Mask1;
		uniform sampler2D _DistortionMap1;
		uniform float4 _DistortionMap1_ST;
		uniform float _DistortionAmmount1;
		uniform float _ScrollSpeed1;
		uniform float _SpeadAmount1;


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
			float2 uv_CorruptedTex1 = i.uv_texcoord * _CorruptedTex1_ST.xy + _CorruptedTex1_ST.zw;
			float4 MainTex90 = tex2D( _CorruptedTex1, uv_CorruptedTex1 );
			float2 uv_DistortionMap1 = i.uv_texcoord * _DistortionMap1_ST.xy + _DistortionMap1_ST.zw;
			float2 panner67 = ( ( _Time.y * _ScrollSpeed1 ) * float2( 0,-1 ) + float2( 0,0 ));
			float2 uv_TexCoord69 = i.uv_texcoord + panner67;
			float4 lerpResult88 = lerp( _CorruptedColorDark1 , _CorruptedColorLight1 , tex2D( _Mask1, ( ( (UnpackNormal( tex2D( _DistortionMap1, uv_DistortionMap1 ) )).xy * _DistortionAmmount1 ) + uv_TexCoord69 ) ));
			float4 temp_cast_0 = (3.0).xxxx;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float4 triplanar77 = TriplanarSamplingSF( _Mask1, ase_worldPos, ase_worldNormal, 1.0, float2( 0.05,0.05 ), 1.0, 0 );
			float4 temp_cast_1 = (_SpeadAmount1).xxxx;
			float4 temp_output_89_0 = step( triplanar77 , temp_cast_1 );
			float4 temp_cast_2 = (_SpeadAmount1).xxxx;
			float4 temp_cast_3 = (( _SpeadAmount1 / 1.1 )).xxxx;
			float4 lerpResult102 = lerp( ( MainTex90 * float4( 0.1226415,0.1226415,0.1226415,0 ) ) , ( pow( lerpResult88 , temp_cast_0 ) * 3.0 ) , ( temp_output_89_0 + ( temp_output_89_0 - step( triplanar77 , temp_cast_3 ) ) ));
			float4 Corruptionfold54 = lerpResult102;
			o.Albedo = Corruptionfold54.rgb;
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
			AlphaToMask Off
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
0;0;1920;1019;108.5649;913.3369;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;51;-2743.974,-2753.412;Inherit;False;1862.961;907.1392;Distortion;13;82;73;72;71;69;68;67;66;61;60;57;56;55;Base Distortion;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;57;-2513.415,-2599.99;Inherit;True;Property;_DistortionMap1;DistortionMap;7;0;Create;True;0;0;False;0;False;None;45b7d34a07e91d14f8fd42ca2b072fe0;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-2693.974,-1961.272;Inherit;False;Property;_ScrollSpeed1;ScrollSpeed;4;0;Create;True;0;0;False;0;False;0;0.142;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;55;-2625.074,-2070.472;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-2353.374,-2019.772;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;61;-2192.059,-2531.096;Inherit;True;Property;_TextureSample1;Texture Sample 0;3;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;66;-1846.838,-2523.246;Inherit;True;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;67;-2099.803,-2069.147;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-1895.706,-2285.616;Inherit;False;Property;_DistortionAmmount1;DistortionAmmount;3;0;Create;True;0;0;False;0;False;0;0.848;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-1559.708,-2361.616;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;69;-1879.399,-2114.863;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;52;-698.9066,-2074.511;Inherit;False;1640.536;721.7322;Comment;8;101;93;89;86;83;77;75;74;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;73;-1434.4,-2171.863;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-591.9974,-1667.114;Inherit;False;Property;_SpeadAmount1;SpeadAmount;2;0;Create;True;0;0;False;0;False;0.6030607;0.595;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-515.3615,-1512.352;Inherit;False;Constant;_DivideAmount1;DivideAmount;7;0;Create;True;0;0;False;0;False;1.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;53;-676.3295,-3425.204;Inherit;False;1187.951;569.4151;Colors;6;98;94;88;85;84;81;Colors;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;72;-1535.811,-2634.861;Inherit;True;Property;_Mask1;Mask;8;0;Create;True;0;0;False;0;False;None;ed200a308c3598c489313a6dd109b644;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;83;-193.7066,-1605.779;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;82;-995.9167,-2528.647;Inherit;True;Property;_TextureSample2;Texture Sample 1;2;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;84;-626.3295,-3167.954;Inherit;False;Property;_CorruptedColorLight1;Corrupted Color Light;6;0;Create;True;0;0;False;0;False;0,0,0,0;0.8113208,0.5713484,0.264062,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;80;-2520.042,-3565.266;Inherit;True;Property;_CorruptedTex1;CorruptedTex;1;0;Create;True;0;0;False;0;False;-1;None;58c55be0bf5932b4a86b2e68c099c830;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TriplanarNode;77;-676.8475,-1988.927;Inherit;True;Spherical;World;False;Top Texture 2;_TopTexture2;white;-1;None;Mid Texture 2;_MidTexture2;white;-1;None;Bot Texture 2;_BotTexture2;white;-1;None;Triplanar Sampler;False;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;0.05,0.05;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;81;-619.3634,-3375.204;Inherit;False;Property;_CorruptedColorDark1;Corrupted Color Dark;5;0;Create;True;0;0;False;0;False;0.05899113,0.02024741,0.1226415,0;0.05899113,0.02024741,0.1226415,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;89;-76.31567,-2024.511;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0.2;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-174.2266,-2970.789;Inherit;False;Constant;_Float1;Float 0;6;0;Create;True;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;86;126.6135,-1769.943;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;88;-239.6376,-3199.786;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;90;-1976.431,-3537.052;Inherit;False;MainTex;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;96;161.8774,-2596.582;Inherit;False;90;MainTex;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;94;31.64136,-3316.961;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;93;426.9144,-1883.389;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;101;706.6305,-1986.944;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;489.4095,-2610.25;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.1226415,0.1226415,0.1226415,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;276.6204,-3178.785;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;102;1053.094,-2390.357;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;54;1363.4,-2366.018;Inherit;False;Corruptionfold;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;110;1051.475,-273.8339;Inherit;False;54;Corruptionfold;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1405.585,-283.5428;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Corruption Aura;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;5;True;True;0;True;Transparent;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;True;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;60;0;55;0
WireConnection;60;1;56;0
WireConnection;61;0;57;0
WireConnection;66;0;61;0
WireConnection;67;1;60;0
WireConnection;71;0;66;0
WireConnection;71;1;68;0
WireConnection;69;1;67;0
WireConnection;73;0;71;0
WireConnection;73;1;69;0
WireConnection;83;0;74;0
WireConnection;83;1;75;0
WireConnection;82;0;72;0
WireConnection;82;1;73;0
WireConnection;77;0;72;0
WireConnection;89;0;77;0
WireConnection;89;1;74;0
WireConnection;86;0;77;0
WireConnection;86;1;83;0
WireConnection;88;0;81;0
WireConnection;88;1;84;0
WireConnection;88;2;82;0
WireConnection;90;0;80;0
WireConnection;94;0;88;0
WireConnection;94;1;85;0
WireConnection;93;0;89;0
WireConnection;93;1;86;0
WireConnection;101;0;89;0
WireConnection;101;1;93;0
WireConnection;99;0;96;0
WireConnection;98;0;94;0
WireConnection;98;1;85;0
WireConnection;102;0;99;0
WireConnection;102;1;98;0
WireConnection;102;2;101;0
WireConnection;54;0;102;0
WireConnection;0;0;110;0
ASEEND*/
//CHKSM=46778685A2A2BFF54DDFC0475004E53321989D44