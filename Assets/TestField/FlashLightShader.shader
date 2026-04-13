Shader "Unlit/Flashlight"
{
    Properties
    {
        _DarkColor ("Dark Color", Color) = (0,0,0,0.95)
        _LightPos ("Light Position", Vector) = (0,0,0,0)
        _LightDir ("Light Direction", Vector) = (1,0,0,0)
        _Radius ("Radius", Float) = 5
        _Angle ("Angle", Float) = 60
    }
    
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _DarkColor;
            float4 _LightPos;
            float4 _LightDir;
            float _Radius;
            float _Angle;

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            v2f vert (float4 vertex : POSITION)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(vertex);
                o.worldPos = mul(unity_ObjectToWorld, vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 toPixel = i.worldPos.xy - _LightPos.xy;
                float dist = length(toPixel);
                if (dist > _Radius) return _DarkColor;
                
                float2 pixelDir = normalize(toPixel);
                float2 lightDir = normalize(_LightDir.xy);
                float dotProd = dot(pixelDir, lightDir);
                float radAngle = _Angle * 3.14159 / 180.0;
                float cosHalfAngle = cos(radAngle / 2.0);
                
                if (dotProd < cosHalfAngle - 0.2) return _DarkColor;
                
                float edge = smoothstep(cosHalfAngle - 0.2, cosHalfAngle + 0.1, dotProd);
                float falloff = 1.0 - smoothstep(0.0, _Radius, dist);
                float alpha = (1.0 - edge * falloff) * _DarkColor.a;
                
                return float4(_DarkColor.rgb, alpha);
            }
            ENDCG
        }
    }
}