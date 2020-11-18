Shader "Unlit/Spaceship"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_DamageEffectLength("DamageEffectLength", Float) = 0.5
		_TimeOfLastDamage("TimeOfLastDamage", Float) = -1
    }
    SubShader
    {
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
			float _DamageEffectLength;
			float _TimeOfLastDamage;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float time = _Time.y;
			    float timeSinceLastDamage = time - _TimeOfLastDamage;
				float fT = 0.5 / _DamageEffectLength * (time - _TimeOfLastDamage);
				float gT = 0.5 - 0.5 / _DamageEffectLength * abs(_TimeOfLastDamage + _DamageEffectLength - time);
				float gbValue = fT + gT;
                fixed4 col = tex2D(_MainTex, i.uv) * fixed4(1, gbValue, gbValue, 1);
                return col;
            }
            ENDCG
        }
    }
}
