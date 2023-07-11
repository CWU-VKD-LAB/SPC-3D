Shader "Unlit/HeightPlane"
//makes a square of connected dots based off their four positions
{
	Properties
	{
		_Pos0("Pos0", Float) = -2.836596
		_Pos1("Pos1", Float) = -2.836596
		_Pos2("Pos2", Float) = -2.836596
		_Pos3("Pos3", Float) = -2.836596
		_Transparency("Transparency", Range(0, 1)) = 0.5
		_Thickness("Thickness", Range(0, 5)) = 0.01
		_Color("Color", Color) = (0, 1, 0.73, 1)
	}

		SubShader
	{
		Pass
		{
			//Ztest Always
			ZWrite Off
			Tags {"LightMode" = "UniversalForward"}
			Blend SrcAlpha OneMinusSrcAlpha
			LOD 100

			HLSLPROGRAM
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag
			// include file that contains UnityObjectToWorldNormal helper function
			#include "UnityCG.cginc"
			float _Transparency;
			float4 _Color;
			float _Pos0;
			float _Pos1;
			float _Pos2;
			float _Pos3;
			float _Thickness;

			struct v2g
			{
				float4 vertex : SV_POSITION;
				float4 color : TEXCOORD0;
			};

			struct g2f
			{
				float4 vertex : SV_POSITION;
				float4 color : TEXCOORD0;

			};

			v2g vert(appdata_full v)
			{
				v2g o;
				o.vertex = v.vertex;
				o.color = 0;
				return o;
			}

			struct Offsets
			{
				float4 one;
				float4 two;
			};

			Offsets lineThickness(float thicknesss, float4 p1, float4 p2)
			{
				float2 dir = normalize(p2.xy - p1.xy);
				float2 normal = float2(-dir.y, dir.x);

				Offsets offs;
				offs.one = float4(normal * thicknesss / 2.0f, thicknesss, 0);
				offs.two = float4(normal * thicknesss / 2.0f, thicknesss, 0);
				return offs;
			}

			//makes a square of connected dots based off their four positions
			[maxvertexcount(25)]
			void geom(point v2g IN[1], inout TriangleStream<g2f> vert)
			{
				g2f o[25];

				float4 p0 = UnityObjectToClipPos(float4(-2.53, _Pos0, -2.53, IN[0].vertex.w));
				float4 p1 = UnityObjectToClipPos(float4(2.53, _Pos1, -2.53, IN[0].vertex.w));
				float4 p2 = UnityObjectToClipPos(float4(2.53, _Pos2, 2.53, IN[0].vertex.w));
				float4 p3 = UnityObjectToClipPos(float4(-2.53, _Pos3, 2.53, IN[0].vertex.w));

				Offsets offsets1 = lineThickness(_Thickness, p0, p1);
				o[0].vertex = p0 + offsets1.one;
				o[1].vertex = p0;
				o[2].vertex = p1 + offsets1.two;
				o[3].vertex = p1;

				o[0].color = _Color;
				o[1].color = _Color;
				o[2].color = _Color;
				o[3].color = _Color;

				vert.Append(o[0]);
				vert.Append(o[1]);
				vert.Append(o[2]);
				vert.Append(o[3]);

				Offsets offsets2 = lineThickness(_Thickness, p1, p2);
				o[4].vertex = p1;
				o[5].vertex = p1 - offsets2.one;
				o[6].vertex = p2;
				o[7].vertex = p2 - offsets2.two;

				o[4].color = _Color;
				o[5].color = _Color;
				o[6].color = _Color;
				o[7].color = _Color;

				vert.Append(o[4]);
				vert.Append(o[5]);
				vert.Append(o[6]);
				vert.Append(o[7]);

				Offsets offsets3 = lineThickness(_Thickness, p2, p3);
				o[8].vertex = p2;
				o[9].vertex = p2 - offsets3.one;
				o[10].vertex = p3;
				o[11].vertex = p3 - offsets3.two;

				o[8].color = _Color;
				o[9].color = _Color;
				o[10].color = _Color;
				o[11].color = _Color;

				vert.Append(o[8]);
				vert.Append(o[9]);
				vert.Append(o[10]);
				vert.Append(o[11]);

				Offsets offsets4 = lineThickness(_Thickness, p3, p0);
				o[12].vertex = p3 + offsets4.one;
				o[13].vertex = p3;
				o[14].vertex = p0 + offsets4.two;
				o[15].vertex = p0;

				o[12].color = _Color;
				o[13].color = _Color;
				o[14].color = _Color;
				o[15].color = _Color;

				vert.Append(o[12]);
				vert.Append(o[13]);
				vert.Append(o[14]);
				vert.Append(o[15]);

				vert.RestartStrip();
			}

			

			////makes a square of connected dots based off their four positions
			//[maxvertexcount(6)]
			//void geom(point v2g IN[1], inout LineStream <g2f> vert)
			//{
			//	g2f o[5];
			//	o[0].vertex = UnityObjectToClipPos(float3(-2.5, _Pos0, -2.51));
			//	o[0].color = _Color;// +0.5;
			//	vert.Append(o[0]);
			//	o[1].vertex = UnityObjectToClipPos(float3(2.5, _Pos1, -2.51));
			//	o[1].color = _Color;// -0.5;
			//	vert.Append(o[1]);
			//	o[2].vertex = UnityObjectToClipPos(float3(2.5, _Pos2, 2.51));
			//	o[2].color = _Color;// -0.8;
			//	vert.Append(o[2]);
			//	o[3].vertex = UnityObjectToClipPos(float3(-2.5, _Pos3, 2.51));
			//	o[3].color = _Color;// -0.5;
			//	vert.Append(o[3]);
			//	//makes a full connected square
			//	o[4].vertex = UnityObjectToClipPos(float3(-2.5, _Pos0, -2.51));
			//	o[4].color = _Color;// +0.5;
			//	vert.Append(o[4]);
			//	vert.RestartStrip();
			//}
			fixed4 frag(g2f i) : SV_Target
			{
				fixed4 col;
				col.rgb = i.color.rgb;;
				col.a = _Transparency;
				return col;
			}
			ENDHLSL
		}
	}
}
