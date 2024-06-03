
Shader "Hidden/AutoExposure"
{
    Properties
    {
    }

    SubShader
    {
        Pass
        {
            Name "AutoExposure"
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings Vert (Attributes input)
            {
                Varyings output;
                output.pos = TransformObjectToHClip(input.vertex);
                output.uv = input.uv;
                return output;
            }

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _ExposureCompensation;

            float4 Frag (Varyings input) : SV_Target
            {
                float3 color = tex2D(_MainTex, input.uv).rgb;

                // Luminance calculation
                float luminance = dot(color, float3(0.2126, 0.7152, 0.0722));
                
                // Auto exposure calculation
                float targetLuminance = 0.5; // target mid-grey
                float exposure = targetLuminance / (luminance + 0.0001);
                
                // Apply exposure
                color *= exposure;

                return float4(color, 1.0);
            }
            ENDHLSL
        }
    }
}
