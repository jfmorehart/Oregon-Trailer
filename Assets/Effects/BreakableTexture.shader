Shader "Unlit/BreakableTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _breakTex ("BreakTexture", 2D) = "black" {}
        hp_ratio ("hp ratio", Float) = 1
    }
    SubShader
    {
        Tags{ "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }

        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGB
        Cull Off Lighting Off ZWrite Off


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _breakTex;
            float4 _breakTex_ST;
            float hp_ratio;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float2 cuv = i.uv - 0.5;
                float4 col = tex2D(_MainTex, i.uv); 
                float4 cracks = 1 - step(0.9, tex2D(_breakTex, i.uv));
                cracks +=  1 - step(0.9, tex2D(_breakTex, (i.uv * 1.3) % 1));
                cracks *= pow(length(cuv), 2) / hp_ratio;
                col -= cracks * (1 - hp_ratio);
                return col;
            }
            ENDCG
        }
    }
}
