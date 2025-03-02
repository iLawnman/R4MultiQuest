Shader "Unlit/PortalHiddenObjectUnlitShader"
{
    Properties
    {
        //_Blend ("Blend", Range (0, 1) ) = 0.0
        //_Color1 ("Color 1", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
        [Enum(CompareFunction)] _StencilComp("Stencil Comp", Int) = 3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
        Pass
        {
            Stencil
        {
            Ref 1
            //Comp Equal
            Comp [_StencilComp]
        }
//            Tags { "LightMode" = "Vertex" }
//               
//            // Setup Basic
//            Material {
//                Diffuse (1,1,1,1)
//                Ambient (1,1,1,1)
//            }
//            Lighting On
                    
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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            //fixed4 _Color1, _Blend;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv); // * _Color1 * _Blend;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
