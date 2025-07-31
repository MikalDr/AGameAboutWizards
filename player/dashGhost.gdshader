shader_type canvas_item;

void fragment() {
    vec4 tex_color = texture(TEXTURE, UV);
    vec3 blended = mix(tex_color.rgb, vec3(1.0), 0.5);
    COLOR = vec4(blended, tex_color.a * COLOR.a);
}