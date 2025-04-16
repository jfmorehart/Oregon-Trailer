Shader "Unlit/River"
{
    Properties
    {
        _bcol ("center color",Color) = (1, 1, 1)
        _col2 ("outer color",Color) = (0, 0, 1)
        _col3 ("grass color",Color) = (0, 1, 0)
        _rockCol ("rock color",Color) = (0.3, 0.3, 0.3)
        _rockCol2 ("rock color2",Color) = (0.35, 0.35, 0.35)
        _thresh("threshold", Float) = 0.3
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 wpos : TEXCOORD1;
            };

            float3 _bcol;
            float3 _col2;
            float3 _col3;
            float3 _rockCol;
            float3 _rockCol2;
            float _thresh;

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
            float scale(float x, float scale){
                return round(x * scale) /scale;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.wpos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture


                //float2 sUV = float2(scale(i.uv.x, 90), scale(i.uv.y, 40));
                float2 uv = i.uv * 30 + float2(-1, 0);


                //return rock.xxxx;
                float n_fixed = fractal_noise(i.wpos * 0.1) * 0.9 + fractal_noise(i.wpos * 0.1 + _Time.x * 3) * 0.1;
                float n = n_fixed;
                float n_time = fractal_noise(i.wpos * 0.4 + _Time.y * 0.5) * 0.9;
                float sinterm = (sin(i.uv.x * 10 * sign(i.uv.y - 0.5)) + 1) * 0.5;
                n += sinterm * 0.05;
                n += abs(i.uv.y - 0.5);
		        ///n = max(pow(n + abs(i.uv.x - 0.5) * 1.7, 3) * 0.4, n);
                float oval = pow(2 * abs(i.uv.x - 0.5), 4) + abs(i.uv.y - 0.5);//
                n += oval * 0.3;
                n_fixed = n;
                if(n > 0.9) discard;
                n = n_time;
                n += sinterm * 0.05;
                n += abs(i.uv.y - 0.5);
                n += 0.05 * (step(n, 0.6) - step(n, 0.5));

                float m1 = pow(scale(n - 0.6, 5), 2);//step(_thresh, n);
                float3 col = m1 * _col2;
                col += (1 - m1) * _bcol;

                float rock = fractal_noise(i.wpos * 0.3);
                rock += abs(i.uv.y - 0.5);
                rock = scale(rock, 10);

                int rock2 = saturate(step(1, rock) - step(rock, 0.95));
                rock = step(0.9, rock);
                
                float3 final = 0;
                final += saturate(rock - rock2) * _rockCol;
                final += rock2 * _rockCol2;
                final += col * (1 - rock);
                //col = rock2 * _rockCol2 + (1 - rock2) * col;
                //col = rock * _rockCol + (1 - rock) * col;

                //return rock2;
                //int m2 = step(n_fixed, _thresh);
                //col = col * m2 + (1 - m2) * _col3;
	
                //n = fractal_noise(i.uv * 10 + float2(1, 0) * _Time.y * 0.5) * 0.9;
 
                return float4(final, 1);
            }
            ENDCG
        }
    }
}
