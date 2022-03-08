// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TwoSidedPlayer"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_DissolveAmount1("DissolveAmount", Range( -1 , 2)) = 0
		_EdgeThickness1("EdgeThickness", Range( 0 , 1.15)) = 0
		_NoiseScrollSpeed1("NoiseScrollSpeed", Float) = 0.34
		[Toggle]_ToggleSwitch1("Toggle Switch0", Float) = 1
		_EdgeColor1("EdgeColor", Color) = (0.5813924,0.2198736,0.764151,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float4 _EdgeColor1;
		uniform float _ToggleSwitch1;
		uniform float _DissolveAmount1;
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
			o.Albedo = tex2D( _TextureSample0, uv_TextureSample0 ).rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 normalizeResult15 = normalize( (( _ToggleSwitch1 )?( ase_vertex3Pos ):( ase_worldPos )) );
			float3 normalizeResult19 = normalize( float3(1,0,0) );
			float dotResult23 = dot( normalizeResult15 , normalizeResult19 );
			float Gradient27 = ( dotResult23 + (-1.29 + (_DissolveAmount1 - 0.0) * (1.06 - -1.29) / (1.0 - 0.0)) );
			float3 normalizeResult7 = normalize( float3(0.76,1,0) );
			float simplePerlin2D28 = snoise( ( ase_vertex3Pos + ( ( normalizeResult7 * _NoiseScrollSpeed1 ) * _Time.y ) ).xy*float2( 1.38,0.01 ).x );
			simplePerlin2D28 = simplePerlin2D28*0.5 + 0.5;
			float Noise30 = simplePerlin2D28;
			float temp_output_41_0 = ( ( ( 1.0 - Gradient27 ) * Noise30 ) - Gradient27 );
			o.Emission = ( _EdgeColor1 * step( temp_output_41_0 , ( _EdgeThickness1 + (0.0 + (0.0 - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) ) ) ).rgb;
			o.Alpha = 1;
			clip( temp_output_41_0 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;0;1920;1019;2007.003;-65.45746;1.342889;True;True
Node;AmplifyShaderEditor.CommentaryNode;2;-3902.779,-482.8402;Inherit;False;1794.989;768.6633;;11;30;28;25;24;22;20;17;14;10;7;5;Fade;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;3;-3873.955,400.3168;Inherit;False;1698.852;1060.583;;15;27;26;23;21;19;18;16;15;13;12;11;9;8;6;4;Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector3Node;5;-3852.779,-255.1331;Inherit;False;Constant;_NoiseDirection1;NoiseDirection;5;0;Create;True;0;0;False;0;False;0.76,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;6;-3820.35,450.3167;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PosVertexDataNode;4;-3823.955,621.4957;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;11;-3734.468,1153.999;Inherit;False;Constant;_LowerLimit1;LowerLimit;7;0;Create;True;0;0;False;0;False;-1.06;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-3729.063,1069.31;Inherit;False;Constant;_UpperLimit1;UpperLimit;7;0;Create;True;0;0;False;0;False;1.29;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-3728.121,-10.32311;Inherit;False;Property;_NoiseScrollSpeed1;NoiseScrollSpeed;4;0;Create;True;0;0;False;0;False;0.34;0.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;7;-3621.487,-231.1021;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ToggleSwitchNode;12;-3515.83,511.5809;Inherit;False;Property;_ToggleSwitch1;Toggle Switch0;5;0;Create;True;0;0;False;0;False;1;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;8;-3518.683,809.3668;Inherit;False;Constant;_DissolveDirection1;DissolveDirection;7;0;Create;True;0;0;False;0;False;1,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NegateNode;16;-3487.608,1155.801;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;19;-3222.794,835.1317;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NegateNode;18;-3471.391,1047.687;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;17;-3732.627,106.8229;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-3703.721,1318.046;Inherit;False;Property;_DissolveAmount1;DissolveAmount;2;0;Create;True;0;0;False;0;False;0;-1;-1;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;15;-3218.517,531.4017;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-3444.262,-168.0232;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-3307.59,-7.320179;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;21;-3170.474,1153.999;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;22;-3688.021,-432.8402;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;23;-2928.423,657.9078;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;24;-3025.013,-43.15722;Inherit;False;Constant;_NoiseScale1;NoiseScale;4;0;Create;True;0;0;False;0;False;1.38,0.01;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-3080.814,-304.3372;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-2707.643,964.2947;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;27;-2418.102,895.2598;Inherit;False;Gradient;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;28;-2686.961,-266.5132;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;29;-1716.251,124.3358;Inherit;False;27;Gradient;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;-2350.791,-206.4411;Inherit;False;Noise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;31;-1500.83,129.0329;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;32;-1700.811,219.1819;Inherit;False;30;Noise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-1334.83,115.0328;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;34;-1727.753,340.9928;Inherit;False;27;Gradient;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;35;-1279.279,1090.013;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-1332.495,968.5538;Inherit;False;Property;_EdgeThickness1;EdgeThickness;3;0;Create;True;0;0;False;0;False;0;0;0;1.15;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;39;-1028.313,961.0267;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;41;-1093.83,263.0328;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;44;-748.0372,1048.846;Inherit;False;Property;_EdgeColor1;EdgeColor;6;0;Create;True;0;0;False;0;False;0.5813924,0.2198736,0.764151,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;43;-736.0738,763.3067;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-434,-156.5;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;False;-1;None;2d9d294072cfe9a4aa21cefd8edc9f3e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-445.2048,857.0887;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;104,-90;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;TwoSidedPlayer;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;0;5;0
WireConnection;12;0;6;0
WireConnection;12;1;4;0
WireConnection;16;0;11;0
WireConnection;19;0;8;0
WireConnection;18;0;9;0
WireConnection;15;0;12;0
WireConnection;14;0;7;0
WireConnection;14;1;10;0
WireConnection;20;0;14;0
WireConnection;20;1;17;2
WireConnection;21;0;13;0
WireConnection;21;3;18;0
WireConnection;21;4;16;0
WireConnection;23;0;15;0
WireConnection;23;1;19;0
WireConnection;25;0;22;0
WireConnection;25;1;20;0
WireConnection;26;0;23;0
WireConnection;26;1;21;0
WireConnection;27;0;26;0
WireConnection;28;0;25;0
WireConnection;28;1;24;0
WireConnection;30;0;28;0
WireConnection;31;0;29;0
WireConnection;36;0;31;0
WireConnection;36;1;32;0
WireConnection;39;0;37;0
WireConnection;39;1;35;0
WireConnection;41;0;36;0
WireConnection;41;1;34;0
WireConnection;43;0;41;0
WireConnection;43;1;39;0
WireConnection;46;0;44;0
WireConnection;46;1;43;0
WireConnection;0;0;1;0
WireConnection;0;2;46;0
WireConnection;0;10;41;0
ASEEND*/
//CHKSM=7F7A3A2FEBF8F44F5B821BC5EE9303C8A85CCADF