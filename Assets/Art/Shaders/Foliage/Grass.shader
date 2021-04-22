// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Grass"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_MyPos("MyPos", Vector) = (0,0,0,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Color("Color", Color) = (0,0,0,0)
		_Radius("Radius", Float) = 1
		_GrassStrength("Grass Strength", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float3 _MyPos;
		uniform float _Radius;
		uniform float _GrassStrength;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float4 _Color;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 appendResult51 = (float3(_MyPos.x , 0.0 , _MyPos.z));
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float temp_output_24_0 = ( distance( appendResult51 , ase_worldPos ) / _Radius );
			float clampResult35 = clamp( temp_output_24_0 , 0.0 , 1.0 );
			float3 normalizeResult42 = normalize( ( ase_worldPos - appendResult51 ) );
			float2 appendResult45 = (float2(_GrassStrength , _GrassStrength));
			float2 appendResult47 = (float2(appendResult45.x , 0.0));
			v.vertex.xyz += ( ( ( 1.0 - clampResult35 ) * ( v.texcoord.xy.y * v.texcoord.xy.y ) ) * ( normalizeResult42 * float3( appendResult47 ,  0.0 ) ) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode19 = tex2D( _TextureSample0, uv_TextureSample0 );
			o.Albedo = ( tex2DNode19 * _Color ).rgb;
			o.Alpha = 1;
			clip( tex2DNode19.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;26;1920;993;3088.974;-909.6682;1;True;True
Node;AmplifyShaderEditor.Vector3Node;2;-2915.949,1121.339;Inherit;False;Property;_MyPos;MyPos;3;0;Create;True;0;0;False;0;False;0,0,0;66.56556,4.005061,-57.00913;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;51;-2439.227,1230.423;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;1;-2887.902,1306.131;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;23;-2026.325,1276.929;Inherit;False;Property;_Radius;Radius;6;0;Create;True;0;0;False;0;False;1;300;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;22;-1995.92,913.6993;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;24;-1740.426,1134.684;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-2228.132,2104.689;Inherit;False;Property;_GrassStrength;Grass Strength;7;0;Create;True;0;0;False;0;False;0;7.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;45;-1956.67,1942.288;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;35;-1405.547,1410.17;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;41;-1976.063,1636.097;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-1651.923,1597.992;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;49;-1905.892,2196.245;Inherit;False;Constant;_Float2;Float 2;8;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-1130.271,1712.66;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;42;-1735.465,1864.098;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;36;-1167.509,1408.876;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;47;-1713.892,1984.245;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;19;-663.8856,-434.3175;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;False;-1;None;40451cd6478eefd4e84ed5e6e137cba2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-936.5953,1392.94;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;20;-655.134,-219.6011;Inherit;False;Property;_Color;Color;5;0;Create;True;0;0;False;0;False;0,0,0,0;0.8235295,0.5490196,0.2627451,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-1399.731,1872.409;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1600.627,495.494;Inherit;False;Property;_Strength;Strength;2;0;Create;True;0;0;False;0;False;0;1.73;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-802.3619,1046.185;Inherit;False;Constant;_Float0;Float 0;8;0;Create;True;0;0;False;0;False;-0.8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;8;-1780.988,279.1526;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-867.3682,883.4766;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-613.5842,1553.295;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;31;-499.5538,757.6437;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-678.3441,24.98303;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1312.659,663.082;Inherit;False;Constant;_Float3;Float 3;4;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-1498.954,154.1047;Inherit;False;Property;_Distance;Distance;1;0;Create;True;0;0;False;0;False;0;1.13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;11;-1108.344,21.98303;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-439.344,186.983;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;12;-1409.627,343.494;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1158.057,841.774;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-804.815,1146.761;Inherit;False;Constant;_Float1;Float 1;8;0;Create;True;0;0;False;0;False;0.8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;4;-1782.635,-17.38406;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;13;-899.344,32.98303;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;27;-1793.541,726.9562;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-342.134,-223.6011;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-665.6271,425.494;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;39;-1361.355,1685.567;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-1397.627,472.494;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;26;-1302.465,1142.781;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;5;-1319.209,19.16086;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;15;-1169.627,461.494;Inherit;False;FLOAT3;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;14;-846.344,129.983;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;25;-1462.908,1054.783;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Grass;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;51;0;2;1
WireConnection;51;2;2;3
WireConnection;22;0;51;0
WireConnection;22;1;1;0
WireConnection;24;0;22;0
WireConnection;24;1;23;0
WireConnection;45;0;30;0
WireConnection;45;1;30;0
WireConnection;35;0;24;0
WireConnection;41;0;1;0
WireConnection;41;1;51;0
WireConnection;40;0;7;2
WireConnection;40;1;7;2
WireConnection;42;0;41;0
WireConnection;36;0;35;0
WireConnection;47;0;45;0
WireConnection;47;1;49;0
WireConnection;37;0;36;0
WireConnection;37;1;40;0
WireConnection;43;0;42;0
WireConnection;43;1;47;0
WireConnection;8;0;1;0
WireConnection;8;1;2;0
WireConnection;29;0;28;0
WireConnection;44;0;37;0
WireConnection;44;1;43;0
WireConnection;31;0;29;0
WireConnection;31;1;32;0
WireConnection;31;2;33;0
WireConnection;16;0;13;0
WireConnection;16;1;14;0
WireConnection;11;0;5;0
WireConnection;18;0;16;0
WireConnection;18;1;17;0
WireConnection;12;0;8;0
WireConnection;28;0;27;0
WireConnection;28;1;26;0
WireConnection;4;0;1;0
WireConnection;4;1;2;0
WireConnection;13;0;11;0
WireConnection;27;0;1;0
WireConnection;27;1;2;0
WireConnection;21;0;19;0
WireConnection;21;1;20;0
WireConnection;17;0;12;0
WireConnection;17;1;15;0
WireConnection;9;0;6;0
WireConnection;9;1;6;0
WireConnection;26;1;25;0
WireConnection;5;0;4;0
WireConnection;5;1;3;0
WireConnection;15;0;9;0
WireConnection;15;2;10;0
WireConnection;25;0;24;0
WireConnection;0;0;21;0
WireConnection;0;10;19;4
WireConnection;0;11;44;0
ASEEND*/
//CHKSM=43EBF7919075593008E97A975595836D0FACDDCA