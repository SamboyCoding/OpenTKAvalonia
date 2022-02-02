#version 330 core
out vec4 outputColor;

in vec2 texCoord;

uniform sampler2D texture0;

float near = 0.1;
float far  = 10.0;

float LinearizeDepth(float depth)
{
    float z = depth * 2.0 - 1.0; // back to NDC 
    return (2.0 * near * far) / (far + near - z * (far - near));
}

void main()
{
    float depth = LinearizeDepth(gl_FragCoord.z) / far; // divide by far for demonstration
    outputColor = vec4(0, depth, 0, 1.0);
//    outputColor = vec4(vec3(depth), 1.0);
}

//void main()
//{
//    outputColor = texture(texture0, texCoord);
//} 