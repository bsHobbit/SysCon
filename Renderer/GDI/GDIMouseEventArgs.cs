namespace Renderer.GDI
{
    /// <summary>
    /// Parameter, die bei einem Maus-Event nicht fehlen dürfen.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    public struct GDIMouseEventArgs
    {

        #region Member
        /// <summary>
        /// Ruft die Position der Maus in Pixel ab.
        /// </summary>
        public Mathematics.Vector.Vector2 LocalLocation { get; private set; }
        /// <summary>
        /// Ruft die globale Position der Maus in Pixel ab.
        /// </summary>
        public Mathematics.Vector.Vector2 GlobalLocation { get; private set; }
        /// <summary>
        /// Ruft den Zustande der Mausknöpfe ab.
        /// </summary>
        public bool[] ButtonStates { get; private set; }
        /// <summary>
        /// Ruft den Zustand des Mausrades ab.
        /// </summary>
        public int Delta { get; private set; }
        #endregion

        #region Konstruktur
        /// <summary>
        /// Initialisiert die GDIMouseEventArgs Struktur.
        /// </summary>
        /// <param name="LocalLocation">Position der Maus in Pixel.</param>
        /// <param name="GlobalLocation">Globale Position der Maus</param>
        /// <param name="ButtonStates">Status der Maus-Knöpfe.</param>
        /// <param name="Delta">Status des Mausrades.</param>
        public GDIMouseEventArgs(Mathematics.Vector.Vector2 LocalLocation, Mathematics.Vector.Vector2 GlobalLocation, bool[] ButtonStates, int Delta)
            : this()
        {
            this.GlobalLocation = GlobalLocation.Clone();
            this.LocalLocation = LocalLocation.Clone();
            this.ButtonStates = ButtonStates;
            this.Delta = Delta;
        }
        #endregion
    }
}
