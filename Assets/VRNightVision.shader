// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "VRNightVision" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
	SubShader{
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _CameraDepthTexture;

			// Projection matrix of the headset(camera between both eyes)
			float4x4 _HeadMat;
			// Projection matrix of current camera
			float4x4 _EyeMat;
			//Grain size
			float2 _Resolution;
			//Areas that are brighter than this value won't receive any grain.
			float _Cutoff;
			//Proportion of dark grains.
			float _Distribution;
			//Brightness of light grains.
			float _Dark;
			//Brighness of dark grains.
			float _Light;

			struct v2f {
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = ComputeScreenPos(o.pos);
				return o;
			}

			//Hashes input to [0,1[ 
			float rand(float3 input) {
				//RNG from here: http://answers.unity3d.com/answers/1141302/view.html
				float ret = frac(sin(dot(input, float3(12.9898, 78.233, 45.5432))) * 43758.5453);
				//Play it safe
				return frac(ret + sin(dot(input, float3(48.34, 68.311, 1.5432))) * 8872.423);
			}

			//Fragment Shader
			float4 frag(v2f i) : COLOR{

				float4 color = tex2D(_MainTex, i.uv);
				float brightness = color.r * 0.3 + color.g * 0.59 + color.b * 0.11;

				//Do nothing if bright enough
				if (brightness >= _Cutoff) {
					return color;
				}

				float depthValue = tex2D(_CameraDepthTexture,i.uv);
				//Vertex position within camera(eye) space
				float4 h = float4(i.uv.x*2.0 - 1.0, (i.uv.y)*2.0 - 1.0, depthValue, 1.0);

				//Vertex position within world space
				float4 d = mul(_EyeMat, h);
				float4 world =  d / d.w;

				//Vertex position within head(camera between your eyes) space
				float4 head = mul(_HeadMat, world);
				head = head / head.w;

				//Hash(RNG) this value + time, nearby vertices(in head space) get the same hash.
				float3 hash = float3(round(head.x * _Resolution.x), round(head.y * _Resolution.y), _Time[0]);

				//Increase the contrast and reduce the number of light vertices.
				float darkness = rand(hash) > _Distribution ? _Light : _Dark;
				
				//Blend it with the scene.
				return color + (1 / _Cutoff * (_Cutoff - brightness)) * float4(darkness, darkness, darkness,1);
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}