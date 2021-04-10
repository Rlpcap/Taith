// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Wind Tree"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Intensity("Intensity", Range( 0 , 1)) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_transparency("transparency", Range( 0 , 1)) = 0
		_TintFront("Tint Front", Color) = (0,0,0,0)
		_Movement("Movement", Float) = 0
		[Toggle(_YORZ_ON)] _YorZ("YorZ", Float) = 0
		_TintBack("Tint Back", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _YORZ_ON
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			half ASEVFace : VFACE;
			float4 screenPosition;
		};

		uniform float _Intensity;
		uniform float _Movement;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float4 _TintFront;
		uniform float4 _TintBack;
		uniform float _transparency;
		uniform float _Cutoff = 0.5;


		inline float Dither4x4Bayer( int x, int y )
		{
			const float dither[ 16 ] = {
				 1,  9,  3, 11,
				13,  5, 15,  7,
				 4, 12,  2, 10,
				16,  8, 14,  6 };
			int r = y * 4 + x;
			return dither[r] / 16; // same # of instructions as pre-dividing due to compiler magic
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float temp_output_3_0 = sin( _Time.y );
			float3 _Vector0 = float3(1,0,0);
			float3 ase_vertex3Pos = v.vertex.xyz;
			#ifdef _YORZ_ON
				float staticSwitch10 = ase_vertex3Pos.z;
			#else
				float staticSwitch10 = ase_vertex3Pos.y;
			#endif
			float3 temp_output_20_0 = ( ( temp_output_3_0 * _Vector0 * _Intensity * v.color.r ) + ( temp_output_3_0 * staticSwitch10 * _Movement * _Vector0 ) );
			v.vertex.xyz += temp_output_20_0;
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			o.screenPosition = ase_screenPos;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode15 = tex2D( _TextureSample0, uv_TextureSample0 );
			float4 switchResult18 = (((i.ASEVFace>0)?(_TintFront):(_TintBack)));
			o.Albedo = ( tex2DNode15 * switchResult18 ).rgb;
			o.Alpha = 1;
			float4 ase_screenPos = i.screenPosition;
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 clipScreen17 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither17 = Dither4x4Bayer( fmod(clipScreen17.x, 4), fmod(clipScreen17.y, 4) );
			dither17 = step( dither17, _transparency );
			clip( ( tex2DNode15.a * dither17 ) - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;0;1920;1019;1948.929;428.3984;1.295914;True;True
Node;AmplifyShaderEditor.SimpleTimeNode;1;-2460.577,358.643;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;2;-1528.474,306.901;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;3;-2211.394,362.4768;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;4;-2230.073,191.063;Inherit;False;Constant;_Vector0;Vector 0;0;0;Create;True;0;0;False;0;False;1,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;12;-1066.187,-600.0923;Inherit;False;Property;_TintFront;Tint Front;4;0;Create;True;0;0;False;0;False;0,0,0,0;0.7610765,0.8867924,0.2886258,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-1214.177,644.3225;Inherit;False;Property;_Movement;Movement;5;0;Create;True;0;0;False;0;False;0;-0.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;10;-1215.787,402.6169;Inherit;False;Property;_YorZ;YorZ;6;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;8;-1824.625,493.3142;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;14;-1055.518,-400.5782;Inherit;False;Property;_TintBack;Tint Back;7;0;Create;True;0;0;False;0;False;0,0,0,0;0.6037736,0.424279,0.259167,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;13;-1792.323,6.097504;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.VertexColorNode;9;-1245.604,94.38989;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;11;-1320.929,-75.46741;Inherit;False;Property;_Intensity;Intensity;1;0;Create;True;0;0;False;0;False;0;0.01;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-972.2064,-206.8164;Inherit;False;Property;_transparency;transparency;3;0;Create;True;0;0;True;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;7;-1830.009,358.7238;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;18;-687.6179,-547.4782;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;15;-696.6546,-851.452;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;False;-1;None;e8004bad833b7f54e8f211f75760a69b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-911.7173,-50.8302;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DitheringNode;17;-539.5617,-200.8853;Inherit;False;0;False;3;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-760.7416,333.4296;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-186.0251,-212.7795;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-213.0235,-488.2064;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;21;-140.9526,243.8404;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-505.0534,53.81702;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;293.5608,-384.0883;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Wind Tree;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;0
WireConnection;10;1;2;2
WireConnection;10;0;2;3
WireConnection;8;0;4;0
WireConnection;13;0;4;0
WireConnection;7;0;3;0
WireConnection;18;0;12;0
WireConnection;18;1;14;0
WireConnection;16;0;3;0
WireConnection;16;1;13;0
WireConnection;16;2;11;0
WireConnection;16;3;9;1
WireConnection;17;0;6;0
WireConnection;19;0;7;0
WireConnection;19;1;10;0
WireConnection;19;2;5;0
WireConnection;19;3;8;0
WireConnection;22;0;15;4
WireConnection;22;1;17;0
WireConnection;23;0;15;0
WireConnection;23;1;18;0
WireConnection;21;0;20;0
WireConnection;20;0;16;0
WireConnection;20;1;19;0
WireConnection;0;0;23;0
WireConnection;0;10;22;0
WireConnection;0;11;20;0
ASEEND*/
//CHKSM=439342EEFFC2013D4306FD81961C3399A78A94A6