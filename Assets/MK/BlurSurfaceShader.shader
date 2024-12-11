Shader "Custom/BlurSurfaceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurColor ("Blur Tint Color", Color) = (1,1,1,1)
        _BlurSize ("Blur Size", Range(0, 10)) = 2.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
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
            float4 _BlurColor;
            float _BlurSize;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 blur(sampler2D tex, float2 uv, float blurSize)
            {
                fixed4 color = 0;
                int samples = 8;
                float2 offsets[8] = {
                    float2(-1, -1), float2(1, -1), float2(-1, 1), float2(1, 1),
                    float2(-1, 0), float2(1, 0), float2(0, -1), float2(0, 1)
                };

                for (int i = 0; i < samples; i++)
                {
                    color += tex2D(tex, uv + offsets[i] * blurSize / _ScreenParams.xy);
                }
                return color / samples;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture with blur
                fixed4 blurredTex = blur(_MainTex, i.uv, _BlurSize);
                // Apply the blur color as a tint
                return blurredTex * _BlurColor;
            }
            ENDCG
        }
    }
}
