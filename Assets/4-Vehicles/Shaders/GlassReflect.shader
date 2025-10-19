Shader "Car/URP_GlassReflect"
{
    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
        _ReflectColor("Reflection Color", Color) = (1,1,1,0.5)
        _MainTex("Base (RGB) RefStrength (A)", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "bump" {}
        _Cube("Reflection Cubemap", Cube) = "_Skybox" {}
        _FresnelPower("_FresnelPower", Range(0.05,5.0)) = 0.75
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "RenderPipeline"="UniversalRenderPipeline"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                float3 viewDir : TEXCOORD3;
            };

            sampler2D _MainTex;
            sampler2D _BumpMap;
            samplerCUBE _Cube;

            float4 _Color;
            float4 _ReflectColor;
            float _FresnelPower;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                float3 worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.worldPos = worldPos;
                OUT.worldNormal = normalize(TransformObjectToWorldNormal(IN.normalOS));
                OUT.viewDir = normalize(_WorldSpaceCameraPos.xyz - worldPos);
                OUT.uv = IN.uv;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float3 normalTS = UnpackNormal(tex2D(_BumpMap, IN.uv));
                float3 worldNormal = normalize(IN.worldNormal);

                // Reflection vector
                float3 reflVec = reflect(-IN.viewDir, worldNormal);
                float4 reflColor = texCUBE(_Cube, reflVec);

                float4 baseTex = tex2D(_MainTex, IN.uv) * _Color;

                // Fresnel effect
                float fcbias = 0.20373;
                float facing = saturate(1.0 - dot(normalize(IN.viewDir), normalize(worldNormal)));
                float fresnel = max(fcbias + (1.0 - fcbias) * pow(facing, _FresnelPower), 0);

                float3 finalColor = baseTex.rgb + reflColor.rgb * _ReflectColor.rgb;
                float alpha = saturate(fresnel);

                return float4(finalColor, alpha * _ReflectColor.a);
            }
            ENDHLSL
        }
    }
    FallBack Off
}
