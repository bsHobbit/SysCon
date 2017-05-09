using Mathematics.Vector;
using System.Drawing;

namespace Renderer.GDI
{
    /// <summary>
    /// Speichert alle wichtigen Informationen, die zum Rendern eines Textes innerhalb von GDI benötigt werden.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    public class RenderText
    {
        #region Member
        /// <summary>
        /// Standardfarbe, die verwendet wird, wenn keine angegeben ist.
        /// </summary>
        public static Color DefaultColor = Color.Black;

        /// <summary>
        /// Standardschriftart, die verwendet wird, wenn keine angegeben ist.
        /// </summary>
        public static Font DefaultFont = new Font("Arial", 12);

        /// <summary>
        /// Text, welcher gerendert werden soll.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Schriftart, mit welcher gerendert werden soll.
        /// </summary>
        public Font Font { get;  set; }

        /// <summary>
        /// Position, an welcher der Text (relativ zum RenderObject) gerendert werden soll.
        /// </summary>
        public Vector2 Location { get;  set; }

        /// <summary>
        /// Farbe, mit welcher gerendert werden soll.
        /// </summary>
        public Color Color { get;  set; }
        #endregion

        #region Konstruktor

        /// <summary>
        /// Initialisiert eine neue Instanz der RenderText-Klasse
        /// </summary>
        /// <param name="Text">Text, welcher gerender werden soll.</param>
        public RenderText(string Text) : this(Text, new Vector2(), DefaultFont, DefaultColor) {  }

        /// <summary>
        /// Initialisiert eine neue Instanz der RenderText-Klasse
        /// </summary>
        /// <param name="Text">Text, welcher gerender werden soll.</param>
        /// <param name="Location">Position, an welcher relativ zum RenderObjekt gerendert werden soll.</param>
        public RenderText(string Text, Vector2 Location) : this(Text, Location, DefaultFont, DefaultColor) {  }

        /// <summary>
        /// Initialisiert eine neue Instanz der RenderText-Klasse
        /// </summary>
        /// <param name="Text">Text, welcher gerender werden soll.</param>
        /// <param name="Location">Position, an welcher relativ zum RenderObjekt gerendert werden soll.</param>
        /// <param name="Font">Schriftart, mit welcher gerendert werden soll.</param>
        /// <param name="Color">Farbe, mit welcher gerendert werden soll.</param>
        public RenderText(string Text, Vector2 Location, Font Font, Color Color)
        {
            this.Text = Text;
            this.Location = Location;
            this.Font = Font;
            this.Color = Color;
        }
        #endregion
    }
}
