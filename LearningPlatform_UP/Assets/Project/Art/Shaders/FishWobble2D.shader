Shader "Custom/RealisticFishSwimURP_Controllable"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _WobbleSpeed("Wobble Speed", Float) = 6
        _WobbleAmount("Wobble Amount", Float) = 0.08
        _WobbleCurve("Body Wave Curviness", Float) = 4
        _WobbleSide("Wobble Side", Float) = 1   // 0 = left wiggle, 1 = right wiggle
        _WobbleRange("Wobble Range", Float) = 1 // 0 = just edge, 1 = full body
    }

        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "UniversalMaterialType" = "Unlit" }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            Pass
            {
                Name "FishWobble"
                Tags { "LightMode" = "UniversalForward" }

                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);
                float4 _MainTex_ST;

                float _WobbleSpeed;
                float _WobbleAmount;
                float _WobbleCurve;
                float _WobbleSide;
                float _WobbleRange;

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

                    // Smooth wave across body with phase shift from head to tail
                    float phase = time * _WobbleSpeed + v.uv.x * _WobbleCurve * 3.1415;
                    float baseWobble = sin(phase);

                    // Determine wobble strength mask based on side and range
                    float sideStrength = _WobbleSide > 0.5
                        ? saturate((v.uv.x - (1.0 - _WobbleRange)) / _WobbleRange) // right wobble
                        : saturate((_WobbleRange - v.uv.x) / _WobbleRange);        // left wobble

                    float sway = baseWobble * _WobbleAmount * sideStrength;

                    v.positionOS.y += sway;

                    o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
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
