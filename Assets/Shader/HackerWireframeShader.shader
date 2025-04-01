Shader "Custom/HackerWireframe"
{
    Properties
    {
        _Color ("Wire Color", Color) = (0, 1, 0, 1)
        _LineThickness ("Line Thickness", Float) = 1.0
        _ScanSpeed ("Scan Speed", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float _LineThickness;
            float _ScanSpeed;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float gX = abs(sin(i.worldPos.x * 10.0));
                float gY = abs(sin(i.worldPos.y * 10.0));
                float gZ = abs(sin(i.worldPos.z * 10.0));
                float gridValue = gX * gY * gZ;

                float scan = sin((_Time.y * _ScanSpeed + i.worldPos.y) * 10.0) * 0.5 + 0.5;
                float thickness = smoothstep(0.5, 0.5 + (_LineThickness * 0.01), gridValue);
                float brightness = lerp(0.1, 1.0, scan);

                float finalVal = thickness * brightness;
                return fixed4(_Color.rgb * finalVal, 1.0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
