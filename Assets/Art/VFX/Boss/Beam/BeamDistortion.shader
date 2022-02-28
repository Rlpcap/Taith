// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BeamDistortion"
{
	Properties
	{
		_Mask("Mask", 2D) = "white" {}
		_SpeedScroll("SpeedScroll", Vector) = (0,0,0,0)
		_MainTex("MainTex", 2D) = "black" {}
		_DistAmount("DistAmount", Range( 0 , 1)) = 0
		_SpeedDist("SpeedDist", Vector) = (1,0,0,0)
		_DistScale("DistScale", Float) = 5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend One One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow nofog 
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
		};

		uniform sampler2D _MainTex;
		uniform float _DistScale;
		uniform float2 _SpeedDist;
		uniform float _DistAmount;
		uniform float2 _SpeedScroll;
		uniform sampler2D _Mask;
		uniform float4 _Mask_ST;


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


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float mulTime30 = _Time.y * _SpeedDist.x;
			float simplePerlin2D28 = snoise( (i.uv_texcoord*_DistScale + mulTime30) );
			simplePerlin2D28 = simplePerlin2D28*0.5 + 0.5;
			float2 temp_cast_1 = (simplePerlin2D28).xx;
			float2 lerpResult27 = lerp( i.uv_texcoord , temp_cast_1 , _DistAmount);
			float2 panner17 = ( 1.0 * _Time.y * _SpeedScroll + float2( 0.84,-2.17 ));
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			o.Emission = ( i.vertexColor * ( tex2D( _MainTex, (lerpResult27*1.2 + panner17) ) * tex2D( _Mask, uv_Mask ) ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
-74;614;1769;767;1509.302;49.76023;1.3;True;True
Node;AmplifyShaderEditor.Vector2Node;32;-2516.528,-141.4014;Inherit;False;Property;_SpeedDist;SpeedDist;5;0;Create;True;0;0;False;0;False;1,0;-1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;30;-2312.901,-143.5997;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-2246.639,36.19275;Inherit;False;Property;_DistScale;DistScale;6;0;Create;True;0;0;False;0;False;5;4.28;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-2363.82,-379.7939;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;29;-2083.053,-146.4281;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;3;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1840.546,175.662;Inherit;False;Property;_DistAmount;DistAmount;4;0;Create;True;0;0;False;0;False;0;0.05801361;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;28;-1817.778,-98.24552;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;18;-1768.531,292.5942;Inherit;False;Property;_SpeedScroll;SpeedScroll;2;0;Create;True;0;0;False;0;False;0,0;1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;17;-1531.369,307.3229;Inherit;False;3;0;FLOAT2;0.84,-2.17;False;2;FLOAT2;1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;27;-1490.855,-127.4236;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;10;-1255.741,-65.49461;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT;1.2;False;2;FLOAT2;0.04,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;22;-1150.856,-336.3682;Inherit;True;Property;_MainTex;MainTex;3;0;Create;True;0;0;False;0;False;None;b5f8940c4ceb0ca41942dd82e6ea4bff;False;black;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;25;-912.6819,-65.24792;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-911.9868,161.564;Inherit;True;Property;_Mask;Mask;0;0;Create;True;0;0;False;0;False;-1;None;2e180f851c6d28e4287ad37e2e4ec758;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;4;-625.9868,-228.436;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-543.8867,24.96397;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-327.3566,-174.0454;Inherit;True;2;2;0;COLOR;1,1,1,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;26;136.038,-38.02963;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;BeamDistortion;False;False;False;False;False;False;False;False;False;True;False;False;False;False;True;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;30;0;32;0
WireConnection;29;0;20;0
WireConnection;29;1;34;0
WireConnection;29;2;30;0
WireConnection;28;0;29;0
WireConnection;17;2;18;0
WireConnection;27;0;20;0
WireConnection;27;1;28;0
WireConnection;27;2;33;0
WireConnection;10;0;27;0
WireConnection;10;2;17;0
WireConnection;25;0;22;0
WireConnection;25;1;10;0
WireConnection;8;0;25;0
WireConnection;8;1;6;0
WireConnection;9;0;4;0
WireConnection;9;1;8;0
WireConnection;26;2;9;0
ASEEND*/
//CHKSM=5BBA08EF62C54382E00213EEB4BCF73E3F8C6EF7