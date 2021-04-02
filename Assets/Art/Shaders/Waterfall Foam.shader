// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Waterfall Foam"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample0;
		uniform sampler2D _TextureSample1;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color53 = IsGammaSpace() ? float4(1,1,1,1) : float4(1,1,1,1);
			float4 color54 = IsGammaSpace() ? float4(1,1,1,1) : float4(1,1,1,1);
			float2 break5 = i.uv_texcoord;
			float2 appendResult2 = (float2(( 1.0 * break5.x ) , break5.y));
			float2 break8 = appendResult2;
			float2 appendResult21 = (float2(( 0.5 * ( ( _Time.y * 0.0 ) + break8.x ) ) , ( ( break8.y - ( _Time.y * -0.85 ) ) * 1.3 )));
			float2 break23 = appendResult2;
			float2 appendResult24 = (float2(( 2.0 * break23.x ) , ( ( break23.y - ( _Time.y * -1.0 ) ) * 1.0 )));
			float4 temp_cast_0 = (0.15).xxxx;
			clip( ( ( ( tex2D( _TextureSample0, appendResult21 ) * tex2D( _TextureSample1, appendResult24 ) ) * ( 1.0 - ( saturate( abs( ( i.uv_texcoord.y - 0.5 ) ) ) * 2.0 ) ) ) * 1.0 ) - temp_cast_0);
			float4 lerpResult52 = lerp( color53 , color54 , float4( 0,0,0,0 ));
			o.Emission = lerpResult52.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;0;1920;1019;2262.589;931.172;3.297509;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-2509.626,129.3205;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;6;-2149.625,26.3204;Inherit;False;Constant;_ScaleWidth;ScaleWidth;0;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;5;-2228.625,123.3205;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1947.625,46.32037;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;9;-1459.495,-538.3757;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;26;-1464.629,665.2012;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1466.828,28.29104;Inherit;False;Constant;_ASpeed;A Speed;0;0;Create;True;0;0;False;0;False;-0.85;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-1470.629,780.2012;Inherit;False;Constant;_BSpeed;B Speed;0;0;Create;True;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1465.495,-423.3759;Inherit;False;Constant;_HorizontalSpeed;HorizontalSpeed;0;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;14;-1460.828,-86.70903;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;2;-1741.625,112.3205;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-1356.932,1300.004;Inherit;False;Constant;_Bias;Bias;2;0;Create;True;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-1190.629,740.2012;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-1186.828,-11.70897;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;8;-1472.495,-281.3758;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-1185.495,-463.3758;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;23;-1476.296,470.5345;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TextureCoordinatesNode;41;-1423.197,1122.144;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;36;-973.2958,731.5345;Inherit;False;Constant;_BScaleY;B ScaleY;0;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-954.4943,-375.3759;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-969.4943,-20.37568;Inherit;False;Constant;_AScaleY;A ScaleY;0;0;Create;True;0;0;False;0;False;1.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-971.2958,231.5346;Inherit;False;Constant;_BScaleX;B ScaleX;0;0;Create;True;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-967.4943,-520.3757;Inherit;False;Constant;_AScaleX;A ScaleX;0;0;Create;True;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;27;-981.2957,576.5344;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;44;-1068.186,1113.808;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;13;-977.4943,-175.3758;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-772.4944,-448.3758;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-795.2958,649.5345;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-776.2958,303.5345;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-791.4944,-102.3757;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;45;-819.9867,1119.907;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;46;-563.012,1160.727;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;21;-570.5546,-299.3633;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;24;-574.3561,452.5471;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;37;-391.8234,397.3069;Inherit;True;Property;_TextureSample1;Texture Sample 0;2;0;Create;True;0;0;False;0;False;-1;1e4c2f9781413ff468fe77a13acad173;1e4c2f9781413ff468fe77a13acad173;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-370.988,1140.832;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;22;-388.0221,-354.6035;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;False;-1;1e4c2f9781413ff468fe77a13acad173;1e4c2f9781413ff468fe77a13acad173;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;48;-111.5285,1126.648;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;42.00586,41.71045;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;51;340.2556,1071.157;Inherit;False;Constant;_ThresholdBias;Threshold Bias;2;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;319.209,645.6083;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;692.8123,807.4237;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;56;803.3897,1202.381;Inherit;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;False;0;False;0.15;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClipNode;55;1023.387,1012.384;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;53;634.2551,174.9405;Inherit;False;Constant;_Color0;Color 0;2;0;Create;True;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;54;597.2692,428.1514;Inherit;False;Constant;_Color1;Color 1;2;0;Create;True;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;52;960.0157,375.5176;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1293.14,458.2931;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Waterfall Foam;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;4;0
WireConnection;7;0;6;0
WireConnection;7;1;5;0
WireConnection;2;0;7;0
WireConnection;2;1;5;1
WireConnection;33;0;26;0
WireConnection;33;1;28;0
WireConnection;15;0;14;0
WireConnection;15;1;16;0
WireConnection;8;0;2;0
WireConnection;11;0;9;0
WireConnection;11;1;10;0
WireConnection;23;0;2;0
WireConnection;12;0;11;0
WireConnection;12;1;8;0
WireConnection;27;0;23;1
WireConnection;27;1;33;0
WireConnection;44;0;41;2
WireConnection;44;1;43;0
WireConnection;13;0;8;1
WireConnection;13;1;15;0
WireConnection;17;0;18;0
WireConnection;17;1;12;0
WireConnection;34;0;27;0
WireConnection;34;1;36;0
WireConnection;25;0;35;0
WireConnection;25;1;23;0
WireConnection;19;0;13;0
WireConnection;19;1;20;0
WireConnection;45;0;44;0
WireConnection;46;0;45;0
WireConnection;21;0;17;0
WireConnection;21;1;19;0
WireConnection;24;0;25;0
WireConnection;24;1;34;0
WireConnection;37;1;24;0
WireConnection;47;0;46;0
WireConnection;22;1;21;0
WireConnection;48;0;47;0
WireConnection;38;0;22;0
WireConnection;38;1;37;0
WireConnection;49;0;38;0
WireConnection;49;1;48;0
WireConnection;50;0;49;0
WireConnection;50;1;51;0
WireConnection;55;1;50;0
WireConnection;55;2;56;0
WireConnection;52;0;53;0
WireConnection;52;1;54;0
WireConnection;52;2;55;0
WireConnection;0;2;52;0
ASEEND*/
//CHKSM=96EEFF7747D8047860524D85219979B8D5DFAA8D