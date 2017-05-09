using System.Drawing;

namespace Renderer.GDI.UI
{
    /// <summary>
    /// Speichert die Daten für ein Thema eines User-Interface Stil.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    public class Theme
    {
        public struct WindowTheme
        {
            public Color BorderColor_Idle { get; private set; }
            public Color BorderColor_MouseOver { get; private set; }
            public Color BorderColor_Active { get; private set; }
            public Color BorderColor_MouseOver_Active { get; private set; }
            public Color BorderColor_Disabled { get; private set; }

            public double RenderWidth_Idle { get; private set; }
            public double RenderWidth_MouseOver { get; private set; }

            public WindowTheme(Color BorderIdle, 
                               Color BorderMouseOver, 
                               Color BorderActive, 
                               Color BorderMouseOverActive, 
                               Color BorderDisabled,
                               double RenderWidthIdle,
                               double RenderWidthMouseOver)
                : this()
            {
                BorderColor_Active = BorderActive;
                BorderColor_Disabled = BorderDisabled;
                BorderColor_Idle = BorderIdle;
                BorderColor_MouseOver = BorderMouseOver;
                BorderColor_MouseOver_Active = BorderMouseOverActive;
                RenderWidth_Idle = RenderWidthIdle;
                RenderWidth_MouseOver = RenderWidthMouseOver;
            }
        }

        public struct PanelTheme
        {
            public Color BorderColor_Idle { get; private set; }
            public Color BorderColor_Disabled { get; private set; }

            public PanelTheme(Color BorderIdle, Color BorderDisabled)
                :this()
            {
                BorderColor_Idle = BorderIdle;
                BorderColor_Disabled = BorderDisabled;
            }
        }

        public WindowTheme Window { get; private set; }
        public PanelTheme Panel { get; private set; }
        public static Theme Orange = new Themes.Orange();

        internal Theme(WindowTheme WindowTheme,
                       PanelTheme PanelTheme)
        {
            Window = WindowTheme;
            Panel = PanelTheme;
        }
    }

}
