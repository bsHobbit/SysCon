using Mathematics.Vector;

namespace Renderer.GDI.UI
{
    /// <summary>
    /// Für alle Controls, die als Container fungieren können.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    public class Container : Control
    {
        #region Member
        /// <summary>
        /// Canvas, welchem der Container zugeordnet ist.
        /// </summary>
        Canvas Canvas;
        /// <summary>
        /// Aktives Control innerhalb des Containers
        /// </summary>
        internal Control ActiveControl { get; private set; }
        #endregion

        #region Konstruktor
        /// <summary>
        /// Initialisiert eine neue Container Klasse.
        /// </summary>
        /// <param name="Location">Position des Containers innerhalb des UI.</param>
        /// <param name="Size">Größe des Containers innerhalb des UI.</param>
        /// <param name="Color">Farbe des Containers</param>
        internal Container(Vector2 Size, Vector2 Location, System.Drawing.Color Color)
            : base(Size, Location, Color)
        {
            OnMouseLeave += Container_OnMouseLeave;
            OnMouseDown += Container_OnMouseDown;
        }


        #endregion

        #region Add / Remove Controls
        /// <summary>
        /// Fügt dem Container ein Control hinzu.
        /// </summary>
        /// <param name="Control">Control, welches dem Container hinzugefügt werden soll.</param>
        public virtual void Add(Control Control)
        {
            base.Add(Control);
        }
        /// <summary>
        /// Entfernt ein Control vom Container.
        /// </summary>
        /// <param name="Control">Control, welches vom Container entfernt werden soll.</param>
        public virtual void Remove(Control Control)
        {
            base.Remove(Control);
        }
        /// <summary>
        /// Entfernt alle Controls vom Container.
        /// </summary>
        public virtual void Clear()
        {
            ClearSubObjects();
        }
        #endregion

        #region Canvas einstellen
        /// <summary>
        /// Stellt den Canvas des Containers ein.
        /// </summary>
        /// <param name="Canvas">Canvas, welcher eingestellt werden soll.</param>
        public void SetCanvas(Canvas Canvas)
        {
            if (this.Canvas != null)
                this.Canvas.Remove(this);
            this.Canvas = Canvas;
            Canvas.Add(this);
        }
        #endregion

        #region Control Interaktion

        /// <summary>
        /// Benutzer verlässt den Container.
        /// </summary>
        /// <param name="Object">This.</param>
        /// <param name="e">Mausdaten des Events.</param>
        private void Container_OnMouseLeave(RenderObject Object, GDIMouseEventArgs e)
        {
            ActiveControl = null;
        }

        /// <summary>
        /// Hebt ein Control in den Vordergrund des Panels.
        /// </summary>
        /// <param name="Control">Control, welches in den Vordergrund gehoben werden soll.</param>
        internal void BringToFront(Control Control)
        {
            if (SubObjects.Contains(Control))
            {
                Remove(Control);
                Add(Control);
            }
        }
        /// <summary>
        /// Stellt ein Aktives Control innerhalb des Containers ein.
        /// </summary>
        /// <param name="Control">Control, welches als aktiv ist.</param>
        internal void SetActiveControl(Control Control)
        {
            ActiveControl = Control;
        }

        /// <summary>
        /// Mousedown sorgt dafür, das alle Child-Controls den Fokus verlieren.
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="e"></param>
        private void Container_OnMouseDown(RenderObject Object, GDIMouseEventArgs e)
        {
            if (SubObjects != null)
            {
                for (int i = 0; i < SubObjects.Count; i++)
                {
                    Control tmpControl = SubObjects[i] as Control;
                    if (tmpControl != null)
                        tmpControl.Focused = false;
                }
            }
        }
        #endregion
    }
}
