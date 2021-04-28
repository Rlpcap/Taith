// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Lava"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Scale("Scale", Float) = 0
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_DissolveAmount("DissolveAmount", Range( 0 , 1)) = 0
		_NoiseScrollSpeed2("NoiseScrollSpeed", Float) = 0.34
		[Toggle]_ToggleSwitch2("Toggle Switch0", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _TextureSample2;
		uniform float _Scale;
		uniform sampler2D _TextureSample1;
		uniform float _ToggleSwitch2;
		uniform float _DissolveAmount;
		uniform float _NoiseScrollSpeed2;
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
			float2 panner32 = ( 1.0 * _Time.y * float2( 0.005,0 ) + ( i.uv_texcoord * _Scale ));
			float4 tex2DNode37 = tex2D( _TextureSample2, panner32 );
			o.Normal = tex2DNode37.rgb;
			float2 uv_TexCoord26 = i.uv_texcoord * float2( 5,5 ) + float2( 5,5 );
			float2 panner33 = ( 1.0 * _Time.y * float2( 0.1,0 ) + ( uv_TexCoord26 * _Scale ));
			o.Emission = tex2D( _TextureSample1, panner33 ).rgb;
			o.Alpha = 1;
			float3 ase_worldPos = i.worldPos;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 normalizeResult58 = normalize( (( _ToggleSwitch2 )?( ase_vertex3Pos ):( ase_worldPos )) );
			float3 normalizeResult59 = normalize( float3(1,0,0) );
			float dotResult66 = dot( normalizeResult58 , normalizeResult59 );
			float Gradient73 = ( dotResult66 + (-1.29 + (_DissolveAmount - 0.0) * (1.06 - -1.29) / (1.0 - 0.0)) );
			float3 normalizeResult57 = normalize( float3(0.76,1,0) );
			float simplePerlin2D72 = snoise( ( ase_vertex3Pos + ( ( normalizeResult57 * _NoiseScrollSpeed2 ) * _Time.y ) ).xy*float2( 1.38,0.01 ).x );
			simplePerlin2D72 = simplePerlin2D72*0.5 + 0.5;
			float Noise75 = simplePerlin2D72;
			float temp_output_83_0 = ( ( ( 1.0 - Gradient73 ) * Noise75 ) - Gradient73 );
			clip( temp_output_83_0 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;0;1920;1019;4444.694;-723.599;2.217522;True;True
Node;AmplifyShaderEditor.CommentaryNode;44;-3312.47,1015.495;Inherit;False;1794.989;768.6633;;11;75;72;70;69;68;65;64;60;57;54;50;Fade;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;45;-3283.646,1898.652;Inherit;False;1698.852;1060.583;;15;73;71;67;66;63;62;61;59;58;56;55;53;52;51;49;Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;49;-3233.646,2119.831;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;50;-3262.47,1243.202;Inherit;False;Constant;_NoiseDirection2;NoiseDirection;5;0;Create;True;0;0;False;0;False;0.76,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;51;-3230.041,1948.652;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;53;-3144.159,2652.334;Inherit;False;Constant;_LowerLimit2;LowerLimit;7;0;Create;True;0;0;False;0;False;-1.06;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;56;-2928.374,2307.702;Inherit;False;Constant;_DissolveDirection2;DissolveDirection;7;0;Create;True;0;0;False;0;False;1,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;57;-3031.178,1267.233;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-3138.754,2567.645;Inherit;False;Constant;_UpperLimit2;UpperLimit;7;0;Create;True;0;0;False;0;False;1.29;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;52;-2925.521,2009.916;Inherit;False;Property;_ToggleSwitch2;Toggle Switch0;8;0;Create;True;0;0;False;0;False;1;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-3137.812,1488.012;Inherit;False;Property;_NoiseScrollSpeed2;NoiseScrollSpeed;7;0;Create;True;0;0;False;0;False;0.34;0.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;62;-2897.299,2654.136;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-2853.953,1330.312;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TimeNode;60;-3142.318,1605.158;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;63;-3113.412,2816.381;Inherit;False;Property;_DissolveAmount;DissolveAmount;5;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;61;-2881.082,2546.022;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;58;-2628.208,2029.737;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;59;-2632.485,2333.467;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;66;-2338.113,2156.243;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-2717.281,1491.015;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;68;-3097.712,1065.495;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;67;-2580.165,2652.334;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;69;-2434.704,1455.178;Inherit;False;Constant;_NoiseScale2;NoiseScale;4;0;Create;True;0;0;False;0;False;1.38,0.01;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;71;-2117.334,2462.63;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;70;-2490.505,1193.998;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;73;-1827.793,2393.595;Inherit;False;Gradient;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;72;-2096.652,1231.822;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-1835.492,178.087;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,5;False;1;FLOAT2;5,5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;28;-1758.066,91.70056;Inherit;False;Property;_Scale;Scale;1;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;75;-1760.482,1291.894;Inherit;False;Noise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;74;-1528.69,527.1946;Inherit;False;73;Gradient;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;-1840.492,-42.46365;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;77;-1513.25,622.0404;Inherit;False;75;Noise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;76;-1313.269,531.8916;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-1388.403,19.79614;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-1384.403,170.7961;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;78;-1540.193,743.8512;Inherit;False;73;Gradient;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;32;-1126.973,6.185502;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.005,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-1147.269,517.8915;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;33;-1124.373,143.9855;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;34;-1121.772,276.5855;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.01,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;37;-873.2481,-134.7833;Inherit;True;Property;_TextureSample2;Texture Sample 2;4;0;Create;True;0;0;False;0;False;-1;None;b16a95c33b7a00145b2b819153d7e01f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;35;-879.6483,276.4167;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;False;-1;None;2c50ca288f32a784882f8090ba06dcc1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;82;-840.7523,1363.885;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-304.4606,-36.63449;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-297.2343,212.0482;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1390.403,303.7961;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;41;-563.0903,328.2859;Inherit;True;3;0;FLOAT;-1;False;1;FLOAT;0.48;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;46;-560.4764,1451.704;Inherit;False;Property;_EdgeColor2;EdgeColor;9;0;Create;True;0;0;False;0;False;0.5813924,0.2198736,0.764151,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;81;-1144.935,1371.412;Inherit;False;Property;_EdgeThickness2;EdgeThickness;6;0;Create;True;0;0;False;0;False;0;0;0;1.15;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-257.6442,1259.947;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;43.57056,600.5263;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;83;-906.2692,665.8913;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;36;-866.848,74.8167;Inherit;True;Property;_TextureSample1;Texture Sample 1;3;0;Create;True;0;0;False;0;False;-1;None;2c50ca288f32a784882f8090ba06dcc1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;79;-1091.719,1492.871;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;47;-548.513,1166.165;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;92.39999,-70.39999;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Lava;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;57;0;50;0
WireConnection;52;0;51;0
WireConnection;52;1;49;0
WireConnection;62;0;53;0
WireConnection;64;0;57;0
WireConnection;64;1;54;0
WireConnection;61;0;55;0
WireConnection;58;0;52;0
WireConnection;59;0;56;0
WireConnection;66;0;58;0
WireConnection;66;1;59;0
WireConnection;65;0;64;0
WireConnection;65;1;60;2
WireConnection;67;0;63;0
WireConnection;67;3;61;0
WireConnection;67;4;62;0
WireConnection;71;0;66;0
WireConnection;71;1;67;0
WireConnection;70;0;68;0
WireConnection;70;1;65;0
WireConnection;73;0;71;0
WireConnection;72;0;70;0
WireConnection;72;1;69;0
WireConnection;75;0;72;0
WireConnection;76;0;74;0
WireConnection;29;0;25;0
WireConnection;29;1;28;0
WireConnection;30;0;26;0
WireConnection;30;1;28;0
WireConnection;32;0;29;0
WireConnection;80;0;76;0
WireConnection;80;1;77;0
WireConnection;33;0;30;0
WireConnection;34;0;31;0
WireConnection;37;1;32;0
WireConnection;35;1;34;0
WireConnection;82;0;81;0
WireConnection;82;1;79;0
WireConnection;43;1;42;0
WireConnection;42;0;37;1
WireConnection;42;1;41;0
WireConnection;31;0;25;0
WireConnection;31;1;28;0
WireConnection;41;2;35;1
WireConnection;48;0;46;0
WireConnection;48;1;47;0
WireConnection;84;1;48;0
WireConnection;83;0;80;0
WireConnection;83;1;78;0
WireConnection;36;1;33;0
WireConnection;47;0;83;0
WireConnection;47;1;82;0
WireConnection;0;1;37;0
WireConnection;0;2;36;0
WireConnection;0;10;83;0
ASEEND*/
//CHKSM=33A4247C21A360D7CB927D05B5EEA4FBDA991E35