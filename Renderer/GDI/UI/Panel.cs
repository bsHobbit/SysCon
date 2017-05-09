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
        public Panel(Vector2 Size, Vector2 Location)
            : base(Size, Location, System.Drawing.Color.Red)
        {

        }


        public override void ApplyTheme(Theme Theme)
        {
            base.ApplyTheme(Theme);

            if (Enabled)
                Color = Theme.Panel.BorderColor_Idle;
            else
                Color = Theme.Panel.BorderColor_Disabled;
        }
    }
}
