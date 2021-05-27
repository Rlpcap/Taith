// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Rock"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_UpNode("UpNode", Range( 0 , 1)) = 0.5
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_GrassColor("Grass Color", Color) = (0.2824891,0.8301887,0.09789959,0)
		_DissolveAmount("DissolveAmount", Range( -0.5 , 1)) = 0
		_RockColor("Rock Color", Color) = (0.2824891,0.8301887,0.09789959,0)
		_EdgeThickness1("EdgeThickness", Range( 0 , 1.15)) = 0
		_NoiseScrollSpeed1("NoiseScrollSpeed", Float) = 0.34
		[Toggle]_ToggleSwitch1("Toggle Switch0", Float) = 1
		_EdgeColor1("EdgeColor", Color) = (0.5813924,0.2198736,0.764151,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
			float3 worldPos;
		};

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float4 _RockColor;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform float4 _GrassColor;
		uniform float _UpNode;
		uniform float4 _EdgeColor1;
		uniform float _ToggleSwitch1;
		uniform float _DissolveAmount;
		uniform float _NoiseScrollSpeed1;
		uniform float _EdgeThickness1;
		uniform float _Cutoff = 0.5;


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


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float3 temp_cast_0 = (_UpNode).xxx;
			float3 ase_worldNormal = i.worldNormal;
			float dotResult3 = dot( temp_cast_0 , ase_worldNormal );
			float clampResult9 = clamp( dotResult3 , 0.0 , 1.0 );
			float4 lerpResult7 = lerp( ( tex2D( _TextureSample0, uv_TextureSample0 ).r * _RockColor ) , ( tex2D( _TextureSample1, uv_TextureSample1 ).r * _GrassColor ) , clampResult9);
			o.Albedo = lerpResult7.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 normalizeResult30 = normalize( (( _ToggleSwitch1 )?( ase_vertex3Pos ):( ase_worldPos )) );
			float3 normalizeResult28 = normalize( float3(1,0,0) );
			float dotResult36 = dot( normalizeResult30 , normalizeResult28 );
			float Gradient42 = ( dotResult36 + (-1.29 + (_DissolveAmount - 0.0) * (1.06 - -1.29) / (1.0 - 0.0)) );
			float3 normalizeResult22 = normalize( float3(0.76,1,0) );
			float simplePerlin2D43 = snoise( ( ase_vertex3Pos + ( ( normalizeResult22 * _NoiseScrollSpeed1 ) * _Time.y ) ).xy*float2( 1.38,0.01 ).x );
			simplePerlin2D43 = simplePerlin2D43*0.5 + 0.5;
			float Noise44 = simplePerlin2D43;
			float temp_output_50_0 = ( ( ( 1.0 - Gradient42 ) * Noise44 ) - Gradient42 );
			o.Emission = ( _EdgeColor1 * step( temp_output_50_0 , ( _EdgeThickness1 + (0.0 + (0.0 - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) ) ) ).rgb;
			float temp_output_12_0 = 0.0;
			o.Metallic = temp_output_12_0;
			o.Smoothness = temp_output_12_0;
			o.Alpha = 1;
			clip( temp_output_50_0 - _Cutoff );
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
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
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
				o.worldNormal = worldNormal;
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
				surfIN.worldNormal = IN.worldNormal;
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
0;0;1920;1019;4431.163;-1384.802;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;17;-3876.931,517.7788;Inherit;False;1794.989;768.6633;;11;44;43;40;39;37;35;32;29;25;22;20;Fade;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;18;-3860.931,1397.779;Inherit;False;1698.852;1060.583;;15;42;41;38;36;34;33;31;30;28;27;26;24;23;21;19;Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;19;-3796.931,1621.779;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;20;-3828.931,741.7788;Inherit;False;Constant;_NoiseDirection1;NoiseDirection;5;0;Create;True;0;0;False;0;False;0.76,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;21;-3796.931,1445.779;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ToggleSwitchNode;27;-3492.931,1509.779;Inherit;False;Property;_ToggleSwitch1;Toggle Switch0;9;0;Create;True;0;0;False;0;False;1;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-3716.931,2149.779;Inherit;False;Constant;_LowerLimit1;LowerLimit;7;0;Create;True;0;0;False;0;False;-1.06;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-3700.931,981.7792;Inherit;False;Property;_NoiseScrollSpeed1;NoiseScrollSpeed;8;0;Create;True;0;0;False;0;False;0.34;0.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-3716.931,2069.779;Inherit;False;Constant;_UpperLimit1;UpperLimit;7;0;Create;True;0;0;False;0;False;1.29;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;23;-3492.931,1797.779;Inherit;False;Constant;_DissolveDirection1;DissolveDirection;7;0;Create;True;0;0;False;0;False;1,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;22;-3604.931,757.7788;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NegateNode;34;-3460.931,2149.779;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-3428.931,821.7788;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;28;-3204.931,1829.779;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-3682.931,2309.779;Inherit;False;Property;_DissolveAmount;DissolveAmount;5;0;Create;True;0;0;False;0;False;0;0;-0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;30;-3204.931,1525.779;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TimeNode;32;-3716.931,1109.779;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NegateNode;33;-3444.931,2037.779;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-3284.931,981.7792;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;38;-3156.931,2149.779;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;35;-3668.931,565.7787;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;36;-2900.931,1653.779;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-3060.931,693.7787;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;41;-2692.931,1957.779;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;39;-3012.931,949.7791;Inherit;False;Constant;_NoiseScale1;NoiseScale;4;0;Create;True;0;0;False;0;False;1.38,0.01;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;42;-2404.932,1893.779;Inherit;False;Gradient;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;43;-2660.931,725.7788;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;45;-1452.995,850.0188;Inherit;False;42;Gradient;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;44;-2324.932,789.7788;Inherit;False;Noise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;47;-1437.555,944.8651;Inherit;False;44;Noise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;46;-1237.574,854.7158;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;2;-1432.689,521.3315;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;15;-1494,-884.604;Inherit;False;610.0786;500.104;;3;5;14;13;Rock;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1455.089,397.7315;Inherit;False;Property;_UpNode;UpNode;2;0;Create;True;0;0;False;0;False;0.5;0.2411765;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;16;-1439,-331.4;Inherit;False;624.2151;444.3994;;3;6;10;11;Moss;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;48;-1464.497,1066.676;Inherit;False;42;Gradient;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-1069.239,1694.237;Inherit;False;Property;_EdgeThickness1;EdgeThickness;7;0;Create;True;0;0;False;0;False;0;0;0;1.15;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;51;-1016.023,1815.696;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-1071.573,840.7158;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-1346.384,-94.00064;Inherit;False;Property;_GrassColor;Grass Color;4;0;Create;True;0;0;False;0;False;0.2824891,0.8301887,0.09789959,0;0.57603,1,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-1389,-281.4;Inherit;True;Property;_TextureSample1;Texture Sample 1;3;0;Create;True;0;0;False;0;False;-1;None;57833696e0ffd3846b7e3da5750a8c03;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;3;-1107.689,427.3315;Inherit;True;2;0;FLOAT;1;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;13;-1411.721,-834.604;Inherit;False;Property;_RockColor;Rock Color;6;0;Create;True;0;0;False;0;False;0.2824891,0.8301887,0.09789959,0;0.3962264,0.1625367,0.02803488,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-765.0567,1686.71;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-1444,-614.4999;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;False;-1;None;0f601c106ae1f6b43a8d0509d8250271;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;50;-830.5736,988.7162;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-1118.921,-789.104;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-1049.785,-201.7006;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;54;-472.8177,1488.99;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;55;-484.781,1774.529;Inherit;False;Property;_EdgeColor1;EdgeColor;10;0;Create;True;0;0;False;0;False;0.5813924,0.2198736,0.764151,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;9;-852.5565,363.2976;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-181.9487,1582.772;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-182.5211,117.646;Inherit;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;7;-499.0057,-143.2316;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Rock;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;27;0;21;0
WireConnection;27;1;19;0
WireConnection;22;0;20;0
WireConnection;34;0;26;0
WireConnection;29;0;22;0
WireConnection;29;1;25;0
WireConnection;28;0;23;0
WireConnection;30;0;27;0
WireConnection;33;0;24;0
WireConnection;37;0;29;0
WireConnection;37;1;32;2
WireConnection;38;0;31;0
WireConnection;38;3;33;0
WireConnection;38;4;34;0
WireConnection;36;0;30;0
WireConnection;36;1;28;0
WireConnection;40;0;35;0
WireConnection;40;1;37;0
WireConnection;41;0;36;0
WireConnection;41;1;38;0
WireConnection;42;0;41;0
WireConnection;43;0;40;0
WireConnection;43;1;39;0
WireConnection;44;0;43;0
WireConnection;46;0;45;0
WireConnection;49;0;46;0
WireConnection;49;1;47;0
WireConnection;3;0;4;0
WireConnection;3;1;2;0
WireConnection;53;0;52;0
WireConnection;53;1;51;0
WireConnection;50;0;49;0
WireConnection;50;1;48;0
WireConnection;14;0;5;1
WireConnection;14;1;13;0
WireConnection;11;0;6;1
WireConnection;11;1;10;0
WireConnection;54;0;50;0
WireConnection;54;1;53;0
WireConnection;9;0;3;0
WireConnection;56;0;55;0
WireConnection;56;1;54;0
WireConnection;7;0;14;0
WireConnection;7;1;11;0
WireConnection;7;2;9;0
WireConnection;0;0;7;0
WireConnection;0;2;56;0
WireConnection;0;3;12;0
WireConnection;0;4;12;0
WireConnection;0;10;50;0
ASEEND*/
//CHKSM=5DFEC27F22F26C9280DA3C88DBE4159A68470EB7