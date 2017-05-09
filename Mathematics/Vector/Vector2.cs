namespace Mathematics.Vector
{
    /// <summary>
    /// Repräsentation eines 2D Vektors und seiner zwei Komponenten.
    /// 
    /// X : Double
    /// Y : Double
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    public class Vector2
    {
        #region Member
        /// <summary>
        /// Ruft die X-Komponente des Vektors ab, oder legt diese fest.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Ruft die Y-Komponente des Vektors ab, oder legt diese fest.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Berechnet die Länge des Vektors und ruft diese ab.
        /// </summary>
        public double Length { get { return System.Math.Sqrt(X * X + Y * Y); } }
        #endregion

        #region Konstruktoren
        /// <summary>
        /// Initialisiert eine neue Instanz der Vector-Struktur.
        /// </summary>
        public Vector2() : this(0f, 0f) {  }
        /// <summary>
        /// Initialisiert eine neue Instanz der Vector-Struktur.
        /// </summary>
        /// <param name="X">Der X-Offset des neuen Vector.</param>
        /// <param name="Y">Der Y-Offset des neuen Vector.</param>
        public Vector2(double X, double Y) { this.X = X; this.Y = Y; }
        /// <summary>
        /// Initialisiert eine neue Instanz der Vector-Struktur.
        /// </summary>
        /// <param name="V">Wert für die X -und Y-Komponente des Vektors</param>
        public Vector2(double V) : this(V, V){  }
        #endregion

        #region Operatoren
        /// <summary>
        /// Addiert zwei Vektoren und gibt das Ergebnis als Vektor zurück.
        /// </summary>
        /// <param name="a">Der erste zu addierende Vektor.</param>
        /// <param name="b">Der zweite zu addierende Vektor.</param>
        /// <returns>Die Summe von a und b.</returns>
        public static Vector2 operator +(Vector2 a, Vector2 b)          { return new Vector2(a.X + b.X, a.Y + b.Y); }
        /// <summary>
        /// Subtrahiert einen angegebenen Vektor von einem anderen.
        /// </summary>
        /// <param name="a">Der Vektor, von dem b subtrahiert wird.</param>
        /// <param name="b">Der Vektpr, der von a subtrahiert wird.</param>
        /// <returns>Die Differenz zwischen a und b.</returns>
        public static Vector2 operator -(Vector2 a, Vector2 b)          { return new Vector2(a.X - b.X, a.Y - b.Y); }
        /// <summary>
        /// Multipliziert den angegebenen Skalar mit dem angegebenen Vektor und gibt den sich ergebenden Vektor zurück.
        /// </summary>
        /// <param name="a">Der zu multiplizierende Vektor.</param>
        /// <param name="v">Der zu multiplizierende Skalar.</param>
        /// <returns>Das Ergebnis der Multiplikation von scalar und vector.</returns>
        public static Vector2 operator *(Vector2 a, double v)           { return new Vector2(a.X * v, a.Y * v); }
        /// <summary>
        /// Multipliziert den angegebenen Skalar mit dem angegebenen Vektor und gibt den sich ergebenden Vektor zurück.
        /// </summary>
        /// <param name="a">Der zu multiplizierende Vektor.</param>
        /// <param name="v">Der zu multiplizierende Skalar.</param>
        /// <returns>Das Ergebnis der Multiplikation von scalar und vector.</returns>
        public static Vector2 operator *(double v, Vector2 a)           { return a * v; }
        /// <summary>
        /// Berechnet das Skalarprodukt von zwei angegebenen Vektorem und gibt das Ergebnis als eine Double zurück.
        /// </summary>
        /// <param name="a">Der erste zu multiplizierende Vektor.</param>
        /// <param name="b">Der zweite zu multiplizierende Vektor.</param>
        /// <returns>Double mit dem Skalarprodukt von a und b zurück</returns>
        public static double  operator *(Vector2 a, Vector2 b)          { return a.X * b.X + a.Y * b.Y; }
        /// <summary>
        /// Transformiert den Koordinatenbereich des angegebenen Vektors mithilfe des angegebenen Matrix.
        /// </summary>
        /// <param name="Vector">Der zu transformierende Vektor.</param>
        /// <param name="Matrix">Die Transformation, die auf den Vektor angewendet werden soll.</param>
        /// <returns>Das Ergebnis der Transformation vector von matrix.</returns>
        public static Vector2 operator *(Vector2 Vector, Matrix Matrix) { return Matrix * (Matrix)Vector; }
        /// <summary>
        /// Dividiert den angegebenen Vektor durch den angegebenen Skalar und gibt den sich ergebenden Vektor zurück.
        /// </summary>
        /// <param name="a">Der zu dividierende Vektor.</param>
        /// <param name="v">Der Skalar, durch den vector geteilt wird.</param>
        /// <returns>Das Ergebnis der Division von vector durch scalar.</returns>
        public static Vector2 operator /(Vector2 a, double v)           { return new Vector2(a.X / v, a.Y / v); }
        /// <summary>
        /// Negiert den angegebenen Vektor.
        /// </summary>
        /// <param name="a">Der zu negierende Vektor.</param>
        /// <returns>Ein Vektor, dessen X und Y negiert wurden.</returns>
        public static Vector2 operator -(Vector2 a)                     { return new Vector2(-a.X, -a.Y); }
        #endregion

        #region Projektion
        /// <summary>
        /// Berechnet den abschnitt der Projektion von Vektor a auf Vektor b.
        /// </summary>
        /// <param name="a">Vektor, welcher auf b Porjeziert wird.</param>
        /// <param name="b">Vektor, auf welchen projeziert werden soll.</param>
        /// <returns>Projektion des Vektors a auf b</returns>
        public static double  ScalarProjection(Vector2 a, Vector2 b)    { return (a * b) / b.Length; }
        /// <summary>
        /// Berechnet dne projezierten Vektor, wenn man den Vektor a auf b projeziert
        /// </summary>
        /// <param name="a">Vektor, welcher auf b Porjeziert wird.</param>
        /// <param name="b">Vektor, auf welchen projeziert werden soll.</param>
        /// <returns>Projektion des Vektors a auf b</returns>
        public static Vector2 VectorProjection(Vector2 a, Vector2 b)    { return ((a * b) / (b.Length * b.Length)) * b; }
        #endregion

        #region Normale
        /// <summary>
        /// Normiert einen Vektor.
        /// </summary>
        /// <param name="a">Vektor, welcher normiert werden soll.</param>
        /// <returns>Ergebnis der normierung in einer neuen Vector2 Instanz.</returns>
        public static Vector2 Normalize(Vector2 a)                      { return new Vector2(a.X / a.Length, a.Y / a.Length); }
        /// <summary>
        /// Normiert den Vektor
        /// </summary>
        public void Normalize()                                         { X /= Length; Y /= Length; }
        /// <summary>
        /// Berechnet einen Vektor, welcher senkrecht zum übergebenen Vektor verläuft.
        /// </summary>
        /// <param name="a">Vektor, zu welchem der berechnete Vektor senkrecht verlaufen soll.</param>
        /// <returns>Ergebnis der Berechnung in einer neuen Vector2 Struktur</returns>
        public static Vector2 LeftPerpendicular (Vector2 a)             { return new Vector2(-a.Y, a.X); }
        /// <summary>
        /// Berechnet einen Vektor, welcher senkrecht zum übergebenen Vektor verläuft.
        /// </summary>
        /// <param name="a">Vektor, zu welchem der berechnete Vektor senkrecht verlaufen soll.</param>
        /// <returns>Ergebnis der Berechnung in einer neuen Vector2 Struktur</returns>
        public static Vector2 RightPerpendicular(Vector2 a)             { return new Vector2(a.Y, -a.X); }
        /// <summary>
        /// Berechnet einen Vektor, welcher senkrecht zum übergebenen Vektor verläuft und normiert diesen.
        /// </summary>
        /// <param name="a">Vektor, zu welchem der berechnete Vektor senkrecht verlaufen soll.</param>
        /// <returns>Ergebnis der Berechnung in einer neuen Vector2 Struktur</returns>
        public static Vector2 LeftNormal(Vector2 a)                     { return Normalize(LeftPerpendicular(a)); }
        /// <summary>
        /// Berechnet einen Vektor, welcher senkrecht zum übergebenen Vektor verläuft und normiert diesen.
        /// </summary>
        /// <param name="a">Vektor, zu welchem der berechnete Vektor senkrecht verlaufen soll.</param>
        /// <returns>Ergebnis der Berechnung in einer neuen Vector2 Struktur</returns>
        public static Vector2 RightNormal(Vector2 a)                    { return Normalize(RightPerpendicular(a)); }
        #endregion

        #region Vergleichsoperator
        /// <summary>
        /// Vergleicht die Komponenten zweier Vektoren auf gleichheit.
        /// </summary>
        /// <param name="a">Einer der Vektoren, die auf gleichheit geprüft werden sollen.</param>
        /// <param name="b">Einer der Vektoren, die auf gleichheit geprüft werden sollen.</param>
        /// <returns>True, wenn die Komponenten der Vektoren gleich sind</returns>
        public static bool operator ==(Vector2 a, Vector2 b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if ((object)a == null || ((object)b == null))
                return false;
            return a.Equals(b);

        }

        /// <summary>
        /// Vergleicht die Komponenten zweier Vektoren auf ungleichheit.
        /// </summary>
        /// <param name="a">Einer der Vektoren, die auf ungleichheit geprüft werden sollen.</param>
        /// <param name="b">Einer der Vektoren, die auf ungleichheit geprüft werden sollen.</param>
        /// <returns>True, wenn die Komponenten der Vektoren nicht gleich sind</returns>
        public static bool operator !=(Vector2 a, Vector2 b) { return !(a == b); }

        /// <summary>
        /// Prüft ob ein objekt, den selben Inhalt hat wie diese Vector2 Instanz.
        /// </summary>
        /// <param name="obj">Objekt, welches auf gleichheit geprüft werden soll.</param>
        /// <returns>True, wenn es sich um ein Vector2 mit den selben Komponenten handelt.</returns>
        public override bool Equals(object obj)
        {
            if (obj != null && obj is Vector2)
                return FloatingPointComparison.Equals(X, ((Vector2)obj).X) && FloatingPointComparison.Equals(Y, ((Vector2)obj).Y);
            return false;
        }

        /// <summary>
        /// Generiert einen für diese Instanz eindeutigen HashCode.
        /// </summary>
        /// <returns>Eindeutiger HashCode für diese Vector2 Instanz.</returns>
        public override int GetHashCode() { return X.GetHashCode() ^ Y.GetHashCode(); }
        #endregion

        #region Konvertierungsoperatoren
        /// <summary>
        /// Ermtöglicht die Konvertierung eines Vector2 in eine Matrix.
        /// </summary>
        /// <param name="a">Vektor, welcher in eine Matrix konvertiert werden soll.</param>
        public static explicit operator Matrix(Vector2 a) { return new Matrix(a); }

        /// <summary>
        /// Ermöglicht die impliziete Konvertierung eines Vector2 in einen String.
        /// </summary>
        /// <param name="a">Vektor, welcher in einen String umgewandelt werden soll.</param>
        public static implicit operator string(Vector2 a) { return a.ToString(); }
        #endregion

        #region Clone
        /// <summary>
        /// Erstellt eine exakte Kopie dieser Vector2 Instanz.
        /// </summary>
        /// <returns>Neue Vector2 Instanz mit den selben Komponenten wie diese Instanz.</returns>
        public Vector2 Clone() { return new Vector2(X, Y); }
        #endregion

        #region Konvertierung in String
        /// <summary>
        /// Wandelt den Vektor für die Ausgabe in einen String um.
        /// </summary>
        /// <returns>String, welcher den Vektor in Folgendem Format repräsentiert: {X:0.000}, {Y:0.000}</returns>
        public override string ToString() { return "{" + X.ToString("0.000") + "}, {" + Y.ToString("0.000") + "}"; }
        #endregion
    }
}