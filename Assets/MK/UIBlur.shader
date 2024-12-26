Shader "Custom/UIBlur"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 10)) = 2.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            sampler2D _MainTex;
            float _BlurSize;
            
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
            
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float4 col = 0;

                float2 offset = float2(_BlurSize / _ScreenParams.x, _BlurSize / _ScreenParams.y);

                col += tex2D(_MainTex, uv + offset * -1.0);
                col += tex2D(_MainTex, uv + offset *  1.0);
                col += tex2D(_MainTex, uv + offset * -0.5);
                col += tex2D(_MainTex, uv + offset *  0.5);
                
                return col / 4;
            }
            ENDCG
        }
    }
}
