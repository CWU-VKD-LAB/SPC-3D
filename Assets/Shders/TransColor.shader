Shader "Unlit/TransColor"
//sets an unlit transparent color for an object
{
    Properties
    {
        _Trans("Transparenct", Float) = 1
        _Color("Color", Color) = (0,0,0,0)
    }
        SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            float _Trans;
            fixed4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = fixed4(_Color.rgb, _Trans);
                return col;
            }
            ENDCG
        }
    }
}
