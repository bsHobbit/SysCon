using Mathematics.Vector;

namespace Renderer.GDI.UI
{
    /// <summary>
    /// Panel, welches mehrere Controls beinhalten kann.
    /// 
    /// Autor: Hendrik Rost.
    /// </summary>
    public class Panel : Container
    {
        #region Konstruktor
        /// <summary>
        /// Initialisiert eine neue Instanz der Panel Klasse.
        /// </summary>
        /// <param name="Size">Größe des Panels.</param>
        /// <param name="Location">Position des Panels.</param>
        public Panel(Vector2 Size, Vector2 Location)
            : base(Size, Location, System.Drawing.Color.Red)
        {

        }
        #endregion

        #region Theme
        /// <summary>
        /// Übernimmt ein Theme.
        /// </summary>
        /// <param name="Theme">Theme, welches das Panel übernehmen soll.</param>
        public override void ApplyTheme(Theme Theme)
        {
            base.ApplyTheme(Theme);

            if (Enabled)
                Color = Theme.Panel.BorderColor_Idle;
            else
                Color = Theme.Panel.BorderColor_Disabled;
        }
        #endregion
    }
}
