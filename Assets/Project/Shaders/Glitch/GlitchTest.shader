Shader "UI/GlitchButton"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GlitchAmount ("Glitch Amount", Range(0,1)) = 0
        _GlitchSpeed ("Glitch Speed", Range(0,100)) = 50
        _Color ("Tint", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _GlitchAmount;
            float _GlitchSpeed;
            float4 _Color;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Nombre élevé de bandes horizontales
                float bands = 50.0;

                // Index de bande
                float bandIndex = floor(uv.y * bands);

                // Générer un "pseudo-random" décalage par bande basé sur bandIndex et temps
                float time = _Time.y * _GlitchSpeed;

                // Fonction simple pour pseudo aléatoire
                float rand = frac(sin(bandIndex * 12.9898 + time * 78.233) * 43758.5453);

                // Décalage horizontal aléatoire entre -_GlitchAmount et +_GlitchAmount
                float offset = (rand - 0.5) * 2.0 * _GlitchAmount;

                // Appliquer le décalage sur x
                uv.x += offset;

                fixed4 col = tex2D(_MainTex, uv) * _Color;

                return col;
            }



            
            ENDCG
        }
    }
}
