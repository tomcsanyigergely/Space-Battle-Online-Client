Shader "Unlit/LoopTextureAsteroid"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_TextureScale("Texture Scale", Float) = 0.16
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
				float2 uv :		TEXCOORD0;
            };

            struct v2f
            {
				float4 position :	SV_POSITION;
				float2 uv :			TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _TextureScale;

            v2f vert (appdata v)
            {
				v2f output;
				output.position = UnityObjectToClipPos(v.vertex);
				output.uv = v.uv;
				return output;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 col = tex2D(_MainTex, fixed2(i.uv.x * _TextureScale +1000, i.uv.y * _TextureScale +1000));
                return col;
            }
            ENDCG
        }
    }
}
