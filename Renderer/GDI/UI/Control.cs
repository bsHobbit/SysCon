using Mathematics.Vector;
using System.Drawing;

namespace Renderer.GDI.UI
{
    /// <summary>
    /// Basisklasse für jedes Control, welches innerhalb des User-Interfaces gerendert werden kann.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    public abstract class Control: RenderObject
    {
        #region Geschachtelte Typen
        /// <summary>
        /// Mögliche Textpositionen innerhalb eines Controls.
        /// </summary>
        public enum eTextLocation
        {
            /// <summary>
            /// Text wird in der oberen linken Ecke des Controls positioniert.
            /// </summary>
            TopLeft,
            /// <summary>
            /// Text wird mittig an der oberen Kante des Controls positioniert.
            /// </summary>
            TopCenter,
            /// <summary>
            /// Text wird in der oberen rechten Ecke des Controls positioniert.
            /// </summary>
            TopRight,
            /// <summary>
            /// Text wird zentiert an der linken Kante des Controls positioniert.
            /// </summary>
            CenterLeft,
            /// <summary>
            /// Text wird zentriert im Control positioniert.
            /// </summary>
            Center,
            /// <summary>
            /// Text wird zentriert an der rechten Kante des Controls positioniert.
            /// </summary>
            CenterRight,
            /// <summary>
            /// Text wird in der linken unteren Ecke des Controls positioniert.
            /// </summary>
            BottomLeft,
            /// <summary>
            /// Text wird an der unteren Kante zentriert.
            /// </summary>
            BottomCenter,
            /// <summary>
            /// Text wird in der rechten unteren Ecke des Controls positioniert.
            /// </summary>
            BottomRight,
        }
        #endregion

        #region Events
        /// <summary>
        /// Funktionszeiger für Events die ein Control und dessen Thema betreffen.
        /// </summary>
        /// <param name="Control">Control, welches von der Thema-Änderung betroffen ist.</param>
        /// <param name="Theme">Neues Thema des Controls.</param>
        public delegate void ControlThemeChangedEventHandler(Control Control, Theme Theme);

        /// <summary>
        /// Funktionszeiger für Events, die ein Control betreffen.
        /// </summary>
        /// <param name="Control">Control, welches das Event gefeurt hat.</param>
        public delegate void ControlEventHandler(Control Control);

        /// <summary>
        /// Wird immer dann aufgerufen, wenn sich das Thema des Controls ändert.
        /// </summary>
        public event ControlThemeChangedEventHandler OnThemeChanged;

        /// <summary>
        /// Wird immer dann aufgerufen, wenn sich der Focus des Controls ändert.
        /// </summary>
        public event ControlEventHandler OnFocusChanged;

        /// <summary>
        /// Wir immer dann aufgerufen, wenn sich die Größe des Controls ändert.
        /// </summary>
        public event ControlEventHandler OnSizeChanged;
        #endregion

        #region Member
        /// <summary>
        /// Ruft die Größe des Controls in Pixel ab.
        /// </summary>
        public Vector2 Size { get; private set; }

        /// <summary>
        /// Speichert das Thema des Controls.
        /// </summary>
        protected Theme Theme;

        /// <summary>
        /// Speichert einen Wert, der angibt, ob das Control im Fokus ist oder nicht.
        /// </summary>
        bool focused;

        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob das Control im Focus ist oder nicht.
        /// </summary>
        public bool Focused
        {
            get { return focused; }
            set
            {
                bool old = focused;
                focused = value;
                if (old != focused && OnFocusChanged != null)
                    OnFocusChanged(this);
            }
        }       

        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob das Eltern-Control enabled ist.
        /// </summary>
        protected bool ParentEnabled
        {
            get
            {
                if (Container != null)
                {
                    if (!Container.Enabled)
                        return false;
                    return Container.Enabled;
                }
                else
                    return Enabled;
            }
        }
        
        /// <summary>
        ///  Inidikator, ob sich die Maus über dem Control befindet.
        /// </summary>
        protected bool IsMouseOver;
        #endregion

        #region Konstruktor
        /// <summary>
        /// Initialisiert eine neue Instanz der Control Klasse.
        /// </summary>
        /// <param name="Size">Größe des Controls in Pixel.</param>
        /// <param name="Location">Position des Controls.</param>
        /// <param name="Color">Farbe des Controls.</param>
        public Control(Vector2 Size, Vector2 Location, Color Color)
            : base(CreateRectangle(Size.X, Size.Y), Location, new Vector2(1, 1), 0, Color, false)
        {
            this.Size = Size;
            ApplyTheme(Theme.Orange); //Default-Theme für alle Controls
            OnMouseEnter += Control_OnMouseEnter;
            OnMouseLeave += Control_OnMouseLeave;

            OnEnabledChanged += Control_OnEnabledChanged;
        }




        #endregion

        #region Container / Parent-Control

        /// <summary>
        /// Enabled Eigenschaft hat sich verändert ggf. müssen Controls das Theme aktualisieren.
        /// </summary>
        /// <param name="Object">Objekt, bei welchem sich die Enabled Eigenschaft verändert hat.</param>
        private void Control_OnEnabledChanged(RenderObject Object)
        {
            ParentEnabgledChanged();
        }

        /// <summary>
        /// Kann verwendet werden um das Them
        /// </summary>
        protected virtual void ParentEnabgledChanged()
        {
            ApplyTheme(Theme);
            if (SubObjects != null)
            {
                for (int i = 0; i < SubObjects.Count; i++)
                {
                    Control tmpControl = SubObjects[i] as Control;
                    if (tmpControl != null)
                        tmpControl.ParentEnabgledChanged();
                }
            }
        }

        /// <summary>
        /// Ruft den Container für dieses Objekt ab, falls es einen gibt.
        /// </summary>
        public Container Container { get { return Owner as Container; } }

        /// <summary>
        /// Ruft den Ursprungscontainer für dieses Objekt ab, falls es einen gibt.
        /// </summary>
        public Container RootContainer
        {
            get
            {
                RenderObject ro = this;
                while (ro.Owner != null)
                    ro = ro.Owner;

                if (ro is Container)
                    return ro as Container;
                else
                    return null;
            }
        }
        #endregion

        #region Theme
        /// <summary>
        /// Stellt das Thema für dieses Control ein.
        /// </summary>
        /// <param name="Theme">Thema, welches für dieses Control eingestellt werden soll.</param>
        public virtual void ApplyTheme(Theme Theme)
        {
            this.Theme = Theme;
            OnThemeChanged?.Invoke(this, Theme);
            if (SubObjects != null)
            {
                for (int i = 0; i < SubObjects.Count; i++)
                {
                    if (SubObjects[i] is Control)
                        ((Control)SubObjects[i]).ApplyTheme(Theme);
                }
            }
        }
        #endregion

        #region Fokus
        /// <summary>
        /// Fokusiert das Control.
        /// </summary>
        public void Focus()
        {
            Container c = Container;
            if (c!= null && c.SubObjects != null)
            {
                for (int i = 0; i < c.SubObjects.Count; i++)
                {
                    Control tmpControl = c.SubObjects[i] as Control;
                    if (tmpControl != null)
                        tmpControl.Focused = false;
                }
            }

            Focused = true;
        }
        #endregion

        #region Benutzereingaben

        /// <summary>
        /// Maus verlässt das Control.
        /// </summary>
        /// <param name="Object">This.</param>
        /// <param name="e">Mausdaten.</param>
        private void Control_OnMouseLeave(RenderObject Object, GDIMouseEventArgs e)
        {
            IsMouseOver = false;
        }

        /// <summary>
        /// Maus bewegt sich in das Control.
        /// </summary>
        /// <param name="Object">This.</param>
        /// <param name="e">Mausdaten.</param>
        private void Control_OnMouseEnter(RenderObject Object, GDIMouseEventArgs e)
        {
            IsMouseOver = true;
        }

        #endregion

        #region Größenänderung
        /// <summary>
        /// Aktualisiert die Größe des Controls
        /// </summary>
        /// <param name="Size"></param>
        public void UpdateSize(Vector2 Size)
        {
            this.Size = Size;
            UpdateVertices(CreateRectangle(Size.X, Size.Y));
            OnSizeChanged?.Invoke(this);
        }

        #endregion

        #region Berechnung Text
        /// <summary>
        /// Berechnet die position eines Textes anhand der gewünschten Zielkoordinaten.
        /// </summary>
        /// <param name="Location">Gewünschtes Ziel des Textes.</param>
        /// <param name="Text">Text, welcher an die Position geschoben werden soll.</param>
        protected void SetTextLocation(eTextLocation Location, ref RenderText Text)
        {
            try
            {
                Bitmap tmpBitmap = new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(tmpBitmap);

                SizeF tmpstringSize = g.MeasureString(Text.Text, Text.Font);
                Vector2 stringSize = new Vector2(tmpstringSize.Width, tmpstringSize.Height);

                switch (Location)
                {
                    case eTextLocation.TopLeft:
                        Text.Location = new Vector2(-Size.X / 2, -Size.Y / 2);
                        break;
                    case eTextLocation.TopCenter:
                        Text.Location = new Vector2(-stringSize.X / 2, -Size.Y / 2);
                        break;
                    case eTextLocation.TopRight:
                        Text.Location = new Vector2((Size.X / 2) - (stringSize.X), -Size.Y / 2);
                        break;
                    case eTextLocation.CenterLeft:
                        Text.Location = new Vector2(-Size.X / 2, -stringSize.Y / 2);
                        break;
                    case eTextLocation.Center:
                        Text.Location = new Vector2(-stringSize.X / 2, -stringSize.Y / 2);
                        break;
                    case eTextLocation.CenterRight:
                        Text.Location = new Vector2((Size.X / 2) - stringSize.X, -stringSize.Y / 2);
                        break;
                    case eTextLocation.BottomLeft:
                        Text.Location = new Vector2(-Size.X / 2, Size.Y / 2 - stringSize.Y);
                        break;
                    case eTextLocation.BottomCenter:
                        Text.Location = new Vector2(-stringSize.X / 2, Size.Y / 2 - stringSize.Y);
                        break;
                    case eTextLocation.BottomRight:
                        Text.Location = new Vector2(Size.X / 2 - stringSize.X, Size.Y / 2 - stringSize.Y);
                        break;
                    default:
                        Text.Location = new Vector2();
                        break;
                }

                tmpBitmap.Dispose();
                g.Dispose();
            }
            catch { }
            
        }

        /// <summary>
        /// Berechnet die Rendergröße eines Textes.
        /// </summary>
        /// <param name="Text">Text, von welchem die Größe berechnet werden soll.</param>
        /// <returns>Größe des Textes in Pixel im Vector2 Format.</returns>
        protected Vector2 TextSize(RenderText Text)
        {
            if (Text!=null)
            {

                try
                {
                    Bitmap tmpBitmap = new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    Graphics g = Graphics.FromImage(tmpBitmap);
                    SizeF tmpstringSize = g.MeasureString(Text.Text, Text.Font);
                    g.Dispose();
                    tmpBitmap.Dispose();
                    return new Vector2(tmpstringSize.Width, tmpstringSize.Height);
                }
                catch (System.Exception)
                {
                    
                }
            }
            return new Vector2();
        }

        #endregion
    }
}
