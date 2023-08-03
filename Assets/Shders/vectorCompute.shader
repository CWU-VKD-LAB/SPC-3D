// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/vectorCompute"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

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
                float4 color : TEXCOORD0;
            };

            struct vertex
            {
                float4 pos;
                float3 color;
            };

            StructuredBuffer<vertex> verti;

            int vertsPerVector;
            int disabled = 0;
            float transparency = 1;

            v2f vert (uint vID : SV_VertexID, uint id : SV_InstanceID)
            {
                v2f o;
                int index = id * vertsPerVector + vID;
                float4 posi = mul(UNITY_MATRIX_VP, float4(verti[index].pos.xyz,1)); //all vectors at origin
                //posi -= 10;
                o.pos = disabled == 0 ? posi : posi - 10;
                o.color = disabled == 0 ? float4(verti[index].color, transparency) : 0;
                //o.color = float4(verti[index].color, 1);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = i.color;
                return col;
            }
            ENDCG
        }
    }
}
