shader_type canvas_item;

uniform bool flash_white = false;
uniform bool flash_red = false;

void fragment() {
    vec4 tex_color = texture(TEXTURE, UV);
	if (flash_red){
        vec3 blended = mix(tex_color.rgb, vec3(1.0, 0.0, 0.0), 0.5);
        COLOR = vec4(blended, tex_color.a);
	}
    else if (flash_white) {
        vec3 blended = mix(tex_color.rgb, vec3(1.0), 0.5);
        COLOR = vec4(blended, tex_color.a);
    } else {
        COLOR = tex_color;
    }
}