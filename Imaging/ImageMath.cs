using Mathematics.Vector;
using System.Drawing;

namespace Imaging
{
    /// <summary>
    /// Klasse, welche allgemeine Berechnungen für Bilder zur verfügung stellt.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    public static class ImageMath
    {
        /// <summary>
        /// Berechnet das Rechteck, in welchem ein Bild gerendert werden muss, damit das Seitenverhältnis beibehalten wird.
        /// </summary>
        /// <param name="imageWidth">Breite des Bildes in Pixel.</param>
        /// <param name="imageHeight">Breite des Bildes in Pixel.</param>
        /// <param name="renderWidth">Breite des Bereichs in welchem gerendert werden soll.</param>
        /// <param name="renderHeight">Höhe des Bereichs in welchem gerendert werden soll.</param>
        /// <returns>Rechteck, in welchem gerendert werden muss, damit das Seitenverhältnis des Bildes beibehalten wird.</returns>
        public static RectangleF GetRenderRect(int imageWidth, int imageHeight, int renderWidth, int renderHeight)
        {
            return GetRenderRect(new Vector2(imageWidth, imageHeight), new Vector2(renderWidth, renderHeight));
        }

        /// <summary>
        /// Berechnet das Rechteck, in welchem ein Bild gerendert werden muss, damit das Seitenverhältnis beibehalten wird.
        /// </summary>
        /// <param name="ImageSize">Größe des Bildes in Pixel.</param>
        /// <param name="RenderSize">Größe des Renderbereichs in Pixel.</param>
        /// <returns>Rechteck, in welchem gerendert werden muss, damit das Seitenverhältnis des Bildes beibehalten wird.</returns>
        public static RectangleF GetRenderRect(Vector2 ImageSize, Vector2 RenderSize)
        {
            RectangleF result = new RectangleF();
            if (ImageSize != null && RenderSize != null)
            {
                double aspectRatio = ImageSize.Y / ImageSize.X;
                double aspectHeight = RenderSize.X * aspectRatio;
                double aspectWidth = RenderSize.X;
                if (aspectHeight > RenderSize.Y)
                {
                    aspectRatio = ImageSize.X / ImageSize.Y;
                    aspectHeight = RenderSize.Y;
                    aspectWidth = RenderSize.Y * aspectRatio;
                }
                result = new RectangleF((float)(RenderSize.X / 2 - aspectWidth / 2), 
                                        (float)(RenderSize.Y / 2 - aspectHeight / 2), 
                                        (float)aspectWidth, 
                                        (float)aspectHeight);
            }
            return result;
        }

        /// <summary>
        /// Berechnet den Stride, den ein Bild im .NET Bitmap-Format haben würde.
        /// </summary>
        /// <param name="ImageWidth">Breite des Bildes in Pixel.</param>
        /// <param name="PixelFormat">Format des Bitmap.</param>
        /// <returns>Bytes pro Zeile, die im .NET Speicher für das Bitmap hinterlegt ist.</returns>
        public static int GetBitmapStride(int ImageWidth, System.Drawing.Imaging.PixelFormat PixelFormat)
        {
            int result = ImageWidth;

            if (PixelFormat == System.Drawing.Imaging.PixelFormat.Format16bppGrayScale)
                result *= 2;
            else if (PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                result *= 3;
            else if (PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                result *= 4;
            else if (PixelFormat == System.Drawing.Imaging.PixelFormat.Format48bppRgb)
                result *= 6;

            int strideRemainder = result % 4;
            if (strideRemainder > 0)
                result += (4 - strideRemainder);

            return result;
        }
    }
}
