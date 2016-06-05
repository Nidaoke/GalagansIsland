//This was made following a tutorial at http://www.gamasutra.com/blogs/SvyatoslavCherkasov/20140531/218753/Shader_tutorial_CRT_emulation.php

Shader "Custom/ExperimentalCRTShader"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_VertsColor("Verts fill color", Float) = 0
		_VertsColor("Verts fill color 2", Float) = 0
		_Contrast("Contrast", Float) = 0
		_Brightness("Brightness", Float) = 0
		_Brightness("ScanStrength", Float) = 0

	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"

			#pragma target 3.0


//			struct appdata
//			{
//				float4 vertex : POSITION;
//				float2 uv : TEXCOORD0;
//			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
				float4 scr_pos: TEXCOORD1;
			};

			uniform sampler2D _MainTex;
			uniform float _VertsColor;
			uniform float _VertsColor2;
			uniform float _Contrast;
			uniform float _Brightness;
			uniform float _ScanStrength;


			v2f vert(appdata_img v)
			{
				v2f o;

				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);

				o.scr_pos = ComputeScreenPos(o.pos);


				return o;


        
			}
			
//			sampler2D _MainTex;

			half4 frag(v2f i): COLOR
			{
				half4 col = tex2D(_MainTex, i.uv);


				float2 ps = i.scr_pos.xy * _ScreenParams.xy / i.scr_pos.w; 

				int pp = (int)ps.x % 3;
				float4 outColor = float4(0.1f,0.1f,0.1f,1);
				float4 muls = float4(0,0,0,1);
				if(pp == 1)
				{
					outColor.r = col.r;
					muls.r = _VertsColor;
					muls.g = _VertsColor2;
					//muls.b = _VertsColor2;
				}
				else if (pp == 2)
				{
					outColor.g = col.g;
					muls.g = _VertsColor;
					muls.b = _VertsColor2;
					//muls.r = _VertsColor2;
				}
				else
				{
					outColor.b = col.b;
					muls.b = _VertsColor;
					muls.r = _VertsColor2;
					//muls.g = _VertsColor2;
				}

				if( (((int)ps.x % 6<3 && (int)ps.y % 3 == 0)) ||  
					(((int)ps.x % 6>=3 && ((int)ps.y+2) % 3 == 0)))
				{
					muls *= float4(_ScanStrength, _ScanStrength, _ScanStrength, 1);
				}

				outColor = outColor*muls;
				outColor += (_Brightness / 255);
				outColor = outColor - _Contrast*(outColor-1.0)*outColor*(outColor-0.5);
				return outColor;

//				return col;
			}

//			fixed4 frag (v2f i) : SV_Target
//			{
//				fixed4 col = tex2D(_MainTex, i.uv);
//				// just invert the colors
//				col = 1 - col;
//				return col;
//			}
			ENDCG
		}
	}
}
