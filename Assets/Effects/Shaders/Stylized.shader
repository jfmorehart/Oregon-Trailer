Shader "Unlit/Stylized"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        brite ("brite", Float) = 0.5
        n1scale("n1scale", Float) = 1

    }
    SubShader
    {
        Tags{ "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }

        Blend SrcAlpha One
        ColorMask RGB
        Cull Off Lighting Off ZWrite Off

        LOD 100

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
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 worldUV : TEXCOORD1;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;


            float rand (float2 uv) { 
                return frac(sin(dot(uv.xy, float2(13.9898, 77.233))) * 43758.5453123);
            } 
             

            float noise (float2 uv) {
                float2 ipos = floor(uv); 
                float2 fpos = frac(uv); 
                
                float o  = rand(ipos);
                float x  = rand(ipos + float2(1, 0));
                float y  = rand(ipos + float2(0, 1));
                float xy = rand(ipos + float2(1, 1));

                float2 smooth = smoothstep(0, 1, fpos);
                return lerp( lerp(o,  x, smooth.x), lerp(y, xy, smooth.x), smooth.y);
            }

            float fractal_noise (float2 uv) {
                float n = 0;
                // fractal noise is created by adding together "octaves" of a noise
                // an octave is another noise value that is half the amplitude and double the frequency of the previously added noise
                // below the uv is multiplied by a value double the previous. multiplying the uv changes the "frequency" or scale of the noise becuase it scales the underlying grid that is used to create the value noise
                // the noise result from each line is multiplied by a value half of the previous value to change the "amplitude" or intensity or just how much that noise contributes to the overall resulting fractal noise.

                n  = (1 / 2.0)  * noise( uv * 1);
                n += (1 / 4.0)  * noise( uv * 2); 
                n += (1 / 8.0)  * noise( uv * 4); 
                n += (1 / 16.0) * noise( uv * 8); 
                
                return n;
            }

            float2 scale2d(float2 uv, float scale){
	            return round(uv * scale) / scale;
	        }


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldUV = mul(unity_ObjectToWorld, v.vertex.xyz).xy -  _Time.z;
                o.color = v.color;
                return o;
            }
            float brite;
            float n1scale;

            fixed4 frag (v2f i) : SV_Target
            {

                float2 pol = (i.uv - 0.5) * 2;
                float n1 = fractal_noise(i.worldUV * n1scale + _Time.y + pol);
                float n2 = fractal_noise(i.worldUV * n1scale * 0.7 + _Time.z + pol);

                float4 col = 0.15;
                col += 0.1 *  length(pol + 0.1);
                col.xyz *= brite;
                //col.a += step(length(pol + n1 * 2), 0.5);

                col.a *= fractal_noise(i.worldUV * 3);
                col.a += pow(n1, 2);
                col.a += pow(n2, 2);
                //col.a *= pow(length(pol), 2);
                col.a -= pow(length(pol), 2);
                col.a *= 10;//fractal_noise(i.uv* 100);
                col.a * i.color.a;
                return col;
            }
            ENDCG
        }
    }
}
