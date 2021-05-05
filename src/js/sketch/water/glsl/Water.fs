precision highp float;

uniform mat4 modelMatrix;
uniform vec3 cameraPosition;
uniform float time;
uniform vec2 resolution;
uniform sampler2D tScene;
uniform sampler2D tNormal;

varying vec3 vPosition;
varying vec2 vUv;

#pragma glslify: convertHsvToRgb = require(glsl-util/convertHsvToRgb)

void main() {
  vec3 normal1 = texture2D(tNormal, vUv + vec2(time * 0.012, 0.0)).xyz * 2.0 - 1.0;
  vec3 normal2 = texture2D(tNormal, (vUv + vec2(0.0, 0.1)) * 0.4 + vec2(time * 0.04, 0.0)).xyz * 2.0 - 1.0;
  vec3 normal3 = texture2D(tNormal, (vUv + vec2(0.0, 0.6)) + vec2(time * -0.01, 0.0)).xyz * 2.0 - 1.0;
  vec3 normal4 = texture2D(tNormal, (vUv + vec2(0.0, 0.7)) * 0.4 + vec2(time * -0.05, 0.0)).xyz * 2.0 - 1.0;
  vec3 normal = (modelMatrix * vec4(normalize(normal1 + normal2 + normal3 + normal4), 1.0)).xyz;
  vec3 light = normalize(vec3(0.0, 1.0, 0.5));
  float diffuse = clamp(dot(normal, light), 0.0, 1.0);
  vec3 hsv = vec3(
    0.46 - diffuse * 2.0,
    1.0,
    diffuse * 2.0 - 0.6
  );
  vec3 rgb = convertHsvToRgb(hsv);

  vec2 uv = gl_FragCoord.xy / resolution;
  vec4 tSceneColor = texture2D(tScene, uv + normal.xy * 0.2);

  gl_FragColor = vec4(rgb + pow(diffuse * 2.0, 30.0) * 0.2, 1.0) + tSceneColor;
}