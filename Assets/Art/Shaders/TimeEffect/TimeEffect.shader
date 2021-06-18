// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TimeEffect"
{
	Properties
	{
		_enemyPos1("enemyPos", Vector) = (0,0,0,0)
		_radius("radius", Float) = 0
		_FallOff2("Fall Off", Float) = 0
		_GrassTex1("GrassTex", 2D) = "white" {}
		_NoiseScale2("NoiseScale", Vector) = (1.38,0.01,0,0)
		_NoiseScrollSpeed2("NoiseScrollSpeed", Float) = 0.34
		_EdgeColor2("EdgeColor", Color) = (0.5813924,0.2198736,0.764151,0)
		_TimeColor("TimeColor", Color) = (0.1320755,0.1320755,0.1320755,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform float4 _TimeColor;
		uniform sampler2D _GrassTex1;
		uniform float4 _GrassTex1_ST;
		uniform float4 _EdgeColor2;
		uniform float3 _enemyPos1;
		uniform float _radius;
		uniform float _FallOff2;
		uniform float _NoiseScrollSpeed2;
		uniform float2 _NoiseScale2;


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


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_GrassTex1 = i.uv_texcoord * _GrassTex1_ST.xy + _GrassTex1_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float Mask14 = saturate( pow( ( distance( _enemyPos1 , ase_worldPos ) / _radius ) , _FallOff2 ) );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 normalizeResult39 = normalize( float3(0.76,1,0) );
			float simplePerlin2D36 = snoise( ( ase_vertex3Pos + ( ( normalizeResult39 * _NoiseScrollSpeed2 ) * _Time.y ) ).xy*_NoiseScale2.x );
			simplePerlin2D36 = simplePerlin2D36*0.5 + 0.5;
			float Noise16 = simplePerlin2D36;
			float4 lerpResult42 = lerp( _TimeColor , tex2D( _GrassTex1, uv_GrassTex1 ) , ( _EdgeColor2 * step( ( ( ( 1.0 - Mask14 ) * Noise16 ) - Mask14 ) , 0.0 ) ));
			o.Albedo = lerpResult42.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;0;1920;1019;2240.078;748.5421;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;1;-3539.131,-729.2092;Inherit;False;1794.989;768.6633;;11;44;43;41;39;38;37;36;35;34;33;16;Fade;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector3Node;37;-3489.131,-501.5029;Inherit;False;Constant;_NoiseDirection2;NoiseDirection;5;0;Create;True;0;0;False;0;False;0.76,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;5;-2644.127,537.3397;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;6;-2676.299,345.9418;Inherit;False;Property;_enemyPos1;enemyPos;0;0;Create;True;0;0;False;0;False;0,0,0;59.48609,4.16,-37.53485;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;39;-3257.839,-477.4716;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-2255.16,553.1576;Inherit;False;Property;_radius;radius;2;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-3364.473,-256.6927;Inherit;False;Property;_NoiseScrollSpeed2;NoiseScrollSpeed;7;0;Create;True;0;0;False;0;False;0.34;0.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;8;-2396.418,451.3886;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;43;-3368.979,-139.547;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;10;-2078.461,451.4574;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1998.675,559.5429;Inherit;False;Property;_FallOff2;Fall Off;3;0;Create;True;0;0;False;0;False;0;3.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-3080.614,-414.3925;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-2943.942,-253.6898;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;11;-1842.675,450.5424;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;44;-3324.373,-679.2093;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;12;-1637.536,449.9472;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;35;-2717.165,-550.7068;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector2Node;41;-2661.364,-289.5267;Inherit;False;Property;_NoiseScale2;NoiseScale;6;0;Create;True;0;0;False;0;False;1.38,0.01;4.8,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;14;-1452.235,443.5073;Inherit;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;36;-2323.313,-512.8828;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;16;-1987.144,-452.8105;Inherit;False;Noise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;15;-1361.449,658.9658;Inherit;False;14;Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;17;-1350.971,746.5523;Inherit;False;16;Noise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;18;-1136.451,641.665;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-1127.036,891.6171;Inherit;False;14;Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-950.8173,661.1567;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;26;-709.8173,809.1568;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-248.6998,1581.64;Inherit;False;Property;_EdgeColor2;EdgeColor;8;0;Create;True;0;0;False;0;False;0.5813924,0.2198736,0.764151,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;2;-420.6858,1226.207;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-118.3089,743.5225;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;45;-1060.438,-562.1119;Inherit;False;Property;_TimeColor;TimeColor;10;0;Create;True;0;0;False;0;False;0.1320755,0.1320755,0.1320755,0;0.1320755,0.1320755,0.1320755,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;31;-1292.384,55.66889;Inherit;True;Property;_GrassTex1;GrassTex;4;0;Create;True;0;0;False;0;False;-1;None;116bd1fa44352be4ab1d4dcce10b516e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;23;-1424.691,-363.9308;Inherit;True;Property;_Texture0;Texture 0;1;0;Create;True;0;0;False;0;False;None;c65b114eeb365e54a9d77845008e81f4;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.LerpOp;42;-550.0082,-13.07379;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;13;-2500.885,-904.3931;Inherit;True;Property;_Noise1;Noise;9;0;Create;True;0;0;False;0;False;-1;None;759a45d2aa606de469cde71db0d2136c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;21;-1393.734,-129.4266;Inherit;False;Property;_IceTiling2;Ice Tiling;5;0;Create;True;0;0;False;0;False;1;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;24;-1192.052,-120.6976;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TriplanarNode;29;-1030.373,-186.6887;Inherit;True;Spherical;World;False;Top Texture 0;_TopTexture0;white;0;None;Mid Texture 0;_MidTexture0;white;-1;None;Bot Texture 0;_BotTexture0;white;-1;None;Triplanar Sampler;False;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;40;-1199.037,266.1603;Inherit;False;14;Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;TimeEffect;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;39;0;37;0
WireConnection;8;0;6;0
WireConnection;8;1;5;0
WireConnection;10;0;8;0
WireConnection;10;1;7;0
WireConnection;33;0;39;0
WireConnection;33;1;34;0
WireConnection;38;0;33;0
WireConnection;38;1;43;2
WireConnection;11;0;10;0
WireConnection;11;1;9;0
WireConnection;12;0;11;0
WireConnection;35;0;44;0
WireConnection;35;1;38;0
WireConnection;14;0;12;0
WireConnection;36;0;35;0
WireConnection;36;1;41;0
WireConnection;16;0;36;0
WireConnection;18;0;15;0
WireConnection;20;0;18;0
WireConnection;20;1;17;0
WireConnection;26;0;20;0
WireConnection;26;1;19;0
WireConnection;2;0;26;0
WireConnection;4;0;3;0
WireConnection;4;1;2;0
WireConnection;42;0;45;0
WireConnection;42;1;31;0
WireConnection;42;2;4;0
WireConnection;24;0;21;0
WireConnection;24;1;21;0
WireConnection;29;0;23;0
WireConnection;29;3;24;0
WireConnection;0;0;42;0
ASEEND*/
//CHKSM=FB845AFD64B39DA16D66672DC3AA9AC4C0F243B1