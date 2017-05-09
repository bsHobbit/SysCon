namespace Mathematics
{
    /// <summary>
    /// Einige Hilfsfunktionen zum berechnen von Dingen, die man immer wieder benötigt.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    public static class Calculations
    {
        /// <summary>
        /// Berechnet das umschließende Rechteck für ein Vector2 Array.
        /// </summary>
        /// <param name="Vertices">Vector2 Array, für welches das umschließende Array berechnet werden soll.</param>
        /// <returns>RectangleF Struktur mit den umschließenden Koordinaten.</returns>
        public static System.Drawing.RectangleF GetBoundingRect(System.Drawing.PointF[] Vertices)
        {
            Vector.Vector2[] vertices = new Vector.Vector2[Vertices.Length];
            for (int i = 0; i < Vertices.Length; i++)
                vertices[i] = new Vector.Vector2(Vertices[i].X, Vertices[i].Y);
            return GetBoundingRect(vertices);
        }

        /// <summary>
        /// Berechnet das umschließende Rechteck für ein Vector2 Array.
        /// </summary>
        /// <param name="Vertices">Vector2 Array, für welches das umschließende Array berechnet werden soll.</param>
        /// <returns>RectangleF Struktur mit den umschließenden Koordinaten.</returns>
        public static System.Drawing.RectangleF GetBoundingRect(Vector.Vector2[] Vertices)
        {
            double xmin = double.MaxValue, ymin = double.MaxValue, xmax = double.MinValue, ymax = double.MinValue;

            if (Vertices == null || Vertices.Length == 0) { xmin = 0; ymin = 0; xmax = 1; ymax = 1; }
            else
            {
                for (int i = 0; i < Vertices.Length; i++)
                {
                    if (Vertices[i].X > xmax)
                        xmax = Vertices[i].X;
                    if (Vertices[i].X < xmin)
                        xmin = Vertices[i].X;


                    if (Vertices[i].Y > ymax)
                        ymax = Vertices[i].Y;
                    if (Vertices[i].Y < ymin)
                        ymin = Vertices[i].Y;
                }
            }

            return new System.Drawing.RectangleF(System.Convert.ToSingle(xmin),
                                                 System.Convert.ToSingle(ymin),
                                                 System.Convert.ToSingle(xmax - xmin),
                                                 System.Convert.ToSingle(ymax - ymin));
        }

        /// <summary>
        /// Berechnet den Mittelpunkt eines Polygons.
        /// </summary>
        /// <param name="Vertices">Eckpunkte, welche das Polygon definieren.</param>
        /// <returns>Mittelpunkt des Polygons, oder null falls dieser nicht berechnet werden konnte.</returns>
        public static Vector.Vector2 CenterOfFigure(Vector.Vector2[] Vertices)
        {
            if (Vertices == null || Vertices.Length == 0)
                return null;
            return CenterOfFigure(GetBoundingRect(Vertices));
        }

        /// <summary>
        /// Berechnet den Mittelpunkt eines Polygons.
        /// </summary>
        /// <param name="Vertices">Eckpunkte, welche das Polygon definieren.</param>
        /// <returns>Mittelpunkt des Polygons, oder null falls dieser nicht berechnet werden konnte.</returns>
        public static Vector.Vector2 CenterOfFigure(System.Collections.Generic.List<Vector.Vector2> Vertices)
        {
            if (Vertices == null || Vertices.Count == 0)
                return null;
            return CenterOfFigure(Vertices.ToArray());
        }

        /// <summary>
        /// Berechnet den Mittelpunkt eines Rechteckes.
        /// </summary>
        /// <param name="Rectangle">Rechteck, von welchem der Mittelpunkt berechnet werden soll.</param>
        /// <returns>Mittelpunkt des Polygons, oder null falls dieser nicht berechnet werden konnte.</returns>
        public static Vector.Vector2 CenterOfFigure(System.Drawing.RectangleF Rectangle)
        {
            return new Vector.Vector2(Rectangle.X + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height / 2);
        }


        /// <summary>
        /// Wandelt einen Winkel, der im Bogenmaß angegeben wurde, in einen Winkel um, der im Winkelmaß angegeben ist.
        /// </summary>
        /// <param name="Angle">Winkel im Bogenmaß.</param>
        /// <returns>Berechneter Winkel im Winkelmaß.</returns>
        public static double RadianToDegree(double Angle)
        {
            return Angle * (180.0 / System.Math.PI);
        }

        /// <summary>
        /// Wandelt einen Winkel, der im Gradmaß angegeben wurde, in einen Winkel um, der im Bogenmaß angegeben ist.
        /// </summary>
        /// <param name="Angle">Winkel im Gradmaß.</param>
        /// <returns>Berechneter Winkel im Bogenmaß.</returns>
        public static double DegreeToRadian(double Angle)
        {
            return System.Math.PI * Angle / 180.0;
        }

    }
}
