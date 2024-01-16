Shader "Kein/bend"
{
Properties
{
    _Color("Color", Color) = (1,1,1,1)
    _MainTex ("Texture", 2D) = "white" {}
    _StartBend("Bend Start Point", Range(0,500)) = 0
    _Bend ("Bend Value", Vector) = (0,0,0,0)
}
SubShader
{
    Tags { "Queue" = "Geometry"  }
    Pass
    {
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"
        float4 _Color;
        sampler2D _MainTex;
        float4 _MainTex_ST;
        float4 _Bend;
        float _StartBend;
        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };
        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
        };
        float4x4 Rotation(float4 rotation)
        {
            float radX = radians(rotation.x);
            float radY = radians(rotation.y);
            float radZ = radians(rotation.z);
            float sinX = sin(radX);
            float cosX = cos(radX);
            float sinY = sin(radY);
            float cosY = cos(radY);
            float sinZ = sin(radZ);
            float cosZ = cos(radZ);
            return float4x4(
                cosY * cosZ, -cosY * sinZ, sinY, 0.0,
                cosX * sinZ + sinX * sinY * cosZ, cosX * cosZ - sinX * sinY * sinZ, -sinX * cosY, 0.0,
                sinX * sinZ - cosX * sinY * cosZ, sinX * cosZ + cosX * sinY * sinZ, cosX * cosY, 0.0,
                0.0, 0.0, 0.0, 1.0
                );
        }
        float4 WorldRotation(float4 vertex) 
        {
            float4 worldPos = mul(unity_ObjectToWorld, vertex);
            float startPos = _WorldSpaceCameraPos.z + _StartBend;
            float dis = worldPos.z - startPos;
            
            if (worldPos.z > startPos)
            {
                float4 rot;
                _Bend.x = _Bend.x * dis * dis * 0.0005;
                _Bend.y = _Bend.y * dis * dis * 0.0005;
                _Bend.z = _Bend.z * dis * dis * 0.0005;
                rot.x = -_Bend.y;
                rot.y = _Bend.x;
                rot.z = -_Bend.z;
                return float4(mul(Rotation(rot), worldPos));
            }
            return worldPos;
        }
        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = WorldRotation(v.vertex);
            o.vertex = mul(UNITY_MATRIX_VP, o.vertex);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            return o;
        }
        fixed4 frag(v2f i) : SV_Target
        {
            fixed4 col = tex2D(_MainTex, i.uv) * _Color;
            return col;
        }
        ENDCG
    }
}
}