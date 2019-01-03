Shader "Custom/B" {
	SubShader{
		Tags{"Queue" = "Transparent"}
		LOD 200

		CGPROGRAM

		#pragma surface surf Standard alpha:fade
		#pragma target 3.0

		struct Input {
			float3 worldNormal; //頂点の法線ベクトル？
			float3 viewDir; //カメラから頂点への入射角？
		};

		void surf(Input IN, inout SurfaceOutputStandard o) {
			float alpha = 1 - (abs(dot(IN.viewDir, IN.worldNormal)));
			o.Albedo = fixed4(1, 1, 1, 1);
			o.Alpha = alpha * 4;
		}

		ENDCG
	}

		//処理失敗時にDiffuseを返す
	    FallBack "Diffuse"
}
