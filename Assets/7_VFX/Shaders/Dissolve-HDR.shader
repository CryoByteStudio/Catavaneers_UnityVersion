Shader "Custom/Dissolve HDR"
{
    Properties
    {
        [HDR] _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

		_DissolveTex ("Dissolve Texture", 2D) = "white" {}
		_Amount ("Dissolve Amount", Range(0,1)) = 0
		_BurnSize ("Burn Size", Range(0,1)) = 0.15
		_BurnRamp ("Burn Ramp (RGB)", 2D) = "white" {}
		_BurnColor ("Burn Color", Color) = (1,1,1,1)
		_EmissionAmount ("Emission Amount", Float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
		Cull Off
        CGPROGRAM

        //#pragma surface surf Lambert addshadow
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _DissolveTex;
		sampler2D _BurnRamp;
		fixed4 _Color;
		fixed4 _BurnColor;
		half _Glossiness;
		half _Metallic;
		float _BurnSize;
		float _Amount;
		float _EmissionAmount;

        struct Input
        {
            float2 uv_MainTex;
        };

        //void surf (Input IN, inout SurfaceOutput o)
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			half dissolveValue = tex2D(_DissolveTex, IN.uv_MainTex).rgb - _Amount;
			clip(dissolveValue);

			if (dissolveValue < _BurnSize && _Amount > 0)
			{
				o.Emission = tex2D(_BurnRamp, float2(dissolveValue * (1 / _BurnSize), 0)) * _BurnColor * _EmissionAmount;
			}

            o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
