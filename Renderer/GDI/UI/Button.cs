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
        public Button(Vector2 Size, Vector2 Location)
            : base(Size, Location, Color.Black)
        {


        }
    }
}
