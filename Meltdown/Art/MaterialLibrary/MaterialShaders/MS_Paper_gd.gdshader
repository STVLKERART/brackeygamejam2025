shader_type spatial;
render_mode blend_mix, unshaded;

void fragment() {
    vec2 edge = smoothstep(0.0, 0.2, UV) * (1.0 - smoothstep(0.8, 1.0, UV));
    float alpha = edge.x * edge.y;

    ALBEDO = vec3(0.95);
    ALPHA = alpha;

}