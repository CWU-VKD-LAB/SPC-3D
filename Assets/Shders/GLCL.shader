Shader "Unlit/GLCL"
//sets an RGB coordinate based off 4 values
{
	Properties
	{
		//arbitrary default positions
		//_Pos0("Pos0", Vector) = (0,0,0,0)
		//_Pos1("Pos1", Vector) = (0,1,1,0)
		//_Pos2("Pos2", Vector) = (0,1,1,0)
		//_Pos3("Pos3", Vector) = (1,2,0,0)
		//_Pos4("Pos4", Vector) = (1,2,0,0)

		_Color("Color", Color) = (1,1,1,1)
		_Point("Point", Vector) = (1,1,1,1)
		_Transparency("Transparency", Range(0, 1)) = 0.5
		_AttribCount("attrib Count", Int) = 5
	}

		SubShader
	{

		Pass
		{
			//Ztest Always
			Tags {"LightMode" = "UniversalForward"}
			//Blend SrcAlpha OneMinusSrcAlpha

			HLSLPROGRAM
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag
			// include file that contains UnityObjectToWorldNormal helper function
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc" // for _LightColor0

			float _Transparency;
			float4 _Pos0;
			float4 _Pos1;
			float4 _Pos2;
			float4 _Pos3;
			float4 _Pos4;
			float4 _Point;
			float4 _Color;
			static float3 fadedCol = 0.2;
			bool faded;
			int _AttribCount;
			
			//we need to know how to access the correct element
			struct GLCLLine
			{
				float4 GLCLPos;
				float3 color;
				int totalLines;
				int attribCount; 
			};

			StructuredBuffer<GLCLLine> glclLines;

			StructuredBuffer<float4> GLCLPos;

			struct v2g
			{
				float4 vertex : SV_POSITION;
			};

			struct g2f
			{
				float4 vertex : SV_POSITION;
				float3 color : TEXCOORD0;
			};

			v2g vert(appdata_full v)
			{
				v2g o;
				o.vertex = v.vertex;
				return o;
			}

			//sets an RGB coordinate based off 4 values
			[maxvertexcount(512)]
			void geom(point v2g IN[1], inout LineStream <g2f> vert)
			{

				g2f o[146];
				int x = 0;
				
				for (int j = 0; j < glclLines[0].totalLines; j++)
				{
					for (int i = 0; i < glclLines[0].attribCount + 1; i++)//needs to be automated
					{
						o[x].vertex = UnityObjectToClipPos(glclLines[i].GLCLPos);
						vert.Append(o[x]);
						x++;
						o[x].vertex = UnityObjectToClipPos(glclLines[i + 1].GLCLPos);
						vert.Append(o[x]);
						vert.RestartStrip();
						x++;
					}
				}
				/*o[0].vertex = UnityObjectToClipPos(_Pos0);
				o[0].color = faded ? fadedCol : _Color;
				vert.Append(o[0]);
				o[1].vertex = UnityObjectToClipPos(_Pos1);
				o[1].color = faded ? fadedCol : _Color;
				vert.Append(o[1]);
				vert.RestartStrip();

				o[2].vertex = UnityObjectToClipPos(_Pos1);
				o[2].color = faded ? fadedCol : _Color;
				vert.Append(o[2]);
				o[3].vertex = UnityObjectToClipPos(_Pos2);
				o[3].color = faded ? fadedCol : _Color;
				vert.Append(o[3]);
				vert.RestartStrip();

				o[4].vertex = UnityObjectToClipPos(_Pos2);
				o[4].color = faded ? fadedCol : _Color;
				vert.Append(o[4]);
				o[5].vertex = UnityObjectToClipPos(_Pos3);
				o[5].color = faded ? fadedCol : _Color;
				vert.Append(o[5]);
				vert.RestartStrip();

				o[6].vertex = UnityObjectToClipPos(_Pos3);
				o[6].color = faded ? fadedCol : _Color;
				vert.Append(o[6]);
				o[7].vertex = UnityObjectToClipPos(_Pos4);
				o[7].color = faded ? fadedCol : _Color;
				vert.Append(o[7]);
				vert.RestartStrip();*/

				//float4 segLength = float4((_Pos4.x - _Point.x) / 16, 0, 0, 0);//makes six dotted lines between the point and the c-Plane
				//float4 lowHeight = _Pos4 - segLength;
				//float4 highHeight = lowHeight - segLength;
				//for (int i = 16; i < 32; i += 2)
				//{
				//	o[i].vertex = UnityObjectToClipPos(lowHeight);
				//	o[i].color = faded ? fadedCol : _Color - 0.3;
				//	vert.Append(o[i]);
				//	o[i + 1].vertex = UnityObjectToClipPos(highHeight);
				//	o[i + 1].color = faded ? fadedCol : _Color - 0.3;
				//	vert.Append(o[i + 1]);
				//	vert.RestartStrip();
				//	lowHeight -= segLength * 2;
				//	highHeight -= segLength * 2;
				//}
			}

			fixed4 frag(g2f i) : SV_Target
			{
				fixed4 col;
				col.a = _Transparency;
				return col;
			}
			ENDHLSL
		}
	}
}

