Shader "Custom/Ocean" {
    Properties{
        _WaveFrequency("Wave Frequency", Vector2) = (1, 1)
        _WaveHeight("Wave Height", Range(0, 5)) = 1
        _WaveChoppiness("Wave Choppiness", Range(0, 1)) = 0.5
        _WaveSpeed("Wave Speed", Range(0, 5)) = 1
        _SurfaceColor("Surface Color", Color) = (0.5, 0.5, 0.5, 1)
    }

        SubShader{
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
            LOD 200

            CGPROGRAM
            #pragma surface surf Lambert

            struct Input {
                float3 viewDir;
            };

            sampler2D _MainTex;
            sampler2D _ReflectionTex;
            fixed4 _SurfaceColor;
            fixed _WaveHeight;
            fixed _WaveChoppiness;
            fixed _WaveSpeed;
            fixed2 _WaveFrequency;

            struct SurfaceOutput {
                fixed3 Albedo;
                fixed3 Normal;
                fixed3 Emission;
                fixed Specular;
                fixed Gloss;
                fixed Alpha;
            };

            fixed4 _ReflectionColor;
            fixed _ReflectionStrength;

            inline fixed4 Lighting(SurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed3 normal) {
                fixed4 c;
                c.rgb = s.Albedo * _LightColor0.rgb;
                c.a = s.Alpha;
                return c;
            }

            void surf(Input IN, inout SurfaceOutput o) {
                fixed2 uv = texcoord0.xy;

                // Calculate the normal vector
                fixed3 normal = tex2D(_MainTex, uv * _WaveFrequency).rgb;
                normal = UnpackNormal(normal);

                // Add wave height and choppiness to the normal
                normal.z = tex2D(_MainTex, (uv + _Time.y * _WaveSpeed) * _WaveFrequency).r;
                normal = normalize(normal + _WaveHeight * _WaveChoppiness);

                // Assign the surface properties
                o.Albedo = _SurfaceColor.rgb;
                o.Normal = normal;
                o.Gloss = 0;
                o.Specular = 0;
                o.Alpha = _SurfaceColor.a;

                // Add reflection
                fixed4 reflection = tex2D(_ReflectionTex, uv);
                o.Albedo = lerp(o.Albedo, _ReflectionColor.rgb, _ReflectionStrength * reflection.r);
            }
            ENDCG
    }
        FallBack "Diffuse"
}
