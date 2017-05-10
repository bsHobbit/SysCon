using System.Drawing;

namespace Renderer.GDI
{
    /// <summary>
    /// Klasse zum verwalten eines Farbverlaufes.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    internal class ColorShift
    {
        #region Events
        /// <summary>
        /// Funktionszeiger für Events, die eine Farbe übergeben.
        /// </summary>
        /// <param name="Color">Farbe des Events.</param>
        /// <param name="Finished">True, wenn der ColorShift abgeschlossen ist.</param>
        public delegate void UpdateColorEventHandler(Color Color, bool Finished);
        /// <summary>
        /// Tritt immer dann auf, wenn eine neue Farbe berechnet wurde.
        /// </summary>
        public event UpdateColorEventHandler UpdateColor;
        #endregion

        #region Member
        /// <summary>
        /// Farbe, bei welcher der Shift Startet.
        /// </summary>
        public Color StartColor { get; set; }
        /// <summary>
        /// Farbe, zu welcher der Shift werden soll.
        /// </summary>
        public Color TargetColor { get; set; }
        /// <summary>
        /// Verstrichene Zeit innerhalb des Shifts
        /// </summary>
        double Elapsed;
        /// <summary>
        /// Zeit, in welcher der Shift vollendet werden soll.
        /// </summary>
        public double TotalTime { get; set; }
        /// <summary>
        /// Zeitgeber für den ColorShift.
        /// </summary>
        System.Windows.Forms.Timer Timer;
        #endregion

        #region Konstruktor

        /// <summary>
        /// Initilisiert eine neue Instanz der ColorShift-Klasse mit Standardwerten.
        /// </summary>
        public ColorShift()
            : this(Color.Black, Color.Black, 0)
        {

        }
        /// <summary>
        /// Initialisiert eine neue Instanz der ColorShift-Klasse.
        /// </summary>
        /// <param name="Start">Farbe, bei welcher der Shift starten soll.</param>
        /// <param name="Target">Farbe, bei welcher der Shift enden soll.</param>
        /// <param name="Time">Zeit, in welcher der Shift beendet werden soll.</param>
        public ColorShift(Color Start, Color Target, double Time)
        {
            StartColor = Start;
            TargetColor = Target;
            TotalTime = Time;
        }
        #endregion

        #region Timer
        /// <summary>
        /// Neuer Tick des Zeitgebers.
        /// </summary>
        /// <param name="sender">Windows Timer.</param>
        /// <param name="e">Nichts von Belangen.</param>
        private void Timer_Tick(object sender, System.EventArgs e)
        {
            Elapsed += Timer.Interval;
            if (Elapsed > TotalTime)
            {
                Elapsed = TotalTime;
                Timer.Enabled = false;
            }
            int newA = (int)(StartColor.A + (((TargetColor.A - StartColor.A) * Elapsed) / TotalTime));
            int newR = (int)(StartColor.R + (((TargetColor.R - StartColor.R) * Elapsed) / TotalTime));
            int newG = (int)(StartColor.G + (((TargetColor.G - StartColor.G) * Elapsed) / TotalTime));
            int newB = (int)(StartColor.B + (((TargetColor.B - StartColor.B) * Elapsed) / TotalTime));

            Color currentColor = Color.FromArgb(newA, newR, newG, newB); ;
            UpdateColor?.Invoke(currentColor, !Timer.Enabled);
        }

        /// <summary>
        /// Startet den ColorShift Timer.
        /// </summary>
        public void Start()
        {
            Elapsed = 0;
            if (Timer == null)
            {
                Timer = new System.Windows.Forms.Timer();
                Timer.Enabled = true;
                Timer.Interval = 33;
                Timer.Tick += Timer_Tick;
            }
            else
                Timer.Enabled = true;
        }
        #endregion

        #region Einstellungen
        /// <summary>
        /// Stellt die Daten für den ColorShift ein.
        /// </summary>
        /// <param name="Start">Farbe, bei welcher der Shift starten soll.</param>
        /// <param name="Target">Farbe, bei welcher der Shift enden soll.</param>
        /// <param name="Time">Zeit, in welcher der Shift stattfinden soll.</param>
        public void Set(Color Start, Color Target, double Time)
        {
            StartColor = Start;
            TargetColor = Target;
            TotalTime = Time;
        }
        #endregion
    }
}
