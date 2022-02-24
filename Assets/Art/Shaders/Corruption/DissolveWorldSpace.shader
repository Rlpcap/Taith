// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DissolveWorldSpace"
{
	Properties
	{
		_MainText("MainText", 2D) = "white" {}
		_DissolveAmount1("DissolveAmount", Range( -0.5 , 1.5)) = 0
		_enemyPos("enemyPos", Vector) = (0,0,0,0)
		_radius("enemyRadius", Float) = 0
		_IceMudLerp("IceMudLerp", Range( 0 , 1)) = 0
		_Ice("Ice", 2D) = "white" {}
		_Mud("Mud", 2D) = "white" {}
		_SpeadAmount1("SpeadAmount", Range( 0 , 1)) = 0.6030607
		_CorruptedColorDark1("Corrupted Color Dark", Color) = (0.05899113,0.02024741,0.1226415,0)
		_CorruptedColorLight1("Corrupted Color Light", Color) = (0,0,0,0)
		_FallOff1("Fall Off", Float) = 0
		[Toggle]_ToggleSwitch1("Toggle Switch0", Float) = 1
		_DistortionMap1("DistortionMap", 2D) = "white" {}
		_IceTiling1("Ice Tiling", Float) = 1
		_Mask1("Mask", 2D) = "white" {}
		_IceTiling2("Ice Tiling", Float) = 1
		_EdgeColor2("EdgeColor", Color) = (0.5813924,0.2198736,0.764151,0)
		_EdgeColor1("EdgeColor", Color) = (0.5813924,0.2198736,0.764151,0)
		_Noise("Noise", 2D) = "white" {}
		_Noise1("Noise", 2D) = "white" {}
		_playerPos("playerPos", Vector) = (0,0,0,0)
		_playerRadius("playerRadius", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
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

		uniform sampler2D _MainText;
		uniform float4 _MainText_ST;
		uniform float4 _CorruptedColorDark1;
		uniform float4 _CorruptedColorLight1;
		uniform sampler2D _Mask1;
		uniform sampler2D _DistortionMap1;
		uniform float4 _DistortionMap1_ST;
		uniform float _SpeadAmount1;
		uniform sampler2D _Ice;
		uniform float _IceTiling1;
		uniform sampler2D _Mud;
		uniform float _IceTiling2;
		uniform float _IceMudLerp;
		uniform float4 _EdgeColor1;
		uniform float3 _enemyPos;
		uniform float _radius;
		uniform float _FallOff1;
		uniform sampler2D _Noise;
		uniform float4 _Noise_ST;
		uniform float3 _playerPos;
		uniform float _playerRadius;
		uniform float4 _EdgeColor2;
		uniform float _ToggleSwitch1;
		uniform float _DissolveAmount1;
		uniform sampler2D _Noise1;


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
			float2 uv_MainText = i.uv_texcoord * _MainText_ST.xy + _MainText_ST.zw;
			float4 MainTex146 = tex2D( _MainText, uv_MainText );
			float2 uv_DistortionMap1 = i.uv_texcoord * _DistortionMap1_ST.xy + _DistortionMap1_ST.zw;
			float2 panner126 = ( ( _Time.y * 0.3058824 ) * float2( 0,-1 ) + float2( 0,0 ));
			float2 uv_TexCoord129 = i.uv_texcoord + panner126;
			float4 lerpResult143 = lerp( _CorruptedColorDark1 , _CorruptedColorLight1 , tex2D( _Mask1, ( ( (UnpackNormal( tex2D( _DistortionMap1, uv_DistortionMap1 ) )).xy * 0.65 ) + uv_TexCoord129 ) ));
			float4 temp_cast_0 = (3.0).xxxx;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float4 triplanar137 = TriplanarSamplingSF( _Mask1, ase_worldPos, ase_worldNormal, 1.0, float2( 0.05,0.05 ), 1.0, 0 );
			float4 temp_cast_1 = (_SpeadAmount1).xxxx;
			float4 temp_output_145_0 = step( triplanar137 , temp_cast_1 );
			float4 temp_cast_2 = (_SpeadAmount1).xxxx;
			float4 temp_cast_3 = (( _SpeadAmount1 / 1.1 )).xxxx;
			float4 lerpResult153 = lerp( ( MainTex146 * float4( 0.1226415,0.1226415,0.1226415,0 ) ) , ( pow( lerpResult143 , temp_cast_0 ) * 3.0 ) , ( temp_output_145_0 + ( temp_output_145_0 - step( triplanar137 , temp_cast_3 ) ) ));
			float4 Corruptionfold154 = lerpResult153;
			float4 color186 = IsGammaSpace() ? float4(0.7122642,0.8911937,1,0) : float4(0.4656525,0.7700986,1,0);
			float2 appendResult60 = (float2(_IceTiling1 , _IceTiling1));
			float4 triplanar62 = TriplanarSamplingSF( _Ice, ase_worldPos, ase_worldNormal, 1.0, appendResult60, 1.0, 0 );
			float2 appendResult68 = (float2(_IceTiling2 , _IceTiling2));
			float4 triplanar69 = TriplanarSamplingSF( _Mud, ase_worldPos, ase_worldNormal, 1.0, appendResult68, 1.0, 0 );
			float4 lerpResult64 = lerp( triplanar62 , triplanar69 , _IceMudLerp);
			float Mask28 = saturate( pow( ( distance( _enemyPos , ase_worldPos ) / _radius ) , _FallOff1 ) );
			float2 uv_Noise = i.uv_texcoord * _Noise_ST.xy + _Noise_ST.zw;
			float4 Noise48 = tex2D( _Noise, uv_Noise );
			float4 temp_cast_7 = (Mask28).xxxx;
			float4 lerpResult33 = lerp( lerpResult64 , MainTex146 , ( _EdgeColor1 * step( ( ( ( 1.0 - Mask28 ) * Noise48 ) - temp_cast_7 ) , float4( 0,0,0,0 ) ) ));
			float4 color179 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float PlayerMask168 = saturate( pow( ( distance( _playerPos , ase_worldPos ) / _playerRadius ) , 3.5 ) );
			float4 temp_cast_8 = (PlayerMask168).xxxx;
			float4 lerpResult182 = lerp( ( color186 * triplanar62 ) , lerpResult33 , ( color179 * step( ( ( ( 1.0 - PlayerMask168 ) * Noise48 ) - temp_cast_8 ) , float4( 0,0,0,0 ) ) ));
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 normalizeResult85 = normalize( (( _ToggleSwitch1 )?( ase_vertex3Pos ):( ase_worldPos )) );
			float3 normalizeResult89 = normalize( float3(0,1,0) );
			float dotResult93 = dot( normalizeResult85 , normalizeResult89 );
			float Gradient97 = ( dotResult93 + (-1.29 + (_DissolveAmount1 - 0.0) * (1.06 - -1.29) / (1.0 - 0.0)) );
			float3 normalizeResult77 = normalize( float3(0.76,1,0) );
			float3 temp_output_95_0 = ( ase_vertex3Pos + ( ( normalizeResult77 * 0.27 ) * _Time.y ) );
			float4 CorruptNoise99 = tex2D( _Noise1, temp_output_95_0.xy );
			float4 temp_cast_10 = (Gradient97).xxxx;
			float4 temp_cast_11 = (( 0.0 + (0.0 + (0.0 - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) )).xxxx;
			float4 lerpResult118 = lerp( Corruptionfold154 , lerpResult182 , ( _EdgeColor2 * step( ( ( ( 1.0 - Gradient97 ) * CorruptNoise99 ) - temp_cast_10 ) , temp_cast_11 ) ));
			o.Albedo = lerpResult118.rgb;
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
0;0;1920;1019;1755.581;2070.55;2.735487;True;True
Node;AmplifyShaderEditor.CommentaryNode;70;-2898.752,-1190.558;Inherit;False;1894.989;923.7788;Comment;11;21;20;22;23;24;25;26;27;28;48;63;Dissolve Ice/Mud;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;117;-2961.897,21.99636;Inherit;False;1971.741;2153.503;;2;72;71;Corruption;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector3Node;21;-2587.558,-1140.557;Inherit;False;Property;_enemyPos;enemyPos;2;0;Create;True;0;0;False;0;False;0,0,0;64.26744,4.242745,-7.295326;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;20;-2555.386,-949.1597;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;158;-2582.449,-1565.616;Inherit;False;Property;_playerPos;playerPos;20;0;Create;True;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;119;-4487.657,-3114.403;Inherit;False;1862.961;907.1392;Distortion;13;141;133;132;130;129;127;126;125;124;123;122;121;120;Base Distortion;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;72;-2836.168,1048.962;Inherit;False;1698.852;1060.583;;15;97;96;93;91;89;88;86;85;83;82;81;79;78;76;74;Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;71;-2886.312,165.8053;Inherit;False;1794.989;768.6633;;12;99;98;95;94;92;90;87;84;80;77;75;157;Fade;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-2166.419,-933.3422;Inherit;False;Property;_radius;enemyRadius;3;0;Create;False;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;22;-2307.677,-1035.111;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;159;-2596.258,-1406.074;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;75;-2836.312,393.512;Inherit;False;Constant;_NoiseDirection1;NoiseDirection;5;0;Create;True;0;0;False;0;False;0.76,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;162;-2384.556,-1360.053;Inherit;False;Property;_playerRadius;playerRadius;21;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1909.933,-926.9569;Inherit;False;Property;_FallOff1;Fall Off;10;0;Create;True;0;0;False;0;False;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;122;-4257.098,-2960.98;Inherit;True;Property;_DistortionMap1;DistortionMap;12;0;Create;True;0;0;False;0;False;None;45b7d34a07e91d14f8fd42ca2b072fe0;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;24;-1989.719,-1035.042;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;76;-2782.563,1098.962;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleTimeNode;120;-4368.757,-2431.463;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;160;-2355.41,-1487.38;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;74;-2786.168,1270.141;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;121;-4437.657,-2322.263;Inherit;False;Constant;_ScrollSpeed;ScrollSpeed;7;0;Create;True;0;0;False;0;False;0.3058824;0.378;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-2711.654,638.322;Inherit;False;Constant;_NoiseScrollSpeed;NoiseScrollSpeed;11;0;Create;True;0;0;False;0;False;0.27;0.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;164;-2113.024,-1350.849;Inherit;False;Constant;_FallOff;Fall Off;22;0;Create;True;0;0;False;0;False;3.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;26;-1753.933,-1035.957;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;161;-2140.639,-1485.846;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-2691.276,1717.956;Inherit;False;Constant;_UpperLimit1;UpperLimit;7;0;Create;True;0;0;False;0;False;1.29;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;77;-2605.02,417.543;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;123;-4097.057,-2380.763;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;82;-2478.043,1160.226;Inherit;False;Property;_ToggleSwitch1;Toggle Switch0;11;0;Create;True;0;0;False;0;False;1;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;124;-3935.742,-2892.087;Inherit;True;Property;_TextureSample1;Texture Sample 0;3;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;81;-2696.681,1802.646;Inherit;False;Constant;_LowerLimit1;LowerLimit;7;0;Create;True;0;0;False;0;False;-1.06;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;78;-2480.896,1458.013;Inherit;False;Constant;_DissolveDirection1;DissolveDirection;7;0;Create;True;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-2427.795,480.622;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;27;-1548.794,-1036.552;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-2665.934,1966.692;Inherit;False;Property;_DissolveAmount1;DissolveAmount;1;0;Create;True;0;0;False;0;False;0;1.5;-0.5;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;86;-2449.821,1804.448;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;126;-3843.486,-2430.138;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;125;-3590.52,-2884.237;Inherit;True;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;163;-1924.334,-1484.313;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;88;-2433.604,1696.333;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;89;-2185.006,1483.777;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TimeNode;87;-2716.16,755.468;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;127;-3640.622,-2646.607;Inherit;False;Constant;_DistortionAmmount;DistortionAmmount;7;0;Create;True;0;0;False;0;False;0.65;0.65;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;85;-2180.73,1180.047;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;28;-1363.493,-1042.992;Inherit;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;63;-1797.061,-644.6658;Inherit;True;Property;_Noise;Noise;18;0;Create;True;0;0;False;0;False;-1;None;759a45d2aa606de469cde71db0d2136c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;166;-1709.566,-1482.779;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;128;-2442.589,-2435.501;Inherit;False;1640.536;721.7322;Comment;8;152;149;145;142;139;137;135;134;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;92;-2671.554,215.8053;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;-2291.124,641.3249;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;91;-2132.688,1802.646;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;129;-3623.081,-2475.854;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;93;-1890.635,1306.553;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;184;-663.9258,-160.276;Inherit;False;1577.205;629.7732;;9;49;50;52;53;51;54;55;57;56;Enemy Ice/Dirt;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;-3303.39,-2722.607;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;132;-3279.493,-2995.852;Inherit;True;Property;_Mask1;Mask;14;0;Create;True;0;0;False;0;False;None;ed200a308c3598c489313a6dd109b644;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;48;-1353.901,-609.856;Inherit;False;Noise;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;185;203.1145,592.3724;Inherit;False;1361.37;485.4672;;9;171;177;174;170;173;172;169;176;179;Player Ice;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;168;-1522.41,-1488.915;Inherit;False;PlayerMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;96;-1669.855,1612.941;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;133;-3178.082,-2532.854;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;95;-2064.347,344.3079;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;135;-2259.044,-1873.343;Inherit;False;Constant;_DivideAmount1;DivideAmount;7;0;Create;True;0;0;False;0;False;1.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;134;-2335.68,-2028.105;Inherit;False;Property;_SpeadAmount1;SpeadAmount;7;0;Create;True;0;0;False;0;False;0.6030607;0.542;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;131;-2420.012,-3786.195;Inherit;False;1187.951;569.4151;Colors;6;151;148;144;143;140;138;Colors;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;49;-613.9258,-92.97494;Inherit;False;28;Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;169;254.7234,642.3724;Inherit;False;168;PlayerMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;183;697.8466,1228.62;Inherit;False;1513.373;1226.981;;12;100;101;102;107;106;104;105;111;109;114;113;116;Corruption;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;140;-2370.012,-3528.945;Inherit;False;Property;_CorruptedColorLight1;Corrupted Color Light;9;0;Create;True;0;0;False;0;False;0,0,0,0;0.4766821,0.4466002,0.490566,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;138;-2363.046,-3736.195;Inherit;False;Property;_CorruptedColorDark1;Corrupted Color Dark;8;0;Create;True;0;0;False;0;False;0.05899113,0.02024741,0.1226415,0;0.05899113,0.02024741,0.1226415,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;52;-388.9271,-110.276;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;50;-603.4477,-5.388641;Inherit;False;48;Noise;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;157;-1670.968,171.4319;Inherit;True;Property;_Noise1;Noise;19;0;Create;True;0;0;False;0;False;-1;None;759a45d2aa606de469cde71db0d2136c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;141;-2739.599,-2889.638;Inherit;True;Property;_TextureSample2;Texture Sample 1;2;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;97;-1380.314,1543.906;Inherit;False;Gradient;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;139;-1937.389,-1966.769;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;137;-2420.53,-2349.918;Inherit;True;Spherical;World;False;Top Texture 2;_TopTexture2;white;-1;None;Mid Texture 2;_MidTexture2;white;-1;None;Bot Texture 2;_BotTexture2;white;-1;None;Triplanar Sampler;False;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;0.05,0.05;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;136;-4263.725,-3926.257;Inherit;True;Property;_MainText;MainText;0;0;Create;True;0;0;False;0;False;-1;None;116bd1fa44352be4ab1d4dcce10b516e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;51;-379.5128,139.6759;Inherit;False;28;Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;170;500.8885,647.1992;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;172;253.1145,796.829;Inherit;False;48;Noise;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;100;759.3495,1287.922;Inherit;False;97;Gradient;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-203.2933,-90.78426;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;99;-1334.324,440.47;Inherit;False;CorruptNoise;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;142;-1617.069,-2130.934;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;146;-3720.114,-3898.042;Inherit;False;MainTex;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;145;-1819.998,-2385.501;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0.2;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;58;87.71027,-1004.46;Inherit;False;Property;_IceTiling1;Ice Tiling;13;0;Create;True;0;0;False;0;False;1;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;144;-1917.909,-3331.78;Inherit;False;Constant;_Float1;Float 0;6;0;Create;True;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-6.227267,-600.0629;Inherit;False;Property;_IceTiling2;Ice Tiling;15;0;Create;True;0;0;False;0;False;1;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;143;-1983.32,-3560.777;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;59;128.4662,-1250.638;Inherit;True;Property;_Ice;Ice;5;0;Create;True;0;0;False;0;False;None;c65b114eeb365e54a9d77845008e81f4;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PowerNode;148;-1712.041,-3677.952;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;68;195.4533,-591.3334;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;149;-1316.768,-2244.381;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;60;289.3913,-995.731;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;67;34.52877,-846.2407;Inherit;True;Property;_Mud;Mud;6;0;Create;True;0;0;False;0;False;None;f885b453cfec7a74eb865221f4e11a88;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.OneMinusNode;101;974.7706,1292.62;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;174;660.1722,793.6112;Inherit;False;168;PlayerMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;102;774.7894,1382.769;Inherit;False;99;CorruptNoise;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;54;37.70632,57.21555;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;147;-1581.805,-2957.572;Inherit;False;146;MainTex;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;171;708.4397,648.8081;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;105;1196.321,2253.6;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;62;451.0703,-1061.722;Inherit;True;Spherical;World;False;Top Texture 0;_TopTexture0;white;0;None;Mid Texture 0;_MidTexture0;white;-1;None;Bot Texture 0;_BotTexture0;white;-1;None;Triplanar Sampler;False;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;57;401.757,262.4972;Inherit;False;Property;_EdgeColor1;EdgeColor;17;0;Create;True;0;0;False;0;False;0.5813924,0.2198736,0.764151,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;104;747.8466,1504.58;Inherit;False;97;Gradient;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;173;898.2926,648.8081;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TriplanarNode;69;357.132,-657.3245;Inherit;True;Spherical;World;False;Top Texture 1;_TopTexture1;white;0;None;Mid Texture 1;_MidTexture1;white;-1;None;Bot Texture 1;_BotTexture1;white;-1;None;Triplanar Sampler;False;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;150;-1254.273,-2971.24;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.1226415,0.1226415,0.1226415,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;107;1143.106,2132.141;Inherit;False;Constant;_EdgeThickness;EdgeThickness;10;0;Create;True;0;0;False;0;False;0;0;0;1.15;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;151;-1467.062,-3539.776;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;152;-1037.052,-2347.935;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;65;405.4973,-412.8808;Inherit;False;Property;_IceMudLerp;IceMudLerp;4;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;106;1140.771,1278.62;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;55;397.2877,34.05615;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;153;-690.5883,-2751.348;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;109;1447.288,2124.614;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;111;1381.771,1426.62;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;179;1057.576,870.8396;Inherit;False;Constant;_PlayerEdgeColor;PlayerEdgeColor;22;0;Create;True;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;678.2787,26.13126;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;156;944.3459,-206.4332;Inherit;False;146;MainTex;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;64;966.7682,-762.17;Inherit;True;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;186;1196.072,-1174.422;Inherit;False;Constant;_Color0;Color 0;22;0;Create;True;0;0;False;0;False;0.7122642,0.8911937,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;176;1096.19,653.635;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;114;1673.386,1806.108;Inherit;False;Property;_EdgeColor2;EdgeColor;16;0;Create;True;0;0;False;0;False;0.5813924,0.2198736,0.764151,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;33;1181.916,-222.2683;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;154;-380.282,-2727.008;Inherit;True;Corruptionfold;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;113;1685.35,1520.569;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;177;1329.484,679.3776;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;187;1522.798,-945.4917;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;182;1696.723,-55.07269;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;155;1812.152,-383.9538;Inherit;False;154;Corruptionfold;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;116;1976.219,1614.351;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;98;-1632.321,469.5617;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;118;2107.627,-226.4321;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;94;-2008.546,605.488;Inherit;False;Constant;_NoiseScale1;NoiseScale;4;0;Create;True;0;0;False;0;False;1.38,0.01;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2391.107,7.20437;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;DissolveWorldSpace;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;22;0;21;0
WireConnection;22;1;20;0
WireConnection;24;0;22;0
WireConnection;24;1;23;0
WireConnection;160;0;158;0
WireConnection;160;1;159;0
WireConnection;26;0;24;0
WireConnection;26;1;25;0
WireConnection;161;0;160;0
WireConnection;161;1;162;0
WireConnection;77;0;75;0
WireConnection;123;0;120;0
WireConnection;123;1;121;0
WireConnection;82;0;76;0
WireConnection;82;1;74;0
WireConnection;124;0;122;0
WireConnection;84;0;77;0
WireConnection;84;1;80;0
WireConnection;27;0;26;0
WireConnection;86;0;81;0
WireConnection;126;1;123;0
WireConnection;125;0;124;0
WireConnection;163;0;161;0
WireConnection;163;1;164;0
WireConnection;88;0;79;0
WireConnection;89;0;78;0
WireConnection;85;0;82;0
WireConnection;28;0;27;0
WireConnection;166;0;163;0
WireConnection;90;0;84;0
WireConnection;90;1;87;2
WireConnection;91;0;83;0
WireConnection;91;3;88;0
WireConnection;91;4;86;0
WireConnection;129;1;126;0
WireConnection;93;0;85;0
WireConnection;93;1;89;0
WireConnection;130;0;125;0
WireConnection;130;1;127;0
WireConnection;48;0;63;0
WireConnection;168;0;166;0
WireConnection;96;0;93;0
WireConnection;96;1;91;0
WireConnection;133;0;130;0
WireConnection;133;1;129;0
WireConnection;95;0;92;0
WireConnection;95;1;90;0
WireConnection;52;0;49;0
WireConnection;157;1;95;0
WireConnection;141;0;132;0
WireConnection;141;1;133;0
WireConnection;97;0;96;0
WireConnection;139;0;134;0
WireConnection;139;1;135;0
WireConnection;137;0;132;0
WireConnection;170;0;169;0
WireConnection;53;0;52;0
WireConnection;53;1;50;0
WireConnection;99;0;157;0
WireConnection;142;0;137;0
WireConnection;142;1;139;0
WireConnection;146;0;136;0
WireConnection;145;0;137;0
WireConnection;145;1;134;0
WireConnection;143;0;138;0
WireConnection;143;1;140;0
WireConnection;143;2;141;0
WireConnection;148;0;143;0
WireConnection;148;1;144;0
WireConnection;68;0;66;0
WireConnection;68;1;66;0
WireConnection;149;0;145;0
WireConnection;149;1;142;0
WireConnection;60;0;58;0
WireConnection;60;1;58;0
WireConnection;101;0;100;0
WireConnection;54;0;53;0
WireConnection;54;1;51;0
WireConnection;171;0;170;0
WireConnection;171;1;172;0
WireConnection;62;0;59;0
WireConnection;62;3;60;0
WireConnection;173;0;171;0
WireConnection;173;1;174;0
WireConnection;69;0;67;0
WireConnection;69;3;68;0
WireConnection;150;0;147;0
WireConnection;151;0;148;0
WireConnection;151;1;144;0
WireConnection;152;0;145;0
WireConnection;152;1;149;0
WireConnection;106;0;101;0
WireConnection;106;1;102;0
WireConnection;55;0;54;0
WireConnection;153;0;150;0
WireConnection;153;1;151;0
WireConnection;153;2;152;0
WireConnection;109;0;107;0
WireConnection;109;1;105;0
WireConnection;111;0;106;0
WireConnection;111;1;104;0
WireConnection;56;0;57;0
WireConnection;56;1;55;0
WireConnection;64;0;62;0
WireConnection;64;1;69;0
WireConnection;64;2;65;0
WireConnection;176;0;173;0
WireConnection;33;0;64;0
WireConnection;33;1;156;0
WireConnection;33;2;56;0
WireConnection;154;0;153;0
WireConnection;113;0;111;0
WireConnection;113;1;109;0
WireConnection;177;0;179;0
WireConnection;177;1;176;0
WireConnection;187;0;186;0
WireConnection;187;1;62;0
WireConnection;182;0;187;0
WireConnection;182;1;33;0
WireConnection;182;2;177;0
WireConnection;116;0;114;0
WireConnection;116;1;113;0
WireConnection;98;0;95;0
WireConnection;98;1;94;0
WireConnection;118;0;155;0
WireConnection;118;1;182;0
WireConnection;118;2;116;0
WireConnection;0;0;118;0
ASEEND*/
//CHKSM=0181999B7E74DA481B0A424ECB506E860FA63CAA