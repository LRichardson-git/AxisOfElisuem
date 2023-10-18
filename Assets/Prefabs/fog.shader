Shader "Custom/fog"
{
    Properties{
        _MainTex("Texture", 3D) = "white" {}
        _PlayerPos("Player Position", Vector) = (0,0,0)
        _FogDistance("Fog Distance", Range(0,10)) = 1
    }
        SubShader{
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 100

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float3 uv : TEXCOORD0;
                };

                struct v2f {
                    float3 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler3D _MainTex;
                float4 _MainTex_ST;
                float4 _PlayerPos;
                float _FogDistance;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    // sample the texture
                    fixed4 col = tex3D(_MainTex, i.uv);
                // calculate distance to player
                float dist = distance(_PlayerPos.xyz, i.uv);
                // if distance is greater than fog distance, make color transparent
                if (dist > _FogDistance) {
                    col.a = 0;
                }
                return col;
            }
            ENDCG
        }
        }
}
