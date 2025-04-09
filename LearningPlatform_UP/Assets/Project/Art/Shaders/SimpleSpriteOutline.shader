Shader "Custom/SimpleSpriteOutline"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Main Color", Color) = (1,1,1,1)
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineSize("Outline Size", Float) = 1
        _AlphaThreshold("Alpha Threshold", Range(0,1)) = 0.1
    }

        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 200

            Cull Off
            Lighting Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata
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
                float4 _MainTex_ST;
                float4 _Color;
                float4 _OutlineColor;
                float _OutlineSize;
                float _AlphaThreshold;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float2 texelSize = _OutlineSize / _ScreenParams.xy;

                    float alpha = 0.0;
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            float2 offset = float2(x, y) * texelSize;
                            float a = tex2D(_MainTex, i.uv + offset).a;
                            alpha = max(alpha, a);
                        }
                    }

                    fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                    // Draw outline if current pixel is transparent but neighbors are not
                    if (col.a < _AlphaThreshold && alpha > _AlphaThreshold)
                    {
                        return _OutlineColor;
                    }

                    return col;
                }
                ENDCG
            }
        }
}
