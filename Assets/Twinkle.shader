Shader "Custom/Twinkle"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct appdata_t members worldPos)
#pragma exclude_renderers d3d11 xbox360
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile DUMMY PIXELSNAP_ON
			#include "UnityCG.cginc"
			float tri(float num, float mod)
			{
				//float a = mod - (abs(fmod(abs(num), (2 * mod)) - mod));
			    float a = fmod(abs(num), (2 * mod));
			    float b = a - mod;
			    float c = abs(b);
			    float d = mod - c;
			    return d;
			}
			float rand(vec2 co){
				return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
			}
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldpos;
			};
			
			fixed4 _Color;
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				float4 wat = IN.vertex;
				OUT.vertex = mul(UNITY_MATRIX_MVP, wat);
				
				OUT.worldpos = mul(_Object2World, IN.vertex);

				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				return OUT;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f IN) : SV_Target
			{
				const float PI = 3.14159;
				float increment = 4f;
				float deadzone = 0.2f;
				float4 wp = IN.worldpos;
				fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;

				//graph
				//float val = fmod(sin(_Time.x * 10)*(wp.x * wp.y), 0.1f);
				//if (val < 0.01f)
				//{
				//	c *= 1-(val*100);
				//}

				//circle grid
				//float div = 3f;
				//float x = tri(wp.x + _Time.y, 1f / div);
				//float y = tri(wp.y + _Time.y, 1f / div);
				//float dist = sqrt(x * x + y * y);
				//
				//float amp = 3f;
				//float val2 = tri(dist + _Time.x * 50, amp) + 1f - amp/2;
				//c.rgb *= val2;

				float div = 4f;
				float x = tri(wp.x, 1f / div);
				float y = tri(wp.y, 1f / div);
				float dist = sqrt(x * x + y * y);
				float val2 = tri(dist + _Time.x * 10, 0.5f) + 0.2f;
				c.rgb *= val2;
				
				//float random = tri(frac(_Time) * 100, 10);
				////c.a = tri(c.a + random, 1f);
				//float r2 = fmod(rand(wp.xy), 1f);
				//if (r2 < 0.3) c.r *= random;
				//else if (r2 < 0.7) c.g *= random;
				//else c.b *= random;

				return c;
			}
		ENDCG
		}
	}
}

