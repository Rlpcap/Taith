// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Ice_Models"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
		_Frost("Frost", 2D) = "white" {}
		_ColorTint("Color Tint", Color) = (0,0,0,0)
		_IceSlider("Ice Slider", Range( 0 , 1)) = 0
		_IceLengths("IceLengths", Range( 0 , 1)) = 0
		_Noise("Noise", 2D) = "white" {}
		_MaskTile("Mask Tile", Range( 0 , 1)) = 1
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 4
		_TessMin( "Tess Min Distance", Float ) = 2
		_TessMax( "Tess Max Distance", Float ) = 20
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
			float3 worldPos;
		};

		uniform float _IceLengths;
		uniform sampler2D _Noise;
		uniform float _MaskTile;
		uniform float _IceSlider;
		uniform sampler2D _Texture;
		uniform float4 _Texture_ST;
		uniform sampler2D _Frost;
		uniform float4 _Frost_ST;
		uniform float4 _ColorTint;
		uniform float _TessValue;
		uniform float _TessMin;
		uniform float _TessMax;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityDistanceBasedTess( v0.vertex, v1.vertex, v2.vertex, _TessMin, _TessMax, _TessValue );
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_vertexNormal = v.normal.xyz;
			float3 ase_worldNormal = UnityObjectToWorldNormal( v.normal );
			float2 appendResult20 = (float2(ase_worldNormal.x , ase_worldNormal.z));
			float4 OffsetMask25 = tex2Dlod( _Noise, float4( (( _MaskTile * appendResult20 )*1.0 + 0.0), 0, 0.0) );
			float IceSlider29 = _IceSlider;
			float Mask12 = saturate( ( IceSlider29 * ( ase_worldNormal.y * -0.3 ) ) );
			float YMaskTop42 = saturate( ( ase_worldNormal.y * 3.0 ) );
			float4 VertexOffset17 = ( float4( ase_vertexNormal , 0.0 ) * ( ( _IceLengths * ( OffsetMask25 * Mask12 ) ) + ( (0.0 + (IceSlider29 - 0.0) * (0.01 - 0.0) / (1.0 - 0.0)) * YMaskTop42 ) ) );
			v.vertex.xyz += VertexOffset17.rgb;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
			float2 uv_Frost = i.uv_texcoord * _Frost_ST.xy + _Frost_ST.zw;
			float4 tex2DNode1 = tex2D( _Frost, uv_Frost );
			float IceSlider29 = _IceSlider;
			float3 ase_worldNormal = i.worldNormal;
			float Mask12 = saturate( ( IceSlider29 * ( ase_worldNormal.y * -0.3 ) ) );
			float4 lerpResult2 = lerp( tex2D( _Texture, uv_Texture ) , tex2DNode1 , saturate( ( Mask12 * 8.0 ) ));
			float4 lerpResult36 = lerp( lerpResult2 , tex2DNode1 , IceSlider29);
			float4 LerpTexts5 = lerpResult36;
			o.Albedo = LerpTexts5.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV50 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode50 = ( 0.0 + 3.0 * pow( 1.0 - fresnelNdotV50, 2.5 ) );
			float4 Emission55 = ( ( ( tex2D( _Frost, uv_Frost ) * fresnelNode50 ) * IceSlider29 ) * _ColorTint );
			o.Emission = Emission55.rgb;
			o.Smoothness = IceSlider29;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;0;1920;1019;3691.894;1600.327;2.845119;True;True
