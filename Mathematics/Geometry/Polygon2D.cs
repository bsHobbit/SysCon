using Mathematics.Vector;

namespace Mathematics.Geometry
{
    /// <summary>
    /// Repräsentation eines Polygon.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    public class Polygon2D
    {
        #region Member
        /// <summary>
        /// Ruft die Eckpunkte des Polygons ab.
        /// </summary>
        public Vector2[] Vertices { get; private set; }

        /// <summary>
        /// Ruft das Zentrum der BoundingBox des Polygons ab.
        /// </summary>
        public Vector2 Center
        {
            get
            {
                if (Vertices != null)
                    return Calculations.CenterOfFigure(Vertices);
                return null;
            }
        }
        #endregion

        #region Konstruktoren
        /// <summary>
        /// Initialisiert eine neue Instanz der Polygon2D Klasse.
        /// </summary>
        /// <param name="Vertices">Eckpunkte des Polygons.</param>
        public Polygon2D(System.Drawing.PointF[] Vertices)
        {
            this.Vertices = new Vector2[Vertices.Length];
            for (int i = 0; i < Vertices.Length; i++)
                this.Vertices[i] = new Vector2(Vertices[i].X, Vertices[i].Y);
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Polygon2D Klasse.
        /// </summary>
        /// <param name="Vertices">Eckpunkte des Polygons.</param>
        public Polygon2D(Vector2[] Vertices)
        {
            this.Vertices = new Vector2[Vertices.Length];
            for (int i = 0; i < Vertices.Length; i++)
                this.Vertices[i] = Vertices[i].Clone();
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Polygon2D Klasse.
        /// </summary>
        /// <param name="Vertices">Eckpunkte des Polygons.</param>
        public Polygon2D(System.Collections.Generic.List<Vector2> Vertices)
        {
            this.Vertices = new Vector2[Vertices.Count];
            for (int i = 0; i < Vertices.Count; i++)
                this.Vertices[i] = Vertices[i].Clone();
        }
        #endregion

        #region Bereichszuschnitt
        /// <summary>
        /// Schneidet ein Polygon auf ein anderes zu, falls dieses aus diesem "herausläuft".
        /// </summary>
        /// <param name="p1">Äußeres Polygon</param>
        /// <param name="p2">Inneres Polygon</param>
        /// <returns>Zugeschnittenes Polygon.</returns>
        public static Polygon2D Clip(Polygon2D p1, Polygon2D p2)
        {
            Vector2[] result = new Vector2[p1.Vertices.Length];
            Vector2 c = p1.Center;
            for (int i = 0; i < p1.Vertices.Length; i++)
            {
                Vector2 newV = SegmentIntersection(p2, new Ray2D(c, p1.Vertices[i]));
                if (newV != null)
                    result[i] = newV;
                else
                    result[i] = p1.Vertices[i].Clone();
            }
            return new Polygon2D(result);
        }
        #endregion

        #region Kollisionserkennung / Bereichsprüfungen

        /// <summary>
        /// Berechnet den Schnittpunkt eines Strahlensegmentes mit diesem Polygon.
        /// </summary>
        /// <param name="r">Strahl, zu welchem der Schnittpunkt berechnet werden soll.</param>
        /// <returns>Schnittpunkt diesem Polygon und dem Strahl r - falls vorhanden, sonst null.</returns>
        public Vector2 SegmentIntersection(Ray2D r)
        {
            return SegmentIntersection(this, r);
        }

        /// <summary>
        /// Berechnet den Schnittpunkt eines Strahlensegmentes mit einem Polygon.
        /// </summary>
        /// <param name="p">Polygon, mit welchem der Schnittpunkt berechnet werden soll.</param>
        /// <param name="r">Strahl, zu welchem der Schnittpunkt berechnet werden soll.</param>
        /// <returns>Schnittpunkt von p und r, falls vorhanden, sonst null.</returns>
        public static Vector2 SegmentIntersection(Polygon2D p, Ray2D r)
        {
            for (int i = 0; i < p.Vertices.Length; i++)
            {
                Ray2D ray;
                if (i == p.Vertices.Length - 1)
                    ray = new Ray2D(p.Vertices[i], p.Vertices[0]);
                else
                    ray = new Ray2D(p.Vertices[i], p.Vertices[i + 1]);

                Vector2 intersectionPoint = Ray2D.SegmentIntersection(ray, r);
                if (intersectionPoint != null)
                    return intersectionPoint;

            }
            return null;
        }

        /// <summary>
        /// Berechnet den Schnittpunkt eines Strahls mit diesem Polygon.
        /// </summary>
        /// <param name="r">Strahl, zu welchem der Schnittpunkt berechnet werden soll.</param>
        /// <returns>Schnittpunkt von diesem Polygon und dem Strahl r - falls vorhanden, sonst null.</returns>
        public Vector2 Intersection(Ray2D r)
        {
            return Intersection(this, r);
        }

        /// <summary>
        /// Berechnet den Schnittpunkt eines Strahls mit einem Polygon.
        /// </summary>
        /// <param name="p">Polygon, mit welchem der Schnittpunkt berechnet werden soll.</param>
        /// <param name="r">Strahl, zu welchem der Schnittpunkt berechnet werden soll.</param>
        /// <returns>Schnittpunkt von p und r, falls vorhanden, sonst null.</returns>
        public static Vector2 Intersection(Polygon2D p, Ray2D r)
        {
            for (int i = 0; i < p.Vertices.Length; i++)
            {
                Ray2D ray;
                if (i == p.Vertices.Length - 1)
                    ray = new Ray2D(p.Vertices[i], p.Vertices[0]);
                else
                    ray = new Ray2D(p.Vertices[i], p.Vertices[i + 1]);

                Vector2 intersectionPoint = Ray2D.Intersection(ray, r);
                if (intersectionPoint != null)
                    return intersectionPoint;

            }
            return null;
        }
        /// <summary>
        /// Prüft ob sich dieses Polygon mit einem anderem überschneidet.
        /// </summary>
        /// <param name="p">Polygon, mit welchem eine überschneidung geprüft werden soll.</param>
        /// <returns>True, wenn sich beide Polygone überschneiden.</returns>
        public bool IntersectsWith(Polygon2D p)
        {
            return IntersectsWith(this, p);
        }

        /// <summary>
        /// Prüft ob sich zwei Polygone überschneiden.
        /// </summary>
        /// <param name="p1">Polygon, mit welchem eine überschneidung geprüft werden soll.</param>
        /// <param name="p2">Polygon, mit welchem eine überschneidung geprüft werden soll.</param>
        /// <returns>True, wenn sich beide Polygone überschneiden.</returns>
        public static bool IntersectsWith(Polygon2D p1, Polygon2D p2)
        {
            for (int i = 0; i < p1.Vertices.Length; i++)
            {
                Ray2D r1;
                if (i == p1.Vertices.Length - 1)
                    r1 = new Ray2D(p1.Vertices[i], p1.Vertices[0]);
                else
                    r1 = new Ray2D(p1.Vertices[i], p1.Vertices[i + 1]);

                for (int j = 0; j < p2.Vertices.Length; j++)
                {
                    Ray2D r2;
                    if (j == p2.Vertices.Length - 1)
                        r2 = new Ray2D(p2.Vertices[j], p2.Vertices[0]);
                    else
                        r2 = new Ray2D(p2.Vertices[j], p2.Vertices[j + 1]);

                    if (Ray2D.SegmentIntersection(r1, r2) != null)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Prüft, ob sich ein Punkt innerhalb dieses Polygons befindet.
        /// </summary>
        /// <param name="v">Punkt, welcher überprüft werden soll.</param>
        /// <returns>True, wenn sich der Punkt innerhalb des Polygons befindet, sonst false.</returns>
        public bool Contains(Vector2 v)
        {
            return Contains(this, v);
        }

        /// <summary>
        /// Prüft, ob sich ein Punkt innerhalb eines Polygons befindet.
        /// </summary>
        /// <param name="p1">Polygon, welches zur prüfung verwendet wird.</param>
        /// <param name="v">Punkt, welcher geprüft werden soll.</param>
        /// <returns>True, wenn sich der Punkt v innerhalb des Polyogn p1 befindet.</returns>
        public static bool Contains(Polygon2D p1, Vector2 v)
        {
            int rCross = 0;
            int lCross = 0;
            for (int i = 0; i < p1.Vertices.Length; i++)
            {
                Vector2 edgeStart = p1.Vertices[i];
                Vector2 edgeEnd = null;
                if (i == (p1.Vertices.Length - 1))
                    edgeEnd = p1.Vertices[0];
                else
                    edgeEnd = p1.Vertices[i + 1];
                if (FloatingPointComparison.Equals(v.X, edgeStart.X) && FloatingPointComparison.Equals(v.Y, edgeStart.Y)) return true;
                if ((edgeStart.Y > v.Y) != (edgeEnd.Y > v.Y))
                {
                    int Term1 = (int)((edgeStart.X - v.X) * (edgeEnd.Y - v.Y) - (edgeEnd.X - v.X) * (edgeStart.Y - v.Y));
                    int Term2 = (int)((edgeEnd.Y - v.Y) - (edgeStart.Y - edgeEnd.Y));
                    if ((Term1 > 0) == (Term2 >= 0)) rCross++;
                }
                if ((edgeStart.Y < v.Y) != (edgeEnd.Y < v.Y))
                {
                    int Term1 = (int)((edgeStart.X - v.X) * (edgeEnd.Y - v.Y) - (edgeEnd.X - v.X) * (edgeStart.Y - v.Y));
                    int Term2 = (int)((edgeEnd.Y - v.Y) - (edgeStart.Y - edgeEnd.Y));
                    if ((Term1 < 0) == (Term2 <= 0)) lCross++;
                }
            }
            if ((rCross % 2) != (lCross % 2)) return true;
            return ((rCross % 2) == 1);
        }

        #endregion
    }
}
