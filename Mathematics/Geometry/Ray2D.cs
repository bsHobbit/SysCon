using Mathematics.Vector;

namespace Mathematics.Geometry
{
    /// <summary>
    /// Repräsentation einer 2D Linie
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    public class Ray2D
    {

        #region Member
        /// <summary>
        /// Erster Punkt, welcher die Linie beschreibt.
        /// </summary>
        public Vector2 P1 { get; set; }

        /// <summary>
        /// Zweiter Punkt, welcher die Linie beschreibt.
        /// </summary>
        public Vector2 P2 { get; set; }
        #endregion

        #region Konstruktor
        /// <summary>
        /// Initialisiert eine neue Instanz der Line2D Klasse.
        /// </summary>
        /// <param name="P1">Ein Punkt auf der Linie.</param>
        /// <param name="P2">Ein weiterer Punkt auf der Linie.</param>
        public Ray2D(Vector2 P1, Vector2 P2)
        {
            this.P1 = P1;
            this.P2 = P2;
        }
        #endregion

        #region Orientierung von Punkten zur Linie
        /// <summary>
        /// Berechnet die Orientierung eines Punktes p im Verhältnis zur Linie die von a und b aufgespannt werden.
        /// </summary>
        /// <param name="l">Linie, zu welcher die Orientierung berechnet werden soll.</param>
        /// <param name="v">Vektor, von welchem die Orientierung berechnet werden soll.</param>
        /// <returns>Kleiner null "Links" von der Linie, größer null rechts von der Linie, gleich null auf der Linie.</returns>
        public static double OrientationToLine(Ray2D l, Vector.Vector2 v) { return (l.P2.X - l.P1.X) * (v.Y - l.P1.Y) - (l.P2.Y - l.P1.Y) * (v.X - l.P1.X); }

        /// <summary>
        /// Berechnet die Orientierung eines Punktes p im Verhältnis zur Linie die von a und b aufgespannt werden.
        /// </summary>
        /// <param name="l">Linie, zu welcher die Orientierung berechnet werden soll.</param>
        /// <param name="v">Vektor, von welchem die Orientierung berechnet werden soll.</param>
        /// <returns>Kleiner null "Links" von der Linie, größer null rechts von der Linie, gleich null auf der Linie.</returns>
        public double OrientationToLine(Vector2 v) { return OrientationToLine(this, v); }
        #endregion

        #region Schnittpunktberechnung
        /// <summary>
        /// Berechnet den Schnittpunkt zweier Linien.
        /// </summary>
        /// <param name="r1">Linie, zu welcher der Schnittpunkt mit r2 berechnet werden soll.</param>
        /// <param name="r2">Linie, zu welcher der Schnittpunkt mit r1 berechnet werden soll.</param>
        /// <param name="ua">Schnittpunktabschnitt auf der ersten Linie.</param>
        /// <param name="ub">Schnittpunktabschnitt auf der zweiten Linie.</param>
        private static bool Intersection(Ray2D r1, Ray2D r2, ref double ua, ref double ub)
        {
            double denominator = (r2.P2.Y - r2.P1.Y) * (r1.P2.X - r1.P1.X) - (r2.P2.X - r2.P1.X) * (r1.P2.Y - r1.P1.Y);

            if (!FloatingPointComparison.Equals(denominator, 0))
            {
                ua = ((r2.P2.X - r2.P1.X) * (r1.P1.Y - r2.P1.Y) - (r2.P2.Y - r2.P1.Y) * (r1.P1.X - r2.P1.X)) / denominator;
                ub = ((r1.P2.X - r1.P1.X) * (r1.P1.Y - r2.P1.Y) - (r1.P2.Y - r1.P1.Y) * (r1.P1.X - r2.P1.X)) / denominator;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Berechnet den Schnittpunkt zweier Linien.
        /// </summary>
        /// <param name="r1">Linie, zu welcher der Schnittpunkt mit r2 berechnet werden soll.</param>
        /// <param name="t2">Linie, zu welcher der Schnittpunkt mit r1 berechnet werden soll.</param>
        /// <returns>Schnittpunkt der beiden Linien. Null, wenn es keinen Schnittpunkt gibt.</returns>
        public static Vector2 Intersection(Ray2D r1, Ray2D t2)
        {
            double ua = 0, ub = 0;
            if (Intersection (r1,t2, ref ua, ref ub))
                return (r1.P2 - r1.P1) * ua;
            return null;
        }

        /// <summary>
        /// Berechnet den Schnittpunkt zweier Linien innerhalb des angegeben Segmentes.
        /// </summary>
        /// <param name="r1">Linie, zu welcher der Schnittpunkt mit r2 berechnet werden soll.</param>
        /// <param name="r2">Linie, zu welcher der Schnittpunkt mit r1 berechnet werden soll.</param>
        /// <returns>Schnittpunkt der beiden Linien. Null, wenn es keinen Schnittpunkt gibt.</returns>
        public static Vector2 SegmentIntersection(Ray2D r1, Ray2D r2)
        {
            double ua = 0, ub = 0;
            if (Intersection(r1, r2, ref ua, ref ub))
            {
                if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
                    return r1.P1 + ((r1.P2 - r1.P1) * ua);
            }
            return null;
        }
        #endregion
    }
}
