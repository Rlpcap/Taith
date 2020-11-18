// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WindEffect"
{
	Properties
	{
		_Wind("Wind", 2D) = "white" {}
		_Vector1("Vector 1", Vector) = (0,0,0,0)
		_Mov("Mov", Vector) = (0,0,0,0)
		_Float0("Float 0", Float) = 0
		_SkillDuration("SkillDuration", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Float0;
		uniform float _SkillDuration;
		uniform sampler2D _Wind;
		uniform float2 _Vector1;
		uniform float2 _Mov;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord7 = i.uv_texcoord * _Vector1 + ( _Mov + float2( 2,0 ) );
			float2 uv_TexCoord3 = i.uv_texcoord * _Vector1 + _Mov;
			float temp_output_21_0 = ( ( 1.0 - step( i.uv_texcoord.x , (0.0 + (_Float0 - 0.0) * (1.0 - 0.0) / (_SkillDuration - 0.0)) ) ) * saturate( ( tex2D( _Wind, uv_TexCoord7 ).r + tex2D( _Wind, uv_TexCoord3 ).r ) ) );
			float3 temp_cast_0 = (temp_output_21_0).xxx;
			o.Emission = temp_cast_0;
			o.Alpha = temp_output_21_0;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
343;408;1906;991;1443.966;592.1118;1.184593;True;True
Node;AmplifyShaderEditor.Vector2Node;4;-1050.492,233.6544;Inherit;False;Property;_Mov;Mov;2;0;Create;True;0;0;False;0;False;0,0;6.64,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;11;-816.9143,298.2384;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;2,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;5;-1039.13,60.4277;Inherit;False;Property;_Vector1;Vector 1;1;0;Create;True;0;0;False;0;False;0,0;5.33,-0.25;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-792.2076,128.8584;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;-872.2358,-317.1843;Inherit;False;Property;_SkillDuration;SkillDuration;4;0;Create;True;0;0;False;0;False;0;0.11;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-863.1652,-439.535;Inherit;False;Property;_Float0;Float 0;3;0;Create;True;0;0;False;0;False;0;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-773.0595,-163.7446;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-452.4588,-145.1183;Inherit;True;Property;_TextureSample1;Texture Sample 1;0;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;2;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-436.4282,134.1858;Inherit;True;Property;_Wind;Wind;0;0;Create;True;0;0;False;0;False;-1;None;ed0f60994dbb5c74d9d8f68757fba66a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;18;-582.3061,-442.9636;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-639.4911,-577.9952;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-115.2595,-32.44464;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;16;-343.2044,-545.2031;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;20;-32.00214,-480.5694;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;14;30.21241,-31.456;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;238.5175,-410.6862;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;572.02,-201.5085;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;WindEffect;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;11;0;4;0
WireConnection;3;0;5;0
WireConnection;3;1;4;0
WireConnection;7;0;5;0
WireConnection;7;1;11;0
WireConnection;6;1;7;0
WireConnection;2;1;3;0
WireConnection;18;0;17;0
WireConnection;18;2;19;0
WireConnection;12;0;6;1
WireConnection;12;1;2;1
WireConnection;16;0;15;1
WireConnection;16;1;18;0
WireConnection;20;0;16;0
WireConnection;14;0;12;0
WireConnection;21;0;20;0
WireConnection;21;1;14;0
WireConnection;0;2;21;0
WireConnection;0;9;21;0
ASEEND*/
//CHKSM=9EA4646513AB54996D1F01B22EDCF66D9EE55DCF