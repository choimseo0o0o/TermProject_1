Shader "Custom/AlwaysOnTop"
{
    Properties
    {
        _Color ("Color", Color) = (1, 0, 0, 1)
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        // 다른 오브젝트들보다 나중에(위에) 렌더되도록 Overlay 큐 사용
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }

        Lighting Off
        ZWrite Off        // 깊이 버퍼에 기록하지 않음
        ZTest Always      // 깊이 비교를 항상 통과 = 항상 앞에 그림
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
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }
            ENDCG
        }
    }
}
