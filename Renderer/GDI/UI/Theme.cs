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

            public Color FillColor_Idle { get; set; }
            public Color FillColor_MouseOver { get; set; }

            public Color TextColor_Idle { get; private set; }
            public Color TextColor_MouseOver { get; private set; }
            public Color TextColor_Disabled { get; private set; }

            public double RenderWidth_Idle { get; private set; }
            public double RenderWidth_MouseOver { get; private set; }

            public double BorderColor_ShiftTime { get; private set; }
            public double FillColor_ShiftTime { get; private set; }
            public double TextColor_ShiftTime { get; private set; }

            public WindowTheme(Color BorderIdle, 
                               Color BorderMouseOver, 
                               Color BorderActive, 
                               Color BorderMouseOverActive, 
                               Color BorderDisabled,
                               Color FillIdle,
                               Color FillOver,
                               Color TextIdle,
                               Color TextMouseOver,
                               Color TextDisabled,
                               double BorderColorShiftTime,
                               double FillColorShiftTime,
                               double TextColorShiftTime,
                               double RenderWidthIdle,
                               double RenderWidthMouseOver)
                : this()
            {
                BorderColor_Active = BorderActive;
                BorderColor_Disabled = BorderDisabled;
                BorderColor_Idle = BorderIdle;
                BorderColor_MouseOver = BorderMouseOver;
                BorderColor_MouseOver_Active = BorderMouseOverActive;
                FillColor_Idle = FillIdle;
                FillColor_MouseOver = FillOver;
                TextColor_Idle = TextIdle;
                TextColor_MouseOver = TextMouseOver;
                TextColor_Disabled = TextDisabled;
                BorderColor_ShiftTime = BorderColorShiftTime;
                FillColor_ShiftTime = FillColorShiftTime;
                TextColor_ShiftTime = TextColorShiftTime;
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
        public struct ButtonTheme
        {
            public Color BorderColor_Idle { get; private set; }
            public Color BorderColor_Over { get; private set; }
            public Color BorderColor_Active { get; private set; }
            public Color BorderColor_Disabled { get; private set; }

            public double BorderColor_ShiftTime { get; private set; }
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
