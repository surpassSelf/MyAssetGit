Shader "Custom/shaderTest" {
	Properties {
		_Color ("Color", Color) = (1, 0.2, 0.1, 1)
	}
	SubShader {
		Pass{
			Lighting Off

			Color [_Color]
			/*Material {
				Diffiuse [_Color]
			}*/
		}
	}
	FallBack "Diffuse"
}
