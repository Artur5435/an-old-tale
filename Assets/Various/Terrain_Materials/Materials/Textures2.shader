
Shader "Blackrose/Terrain"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_MainTex2("Debug (RGB)", 2D) = "white" {}
		_BumpMap("Normalmap", 2D) = "bump" {}
		_Grass("Grass Texture", 2D) = "white" {}
		_Sand("Sand Texture", 2D) = "white" {}
		_Road("Road Texture", 2D) = "white" {}
		_Stone("Stone Texture", 2D) ="white" {}
	}

		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 300

		CGPROGRAM
		#pragma surface surf Lambert 

		sampler2D _MainTex;
		sampler2D _Grass;
		sampler2D _Sand;
		sampler2D _Road;
		sampler2D _Stone;
		sampler2D _BumpMap;

		struct Input
		{
			float2 uv_MainTex : TEXCOORD0;
			float2 uv2_MainTex2 : TEXCOORD1;
			float2 uv_BumpMap;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c = ((tex2D(_MainTex, IN.uv_MainTex)));// fixed4(1,1,1,1) ;//= tex2D(_Grass, IN.uv_first);//tex2D(_MainTex, IN.uv_MainTex).g * ;
			if(IN.uv_MainTex.y == 0)
					c = (c.r * tex2D(_Road, IN.uv2_MainTex2)) + (c.g * tex2D(_Grass, IN.uv2_MainTex2)) + (c.b * tex2D(_Sand, IN.uv2_MainTex2));


			c /= 2;
			o.Albedo = c.rgb;
			
			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		}

		ENDCG
	}
	FallBack "Specular"
}