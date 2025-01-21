Shader "Custom/RimLightingSurfaceShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _RimColour ("Rim Colour", Color) = (1, 0, 0, 1) 
        _RimPower ("Rim Power", Range(0.1, 4.0)) = 2.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200
        Blend SrcAlpha One

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0


        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
        };
        sampler2D _MainTex;
        float _RimPower;
        float4 _RimColour;


        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 texColor = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = texColor.rgb;

            float rimFactor = 1.0 - saturate(dot(normalize(IN.viewDir),o.Normal));
            //o.Emission = _RimColour.rgb * rimFactor;
            o.Emission = _RimColour.rgb * pow(rimFactor,_RimPower);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
