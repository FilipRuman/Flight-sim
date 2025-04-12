shader_type spatial;

uniform sampler2D DEPTH_TEXTURE : hint_depth_texture , filter_linear_mipmap;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear_mipmap;

uniform vec3 albedo : source_color;
uniform vec3 albedo2 : source_color;

uniform float metallic;
uniform float roughnnes;

uniform sampler2D normal1;
uniform sampler2D normal2;
uniform sampler2D wave;

uniform vec2 wave1Dir;
uniform vec2 wave2Dir;
uniform float timeScale;

uniform float waveNoiseScale = 10;
uniform float waveHeightScale = 15;


uniform vec4 colorDeep :source_color;
uniform vec4 colorShallow :source_color;
uniform float beersLaw = 2.0;
uniform float depthOffset = -.75;

varying float height;
varying vec3 worldPos;

float fresnel(float amount, vec3 normal, vec3 view){
	return pow((1.0- clamp(dot(normalize(normal),normalize(view)),0.0,1.0)),amount);
}
void vertex (){
	worldPos = (MODEL_MATRIX * vec4(VERTEX,1.0)).xyz;
	height = texture(wave,worldPos.xz / waveNoiseScale + TIME  * timeScale).r;
	VERTEX.y += height * waveHeightScale;
}
void fragment() {
	float depthTexture = texture(DEPTH_TEXTURE,SCREEN_UV).r * 2.0 - 1.0;
	float depth  = PROJECTION_MATRIX[3][2] / (depthTexture + PROJECTION_MATRIX[3][2]);
	float depthBlend = exp((depth +VERTEX.z + depthOffset) * - beersLaw);
	depthBlend = clamp(1.0 - depthBlend,0.0,1.0);
	float	 depthBlendPower = clamp(pow(depthBlend,2.5),0.0,1.0);

	vec3 screenColor = textureLod(SCREEN_TEXTURE, SCREEN_UV,depthBlendPower*2.5).rgb;
	vec3 depthColor = mix(colorShallow.rgb,colorDeep.rgb,depthBlendPower);
	vec3 color = mix(screenColor * depthColor,depthColor*0.25, depthBlendPower * .5);

	vec2 time1 = TIME  * wave1Dir * timeScale;
	vec2 time2 = TIME  * wave2Dir * timeScale;

	vec3 normalBlend = mix(texture(normal1,UV +time1).rgb,texture(normal2,UV + time2).rgb,.5);
	float fresnel = fresnel(5.0,NORMAL,VIEW);
	vec3 surfaceColor = mix(albedo,albedo2,fresnel);
	NORMAL = normalBlend;
	ALBEDO = mix (surfaceColor,color, .75);
METALLIC = metallic;
ROUGHNESS = roughnnes;
}
