// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ProgressBar"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_InnerCircle("InnerCircle", Range( 0 , 1)) = 0.6171036
		_ProgressBarValue("ProgressBarValue", Range( 0 , 1)) = 0
		[HDR]_Color1("Color1", Color) = (0,0,0,0)
		_Color2("Color2", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color1;
		uniform float4 _Color2;
		uniform float _InnerCircle;
		uniform float _ProgressBarValue;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord1 = i.uv_texcoord * float2( 2,2 ) + float2( -1,-1 );
			float temp_output_2_0 = distance( uv_TexCoord1 , float2( 0,0 ) );
			float temp_output_8_0 = ( ( 1.0 - floor( temp_output_2_0 ) ) * floor( ( temp_output_2_0 + _InnerCircle ) ) );
			float2 uv_TexCoord10 = i.uv_texcoord * float2( 2,2 ) + float2( -1,-1 );
			float4 lerpResult23 = lerp( _Color1 , _Color2 , ( temp_output_8_0 * floor( ( _ProgressBarValue + (0.0 + (atan2( uv_TexCoord10.x , uv_TexCoord10.y ) - ( -1.0 * UNITY_PI )) * (1.0 - 0.0) / (UNITY_PI - ( -1.0 * UNITY_PI ))) ) ) ));
			o.Emission = lerpResult23.rgb;
			o.Alpha = 1;
			clip( temp_output_8_0 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;0;1920;1019;2062.387;942.5898;1.6;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1696.303,-515.6844;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;-1,-1;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-1850.446,175.3622;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;-1,-1;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PiNode;15;-1683.249,486.7633;Inherit;False;1;0;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1537.263,-193.0711;Inherit;False;Property;_InnerCircle;InnerCircle;1;0;Create;True;0;0;False;0;False;0.6171036;0.781;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;2;-1373.604,-518.2844;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;16;-1701.248,560.763;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ATan2OpNode;11;-1554.921,194.8348;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;14;-1250.232,313.9612;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;5;-1212.263,-242.0711;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;3;-1075.703,-522.4844;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1155.927,27.93599;Inherit;False;Property;_ProgressBarValue;ProgressBarValue;2;0;Create;True;0;0;False;0;False;0;0.366;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;7;-890.2339,-508.1928;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;17;-836.1276,251.5358;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;4;-1061.263,-263.0711;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;19;-647.6277,211.2358;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-657.534,-359.9929;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;22;-270.0265,-543.7313;Inherit;False;Property;_Color2;Color2;4;0;Create;True;0;0;False;0;False;0,0,0,0;0.4156863,0.427451,0.4901961,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-392.5693,-49.54372;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;21;-266.1132,-723.7372;Inherit;False;Property;_Color1;Color1;3;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;0.9622642,0.1316305,0.1316305,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;23;-75.90625,-264.1451;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;115.2123,-297.0703;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;ProgressBar;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;11;0;10;1
WireConnection;11;1;10;2
WireConnection;14;0;11;0
WireConnection;14;1;15;0
WireConnection;14;2;16;0
WireConnection;5;0;2;0
WireConnection;5;1;6;0
WireConnection;3;0;2;0
WireConnection;7;0;3;0
WireConnection;17;0;18;0
WireConnection;17;1;14;0
WireConnection;4;0;5;0
WireConnection;19;0;17;0
WireConnection;8;0;7;0
WireConnection;8;1;4;0
WireConnection;20;0;8;0
WireConnection;20;1;19;0
WireConnection;23;0;21;0
WireConnection;23;1;22;0
WireConnection;23;2;20;0
WireConnection;0;2;23;0
WireConnection;0;10;8;0
ASEEND*/
//CHKSM=0D23CD0607B80D94C4F9908417DAAAF14A1F430E