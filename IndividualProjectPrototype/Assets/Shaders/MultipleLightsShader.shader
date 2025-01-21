Shader "Unlit/BasketballVertexAndFragmentShader_MultiLight"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpScale ("Bump Scale", Range(0,1)) = 0.5
        _RoughnessMap ("Roughness Map (G)", 2D) = "white" {}
        _HeightMap ("Height Map", 2D) = "white" {}
        _HeightScale ("Height Scale", Range(0, 0.1)) = 0.02
        _Glossiness ("Glossiness", Range(0,1)) = 0.5
        _SpecColor ("Specular Color", Color) = (1, 1, 1, 1)
        _Shininess ("Shininess", Range(1, 100)) = 10
        _ToggleTextures ("Toggle Texture", Range(0,1)) = 1
        _ToggleCheckers ("Toggle checkers", Range(0,1)) = 1
        _CheckerColor1 ("Checker Color 1", Color) = (1,1,1,1)
        _CheckerColor2 ("Checker Color 2", Color) = (0,0,0,1)
        _CheckerScale ("Checker Scale", Vector) = (1,1,0,0)
        _ToggleIllumination ("Toggle Illumination", Range(0,1)) = 1

        _LightPositions ("Light Positions", Vector) = (1, 1, 1, 0)
        _LightColors ("Light Colors", Color) = (1, 1, 1, 1)
        _LightCount ("Number of Lights", Range(1, 4)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD1;
                float4 worldPos : TEXCOORD2;
                float2 uv : TEXCOORD0;
                float2 checkerUV : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST; // Tiling and Offset matrix for MainTex
            sampler2D _BumpMap;
            float4 _BumpMap_ST; // Tiling and Offset matrix for BumpMap
            sampler2D _RoughnessMap;
            float4 _RoughnessMap_ST; // Tiling and Offset matrix for RoughnessMap
            sampler2D _HeightMap;
            float4 _HeightMap_ST; // Tiling and Offset matrix for HeightMap

            float _ToggleCheckers;
            float4 _Color;
            float4 _CheckerColor1;
            float4 _CheckerColor2;
            float2 _CheckerScale;
            float4 _SpecColor;
            float _Shininess;
            float _Glossiness;
            float _BumpScale;
            float _HeightScale;
            float _ToggleTextures;
            float _ToggleIllumination;

            float4 _LightPositions[1]; // Array of light positions
            float4 _LightColors[1];    // Array of light colors
            float _LightCount;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.worldPos = worldPos;
                o.worldNormal = normalize(mul(v.normal, (float3x3)unity_ObjectToWorld));
                o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                o.checkerUV = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 finalColor = float4(1, 1, 1, 1);

                float2 scaledUV = float2(i.checkerUV.x*_CheckerScale.x, i.checkerUV.y*_CheckerScale.y);
                float checkerPattern = fmod(floor(scaledUV.x) + floor(scaledUV.y), 2.0);
                float4 checkers = lerp(_CheckerColor1 - _Color, _CheckerColor2 + _Color, checkerPattern);
                if(_ToggleCheckers<0.5){
                    checkers = _Color;
                }
                // Sample texture and roughness
                float4 texColor = tex2D(_MainTex, i.uv);
                float roughness = tex2D(_RoughnessMap, i.uv).g;
                float2 normalMap = tex2D(_BumpMap, i.uv).xy * _BumpScale;
                float3 normal = normalize(i.worldNormal + float3(normalMap, 0));

                // Initialize ambient light
                float4 ambientLight = UNITY_LIGHTMODEL_AMBIENT;

                finalColor = texColor * checkers + ambientLight;
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

                // Apply lighting for each light
                for (int l = 0; l < _LightCount; l++)
                {
                    float3 lightDir = normalize(_LightPositions[l].xyz);
                    float3 lightColor = _LightColors[l].rgb;

                    // Diffuse lighting
                    float diff = max(dot(normal, lightDir),0.0);
                    float4 diffuse = diff * float4(lightColor, 1);

                    // Specular lighting
                    float3 reflectDir = reflect(-lightDir, normal);
                    float spec = pow(saturate(dot(viewDir, reflectDir)), _Shininess);
                    float4 specular = spec * _SpecColor * float4(lightColor, 1);

                    // Combine light contributions
                    finalColor *= diffuse;
                    finalColor += specular;
                }

                // Combine with texture
                

                return finalColor;
            }
            ENDCG
        }
    }
}
