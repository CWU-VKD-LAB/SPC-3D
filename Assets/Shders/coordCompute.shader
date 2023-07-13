// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/coordCompute"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 color : TEXCOORD0;
            };

            struct vertex
            {
                float4 pos;
                float3 color;
            };

            StructuredBuffer<vertex> verti;

            int vertsPerVector;

            v2f vert(uint vID : SV_VertexID, uint id : SV_InstanceID)
            {
                v2f o;
                int index = id * vertsPerVector + vID;
                float4 posi = mul(UNITY_MATRIX_VP, float4(verti[index].pos.xyz, 1)); //all vectors at origin
                //posi -= 10;
                o.pos = posi;
                o.color = verti[index].color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = float4(i.color,1);
                return col;
            }
            ENDCG
        }
    }
}
