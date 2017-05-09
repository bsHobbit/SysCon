namespace Mathematics
{
    /// <summary>
    /// Repräsentation einer Matrix.
    /// 
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    public class Matrix
    {
        #region Member
        /// <summary>
        /// Daten der Matrix in einer Zeilen x Spalten Struktur,
        /// </summary>
        protected double[,] Data;
        /// <summary>
        /// Ruft die Anzahl der Zeilen der Matrix ab.
        /// </summary>
        public int Rows { get; private set; }
        /// <summary>
        /// Ruft die Anzahl der Spalten der Matrix ab.
        /// </summary>
        public int Columns { get; private set; }
        /// <summary>
        /// Ermöglicht den Zugriff auf einen Wert innerhalb der Matrix mit folgender Syntax:
        /// 
        /// 
        /// Matrix m = new Matrix(...);
        /// double v = m[row, column];
        /// m[row, column] = _some value_;
        /// </summary>
        /// <param name="row">Zeile, auf welche innerhalb der Matrix zugegriffen werden soll.</param>
        /// <param name="col">Spalte, auf welche innerhalb der Matrix zugegriffen werden soll.</param>
        /// <returns>Wert, welcher in der angegebenen Zeile und Spalte der Matrix gepeichert ist</returns>
        public double this[int row, int col] { get { return Data[row, col]; } set { Data[row, col] = value; } }
        #endregion

        #region Delegates
        /// <summary>
        /// Funktionszeiger, welcher für Events verwendet wird, die eine Matrix übergeben.
        /// </summary>
        /// <param name="m">Matrix, welche im Event übergeben wurde.</param>
        public delegate void MatrixEventHandler(Matrix m);
        #endregion

        #region Konstruktoren
        /// <summary>
        /// Initialisiert eine neue Instanz der Matrix Klasse.
        /// </summary>
        /// <param name="Rows">Anzahl der Zeilen der Matrix.</param>
        /// <param name="Columns">Anzahl der Spalten der Matrix.</param>
        public Matrix(int Rows, int Columns)
        {
            this.Rows = Rows;
            this.Columns = Columns;
            if (Rows > 0 && Columns > 0)
                Data = new double[Rows, Columns];
        }
        /// <summary>
        /// Initialisiert eine neue Instanz der Matrix Klasse.
        /// </summary>
        /// <param name="Rows">Anzahl der Zeilen der Matrix.</param>
        /// <param name="Columns">Anzahl der Spalten der Matrix.</param>
        /// <param name="Data">Daten, mit denen die Matrix gefüllt werden soll.</param>
        public Matrix(int Rows, int Columns, double[,] Data)
            : this(Rows, Columns)
        {
            WriteData(Data);
        }
        /// <summary>
        /// Initialisiert eine neue Instanz der Matrix Klasse.
        /// 
        /// Ein 2D Vektor wird wie folgt dargestellt:
        /// 3x1 Matrix 
        /// |X|
        /// |Y|
        /// |1|
        /// 
        /// Das ermöglicht die Transformation mit einer Transformationsmatrix
        /// </summary>
        /// <param name="v">Vector2, welcher als Matrix repräsentiert werden soll.</param>
        public Matrix(Vector.Vector2 v)
            : this(3, 1, new double[,] { { v.X }, { v.Y }, { 1 } })
        {

        }
        #endregion

        #region Typkonvertierung
        /// <summary>
        /// Ermöglicht die Umwandlung einer nx1 Matrix in einen 2D Vektor, die ersten beiden Zeilen werden dabei als X und Y Komponente verwendet.
        /// </summary>
        /// <param name="m">Matric, welche in einen Vektor umgewandelt werden soll.</param>
        public static implicit operator Vector.Vector2(Matrix m)
        {
            if (m != null && m.Rows >= 2 && m.Columns == 1)
                return new Vector.Vector2(m.Data[0, 0], m.Data[1, 0]);
            throw new System.Exception("Invalid column or row count");
        }

        /// <summary>
        /// Wandelt eine Matrix in einen String um.
        /// </summary>
        /// <param name="m">Matrix, welche in einen String umgewandelt werden soll.</param>
        public static implicit operator string (Matrix m)
        {
            return m.ToString();
        }
        #endregion

        #region Operatoren
        /// <summary>
        /// Addiert zwei Matrizen der selben Größe (m x n).
        /// </summary>
        /// <param name="a">Erste Matrix, die bei der Addition verwendet werden soll.</param>
        /// <param name="b">Zweite Matrix, die bei der Addition verwendet werden soll.</param>
        /// <returns>Summe der Matrizen a und b in einer neuen Matrix Klasse.</returns>
        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (SameSize(a, b))
            {
                double[,] res = new double[a.Rows, a.Columns];
                for (int i = 0; i < a.Rows; i++)
                    for (int j = 0; j < a.Columns; j++)
                        res[i, j] = a.Data[i, j] + b.Data[i, j];
                return new Matrix(a.Rows, a.Columns, res);
            }
            throw new System.Exception("Matrices a and b are different in size.");
        }
        /// <summary>
        /// Subtrahiert zwei Matrizen der selben Größe (m x n).
        /// </summary>
        /// <param name="a">Erste Matrix, die bei der Subtraktion verwendet werden soll.</param>
        /// <param name="b">Zweite Matrix, die bei der Subtraktion verwendet werden soll.</param>
        /// <returns>Summe der Matrizen a und b in einer neuen Matrix Klasse.</returns>
        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (SameSize(a, b))
            {
                double[,] res = new double[a.Rows, a.Columns];
                for (int i = 0; i < a.Rows; i++)
                    for (int j = 0; j < a.Columns; j++)
                        res[i, j] = a.Data[i, j] - b.Data[i, j];
                return new Matrix(a.Rows, a.Columns, res);
            }
            throw new System.Exception("Matrices a and b are different in size.");
        }
        /// <summary>
        /// Multipliziert eine Matrix mit einem Skalar.
        /// </summary>
        /// <param name="a">Matrix, welche mit einem Skalar multipliziert werden soll.</param>
        /// <param name="v">Skalar, mit welchem die Matrix multipliziert werden soll.</param>
        /// <returns>Ergebnis der Skalarmultiplikation.</returns>
        public static Matrix operator *(Matrix a, double v)
        {
            double[,] res = new double[a.Rows, a.Columns];
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Columns; j++)
                    res[i, j] = a.Data[i, j] * v;
            return new Matrix(a.Rows, a.Columns, res);
        }
        /// <summary>
        /// Multipliziert zwei Matrizen (l x m und m x n) miteinander.
        /// </summary>
        /// <param name="a">Erste Matrix für die Multiplikation.</param>
        /// <param name="b">Zweite Matrix für die Multiplikation.</param>
        /// <returns>Eine l x n Matrix mit dem Ergebnis der Matrizenmultiplikation</returns>
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.Columns == b.Rows)
            {
                double[,] res = new double[a.Rows, b.Columns];
                for (int i = 0; i < a.Rows; ++i) 
                    for (int j = 0; j < b.Columns; ++j) 
                        for (int k = 0; k < a.Columns; ++k)
                            res[i, j] += a[i, k] * b[k, j];

                return new Matrix(a.Rows, b.Columns, res);
            }
            else
                throw new System.Exception("Cannot multiply these matrices.");
        }
        /// <summary>
        /// Dividiert jeden Eintrag einer Matrix durch ein Skalar.
        /// </summary>
        /// <param name="a">Matrix, dessen Einträge dividiert werden sollen.</param>
        /// <param name="v">Skalar, durch welchen dividiert werden soll.</param>
        /// <returns>Matrix mit dem Ergebnis der division.</returns>
        public static Matrix operator /(Matrix a, double v)
        {
            double[,] res = new double[a.Rows, a.Columns];
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Columns; j++)
                    res[i, j] = a.Data[i, j] / v;
            return new Matrix(a.Rows, a.Columns, res);
        }
        #endregion

        #region Vergleichsoperationen
        /// <summary>
        /// Prüft ob zwei Matrizen den selben Inhalt haben.
        /// </summary>
        /// <param name="a">Matrix, welche auf gleichen Inhalt mit Matrix b geprüft werden soll.</param>
        /// <param name="b">Matrix, welche auf gleichen Inhalt mit Matrix a geprüft werden soll.</param>
        /// <returns>True, wenn der Inhalt der Matrizen a und b gleich sind.</returns>
        public static bool operator ==(Matrix a, Matrix b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if ((object)a == null || ((object)b == null))
                return false;
            return a.Equals(b);

        }
        /// <summary>
        /// Prüft ob zwei Matrizen unterschiedlichen Inhalt haben.
        /// </summary>
        /// <param name="a">Matrix, welche auf unterschiedlichen Inhalt mit Matrix b geprüft werden soll.</param>
        /// <param name="b">Matrix, welche auf unterschiedlichen Inhalt mit Matrix a geprüft werden soll.</param>
        /// <returns>True, wenn der Inhalt der Matrizen a und b unterschiedlich sind.</returns>
        public static bool operator !=(Matrix a, Matrix b)
        {
            return !(a == b);
        }
        /// <summary>
        /// Prüft ob ein Objekt dieser Matrix entspricht.
        /// </summary>
        /// <param name="obj">Objekt, welches auf gleichheit geprüft werden soll.</param>
        /// <returns>True, wenn das Objekt dieser Matrix gleicht.</returns>
        public override bool Equals(object obj)
        {
            Matrix m = obj as Matrix;
            if (m != null)
            {
                if (m.Rows == Rows && m.Columns == Columns)
                {
                    for (int i = 0; i < Rows; i++)
                        for (int j = 0; j < Columns; j++)
                            if (!FloatingPointComparison.Equals(this[i, j], m[i, j]))
                                return false;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Berechnet einen eindeutigen Hashcode für die Matrix.
        /// </summary>
        /// <returns>Eindeutiger Hashcode für diese Matrix.</returns>
        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();
            if (Data != null)
                for (int i = 0; i < Rows; i++)
                    for (int j = 0; j < Columns; j++)
                        hashCode ^= this[i, j].GetHashCode();
            return hashCode;
        }
        #endregion

        #region Clone
        /// <summary>
        /// Erstellt eine Kopie der Matrix.
        /// </summary>
        /// <returns>Einge exakte Kopie der Matrix mit allen Elementen</returns>
        public virtual Matrix Clone() { return new Matrix(Rows, Columns, Data); }
        #endregion

        #region Transponieren
        /// <summary>
        /// Transponiert die Matrix (Aus einer m x n Matrix wird eine n x m Matrix).
        /// </summary>
        /// <returns>n x m Matrix mit den Transponierten Daten.</returns>
        public Matrix Transpose()
        {
            double[,] res = new double[Columns, Rows];
            for (int i = 0; i < Columns; i++)
                for (int j = 0; j < Rows; j++)
                    res[i, j] = Data[j, i];
            return new Matrix(Columns, Rows, res);
        }
        /// <summary>
        /// Transponiert die Matrix (Aus einer m x n Matrix wird eine n x m Matrix).
        /// </summary>
        /// <param name="a">Matrix, welche Transponiert werden soll.</param>
        /// <returns>n x m Matrix mit den Transponierten Daten.</returns>
        public static Matrix Transpose(Matrix a) { return a.Transpose(); }
        #endregion

        #region Einheitsmatrix
        /// <summary>
        /// Erstellt eine Quadratische Matrix dessen Hauptdiagonale eins und alle Außendiagonalelemente null sind.
        /// </summary>
        /// <param name="Size">Größe der Einheitsmatrix.</param>
        /// <returns>Einheitsmatrix in angegebener Größe.</returns>
        public static Matrix IdentityMatrix(int Size)
        {
            
            double[,] res = new double[Size, Size];
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    res[i, j] = i == j ? 1 : 0;
            return new Matrix(Size, Size, res);
        }
        #endregion

        #region Matrix Invertieren
        /// <summary>
        /// Berechnet (wenn möglich) die Inverse zu einer Quadratischen Matrix.
        /// </summary>
        /// <param name="m">Quadratische Matrix, zu welcher die Inverse berechnet werden soll.</param>
        /// <returns>Inverse Matrix, wenn diese berechnet werden konnte, sonst null.</returns>
        public static Matrix Inverse(Matrix m)
        {
            int r = m.Rows;
            int c = m.Columns;
            if (r != c)
                throw new System.Exception("Attempt to MatrixInverse a non - square mattrix");

            int n = r;
            Matrix result = m.Clone();
            double[] col = new double[n];
            double[] x = new double[n];

            int[] indx = new int[n];
            double d;
            Matrix luMatrix = Decompose(m, indx, out d);

            if (luMatrix == null)
                return null;

            for (int j = 0; j < n; ++j)
            {
                for (int i = 0; i < n; ++i) { col[i] = 0.0; }
                col[j] = 1.0;
                x = BackSubstitution(luMatrix, indx, col);
                for (int i = 0; i < n; ++i) { result[i, j] = x[i]; }
            }
            return result;
        }
        /// <summary>
        ///  Berechnet (wenn möglich) die Inverse zu einer Quadratischen Matrix.
        /// </summary>
        /// <returns>Inverse Matrix, wenn diese berechnet werden konnte, sonst null.</returns>
        public Matrix Inverse() { return Inverse(this); }

        /// <summary>
        /// Berechnet für eine Matrix eine Inverse Spalte
        /// </summary>
        /// <param name="m">Matrix, welche berechnet wereden soll.</param>
        /// <param name="indx">Permuation der Decompensation der Matrix.</param>
        /// <param name="b">Spaltenindex für die Berechnung.</param>
        /// <returns>Inverse Spalte der Matrix.</returns>
        private static double[] BackSubstitution(Matrix m, int[] indx, double[] b)
        {
            int rows = m.Rows;
            int cols = m.Columns;
            if (rows != cols)
                throw new System.Exception("Non - square LU mattrix");

            int ii = 0; int ip = 0;
            int n = b.Length;
            double sum = 0.0;

            double[] x = new double[b.Length];
            b.CopyTo(x, 0);

            for (int i = 0; i < n; ++i)
            {
                ip = indx[i];
                sum = x[ip];
                x[ip] = x[i]; 
                if (ii == 0)
                {
                    for (int j = ii; j <= i - 1; j++)
                        sum -= m[i, j] * x[j];
                }
                else if (sum == 0.0)
                    ii = i;
                x[i] = sum;
            }

            for (int i = n - 1; i >= 0; --i)
            {
                sum = x[i];
                for (int j = i + 1; j < n; ++j)
                { sum -= m[i, j] * x[j]; }
                x[i] = sum / m[i, i];
            }
            return x;
        }
        #endregion

        #region Determinante einer Matrix bestimmten
        /// <summary>
        /// Berechnet die Determinante einer Matrix.
        /// </summary>
        /// <param name="m">Matrix, zu welcher die Determinante berechnet werden soll.</param>
        /// <returns>Determinante der Matrix m.</returns>
        public static double Determinant(Matrix m)
        {
            int[] perm = new int[m.Rows];
            double toggle;
            Matrix lum = Decompose(m, perm, out toggle);
            if (lum == null)
                throw new System.Exception("Unable to compute MatrixDeterminant");
            double result = toggle;
            for (int i = 0; i < lum.Rows; ++i)
                result *= lum[i, i];

            return result;
        }

        /// <summary>
        /// Berechnet die Determinante dieser Matrix.
        /// </summary>
        /// <returns>Determinante dieser Matrix.</returns>
        public double Determinant() { return Determinant(this); }
        #endregion

        #region LR/LU/Dreieckszerlegung Zerlegunge der Matrix
        /// <summary>
        /// Dreieckszerlegung einer Matrix.
        /// </summary>
        /// <param name="m">Matrix, welche berechnet werden soll.</param>
        /// <param name="permutation">Permutationsmatrix, welche den Zeilentausch beschreibt.</param>
        /// <param name="toggle">Speichert den Vorzeichenwechsel der Determinante.</param>
        /// <returns>Ergebnis der Dreieckszerlegung.</returns>
        private static Matrix Decompose(Matrix m, int[] permutation, out double toggle)
        {
            int rows = m.Rows;
            int cols = m.Columns;
            if (rows != cols)
                throw new System.Exception("Attempt to MatrixDecomposition a non - square mattrix");

            int n = rows; 
            int imax = 0; 
            double big = 0.0; double temp = 0.0; double sum = 0.0;
            double[] vv = new double[n];
            toggle = 1.0; 

            Matrix result = m.Clone();

            for (int i = 0; i < n; ++i)
            {
                big = 0.0;
                for (int j = 0; j < n; ++j)
                {
                    temp = System.Math.Abs(result[i, j]); 
                    if (temp > big)
                        big = temp;
                }
                if (big == 0.0)
                    return null; 
                vv[i] = 1.0 / big;
            }

            for (int j = 0; j < n; ++j) 
            {
                for (int i = 0; i < j; ++i)
                {
                    sum = result[i, j];
                    for (int k = 0; k < i; ++k) { sum -= result[i, k] * result[k, j]; }
                    result[i, j] = sum;
                } 

                big = 0.0;
                for (int i = j; i < n; ++i)
                {
                    sum = result[i, j];
                    for (int k = 0; k < j; ++k) { sum -= result[i, k] * result[k, j]; }
                    result[i, j] = sum;
                    temp = vv[i] * System.Math.Abs(sum);
                    if (temp >= big)
                    {
                        big = temp;
                        imax = i;
                    }
                } 

                if (j != imax)
                {
                    for (int k = 0; k < n; ++k)
                    {
                        temp = result[imax, k];
                        result[imax, k] = result[j, k];
                        result[j, k] = temp;
                    }
                    toggle = -toggle; 
                    vv[imax] = vv[j];
                }
                permutation[j] = imax;

                if (j != n - 1)
                {
                    temp = 1.0 / result[j, j];
                    for (int i = j + 1; i < n; ++i) { result[i, j] *= temp; }
                }

            }

            return result;
        }
        #endregion

        #region Beschreiben der Matrix

        /// <summary>
        /// Schreibt Daten in die Matrix.
        /// </summary>
        /// <param name="Data">Daten, welche in die Matrix-Struktur geschrieben werden sollen.</param>
        public void WriteData(double[,] Data)
        {
            if (Data != null && Data.Rank == 2 && Data.GetLength(0) == Rows && Data.GetLength(1) == Columns)
            {
                for (int i = 0; i < Rows; i++)
                    for (int j = 0; j < Columns; j++)
                    {
                        this.Data[i, j] = Data[i, j];
                        if (FloatingPointComparison.Equals(System.Math.Abs(this.Data[i, j]), 0))
                            this.Data[i, j] = 0;
                    }
            }
            else
                throw new System.Exception("Data Array does not contain valid information.");
        }
        #endregion

        #region Hilfsfunktionen

        /// <summary>
        /// Prüft ob zwei Matrizen die selbe Größe (m x n) besitzten.
        /// </summary>
        /// <param name="a">Matrix, welche mit der Matrix b verglichen wird.</param>
        /// <param name="b">Matrix, welche mit der Matrix a verglichen wird.</param>
        /// <returns>True, wenn die Anzahl der Zeilen und Spalten der beiden Matrizen a und b gleich sind, sonst false.</returns>
        public static bool SameSize(Matrix a, Matrix b)
        {
            if (a != null && b != null && a.Rows == b.Rows && a.Columns == b.Columns)
                return true;
            return false;
        }
        #endregion

        #region 2D Transformation

        /// <summary>
        /// Transformiert ein Array von 2D Vektoren.
        /// </summary>
        /// <param name="v">Vektore, welche Transformiert werden sollen.</param>
        /// <param name="m">Anzuwendende Transformationsmatrix.</param>
        /// <returns>Transformierte Vectoren.</returns>
        public static Vector.Vector2[] Transform(Vector.Vector2[] v, Matrix m)
        {
            if (v == null || m == null || v.Length == 0)
                return null;

            Vector.Vector2[] result = new Vector.Vector2[v.Length];
            for (int i = 0; i < v.Length; i++)
                result[i] = v[i] * m;

            return result;
        }

        /// <summary>
        /// Erstellt eine Transformationsmatrix für eine 2D Skalierung.
        /// </summary>
        /// <param name="ScaleX">Skalierungsfaktor dex X-Achse.</param>
        /// <param name="ScaleY">Skalierungsfaktor dex Y-Achse.</param>
        /// <returns>Transformationsmatrix, die z.B. zum skalieren eines Vector2 verwendet werden kann.</returns>
        public static Matrix Transform2DScale(double ScaleX, double ScaleY)
        {
            return new Matrix(3, 3, new double[,] { { ScaleX, 0, 0 },
                                                    { 0, ScaleY, 0 },
                                                    { 0, 0, 1 } });

        }
        /// <summary>
        /// Erstellt eine Transformationsmatrix für eine 2D Rotation.
        /// </summary>
        /// <param name="Angle">Winkel im Bogenmaß, um welchen ein 2D Punkt rotiert werden soll.</param>
        /// <returns>Transformationsmatrix, die z.B. zum rotieren eines Vector2 verwendet werden kann.</returns>
        public static Matrix Transform2DRotation(double Angle)
        {
            return new Matrix(3, 3, new double[,] { { System.Math.Cos(Angle), -System.Math.Sin(Angle), 0 },
                                                    { System.Math.Sin(Angle), System.Math.Cos(Angle), 0 },
                                                    { 0, 0, 1 } });
        }
        /// <summary>
        ///  Erstellt eine Transformationsmatrix für eine 2D Verschiebung.
        /// </summary>
        /// <param name="TranslateX">Offset für die Verschiebung auf der X-Achse.</param>
        /// <param name="TranslateY">Offset für die Verschiebung auf der Y-Achse.</param>
        /// <returns>Transformationsmatrix, die z.B. zum veschieben eines Vector2 verwendet werden kann.</returns>
        public static Matrix Transform2DTranslate(double TranslateX, double TranslateY)
        {
            return new Matrix(3, 3, new double[,] { { 1, 0, TranslateX },
                                                    { 0, 1, TranslateY },
                                                    { 0, 0, 1 } });

        }
        #endregion

        #region ToString
        /// <summary>
        /// Wandelt eine Matrix in einen String um.
        /// </summary>
        /// <returns>String, welcher die Matrix repräsentieren soll.</returns>
        public override string ToString()
        {
            string res = "";
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                    res += this[r, c].ToString("0.00") + "  ";
                res += System.Environment.NewLine;
            }
            return res;
        }
        #endregion
    }
}