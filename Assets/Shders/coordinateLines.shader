Shader "Custom/coordinateLines"
//sets an RGB coordinate based off 4 values
{
	Properties
	{
		//arbitrary default positions
		_Pos0("Pos0", Vector) = (0,0,0,0)
		_Pos1("Pos1", Vector) = (0,0,0,0)
		_Pos2("Pos2", Vector) = (1,1,0,0)
		_Pos3("Pos3", Vector) = (1,1,1,0)
		_AttribContrib("attribute contribution", Vector) = (1,1,1,1)
		_C("C", Vector) = (1,1,1,1)
		_Transparency("Transparency", Range(0, 1)) = 0.5
	}

		SubShader
	{

		Pass
		{
			//Ztest Always
			Tags {"LightMode" = "UniversalForward"}
			Blend SrcAlpha OneMinusSrcAlpha
			LOD 100

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
			float4 _AttribContrib;
			float4 _C;
			static float3 fadedCol = 0.2;
			bool faded;



			struct v2g
			{
				float4 vertex : SV_POSITION;
				float3 color : TEXCOORD0;
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
				o.color = 0;
				return o;
			}

			//sets an RGB coordinate based off 4 values
			[maxvertexcount(18)]
			void geom(point v2g IN[1], inout LineStream <g2f> vert)
			{
				//red line
				g2f o[18];
				o[0].vertex = UnityObjectToClipPos(_Pos0);
				o[0].color = faded ? fadedCol : float3(1, 0.2, 0.2);
				vert.Append(o[0]);
				o[1].vertex = UnityObjectToClipPos(_Pos2);
				o[1].color = faded ? fadedCol : float3(0.6, 0, 0);
				vert.Append(o[1]);
				vert.RestartStrip();

				//green line
				o[2].vertex = UnityObjectToClipPos(_Pos1);
				o[2].color = faded ? fadedCol : float3(0.2, 1, 0.2);
				vert.Append(o[2]);
				o[3].vertex = UnityObjectToClipPos(_Pos2);
				o[3].color = faded ? fadedCol : float3(0, 0.6, 0);
				vert.Append(o[3]);
				vert.RestartStrip();

				//dotted line when fx < c
				if (_Pos3.z < _C.z)
				{
				//blue line
					o[4].vertex = UnityObjectToClipPos(_Pos2);
					o[4].color = faded ? fadedCol : float3(0.7, 0.7, 1);
					vert.Append(o[4]);
					o[5].vertex = UnityObjectToClipPos(_Pos3);
					o[5].color = faded ? fadedCol : float3(0.2, 0.2, 0.4);
					vert.Append(o[5]);
					vert.RestartStrip();


					//float4 segLength = float4(0, 0, (_Pos3.z - _C.z) / 12, 0);//makes six dotted lines between the point and the c-Plane
					//float4 lowHeight = _Pos3 - segLength;
					//float4 highHeight = lowHeight - segLength;
					//for (int i = 6; i < 18; i += 2)
					//{
					//	o[i].vertex = UnityObjectToClipPos(lowHeight);
					//	o[i].color = faded ? fadedCol : float3(0.7490196, 0, 0.5490196);
					//	vert.Append(o[i]);
					//	o[i + 1].vertex = UnityObjectToClipPos(highHeight);
					//	o[i + 1].color = faded ? fadedCol : float3(0.7490196, 0, 0.5490196);
					//	vert.Append(o[i + 1]);
					//	vert.RestartStrip();
					//	lowHeight -= segLength * 2;
					//	highHeight -= segLength * 2;
					//}
				}
				else if (_C.z < _Pos3.z)
				{
					o[4].vertex = UnityObjectToClipPos(_Pos2);
					o[4].color = faded ? fadedCol : float3(0.7, 0.7, 1);
					vert.Append(o[4]);
					o[5].vertex = UnityObjectToClipPos(float4(_Pos3.xy, _Pos3.z, _Pos3.w));
					o[5].color = faded ? fadedCol : float3(0.2, 0.2, 0.4);
					vert.Append(o[5]);
					vert.RestartStrip();
				}
				//else if (_C.z < _Pos3.z)
				//{
				//	o[4].vertex = UnityObjectToClipPos(_Pos2);
				//	o[4].color = faded ? fadedCol : float3(0.7, 0.7, 1);
				//	vert.Append(o[4]);
				//	o[5].vertex = UnityObjectToClipPos(float4(_Pos3.xy, _C.z, _Pos3.w));
				//	o[5].color = faded ? fadedCol : float3(0.2, 0.2, 0.4);
				//	vert.Append(o[5]);
				//	vert.RestartStrip();

				//	o[6].vertex = UnityObjectToClipPos(float4(_Pos3.xy, _C.z, _Pos3.w));
				//	o[6].color = faded ? fadedCol : float3(0.5, 0.5, 0);
				//	vert.Append(o[6]);
				//	o[7].vertex = UnityObjectToClipPos(_Pos3);
				//	o[7].color = faded ? fadedCol : float3(0.5, 0.5, 0);
				//	vert.Append(o[7]);
				//	vert.RestartStrip();
				//}
			}

			fixed4 frag(g2f i) : SV_Target
			{
				fixed4 col;
				col.rgb = i.color.rgb;
				col.a = _Transparency;
				return col;
			}
			ENDHLSL
		}
	}
}
