Shader "Custom/SpritePulseDirectionalURP"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _PulseSpeed("Pulse Speed", Float) = 3
        _PulseAmount("Pulse Scale", Float) = 0.1
        _PulseDirection("Pulse Axis", Vector) = (1, 0, 0, 0)
        _PulseSide("Pulse Side", Float) = 1         // 1 = right, 0 = left
        _PulseRange("Pulse Range", Float) = 1       // 0.5 = only edge, 1 = full half
    }

        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "UniversalMaterialType" = "Unlit" }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            Pass
            {
                Name "Pulse"
                Tags { "LightMode" = "UniversalForward" }

                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);
                float4 _MainTex_ST;

                float _PulseSpeed;
                float _PulseAmount;
                float4 _PulseDirection;
                float _PulseSide;
                float _PulseRange;

                struct Attributes
                {
                    float4 positionOS : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct Varyings
                {
                    float2 uv : TEXCOORD0;
                    float4 positionHCS : SV_POSITION;
                };

                Varyings vert(Attributes v)
                {
                    Varyings o;

                    float time = _Time.y;

                    float wave = sin(time * _PulseSpeed + dot(v.uv, _PulseDirection.xy) * 6.2831);

                    // Calculate side influence (uv.x based)
                    float mask = _PulseSide > 0.5
                        ? saturate((v.uv.x - (1.0 - _PulseRange)) / _PulseRange) // right side
                        : saturate((_PulseRange - v.uv.x) / _PulseRange);        // left side

                    float scale = 1.0 + wave * _PulseAmount * mask;

                    // Scale UVs from center (0.5, 0.5)
                    float2 centeredUV = v.uv - 0.5;
                    float2 scaled = centeredUV * scale + 0.5;

                    v.uv = scaled;

                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                    return o;
                }

                half4 frag(Varyings i) : SV_Target
                {
                    return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                }
                ENDHLSL
            }
        }
}
