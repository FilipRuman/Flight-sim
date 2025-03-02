shader_type spatial;
// Prevent the quad from being affected by lighting and fog. This also improves performance.
render_mode unshaded, fog_disabled;
uniform sampler2D depth_texture : source_color, hint_depth_texture;
uniform sampler2D textureToOutput;
uniform sampler2D cloudsDepth;

const float DepthIgnoreDistance =  5000.;
void fragment() {
  vec4 color = texture(textureToOutput, SCREEN_UV);
// color.w != 0
if(color.w > 0.0001){
  ALBEDO = color.xyz;

  float depth = texture(depth_texture, SCREEN_UV).x;
  vec3 ndc = vec3(SCREEN_UV * 2.0 - 1.0, depth);
  vec4 view = INV_PROJECTION_MATRIX * vec4(ndc, 1.0);
  view.xyz /= view.w;
  float linear_depth = -view.z;
	if(linear_depth >DepthIgnoreDistance /* makes clouds visible far away*/){
    ALPHA = color.w;
	}else{
		//	x - ray entry distance y - ray exit distance
		vec2 cloudDepth = texture(cloudsDepth, SCREEN_UV).xy;
 		if(linear_depth > cloudDepth.x  ){
	    	float wholeCloudDepth = cloudDepth.y - cloudDepth.x;
			float linearDepthInsideCloud = linear_depth - cloudDepth.x;
		
			float alphaModifier = min(linearDepthInsideCloud / wholeCloudDepth,1)   ;
		
			ALPHA = color.w * alphaModifier ;
		}
    	else{
 	   		ALPHA = 0.;
		}
	}
	// depth debug
	
	
}else
	ALPHA = 0.;
}
void vertex() {
  POSITION = vec4(VERTEX.xy, 1.0, 1.0);
}