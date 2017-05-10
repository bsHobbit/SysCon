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
            : base(new WindowTheme(Color.Black, Color.Orange, Color.OrangeRed, Color.Blue, Color.Purple, Color.Transparent, Color.Wheat, Color.Orange, Color.LightGoldenrodYellow, Color.Gray, 200, 200, 1, 2),
                   new PanelTheme(Color.Black, Color.Gray))
        {

        }
    }
}
