Shader "Custom/XRay Wall HDR"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
		[HDR] _Color("Tint Color", Color) = (1,1,1,1)

		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		_BumpMap("Normal Map", 2D) = "bump" {}

		[HDR] _ColorXRay("XRay Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Pass
		{
			Tags{"RenderType" = "Opaque" "Queue" = "Geometry+1000"}
			ZWrite Off
			ZTest Always

			Blend SrcAlpha OneMinusSrcAlpha

			Stencil
			{
				Ref 2
				Comp Always
				Pass Replace
				ZFail Keep
			}

			CGPROGRAM

			#pragma fragment frag
			#pragma vertex vert

			#pragma target 3.0

			fixed4 _ColorXRay;

			struct  appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v) 
			{
				v2f  o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				return float4(_ColorXRay);
			}
			ENDCG
		}

		Pass
		{
			ZWrite On
			ZTest LEqual
			Cull Off
		}
			
		/*Stencil
		{
			Ref 1
			Comp Less
			Fail Keep
			Pass Replace
		}*/
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha

		sampler2D _MainTex;
		sampler2D _BumpMap;
		fixed4 _Color;
		half _Glossiness;
		half _Metallic;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		}
		ENDCG		
	}
	FallBack "Diffuse"
}
