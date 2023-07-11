Shader "Unlit/RuleLines"
{
	Properties
	{
		_TX("Top X", Float) = 0.5
		_BX("Bottom X", Float) = 0.2
		_TY("Top Y", Float) = 0.7
		_BY("Bottom Y", Float) = 0.5
		_Transparency("Transparency", Range(0, 1)) = 0.5
		_Color("Color", Color) = (0, 1, 0.73, 1)
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
			float _Transparency;
			float4 _Color;
			float _TX;
			float _BX;
			float _TY;
			float _BY;
			float _MaxX;
			float _MaxY;
			static float scale = 5;


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

			//makes a square of connected dots based off their four positions
			[maxvertexcount(6)]
			void geom(point v2g IN[1], inout LineStream <g2f> vert)
			{
				float highX = scale * (_TX - 0.5);
				float lowX = scale * (_BX - 0.5);
				float highY = scale * (_TY - 0.5);
				float lowY = scale * (_BY - 0.5);

				g2f o[5];
				o[0].vertex = UnityObjectToClipPos(float3(highX, 0.00, highY));
				o[0].color = _Color;// +0.5;
				vert.Append(o[0]);
				o[1].vertex = UnityObjectToClipPos(float3(lowX, 0.00, highY));
				o[1].color = _Color;// -0.5;
				vert.Append(o[1]);
				o[2].vertex = UnityObjectToClipPos(float3(lowX, 0.00, lowY));
				o[2].color = _Color;// -0.8;
				vert.Append(o[2]);
				o[3].vertex = UnityObjectToClipPos(float3(highX, 0.00, lowY));
				o[3].color = _Color;// -0.5;
				vert.Append(o[3]);
				//makes a full connected square
				o[4].vertex = UnityObjectToClipPos(float3(highX, 0.00, highY));
				o[4].color = _Color;// +0.5;
				vert.Append(o[4]);
				vert.RestartStrip();
			}
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
