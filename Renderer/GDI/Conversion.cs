namespace Renderer.GDI
{
    /// <summary>
    /// Diverse Konvertierungen die für GDI notwendig sind.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    internal static class Conversion
    {
        /// <summary>
        /// Konvertiert ein Vector2 in ein System.Drawing.Point.
        /// </summary>
        /// <param name="v">Zu konvertierender Vektor.</param>
        /// <returns>System.Drawing.Point struktur mit dem Inhalt des Vektor (kovertierung von Double nach int!).</returns>
        public static System.Drawing.Point ToPoint(Mathematics.Vector.Vector2 v) { return new System.Drawing.Point((int)v.X, (int)v.Y); }
        /// <summary>
        /// Konvertiert ein Vector2 in ein System.Drawing.PointF.
        /// </summary>
        /// <param name="v">Zu konvertierender Vektor.</param>
        /// <returns>System.Drawing.PointF struktur mit dem Inhalt des Vektor (kovertierung von Double nach float!).</returns>
        public static System.Drawing.PointF ToPointF(Mathematics.Vector.Vector2 v) { return new System.Drawing.PointF((float)v.X, (float)v.Y); }
        /// <summary>
        /// Konvertiert ein Vector2-Array in ein System.Drawing.Point-Array.
        /// </summary>
        /// <param name="v">Zu konvertierendes Vektor-Array.</param>
        /// <returns>System.Drawing.Point-Array struktur mit dem Inhalt des Vektor-Arrays (kovertierung von Double nach int!).</returns>
        public static System.Drawing.Point[] ToPointArray(Mathematics.Vector.Vector2[] VectorArray)
        {
            if (VectorArray == null || VectorArray.Length == 0)
                return null;
            System.Drawing.Point[] res = new System.Drawing.Point[VectorArray.Length];
            for (int i = 0; i < VectorArray.Length; i++)
                res[i] = new System.Drawing.Point((int)VectorArray[i].X, (int)VectorArray[i].Y);
            return res;
        }
        /// <summary>
        /// Konvertiert ein Vector2-Array in ein System.Drawing.PointF-Array.
        /// </summary>
        /// <param name="v">Zu konvertierendes Vektor-Array.</param>
        /// <returns>System.Drawing.PointF-Array struktur mit dem Inhalt des Vektor-Arrays (kovertierung von Double nach float!).</returns>
        public static System.Drawing.PointF[] ToPointFArray(Mathematics.Vector.Vector2[] VectorArray)
        {
            if (VectorArray == null || VectorArray.Length == 0)
                return null;
            System.Drawing.PointF[] res = new System.Drawing.PointF[VectorArray.Length];
            for (int i = 0; i < VectorArray.Length; i++)
                res[i] = new System.Drawing.PointF((int)VectorArray[i].X, (int)VectorArray[i].Y);
            return res;
        }
    }
}
