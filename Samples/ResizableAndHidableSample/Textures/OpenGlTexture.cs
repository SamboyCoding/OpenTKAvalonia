using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace NonFullScreenSample.Textures
{
    public sealed class OpenGlTexture : IDisposable
    {
        private readonly int _handle;
        private bool _disposedValue;

        public OpenGlTexture()
        {
            _handle = GL.GenTexture();
        }

        public void LoadFromFile(string path)
        {
            var image = Image.Load<Rgba32>(path);

            //ImageSharp counts (0, 0) as top-left, OpenGL wants it to be bottom-left. fix.
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            //Convert ImageSharp's format into a byte array, so we can use it with OpenGL.
            List<byte> pixels = new(4 * image.Width * image.Height);

            for (int y = 0; y < image.Height; y++)
            {
                var row = image.GetPixelRowSpan(y);

                for (int x = 0; x < image.Width; x++)
                {
                    pixels.Add(row[x].R);
                    pixels.Add(row[x].G);
                    pixels.Add(row[x].B);
                    pixels.Add(row[x].A);
                }
            }

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, _handle);
        }

        ~OpenGlTexture()
        {
            GL.DeleteTexture(_handle);
        }
		
        public void Dispose()
        {
            if (_disposedValue)
            {
                return;
            }

            GL.DeleteTexture(_handle);

            _disposedValue = true;
            
            GC.SuppressFinalize(this);
        }
    }
}