Shader "Custom/Shader_Rim_Player"
{
	Properties
	{

		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_EffectTex("EffectTex (RGB)", 2D) = "black" {}

		_BumpMap("NormalMap", 2D) = "bump"{}
		_SpecCo1("Specular Color", Color) = (1,1,1,1)
		_SpecPow("Specular Power", Range(10,200)) = 10
		_GlossTex("Gloss Tex", 2D) = "white" {}

		_MaskMap ("MaskMap",2D) = "white" {}

	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }


		CGPROGRAM
		#pragma surface surf Test  fullforwardshadows

		sampler2D _MainTex;
		sampler2D _EffectTex;

		sampler2D _BumpMap;
		sampler2D _GlossTex;
		float4 _SpecCo1;
		float _SpecPow;
		float _DeltaTimeF;
		float _DeltaTimeB;
		float4 _WorldPos;//오브젝트의 위치
		float _HitTime;
		float _StunTime;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_GlossTex;
			float3 worldPos; //픽셀의 위치
		};


		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 e = tex2D(_EffectTex, 0.3 * float2(IN.worldPos.x + _Time.y * 0.2, IN.worldPos.z + _Time.y * 0.2));

			fixed4 m = tex2D(_GlossTex, IN.uv_GlossTex);
			o.Albedo = c.rgb + e.rgb * 0.4;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Gloss = m.a;
			o.Alpha = c.a;

			float3 fPixelWorldPos = IN.worldPos;
			fPixelWorldPos.y = 0;
			float fDis = distance(fPixelWorldPos, float3(_WorldPos.x, _WorldPos.y, _WorldPos.z));

			if (fDis < _DeltaTimeF &&fDis > _DeltaTimeB)
			{
				o.Albedo = o.Albedo + e;

			}
		}

		float4 LightingTest(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten) {
			float4 final;

			//Lamber Test
			float3 DiffColor;
			float ndot1 = saturate(dot(s.Normal, lightDir));
			DiffColor = ndot1 * s.Albedo * _LightColor0.rgb * atten;

			//spect term
			float3 SpecColor;
			float3 H = normalize(lightDir + viewDir);
			float spec = saturate(dot(H, s.Normal));
			spec = pow(spec, _SpecPow);

			if (spec > 0.5)
			{
				spec = 1;
			}
			else
			{
				spec = 0;

			}
			SpecColor = spec * _SpecCo1.rgb * saturate(s.Gloss);

			//rim term
			float3 rimColor;
			float rim = abs(dot(viewDir, s.Normal));
			float invrim = 1 - rim;
			invrim = pow(invrim, 3);

			if (invrim > 0.3)
				invrim = 1;
			else
				invrim = 0;
			rimColor = invrim * float3(0.3, 0.3, 0.3);

			////fake spec term
			//float3 SpecColor2;
			//SpecColor2 = rimColor * float3(0.5, 0.5, 0.5) * s.Gloss;


			//final term
			final.rgb = DiffColor.rgb + SpecColor.rgb + rimColor.rgb/* + SpecColor2.rgb*/;
			final.a = s.Alpha;

			final.rgb = saturate(final.rgb 
				- float3(0, 1, 1) * sin(_StunTime * 30) 
				- float3(0, -1, -1) * sin(_HitTime));

			return final;

		}

		ENDCG
	}
		FallBack "Diffuse"
}