Shader "Unlit/outline"
//Purpose: Outline one fourth of an object
{
	Properties
	{
		_Transparency("Transparency", Range(0, 1)) = 0.5
		_Color("Color", Color) = (0, 1, 0.73, 1)
	}

		SubShader
	{

		Pass
		{
			//Ztest Always
			Tags {"LightMode" = "UniversalForward"}
			//Blend SrcAlpha OneMinusSrcAlpha
			LOD 100

			HLSLPROGRAM
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag
			// include file that contains UnityObjectToWorldNormal helper function
			#include "UnityCG.cginc"
			float _Transparency;
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


			[maxvertexcount(6)]
			void geom(triangle v2g IN[3], inout LineStream <g2f> vert)
			{
				g2f o[4];
				//This loop here sets only -y and -z (ie 1/4th) of whatever is being outlined
				for (int i = 0; i < 3; i++)
				{
					if (IN[i].vertex.y < 0 && IN[i].vertex.x < 0 && IN[i].vertex.z < 0)
					{
						o[0].vertex = UnityObjectToClipPos(IN[1].vertex);
						vert.Append(o[0]);
						o[1].vertex = UnityObjectToClipPos(IN[2].vertex);
						vert.Append(o[1]);
					}
				}
				vert.RestartStrip();
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
