Shader "Unlit/SkyBox"
{
    Properties
    {
     _SkyBoxTexture("SkyBox Texture",Cube) = "White"{}
    }
    SubShader
    {
        Cull Off
        ZWrite Off
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
                float3 worldPos:TEXCOORD1;
            };

            float4 _corners[4];
            TextureCube _SkyBoxTexture;
            SamplerState sampler_SkyBoxTexture;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = v.vertex;
                o.worldPos = _corners[v.uv.x + v.uv.y*2].xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed3 viewDir = normalize(i.worldPos - _WorldSpaceCameraPos);
                return _SkyBoxTexture.Sample(sampler_SkyBoxTexture, viewDir);
            }
            ENDCG
        }
    }
}
