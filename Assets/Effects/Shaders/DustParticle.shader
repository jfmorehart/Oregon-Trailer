Shader "Unlit/DustParticle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _col ("Color1", Color) = (1, 0, 0, 1)
    }
    SubShader
    {
        Tags{ "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }

        Blend SrcAlpha One
        ColorMask RGB
        Cull Off Lighting Off ZWrite Off

        LOD 100

        ZWrite Off

        Cull Back

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
                fixed4 color : COLOR;
                float4 vertex : SV_POSITION;
                float2 worldUV : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _col;

            
            float rand (float2 uv) { 
                return frac(sin(dot(uv.xy, float2(12.9898, 78.233))) * 43758.5453123);
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
                o.uv = v.uv;
                o.color = v.color;
                o.worldUV = mul(unity_ObjectToWorld, v.vertex.xyz).xy;
                return o;
            }



            fixed4 frag (v2f i) : SV_Target
            {
                float2 polar = 2 * (i.uv - 0.5);
                float mag = 1 - length(polar);
        
                float2 scUV = i.uv;//scale2d(i.uv, 20);
                mag *=  pow(fractal_noise(i.worldUV * 0.4 + scUV * 2 + i.color.r - _Time.y), 2);
                //float alpha = 0.5 * (1 -step(pow(rand(scUV) * mag, 0.5), 10));
                float4 outColor = float4(_col.xyz, mag * i.color.a);
                return outColor; 
            }
            ENDCG
        }
    }
}
