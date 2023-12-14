using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace NonFullScreenSample.Shaders
{
    public sealed class UiOpenGlShader : IDisposable
    {
        private readonly int _handle;
        private bool _disposedValue;

		public UiOpenGlShader(string vertexPath, string fragmentPath)
		{
			//Read the shader source files

			using StreamReader vertReader = new(vertexPath, Encoding.UTF8);
			var vertexShaderSource = vertReader.ReadToEnd();

			using StreamReader fragReader = new(fragmentPath, Encoding.UTF8);
			var fragmentShaderSource = fragReader.ReadToEnd();

			//Create GL shaders for the shader source files
			var vertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShader, vertexShaderSource);

			var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShader, fragmentShaderSource);
			
			//Compile and error-check the vertex shader
			GL.CompileShader(vertexShader);

			var infoLogVert = GL.GetShaderInfoLog(vertexShader);
			if (infoLogVert != string.Empty)
			{
				Console.Error.WriteLine($"UI: Error compiling vertex shader: {infoLogVert}");
			}

			//Compile and error-check the fragment shader
			GL.CompileShader(fragmentShader);

			var infoLogFrag = GL.GetShaderInfoLog(fragmentShader);

			if (infoLogFrag != string.Empty)
			{
				Console.Error.WriteLine($"UI: Error compiling fragment shader: {infoLogFrag}");
			}

			//Create a GL program, and attach shaders
			_handle = GL.CreateProgram();
			GL.AttachShader(_handle, vertexShader);
			GL.AttachShader(_handle, fragmentShader);
			
			//Link
			GL.LinkProgram(_handle);
			
			//Cleanup
			GL.DetachShader(_handle, vertexShader);
			GL.DetachShader(_handle, fragmentShader);
			GL.DeleteShader(vertexShader);
			GL.DeleteShader(fragmentShader);
			
			Console.WriteLine("UI: Compiled shaders successfully");
		}
		
		public void Use()
		{
			GL.UseProgram(_handle);
		}

		public int GetAttribLocation(string attribName) => GL.GetAttribLocation(_handle, attribName);

		public int GetUniformLocation(string uniformName) => GL.GetUniformLocation(_handle, uniformName);

		public void SetInt(string name, int value) => GL.Uniform1(GetUniformLocation(name), value);

		public void SetMatrix4(string name, Matrix4 value) => GL.UniformMatrix4(GetUniformLocation(name), true, ref value);

		~UiOpenGlShader()
		{
			GL.DeleteProgram(_handle);
		}
		
		public void Dispose()
		{
			if (_disposedValue)
			{
				return;
			}

			GL.DeleteProgram(_handle);

			_disposedValue = true;
			
			GC.SuppressFinalize(this);
		}
    }
}