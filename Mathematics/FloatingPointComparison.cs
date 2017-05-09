namespace Mathematics
{
    public static class FloatingPointComparison
    {
        /// <summary>
        /// Prüft ob zwei doubles den selben Wert repräsentieren.
        /// </summary>
        /// <param name="d1">Erster Wert, welcher auf gleichheit geprüft werden soll.</param>
        /// <param name="d2">Zweiter Wert, welcher auf gleichheit geprüft werden soll.</param>
        /// <returns>True, wenn die beiden doubles den selben Wert repräsentieren</returns>
        public static bool Equals(double d1, double d2) { return Equals(d1, d2, 1E-10); }

        /// <summary>
        /// Prüft ob zwei doubles den selben Wert repräsentieren.
        /// </summary>
        /// <param name="d1">Erster Wert, welcher auf gleichheit geprüft werden soll.</param>
        /// <param name="d2">Zweiter Wert, welcher auf gleichheit geprüft werden soll.</param>
        /// <param name="precision">Genauigkeit, mit welcher geprüft werden soll</param>
        /// <returns>True, wenn die beiden doubles den selben Wert repräsentieren</returns>
        public static bool Equals(double d1, double d2, double precision)
        {
            return System.Math.Abs(d1 - d2) <= System.Math.Abs(d1 * precision);
        }
    }
}
