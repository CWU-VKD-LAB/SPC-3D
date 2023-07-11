Shader "Custom/vector"
//makes a line based off three points
{
	Properties
	{
		//arbitrary default properties
		_Pos1("Pos1", Vector) = (0,0,0,0)
		_Pos2("Pos2", Vector) = (1,1,0,0)
		_Transparency("Transparency", Range(0, 1)) = 0.5
		_Color("Color", Color) = (0, 1, 0.73, 1)
	}


	SubShader
	{
		Pass
		{
			//Ztest Always
			Tags {"Queue" = "Geometry"}
			//Blend SrcAlpha OneMinusSrcAlpha
			//Cull off

			HLSLPROGRAM
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag
			#pragma enable_d3d11_debug_symbols
			// include file that contains UnityObjectToWorldNormal helper function
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc" // for _LightColor0

			float _Transparency;
			float4 _Pos1;
			float4 _Pos2;
			float4 _Color;


			struct v2g
			{
				float4 vertex : SV_POSITION;
			};

			struct g2f
			{
				float4 vertex : SV_POSITION;
			};

			v2g vert(appdata_full v)
			{
				v2g o;
				o.vertex = v.vertex;
				return o;
			}

			StructuredBuffer <float4> vecPos;

			[maxvertexcount(45)]
			void geom(point v2g IN[1], inout LineStream <g2f> vert)
			{
				g2f o[45];
				int x = 0;
				for (int i = 0; i < /*3*/7; i++)
				{
					o[x].vertex = UnityObjectToClipPos(vecPos[i]);
					vert.Append(o[x]);
					x++;
					o[x].vertex = UnityObjectToClipPos(vecPos[i+1]);
					vert.Append(o[x]);
					vert.RestartStrip();
					x++;
				}
				
			}

			fixed4 frag(g2f i) : SV_Target
			{
				fixed4 col;
				col.rgb = _Color.rgb;
				col.a = 1;
				return col;
			}
			ENDHLSL
		}
	}
}
