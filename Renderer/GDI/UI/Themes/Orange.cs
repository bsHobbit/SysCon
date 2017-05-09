using System.Drawing;

namespace Renderer.GDI.UI.Themes
{
    /// <summary>
    /// Ein Orange angehauchtes Thema für das User-Interface.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    public class Orange: Theme
    {
        public Orange()
            : base(new WindowTheme(Color.Black, Color.Orange, Color.OrangeRed, Color.Blue, Color.Purple, Color.Orange, Color.Black, Color.Gray, 1, 2),
                   new PanelTheme(Color.Black, Color.Gray))
        {

        }
    }
}
