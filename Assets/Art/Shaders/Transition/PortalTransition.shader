// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PortalTransition"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.05
		_Strength1("Strength", Float) = 10
		_Scale1("Scale", Float) = 2
		_Speed1("Speed", Float) = 0.5
		_TextureSample1("Texture Sample 0", 2D) = "white" {}
		[HDR]_Color("Color", Color) = (0.8791969,0.467206,0.09344122,0)
		_DissolveAmount("Dissolve Amount", Range( 0 , 10)) = 4.1
		_PortalScale("PortalScale", Range( 0 , 15)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform float _PortalScale;
		uniform float _Scale1;
		uniform float _Strength1;
		uniform float _Speed1;
		uniform float _DissolveAmount;
		uniform float _Cutoff = 0.05;


		float2 voronoihash7( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi7( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash7( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F1;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float4 temp_cast_0 = (_PortalScale).xxxx;
			float time7 = 2.0;
			float2 center45_g1 = float2( 0.5,0.5 );
			float2 delta6_g1 = ( i.uv_texcoord - center45_g1 );
			float angle10_g1 = ( length( delta6_g1 ) * _Strength1 );
			float x23_g1 = ( ( cos( angle10_g1 ) * delta6_g1.x ) - ( sin( angle10_g1 ) * delta6_g1.y ) );
			float2 break40_g1 = center45_g1;
			float2 temp_cast_1 = (( _Speed1 * _Time.y )).xx;
			float2 break41_g1 = temp_cast_1;
			float y35_g1 = ( ( sin( angle10_g1 ) * delta6_g1.x ) + ( cos( angle10_g1 ) * delta6_g1.y ) );
			float2 appendResult44_g1 = (float2(( x23_g1 + break40_g1.x + break41_g1.x ) , ( break40_g1.y + break41_g1.y + y35_g1 )));
			float2 coords7 = appendResult44_g1 * _Scale1;
			float2 id7 = 0;
			float2 uv7 = 0;
			float voroi7 = voronoi7( coords7, time7, id7, uv7, 0 );
			float4 temp_output_11_0 = ( saturate( pow( tex2D( _TextureSample1, uv_TextureSample1 ) , temp_cast_0 ) ) * pow( voroi7 , _DissolveAmount ) );
			o.Emission = ( _Color * temp_output_11_0 ).rgb;
			o.Alpha = 1;
			clip( temp_output_11_0.r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;0;1920;1019;1474.12;420.3391;1;True;True
Node;AmplifyShaderEditor.TimeNode;1;-2013.417,315.5099;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;2;-1898.641,178.8732;Inherit;False;Property;_Speed1;Speed;3;0;Create;True;0;0;False;0;False;0.5;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-1700.641,220.8732;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1490.027,172.0932;Inherit;False;Property;_Strength1;Strength;1;0;Create;True;0;0;False;0;False;10;15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;5;-1283.391,224.8979;Inherit;True;Twirl;-1;;1;90936742ac32db8449cd21ab6dd337c8;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT;0;False;4;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1187.391,448.8979;Inherit;False;Property;_Scale1;Scale;2;0;Create;True;0;0;False;0;False;2;5.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1231.015,-284.7404;Inherit;False;Property;_PortalScale;PortalScale;7;0;Create;True;0;0;False;0;False;0;0;0;15;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-1304.789,-116.7387;Inherit;True;Property;_TextureSample1;Texture Sample 0;4;0;Create;True;0;0;False;0;False;-1;None;0000000000000000f000000000000000;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;14;-916.5787,-235.5243;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;3.2;False;1;COLOR;0
Node;AmplifyShaderEditor.VoronoiNode;7;-974.3911,262.8979;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;2;False;2;FLOAT;2;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.RangedFloatNode;8;-936.2041,558.4042;Inherit;False;Property;_DissolveAmount;Dissolve Amount;6;0;Create;True;0;0;False;0;False;4.1;0.7;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;9;-758.0614,347.6242;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;17;-729.2841,36.53125;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-583.9658,156.8595;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;12;-619.972,-141.9881;Inherit;False;Property;_Color;Color;5;1;[HDR];Create;True;0;0;False;0;False;0.8791969,0.467206,0.09344122,0;128,5.521569,5.521569,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-329.0468,16.21638;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;PortalTransition;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.05;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;2;0
WireConnection;3;1;1;2
WireConnection;5;3;4;0
WireConnection;5;4;3;0
WireConnection;14;0;10;0
WireConnection;14;1;15;0
WireConnection;7;0;5;0
WireConnection;7;2;6;0
WireConnection;9;0;7;0
WireConnection;9;1;8;0
WireConnection;17;0;14;0
WireConnection;11;0;17;0
WireConnection;11;1;9;0
WireConnection;13;0;12;0
WireConnection;13;1;11;0
WireConnection;0;2;13;0
WireConnection;0;10;11;0
ASEEND*/
//CHKSM=27406BFB9DA04A2169557854F099B32892F8FD5A