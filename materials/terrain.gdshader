shader_type spatial;


uniform float height = 64.0;
uniform float noiseVisibilityModifier = 4;
uniform sampler2D colorGradient;
uniform sampler2D albedoNoise;
uniform sampler2D normalMap;
void fragment() {
	vec4 wordVertex = INV_VIEW_MATRIX * vec4(VERTEX,1.0);
	vec3 modelVertex =(inverse(MODEL_MATRIX) * wordVertex).xyz;
	float gradientUv = (modelVertex.y / height)+ 0.5;
	ALBEDO = ( texture(colorGradient  ,vec2(gradientUv)) - (texture(albedoNoise  ,UV) /noiseVisibilityModifier) ).rgb;
	
	NORMAL_MAP = texture(normalMap,UV).xyz;
}
