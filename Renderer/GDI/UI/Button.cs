using System.Drawing;
using Mathematics.Vector;

namespace Renderer.GDI.UI
{

    /// <summary>
    /// Repräsentation eines Button innerhalb eines GDI User-Interfaces.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    public class Button : Control
    {
        public string Text
        {
            get
            {
                if (RenderText != null)
                    return RenderText.Text;
                else
                    return "";
            }
            set
            {
                if (RenderText != null)
                    RenderText.Text = "";
            }
        }
        RenderText RenderText;


        public Button(Vector2 Size, Vector2 Location)
            : base(Size, Location, Color.Black)
        {
            RenderText = new RenderText("");
        }

        void ApplyTheme()
        {

        }

        public override void ApplyTheme(Theme Theme)
        {
            base.ApplyTheme(Theme);
            ApplyTheme();
        }
    }
}
