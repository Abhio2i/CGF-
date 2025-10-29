Shader "Custom/NewUnlitShader"
{
    Properties
    {
        MainTex("Base (RGB)", 2D) = "white" {}
    _DepthLevel("Depth Level", Range(1, 10)) = 1
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
            uniform sampler2D _LastCameraDepthTexture;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.uv);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : COLOR
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float depth = UNITY_SAMPLE_DEPTH(tex2D(_LastCameraDepthTexture, i.uv));
                depth = Linear01Depth(depth);
                return depth;
                //apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                //return col;
            }
            ENDCG
        }
    }
}
