// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Enemy"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Float0("Float 0", Float) = 0
		_Color0("Color 0", Color) = (0.126344,0.01735494,0.2830189,1)
		_DissolveAmount("DissolveAmount", Range( 0 , 1)) = 0
		_EdgeThickness("EdgeThickness", Range( 0 , 1.15)) = 0
		_NoiseScrollSpeed("NoiseScrollSpeed", Float) = 0.34
		[Toggle]_ToggleSwitch0("Toggle Switch0", Float) = 1
		_EdgeColor("EdgeColor", Color) = (0.5813924,0.2198736,0.764151,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Float0;
		uniform float4 _Color0;
		uniform float4 _EdgeColor;
		uniform float _ToggleSwitch0;
		uniform float _DissolveAmount;
		uniform float _NoiseScrollSpeed;
		uniform float _EdgeThickness;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float3 ase_worldPos = i.worldPos;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 normalizeResult85 = normalize( (( _ToggleSwitch0 )?( ase_vertex3Pos ):( ase_worldPos )) );
			float3 normalizeResult92 = normalize( float3(1,0,0) );
			float dotResult93 = dot( normalizeResult85 , normalizeResult92 );
			float Gradient95 = ( dotResult93 + (-1.29 + (_DissolveAmount - 0.0) * (1.06 - -1.29) / (1.0 - 0.0)) );
			float3 normalizeResult75 = normalize( float3(0.76,1,0) );
			float simplePerlin2D67 = snoise( ( ase_vertex3Pos + ( ( normalizeResult75 * _NoiseScrollSpeed ) * _Time.y ) ).xy*float2( 1.38,0.01 ).x );
			simplePerlin2D67 = simplePerlin2D67*0.5 + 0.5;
			float Noise97 = simplePerlin2D67;
			float temp_output_104_0 = ( ( ( 1.0 - Gradient95 ) * Noise97 ) - Gradient95 );
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 temp_output_6_0 = ( saturate( ( tex2D( _TextureSample0, uv_TextureSample0 ) * _Float0 ) ) * _Color0 );
			c.rgb = temp_output_6_0.rgb;
			c.a = 1;
			clip( temp_output_104_0 - _DissolveAmount );
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 temp_output_6_0 = ( saturate( ( tex2D( _TextureSample0, uv_TextureSample0 ) * _Float0 ) ) * _Color0 );
			o.Albedo = temp_output_6_0.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 normalizeResult85 = normalize( (( _ToggleSwitch0 )?( ase_vertex3Pos ):( ase_worldPos )) );
			float3 normalizeResult92 = normalize( float3(1,0,0) );
			float dotResult93 = dot( normalizeResult85 , normalizeResult92 );
			float Gradient95 = ( dotResult93 + (-1.29 + (_DissolveAmount - 0.0) * (1.06 - -1.29) / (1.0 - 0.0)) );
			float3 normalizeResult75 = normalize( float3(0.76,1,0) );
			float simplePerlin2D67 = snoise( ( ase_vertex3Pos + ( ( normalizeResult75 * _NoiseScrollSpeed ) * _Time.y ) ).xy*float2( 1.38,0.01 ).x );
			simplePerlin2D67 = simplePerlin2D67*0.5 + 0.5;
			float Noise97 = simplePerlin2D67;
			float temp_output_104_0 = ( ( ( 1.0 - Gradient95 ) * Noise97 ) - Gradient95 );
			o.Emission = ( _EdgeColor * step( temp_output_104_0 , ( _EdgeThickness + (0.0 + (0.0 - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) ) ) ).rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows 

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
				float3 worldPos : TEXCOORD2;
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
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				UnityGI gi;
				UNITY_INITIALIZE_OUTPUT( UnityGI, gi );
				o.Alpha = LightingStandardCustomLighting( o, worldViewDir, gi ).a;
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
0;0;1920;1019;3911.104;-493.9301;3.671404;True;True
Node;AmplifyShaderEditor.CommentaryNode;98;-2642.591,1101.064;Inherit;False;1794.989;768.6633;;11;66;67;81;65;97;75;79;76;80;74;77;Fade;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;96;-2613.767,1984.221;Inherit;False;1698.852;1060.583;;15;91;90;92;89;84;94;83;93;85;82;88;68;87;86;95;Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;82;-2563.767,2205.4;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;74;-2592.591,1328.771;Inherit;False;Constant;_NoiseDirection;NoiseDirection;5;0;Create;True;0;0;False;0;False;0.76,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;83;-2560.162,2034.221;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ToggleSwitchNode;84;-2255.642,2095.485;Inherit;False;Property;_ToggleSwitch0;Toggle Switch0;6;0;Create;True;0;0;False;0;False;1;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;88;-2474.28,2737.903;Inherit;False;Constant;_LowerLimit;LowerLimit;7;0;Create;True;0;0;False;0;False;-1.06;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-2467.933,1573.581;Inherit;False;Property;_NoiseScrollSpeed;NoiseScrollSpeed;5;0;Create;True;0;0;False;0;False;0.34;0.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;87;-2468.875,2653.214;Inherit;False;Constant;_UpperLimit;UpperLimit;7;0;Create;True;0;0;False;0;False;1.29;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;86;-2258.495,2393.271;Inherit;False;Constant;_DissolveDirection;DissolveDirection;7;0;Create;True;0;0;False;0;False;1,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;75;-2361.299,1352.802;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NegateNode;89;-2211.203,2631.591;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-2443.533,2901.95;Inherit;False;Property;_DissolveAmount;DissolveAmount;3;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;90;-2227.42,2739.705;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;-2184.074,1415.881;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TimeNode;79;-2472.439,1690.727;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalizeNode;92;-1962.606,2419.036;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;85;-1958.329,2115.306;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;93;-1668.234,2241.812;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;91;-1910.286,2737.903;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;65;-2427.833,1151.064;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-2047.402,1576.584;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;94;-1447.455,2548.199;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;81;-1820.626,1279.567;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector2Node;66;-1764.825,1540.747;Inherit;False;Constant;_NoiseScale;NoiseScale;4;0;Create;True;0;0;False;0;False;1.38,0.01;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.NoiseGeneratorNode;67;-1426.773,1317.391;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;95;-1157.914,2479.164;Inherit;False;Gradient;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;99;-456.0623,1708.24;Inherit;False;95;Gradient;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;97;-1090.603,1377.463;Inherit;False;Noise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;102;-240.6414,1712.937;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;101;-440.6223,1803.086;Inherit;False;97;Noise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;-74.64136,1698.937;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;105;-19.09088,2673.917;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-72.30663,2552.458;Inherit;False;Property;_EdgeThickness;EdgeThickness;4;0;Create;True;0;0;False;0;False;0;0;0;1.15;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;100;-467.5651,1924.897;Inherit;False;95;Gradient;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;658.476,1527.44;Inherit;False;Property;_Float0;Float 0;1;0;Create;True;0;0;False;0;False;0;1.63;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;601.3797,1289.943;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;False;-1;None;88df869c6e9bc5c40b3b501f37578768;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;70;231.8755,2544.931;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;952.9799,1405.144;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;104;166.3586,1846.937;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;4;1095.276,1436.241;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;5;973.6761,1554.64;Inherit;False;Property;_Color0;Color 0;2;0;Create;True;0;0;False;0;False;0.126344,0.01735494,0.2830189,1;0.3058824,0.2745098,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;72;512.1511,2632.75;Inherit;False;Property;_EdgeColor;EdgeColor;7;0;Create;True;0;0;False;0;False;0.5813924,0.2198736,0.764151,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;71;524.1145,2347.211;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;814.9835,2440.993;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;1276.076,1466.64;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1461.727,2006.743;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;Enemy;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;5;True;True;0;True;Transparent;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;True;68;0;0;0;False;1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;84;0;83;0
WireConnection;84;1;82;0
WireConnection;75;0;74;0
WireConnection;89;0;87;0
WireConnection;90;0;88;0
WireConnection;76;0;75;0
WireConnection;76;1;77;0
WireConnection;92;0;86;0
WireConnection;85;0;84;0
WireConnection;93;0;85;0
WireConnection;93;1;92;0
WireConnection;91;0;68;0
WireConnection;91;3;89;0
WireConnection;91;4;90;0
WireConnection;80;0;76;0
WireConnection;80;1;79;2
WireConnection;94;0;93;0
WireConnection;94;1;91;0
WireConnection;81;0;65;0
WireConnection;81;1;80;0
WireConnection;67;0;81;0
WireConnection;67;1;66;0
WireConnection;95;0;94;0
WireConnection;97;0;67;0
WireConnection;102;0;99;0
WireConnection;103;0;102;0
WireConnection;103;1;101;0
WireConnection;70;0;69;0
WireConnection;70;1;105;0
WireConnection;2;0;1;0
WireConnection;2;1;3;0
WireConnection;104;0;103;0
WireConnection;104;1;100;0
WireConnection;4;0;2;0
WireConnection;71;0;104;0
WireConnection;71;1;70;0
WireConnection;73;0;72;0
WireConnection;73;1;71;0
WireConnection;6;0;4;0
WireConnection;6;1;5;0
WireConnection;0;0;6;0
WireConnection;0;2;73;0
WireConnection;0;10;104;0
WireConnection;0;13;6;0
ASEEND*/
//CHKSM=35DCDD250A548FD0DB318686C407CDAE7D98EB53