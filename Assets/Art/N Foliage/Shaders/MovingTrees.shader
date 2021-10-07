// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Foliage/MovingTrees"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Texture("Texture", 2D) = "white" {}
		_TintFront1("Tint Front", Color) = (0,0,0,0)
		_TintBack1("Tint Back", Color) = (0,0,0,0)
		_transparency1("transparency", Range( 0 , 1)) = 0
		_Intensity1("Intensity", Range( 0 , 1)) = 0
		_Movement1("Movement", Float) = 0
		[Toggle(_YORZ1_ON)] _YorZ1("YorZ", Float) = 0
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
		#pragma shader_feature_local _YORZ1_ON
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			half ASEVFace : VFACE;
			float4 screenPosition;
		};

		uniform float _Intensity1;
		uniform float _Movement1;
		uniform sampler2D _Texture;
		uniform float4 _Texture_ST;
		uniform float4 _TintFront1;
		uniform float4 _TintBack1;
		uniform float _transparency1;
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
			float temp_output_4_0 = sin( _Time.y );
			float3 _Vector1 = float3(1,0,0);
			float3 ase_vertex3Pos = v.vertex.xyz;
			#ifdef _YORZ1_ON
				float staticSwitch12 = ase_vertex3Pos.z;
			#else
				float staticSwitch12 = ase_vertex3Pos.y;
			#endif
			float3 temp_output_22_0 = ( ( temp_output_4_0 * _Vector1 * _Intensity1 * v.color.r ) + ( temp_output_4_0 * staticSwitch12 * _Movement1 * _Vector1 ) );
			v.vertex.xyz += temp_output_22_0;
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			o.screenPosition = ase_screenPos;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
			float4 tex2DNode15 = tex2D( _Texture, uv_Texture );
			float4 switchResult16 = (((i.ASEVFace>0)?(_TintFront1):(_TintBack1)));
			o.Albedo = ( tex2DNode15 * switchResult16 ).rgb;
			o.Alpha = 1;
			float4 ase_screenPos = i.screenPosition;
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 clipScreen17 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither17 = Dither4x4Bayer( fmod(clipScreen17.x, 4), fmod(clipScreen17.y, 4) );
			dither17 = step( dither17, _transparency1 );
			clip( ( tex2DNode15.a * dither17 ) - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;0;1920;1019;3379.644;-899.9719;2.31968;True;True
Node;AmplifyShaderEditor.SimpleTimeNode;1;-2611.583,795.2209;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;2;-1679.48,743.4789;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;3;-2381.079,627.6409;Inherit;False;Constant;_Vector1;Vector 0;0;0;Create;True;0;0;False;0;False;1,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StaticSwitch;12;-1324.471,746.8557;Inherit;False;Property;_YorZ1;YorZ;7;0;Create;True;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1365.183,1080.9;Inherit;False;Property;_Movement1;Movement;6;0;Create;True;0;0;False;0;False;0;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;9;-1144.784,-268.1057;Inherit;False;Property;_TintFront1;Tint Front;2;0;Create;True;0;0;False;0;False;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;13;-1943.329,442.6753;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;5;-1164.956,-57.86428;Inherit;False;Property;_TintBack1;Tint Back;3;0;Create;True;0;0;False;0;False;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;8;-1396.61,530.9677;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;11;-1471.935,361.1104;Inherit;False;Property;_Intensity1;Intensity;5;0;Create;True;0;0;False;0;False;0;0.15;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1123.212,229.7614;Inherit;False;Property;_transparency1;transparency;4;0;Create;True;0;0;True;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;7;-1975.631,929.8921;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SinOpNode;4;-2362.4,799.0547;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DitheringNode;17;-690.5677,235.6925;Inherit;False;0;False;3;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1062.723,385.7476;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-911.7476,770.0075;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;15;-1097.66,-643.9197;Inherit;True;Property;_Texture;Texture;1;0;Create;True;0;0;False;0;False;-1;None;be0de7d43eedcf54fb5a6a8a94f674ca;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SwitchByFaceNode;16;-841.3058,-143.0823;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-337.0311,223.7983;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-291.9586,680.4183;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-364.0295,-51.62853;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-656.0594,490.3948;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Foliage/MovingTrees;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;12;1;2;2
WireConnection;12;0;2;3
WireConnection;13;0;3;0
WireConnection;7;0;3;0
WireConnection;4;0;1;0
WireConnection;17;0;6;0
WireConnection;18;0;4;0
WireConnection;18;1;13;0
WireConnection;18;2;11;0
WireConnection;18;3;8;1
WireConnection;19;0;4;0
WireConnection;19;1;12;0
WireConnection;19;2;14;0
WireConnection;19;3;7;0
WireConnection;16;0;9;0
WireConnection;16;1;5;0
WireConnection;23;0;15;4
WireConnection;23;1;17;0
WireConnection;20;0;22;0
WireConnection;21;0;15;0
WireConnection;21;1;16;0
WireConnection;22;0;18;0
WireConnection;22;1;19;0
WireConnection;0;0;21;0
WireConnection;0;10;23;0
WireConnection;0;11;22;0
ASEEND*/
//CHKSM=3302FADD7825BA0D5591557541C71B3D9ACB77EB