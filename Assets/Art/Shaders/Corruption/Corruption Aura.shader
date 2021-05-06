// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Corruption Aura"
{
	Properties
	{
		_Mask("Mask", 2D) = "white" {}
		_DistortionMap("DistortionMap", 2D) = "white" {}
		_DistortionAmmount("DistortionAmmount", Range( 0 , 1)) = 0
		_ScrollSpeed("ScrollSpeed", Range( 0 , 1)) = 0
		_Color2("Color 2", Color) = (0,0,0,0)
		_Texture("Texture", 2D) = "white" {}
		_SpeadAmount("SpeadAmount", Range( 0 , 1)) = 0.6030607
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		AlphaToMask On
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Texture;
		uniform float4 _Texture_ST;
		uniform float4 _Color2;
		uniform sampler2D _Mask;
		uniform sampler2D _DistortionMap;
		uniform float4 _DistortionMap_ST;
		uniform float _DistortionAmmount;
		uniform float _ScrollSpeed;
		uniform float _SpeadAmount;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
			float4 color17 = IsGammaSpace() ? float4(0.05899113,0.02024741,0.1226415,0) : float4(0.004793852,0.001567137,0.01390275,0);
			float2 uv_DistortionMap = i.uv_texcoord * _DistortionMap_ST.xy + _DistortionMap_ST.zw;
			float2 panner10 = ( ( _Time.y * _ScrollSpeed ) * float2( 0,-1 ) + float2( 0,0 ));
			float2 uv_TexCoord8 = i.uv_texcoord + panner10;
			float4 tex2DNode2 = tex2D( _Mask, ( ( (UnpackNormal( tex2D( _DistortionMap, uv_DistortionMap ) )).xy * _DistortionAmmount ) + uv_TexCoord8 ) );
			float4 lerpResult18 = lerp( color17 , _Color2 , tex2DNode2);
			float4 temp_cast_0 = (3.0).xxxx;
			float4 temp_cast_1 = (_SpeadAmount).xxxx;
			float4 temp_output_23_0 = step( tex2DNode2 , temp_cast_1 );
			float4 temp_cast_2 = (_SpeadAmount).xxxx;
			float4 temp_cast_3 = (( _SpeadAmount / 1.1 )).xxxx;
			float4 lerpResult25 = lerp( ( tex2D( _Texture, uv_Texture ) * float4( 0.1226415,0.1226415,0.1226415,0 ) ) , ( pow( lerpResult18 , temp_cast_0 ) * 3.0 ) , ( temp_output_23_0 + ( temp_output_23_0 - step( tex2DNode2 , temp_cast_3 ) ) ));
			o.Emission = lerpResult25.rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			AlphaToMask Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
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
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
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
0;0;1920;1019;2069.469;809.7827;2.2;True;True
Node;AmplifyShaderEditor.CommentaryNode;48;-2694.575,-360.4215;Inherit;False;1862.961;907.1392;Distortion;13;13;15;3;4;14;5;7;10;8;6;9;2;1;Base Distortion;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;3;-2464.016,-206.9993;Inherit;True;Property;_DistortionMap;DistortionMap;2;0;Create;True;0;0;False;0;False;None;45b7d34a07e91d14f8fd42ca2b072fe0;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleTimeNode;13;-2575.675,322.5176;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-2644.575,431.7177;Inherit;False;Property;_ScrollSpeed;ScrollSpeed;4;0;Create;True;0;0;False;0;False;0;0.111;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-2142.661,-138.1065;Inherit;True;Property;_TextureSample1;Texture Sample 1;3;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-2303.975,373.2176;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1846.307,107.3739;Inherit;False;Property;_DistortionAmmount;DistortionAmmount;3;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;10;-2050.404,323.8428;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;5;-1797.438,-130.2562;Inherit;True;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;50;-649.5067,318.4794;Inherit;False;1640.536;721.7322;Comment;8;34;24;22;33;23;32;35;36;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-1510.308,31.3739;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-1829.999,278.1269;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;49;-626.9303,-1032.214;Inherit;False;1187.951;569.4151;Colors;6;16;17;18;21;19;20;Colors;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-1385,221.1269;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;-1486.411,-241.8707;Inherit;True;Property;_Mask;Mask;1;0;Create;True;0;0;False;0;False;None;ed200a308c3598c489313a6dd109b644;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-542.5983,725.8761;Inherit;False;Property;_SpeadAmount;SpeadAmount;7;0;Create;True;0;0;False;0;False;0.6030607;0.5088823;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-465.9622,880.6382;Inherit;False;Constant;_DivideAmount;DivideAmount;7;0;Create;True;0;0;False;0;False;1.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;17;-569.9639,-982.2144;Inherit;False;Constant;_Color1;Color 1;5;0;Create;True;0;0;False;0;False;0.05899113,0.02024741,0.1226415,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;16;-576.9303,-774.9645;Inherit;False;Property;_Color2;Color 2;5;0;Create;True;0;0;False;0;False;0,0,0,0;0.4766821,0.4466002,0.490566,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;33;-144.3072,787.2116;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-946.5168,-135.6569;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;21;-124.8267,-577.7993;Inherit;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;32;176.0133,623.0474;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;18;-190.2378,-806.7966;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;23;-26.91615,368.4794;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.2;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;26;187.7727,-220.4563;Inherit;True;Property;_Texture;Texture;6;0;Create;True;0;0;False;0;False;-1;None;116bd1fa44352be4ab1d4dcce10b516e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;35;476.3137,509.6007;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;19;81.04131,-923.9708;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;538.8095,-217.2593;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.1226415,0.1226415,0.1226415,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;326.0206,-785.7952;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;756.0296,406.0458;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;47;-2709.083,659.3502;Inherit;False;1893.313;604.5759;Heat Wave;8;41;40;37;42;44;45;43;46;Heat Distortion;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;41;-2659.083,742.1618;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;40;-2336.083,714.1618;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;37;-1992.428,709.3502;Inherit;True;Property;_TextureSample3;Texture Sample 3;7;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;42;-1616.679,721.9709;Inherit;True;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;46;-969.7711,915.9233;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-1611.814,943.2032;Inherit;False;Property;_HeatWave;Heat Wave;8;0;Create;True;0;0;False;0;False;0.2535692;0.05805963;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;25;1070.003,-257.3771;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;22;-599.5067,487.4753;Inherit;True;Property;_TextureSample2;Texture Sample 2;6;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;45;-1548.149,1107.926;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-1283.814,728.2034;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1405.585,-283.5428;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Corruption Aura;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;5;True;True;0;True;Transparent;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;True;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;3;0
WireConnection;14;0;13;0
WireConnection;14;1;15;0
WireConnection;10;1;14;0
WireConnection;5;0;4;0
WireConnection;6;0;5;0
WireConnection;6;1;7;0
WireConnection;8;1;10;0
WireConnection;9;0;6;0
WireConnection;9;1;8;0
WireConnection;33;0;24;0
WireConnection;33;1;34;0
WireConnection;2;0;1;0
WireConnection;2;1;9;0
WireConnection;32;0;2;0
WireConnection;32;1;33;0
WireConnection;18;0;17;0
WireConnection;18;1;16;0
WireConnection;18;2;2;0
WireConnection;23;0;2;0
WireConnection;23;1;24;0
WireConnection;35;0;23;0
WireConnection;35;1;32;0
WireConnection;19;0;18;0
WireConnection;19;1;21;0
WireConnection;29;0;26;0
WireConnection;20;0;19;0
WireConnection;20;1;21;0
WireConnection;36;0;23;0
WireConnection;36;1;35;0
WireConnection;40;0;41;0
WireConnection;37;0;3;0
WireConnection;37;1;40;0
WireConnection;42;0;37;0
WireConnection;46;0;43;0
WireConnection;46;1;45;0
WireConnection;25;0;29;0
WireConnection;25;1;20;0
WireConnection;25;2;36;0
WireConnection;22;0;1;0
WireConnection;22;1;46;0
WireConnection;43;0;42;0
WireConnection;43;1;44;0
WireConnection;0;2;25;0
ASEEND*/
//CHKSM=B0754F6B4DBD0903C9E8957A8DFE81057FA96C6C