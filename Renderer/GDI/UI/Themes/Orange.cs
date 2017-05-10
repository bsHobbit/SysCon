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
            : base(new WindowTheme(Color.OrangeRed, //Border Idle
                                   Color.OrangeRed, //Border Over
                                   Color.Blue, //Border Active 
                                   Color.Pink, //Border active over
                                   Color.Purple, //Border Disabled
                                   Color.Transparent, //Fill Idle
                                   Color.Wheat, //Fill Over
                                   Color.Black, //Text Idle
                                   Color.DarkSlateGray, //Text Over
                                   Color.Gray, //Text Disabled
                                   200, //Border Time
                                   200, //Fill Time
                                   200, //Text Time
                                   1, //Renderwidth Idle
                                   2), //Renderwidth Over
                   new PanelTheme(Color.Black, Color.Gray))
        {

        }
    }
}