Node;AmplifyShaderEditor.CommentaryNode;32;-3539.86,-1430.458;Inherit;False;718.7522;167.9373;Comment;2;4;29;MasterSlider;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-3489.86,-1377.521;Inherit;False;Property;_IceSlider;Ice Slider;3;0;Create;True;0;0;False;0;False;0;0.287;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;8;-2980.726,-263.3348;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;28;-2562.792,-592.6089;Inherit;False;1404.374;323.7487;Comment;6;21;24;20;23;22;25;Offset Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;13;-2499.672,-190.2661;Inherit;False;1092.134;781.0293;Y Mask;8;42;41;40;12;11;10;30;9;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;29;-3064.108,-1380.458;Inherit;False;IceSlider;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-2512.792,-542.6089;Inherit;False;Property;_MaskTile;Mask Tile;6;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;20;-2491.086,-438.0176;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-2447.458,-0.4448898;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;-0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;30;-2378.568,-125.5262;Inherit;False;29;IceSlider;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-2180.431,-473.215;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-2095.787,-140.2661;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;24;-1990.3,-471.99;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-2422.01,306.7388;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;11;-1826.193,-98.79008;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;21;-1749.857,-498.8601;Inherit;True;Property;_Noise;Noise;5;0;Create;True;0;0;False;0;False;-1;None;a1c34e2954692534b83ca077f6c5a1d5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;41;-2085.506,370.3464;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;7;-2311.243,-1396.677;Inherit;False;915.189;615.9999;Textures;8;1;3;2;5;31;33;35;36;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;12;-1629.182,-92.5687;Inherit;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;-1401.417,-500.7254;Inherit;False;OffsetMask;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;18;-3636.26,307.3409;Inherit;False;898.0999;973.2206;Vertex Offset;12;17;16;27;14;15;26;43;44;46;47;57;58;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;31;-2270.24,-875.0354;Inherit;False;12;Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;45;-3884.096,762.2756;Inherit;False;29;IceSlider;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;42;-1816.711,397.0206;Inherit;False;YMaskTop;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;15;-3575.26,599.7944;Inherit;False;12;Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;26;-3593.936,527.122;Inherit;False;25;OffsetMask;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-3359.077,552.3214;Inherit;False;Property;_IceLengths;IceLengths;4;0;Create;True;0;0;False;0;False;0;0.5294118;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;50;-3767.195,1910.903;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;3;False;3;FLOAT;2.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;44;-3712.532,980.9937;Inherit;False;42;YMaskTop;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;47;-3630.156,752.783;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-2069.372,-880.7922;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;8;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-3343.936,693.122;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;48;-3810.156,1672.27;Inherit;True;Property;_TextureSample2;Texture Sample 2;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-2261.243,-1346.677;Inherit;True;Property;_Texture;Texture;0;0;Create;True;0;0;False;0;False;-1;None;61f7829d8d82907438be24cd9f24082a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;35;-1917.844,-882.74;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-3320.135,875.9703;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-3135.077,676.3214;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;52;-3441.925,1927.966;Inherit;False;29;IceSlider;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-3404.599,1797.858;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-2245.494,-1117.677;Inherit;True;Property;_Frost;Frost;1;0;Create;True;0;0;False;0;False;-1;None;de2c8ebae3f32c24e87380ccb7fc75e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;43;-2939.904,733.3052;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;14;-3358.371,363.9571;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;37;-1941.437,-758.0154;Inherit;False;29;IceSlider;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;2;-1892.243,-1165.677;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;54;-3158.149,1993.579;Inherit;False;Property;_ColorTint;Color Tint;2;0;Create;True;0;0;False;0;False;0,0,0,0;0.3018868,0.3018868,0.3018868,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-3128.385,1835.184;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-3045.514,480.8572;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;36;-1691.18,-1019.942;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-2860.449,1871.379;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;55;-2651.149,1890.879;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;17;-2869.231,468.3011;Inherit;True;VertexOffset;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;5;-1600.154,-1152.096;Inherit;False;LerpTexts;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;6;-259.1104,11.41862;Inherit;False;5;LerpTexts;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;56;-260.1808,89.4332;Inherit;False;55;Emission;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-312.2025,275.5381;Inherit;False;17;VertexOffset;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;38;-258.2288,171.7704;Inherit;False;29;IceSlider;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Ice_Models;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;0;4;2;20;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;7;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;29;0;4;0
WireConnection;20;0;8;1
WireConnection;20;1;8;3
WireConnection;9;0;8;2
WireConnection;23;0;22;0
WireConnection;23;1;20;0
WireConnection;10;0;30;0
WireConnection;10;1;9;0
WireConnection;24;0;23;0
WireConnection;40;0;8;2
WireConnection;11;0;10;0
WireConnection;21;1;24;0
WireConnection;41;0;40;0
WireConnection;12;0;11;0
WireConnection;25;0;21;0
WireConnection;42;0;41;0
WireConnection;47;0;45;0
WireConnection;33;0;31;0
WireConnection;27;0;26;0
WireConnection;27;1;15;0
WireConnection;35;0;33;0
WireConnection;46;0;47;0
WireConnection;46;1;44;0
WireConnection;57;0;58;0
WireConnection;57;1;27;0
WireConnection;49;0;48;0
WireConnection;49;1;50;0
WireConnection;43;0;57;0
WireConnection;43;1;46;0
WireConnection;2;0;3;0
WireConnection;2;1;1;0
WireConnection;2;2;35;0
WireConnection;51;0;49;0
WireConnection;51;1;52;0
WireConnection;16;0;14;0
WireConnection;16;1;43;0
WireConnection;36;0;2;0
WireConnection;36;1;1;0
WireConnection;36;2;37;0
WireConnection;53;0;51;0
WireConnection;53;1;54;0
WireConnection;55;0;53;0
WireConnection;17;0;16;0
WireConnection;5;0;36;0
WireConnection;0;0;6;0
WireConnection;0;2;56;0
WireConnection;0;4;38;0
WireConnection;0;11;19;0
ASEEND*/
//CHKSM=D6E807F2BEBC3BEE5E7CE13B5F7666E01F8731A9