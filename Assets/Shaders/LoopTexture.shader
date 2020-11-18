Shader "Unlit/LoopTexture"
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
            };

            struct v2f
            {
				float4 position : SV_POSITION;
				float4 worldSpacePos : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _TextureScale;

            v2f vert (appdata v)
            {
				v2f output;
				output.position = UnityObjectToClipPos(v.vertex);
				output.worldSpacePos = mul(unity_ObjectToWorld, v.vertex);
				return output;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 col = tex2D(_MainTex, fixed2(i.worldSpacePos.x * _TextureScale +1000, i.worldSpacePos.y * _TextureScale +1000));
                return col;
            }
            ENDCG
        }
    }
}
