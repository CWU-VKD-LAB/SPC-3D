Shader "Custom/vectorCompute"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float3 pos;
            };

            StructuredBuffer<vertex> verti;

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (uint vID : SV_VertexID, uint id : SV_InstanceID)
            {
                v2f o;
                int index = id * 8 + vID;
                float4 posi = mul(UNITY_MATRIX_VP, verti[index].pos); //all vectors at origin
                o.pos = posi;
                o.color = float3(1, 0, 0);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = float4(i.color,1);
                return col;
            }
            ENDCG
        }
    }
}
