using Mathematics;
using Mathematics.Vector;
using System.Collections.Generic;

namespace Renderer.GDI
{
    /// <summary>
    /// Renderobjekt, welches später zum Rendern verwendet werden kann.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    public class RenderObject : DataStructures.QuadTree.IQuadTreeObject
    {
        #region Events
        /// <summary>
        /// Worldmatrix für das Objekt ändert sich.
        /// </summary>
        /// <param name="o">Objekt, dessen World-Matrix sich verändert hat.</param>
        /// <param name="WorldMatrix">Neue World-Matrix des Objekts.</param>
        public delegate void WorldMatrixChangedEventHandler(RenderObject o, Matrix WorldMatrix);
        /// <summary>
        /// Event, welches ausgelöst wird, sobald sich die WordMatrix des RenderObjects ändert.
        /// </summary>
        public event WorldMatrixChangedEventHandler OnWorldMatrixChanged;
        /// <summary>
        /// Event, welches aufgerufen wird, sobald sich die Boundingbox verändert.
        /// </summary>
        public event DataStructures.QuadTree.IQuadTreeObjectEventHandler OnBoundingBoxChanged;
        /// <summary>
        /// Event, welches aufgerufen wird, sobald sich der Node im IQuadTreeObject verändert
        /// </summary>
        public event DataStructures.QuadTree.IQuadTreeObjectEventHandler OnNodeChanged;
        /// <summary>
        /// Maus-Events für ein Objekt.
        /// </summary>
        /// <param name="Object">Objekt, welches das Event ausgelöst hat.</param>
        /// <param name="e">Mausdaten zu dem Event.</param>
        public delegate void RenderObjectMouseEventHandler(RenderObject Object, GDIMouseEventArgs e);
        /// <summary>
        /// MouseDown Event des Objekts.
        /// </summary>
        /// <remarks>RenderObjectEvents Klasse muss dem Canvas des Objekts zugeordnet sein, damit die Events ausgelöst werden.</remarks>
        public event RenderObjectMouseEventHandler OnMouseDown;
        /// <summary>
        /// MouseUp Event des Objekts.
        /// </summary>
        /// <remarks>RenderObjectEvents Klasse muss dem Canvas des Objekts zugeordnet sein, damit die Events ausgelöst werden.</remarks>
        public event RenderObjectMouseEventHandler OnMouseUp;
        /// <summary>
        /// MouseMove Event des Objekts.
        /// </summary>
        /// <remarks>RenderObjectEvents Klasse muss dem Canvas des Objekts zugeordnet sein, damit die Events ausgelöst werden.</remarks>
        public event RenderObjectMouseEventHandler OnMouseMove;
        /// <summary>
        /// MouseEnter Event für das Objekt.
        /// </summary>
        /// <remarks>RenderObjectEvents Klasse muss dem Canvas des Objekts zugeordnet sein, damit die Events ausgelöst werden.</remarks>
        public event RenderObjectMouseEventHandler OnMouseEnter;
        /// <summary>
        /// MouseLeave Event für das Objekt.
        /// </summary>
        /// <remarks>RenderObjectEvents Klasse muss dem Canvas des Objekts zugeordnet sein, damit die Events ausgelöst werden.</remarks>
        public event RenderObjectMouseEventHandler OnMouseLeave;

        /// <summary>
        /// Handler für Events die einfach nur ein RenderObjekt übergeben.
        /// </summary>
        /// <param name="Object">Objekt, welches das Event ausgelöst hat.</param>
        public delegate void RenderObjectEventHandler(RenderObject Object);

        /// <summary>
        /// Wird aufgerufen, wenn die Visible Eigenschaft des Objekts sich ändert.
        /// </summary>
        public event RenderObjectEventHandler OnVisibleChanged;

        /// <summary>
        /// Wird aufgerufen, wenn die Enabled Eigenschaft des Objekts sich ändert.
        /// </summary>
        public event RenderObjectEventHandler OnEnabledChanged;

        #endregion

        #region Member
        /// <summary>
        /// Liste aller zu renderenden Texte.
        /// </summary>
        internal List<RenderText> TextToRender { get; private set; }
        /// <summary>
        /// Liste aller Sub-Objekte, die innerhalb dieses Render-Objekts gerendert werden soll.
        /// </summary>
        internal List<RenderObject> SubObjects { get; private set; }
        /// <summary>
        /// Besitzer dieses RenderObject, falls es sich um ein SubObjekt handelt.
        /// </summary>
        internal RenderObject Owner { get; private set; }
        /// <summary>
        /// Ruft die Breite mit welcher das Objekt gerendert werden soll ab, oder legt diese fest.
        /// </summary>
        public double RenderWidth { get; set; }
        /// <summary>
        /// Vertices des Objekts
        /// </summary>
        public Vector2[] Vertices;
        /// <summary>
        /// Speichert die Transformierten Vertices.
        /// </summary>
        private Vector2[] TransformedVertices;
        /// <summary>
        /// Ruft die in die Welt Transformierten Koordinaten des Objekts ab.
        /// </summary>
        public Vector2[] WorldVertices { get; private set; }
        /// <summary>
        /// Speichert das Zentrum der Kamera.
        /// </summary>
        Vector2 location;
        /// <summary>
        /// Ruft die Position des Objekts ab, oder legt dieses Fest.
        /// </summary>
        public Vector2 Location { get { return location; } set { location = value; UpdateWorldMatrix(); } }
        /// <summary>
        /// Speichert die Skalierung
        /// </summary>
        Vector2 scale;
        /// <summary>
        /// Ruft die Skalierung des Objekts ab, oder legt diese fest.
        /// </summary>
        public Vector2 Scale { get { return scale; } set { scale = value; UpdateWorldMatrix(); } }
        /// <summary>
        /// Speichert die Rotation des Objekts.
        /// </summary>
        double rotation;
        /// <summary>
        /// Ruft die Rotation des Objekts ab, oder legt diese fest.
        /// </summary>
        public double Rotation { get { return rotation; } set { rotation = value; UpdateWorldMatrix(); } }
        /// <summary>
        /// Berechnet die Rotation inkl. Owner rotation, wird für einige GDI operationen benötigt.
        /// </summary>
        public double TotalRotation { get { if (Owner != null) return Owner.rotation + rotation; return rotation; } }
        /// <summary>
        /// Transformationsmatrix des Objekts vom Lokalem Koordinatensystem in das globale Koordinatensystem.
        /// </summary>
        Matrix worldMatrix;
        /// <summary>
        /// Transformationsmatrix des Objekts vom Lokalem Koordinatensystem in das globale Koordinatensystem.
        /// </summary>
        public Matrix WorldMatrix
        {
            get
            {
                if (Owner != null)
                    return Owner.WorldMatrix * worldMatrix;
                return worldMatrix;
            }
            private set
            {
                worldMatrix = value;
            }
        }
        /// <summary>
        /// Farbe, mit welcher das Objekt gerendert werden soll
        /// </summary>
        public System.Drawing.Color Color { get; set; }
        /// <summary>
        /// Farbe, mit welcher der Rand gerendert werden soll, wenn das Objekt gefüllt ist.
        /// </summary>
        public System.Drawing.Color BorderColor { get; set; }
        /// <summary>
        /// True, wenn das Objekt gefüllt dargestellt werden soll, sonst false
        /// </summary>
        public bool Fill { get; set; }
        /// <summary>
        /// Speichert die BoundingBox des Objekts
        /// </summary>
        System.Drawing.RectangleF boundingBox;
        /// <summary>
        /// Ruft das BoundingRect für dieses Objekt ab.
        /// </summary>
        public System.Drawing.RectangleF BoundingBox { get { return boundingBox; } }
        /// <summary>
        /// Speichert die Transformierten Vertices für GDI
        /// </summary>
        System.Drawing.PointF[] transformedVerticesForGDI;
        /// <summary>
        /// Ruft die Transformierten Vertices für das Rendern in GDI ab.
        /// </summary>
        public System.Drawing.PointF[] TransformedVerticesForGDI { get { return transformedVerticesForGDI; } private set { transformedVerticesForGDI = value; } }
        /// <summary>
        /// Ruft die Transformierten Clipping Vertices für GDI ab.
        /// </summary>
        public System.Drawing.PointF[] TransformedClippingVerticesForGDI
        {
            get
            {
                if (Owner != null)
                    return Conversion.ToPointFArray(Mathematics.Geometry.Polygon2D.Clip(new Mathematics.Geometry.Polygon2D(transformedVerticesForGDI), new Mathematics.Geometry.Polygon2D(Owner.transformedVerticesForGDI)).Vertices);
                return transformedVerticesForGDI;
            }
            private set
            {
                transformedVerticesForGDI = value;
            }
        }
        /// <summary>
        /// Speichert ggf. eine Textur für das Objekt
        /// </summary>
        internal System.Drawing.Bitmap Texture { get; private set; }

        /// <summary>
        /// Speichert den Wert der angibt, ob dieses Objekt sichtbar ist oder nicht.
        /// </summary>
        bool IsVisible;

        /// <summary>
        /// Ruft einen Wert ab, der angibt ob das Objekt sichtbar ist, oder legt diesen fest.
        /// </summary>
        public bool Visible
        {
            get { return IsVisible; }
            set { IsVisible = value; OnVisibleChanged?.Invoke(this); }
        }

        /// <summary>
        /// Speichert einen Wert, der angibt ob das Objekt aktiv ist oder nicht.
        /// </summary>
        bool IsEnabled;

        /// <summary>
        /// Ruft einen Wert ab, der angibt ob das Objekt aktiv ist, oder legt diesen fest.
        /// </summary>
        public bool Enabled
        {
            get { return IsEnabled; }
            set { IsEnabled = value; OnEnabledChanged?.Invoke(this); }
        }
        #endregion

        #region Add/Remove Sub-Objects und RenderText
        
        /// <summary>
        /// Fügt einen Text hinzu, welcher gerendert werden soll.
        /// </summary>
        /// <param name="Text">Text, welcher gerendert werden soll</param>
        public void AddText(RenderText Text)
        {
            if (Text != null)
            {
                if (TextToRender == null)
                    TextToRender = new List<RenderText>();
                TextToRender.Add(Text);
            }
        }

        /// <summary>
        /// Entfernt einen Text, der nicht mehr gerendert werden soll.
        /// </summary>
        /// <param name="Text">Text, welcher nicht mehr gerendert werden soll.</param>
        public void RemoveText(RenderText Text)
        {
            if (TextToRender != null && Text != null && TextToRender.Contains(Text))
            {
                TextToRender.Remove(Text);
                if (TextToRender.Count == 0)
                    TextToRender = null;
            }
        }

        /// <summary>
        /// Fügt dem RenderObject ein Sub-Objekt hinzu, welches innerhalb von diesem Objekt gerendert werden soll.
        /// </summary>
        /// <param name="Item">Objekt, welches als Sub-Objekt hinzugefügt werden soll.</param>
        public void Add(RenderObject Item)
        {
            if (Item != null)
            {
                if (SubObjects == null)
                    SubObjects = new List<RenderObject>();
                SubObjects.Add(Item);
                Item.Owner = this;
                Item.UpdateWorldMatrix();
                Item.OnWorldMatrixChanged += SubObject_OnWorldMatrixChanged;
                OnWorldMatrixChanged?.Invoke(this, WorldMatrix);
            }
        }

        /// <summary>
        /// Event nach oben weiter geben, damit alle Items korrekt gerendert werden.
        /// Dadurch werden die Vertices vom Eltern-Objekt und damit auch von diesem Objekt neu berechnet.
        /// </summary>
        /// <param name="o">Sub-Objekt, dessen World-Matrix sich verändert hat.</param>
        /// <param name="WorldMatrix">Neue World-Matrix des Objekts.</param>
        private void SubObject_OnWorldMatrixChanged(RenderObject o, Matrix WorldMatrix)
        {
            OnWorldMatrixChanged?.Invoke(this, WorldMatrix);
        }

        /// <summary>
        /// Entfernt ein Sub-Objekt von diesem Objekt.
        /// </summary>
        /// <param name="Item">Item, welches als Sub-Objekt hinzugefügt werden soll</param>
        public void Remove(RenderObject Item)
        {
            if (SubObjects != null && Item != null && SubObjects.Contains(Item))
            {
                SubObjects.Remove(Item);
                Item.Owner = null;
            }
            if (SubObjects.Count == 0)
                SubObjects = null;
        }

        /// <summary>
        /// Entfernt alle Sub-Objekte von diesem Objekt.
        /// </summary>
        internal void ClearSubObjects()
        {
            if (SubObjects != null)
            {
                SubObjects.Clear();
                SubObjects = null;
            }
        }
        #endregion

        #region Set/Clear Texture
        /// <summary>
        /// Stellt die Textur für dieses Objekt ein.
        /// </summary>
        /// <param name="Texture">Textur, welche gerendert werden soll.</param>
        public void SetTexture(System.Drawing.Bitmap Texture)
        {
            this.Texture = Texture;
        }

        /// <summary>
        /// Löscht die dem Objekt zugeordnete Textur.
        /// </summary>
        public void ClearTexture()
        {
            Texture = null;
        }
        #endregion

        #region Konsturktor
        /// <summary>
        /// Erstellt eine neue Instanz der RenderObject Klasse.
        /// </summary>
        /// <param name="Vertices">Eckpunkte, welche das Objekt definieren.</param>
        /// <param name="Location">Position des Objekts im globalem Koordinatensystem.</param>
        /// <param name="Scale">Sklalierungs des Objekts.</param>
        /// <param name="Rotation">Rotation des Objekts.</param>
        /// <param name="Color">Farbe des Obejkts.</param>
        /// <param name="Fill">Fülleigenschaft des Objekts.</param>
        internal RenderObject(Vector2[] Vertices, Vector2 Location, Vector2 Scale, double Rotation, System.Drawing.Color Color, bool Fill)
        {
            RenderWidth = 1;
            IsVisible = true;
            IsEnabled = true;
            this.Vertices = Vertices;
            location = Location;
            scale = Scale;
            rotation = Rotation;
            this.Color = Color;
            this.BorderColor = Color;
            this.Fill = Fill;
            UpdateWorldMatrix();
        }
        #endregion

        #region Vertex Update

        /// <summary>
        /// Aktualisiert die Vertices des Objekts.
        /// </summary>
        /// <param name="Vertices">Neue Vertices für das Objekt.</param>
        internal void UpdateVertices(Vector2[] Vertices)
        {
            this.Vertices = Vertices;
            UpdateWorldMatrix();
        }
        #endregion


        #region WorldMatrix
        /// <summary>
        /// Aktualisiert die Transformationsmatrix für dieses Objekt.
        /// Ruft ggf. OnWorldMatrixChanged event auf.
        /// </summary>
        protected void UpdateWorldMatrix() { UpdateWorldMatrix(true); }

        /// <summary>
        /// Aktualisiert die Transformationsmatrix für dieses Objekt.
        /// Ruft ggf. OnWorldMatrixChanged event auf.
        /// </summary>
        /// <param name="InvokeEvents">True, wenn die entsprechenden Events aufgerufen werden sollen</param>
        protected void UpdateWorldMatrix(bool InvokeEvents)
        {
            WorldMatrix = Matrix.Transform2DTranslate(location.X, location.Y) *
                          Matrix.Transform2DRotation(rotation) *
                          Matrix.Transform2DScale(scale.X, scale.Y);


            Transform(Matrix.IdentityMatrix(3));
            boundingBox = Calculations.GetBoundingRect(TransformedVertices);
            WorldVertices = new Vector2[TransformedVertices.Length];

            for (int i = 0; i < Vertices.Length; i++)
                WorldVertices[i] = TransformedVertices[i].Clone();

            if (InvokeEvents)
            {
                OnWorldMatrixChanged?.Invoke(this, WorldMatrix);
                OnBoundingBoxChanged?.Invoke(this);
            }

            if (SubObjects != null)
                for (int i = 0; i < SubObjects.Count; i++)
                    SubObjects[i].UpdateWorldMatrix(InvokeEvents);
        }
        #endregion

        #region NodeChanged
        /// <summary>
        /// Knotenpunkt des Objektes hat sich innerhalb eines QuadTrees verändert.
        /// </summary>
        public void NodeChanged()
        {
            OnNodeChanged?.Invoke(this);
        }
        #endregion

        #region Transformation

        /// <summary>
        /// Rotiert das Objekt um den angegeben Winkel 
        /// </summary>
        /// <param name="Angle">Winkel (in Radians) um welchen das Objekt gedreht werden soll.</param>
        public void Rotate(double Angle) { Rotation += Angle; }

        /// <summary>
        /// Verschibt das Objekt um einen angegebenen Offset.
        /// </summary>
        /// <param name="Offset">Offset, um welchen das Objekt verschoben werden soll.</param>
        public void Translate(Vector2 Offset) { Location += Offset; }

        /// <summary>
        /// Transformiert das Renderobjekt in das Globale koordinatensystem und wendet dann die ViewTransformationMatrix darauf an.
        /// </summary>
        /// <param name="TransformationMatrix">Anzuwendende ViewTransformation Matrix.</param>
        /// <returns>Array mit Vektor2 Einträgen, die zum rendern verwendet werden können.</returns>
        public void Transform(Matrix TransformationMatrix)
        {
            if (Vertices != null && Vertices.Length > 0)
            {
                TransformedVertices = new Vector2[Vertices.Length];
                for (int i = 0; i < Vertices.Length; i++)
                    TransformedVertices[i] = Vertices[i] * (TransformationMatrix * WorldMatrix);
                transformedVerticesForGDI = Conversion.ToPointFArray(TransformedVertices);
                if (SubObjects != null)
                {
                    for (int i = 0; i < SubObjects.Count; i++)
                        SubObjects[i].Transform(TransformationMatrix);
                }
            }
        }
        
        #endregion

        #region Objekterzeugung

        /// <summary>
        /// Erstellt ein Rechteck, dessen lokes Zentrum bei 0,0 liegt.
        /// </summary>
        /// <param name="Width">Breite des Rechteckes.</param>
        /// <param name="Height">Höhe des Rechteckes.</param>
        /// <returns>Vector2 Array mit den Koordinaten der vier Eckpunkte des Rechteckes.</returns>
        public static Vector2[] CreateRectangle(double Width, double Height)
        {


            return new Vector2[] { new Vector2 (-Width /2, -Height /2),
                                   new Vector2 (Width / 2 , - Height / 2),
                                   new Vector2 (Width / 2, Height / 2),
                                   new Vector2 (-Width /2 , Height / 2)};
        }

        /// <summary>
        /// Erstellt ein Rechteck, welches später gerendert werden kann.
        /// </summary>
        /// <param name="Width">Breite des Rechteckes.</param>
        /// <param name="Height">Höhe des Rechteckes.</param>
        /// <param name="Location">Position des Rechteckes.</param>
        /// <param name="c">Farbe des Rechteckes.</param>
        /// <param name="Fill">Fülleigenschaft des Rechteckes.</param>
        /// <returns>RenderObjekt, welches ein Rechteck darstellt.</returns>
        public static RenderObject CreateRectangle(double Width, double Height, Vector2 Location, System.Drawing.Color c, bool Fill)
        {
            return new RenderObject(CreateRectangle(Width, Height), Location, new Vector2(1, 1), 0, c, Fill);
        }
        #endregion

        #region Bereichsprüfung
        /// <summary>
        /// Prüft ob eine Ansammlung von Vertices sich innerhalb eines ViewRects (von einer Kamera) befinden.
        /// </summary>
        /// <param name="Vertices">Zu Überprüfende Vertices.</param>
        /// <param name="ViewRect">ViewRect, in welchem die Vertices liegen müssen.</param>
        /// <returns>True, wenn die Vertices innerhalb des ViewRects liegen.</returns>
        public bool IsInViewRect(Camera2D Camera)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                if (Mathematics.Geometry.Ray2D.OrientationToLine(new Mathematics.Geometry.Ray2D(Camera.ViewRect[0], Camera.ViewRect[1]), Vertices[i] * WorldMatrix) > 0 &&
                    Mathematics.Geometry.Ray2D.OrientationToLine(new Mathematics.Geometry.Ray2D(Camera.ViewRect[1], Camera.ViewRect[2]), Vertices[i] * WorldMatrix) > 0 &&
                    Mathematics.Geometry.Ray2D.OrientationToLine(new Mathematics.Geometry.Ray2D(Camera.ViewRect[2], Camera.ViewRect[3]), Vertices[i] * WorldMatrix) > 0 &&
                    Mathematics.Geometry.Ray2D.OrientationToLine(new Mathematics.Geometry.Ray2D(Camera.ViewRect[3], Camera.ViewRect[0]), Vertices[i] * WorldMatrix) > 0)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Prüft ob sich ein Punk innerhalb des Objektes befindet.
        /// </summary>
        /// <param name="Point">Zu prüfender Punkt.</param>
        /// <returns>True, wenn der Punkt sich innerhalb des Objekts befindet.</returns>
        public bool Contains(Vector2 Point)
        {
            if (boundingBox.Contains(new System.Drawing.PointF((float)Point.X, (float)Point.Y)))
                return Mathematics.Geometry.Polygon2D.Contains(new Mathematics.Geometry.Polygon2D(WorldVertices), Point);
            return false;
        }
        #endregion

        #region Hilfsfunktionen
        /// <summary>
        /// Prüft ob dieses Objekt das Objekt eines Besitzers ist.
        /// </summary>
        /// <param name="Object">Möglicher Besitzer des Objektes.</param>
        /// <returns>True, wenn das Objekt ein SubObjekt des angegebenen Objekts ist.</returns>
        internal bool HasOwner(RenderObject Object)
        {
            RenderObject tmpObject = Owner;

            while (tmpObject != null)
            {
                if (tmpObject == Object)
                    return true;
                tmpObject = tmpObject.Owner;
            }
            return false;
        }
        /// <summary>
        /// Ruft alle Besitzer des Objekts ab.
        /// </summary>
        /// <returns>Liste mit allen Besitzern des Objekts.</returns>
        internal List<RenderObject> GetOwners()
        {
            List<RenderObject> result = new List<RenderObject>();

            RenderObject tmpObject = Owner;
            while (tmpObject != null)
            {
                result.Add(tmpObject);
                tmpObject = tmpObject.Owner;
            }
            return result;
        }
        #endregion

        #region Maus-Events auslösen (wird in der RenderObjectEvents Klasse erledigt.)
        /// <summary>
        /// Löst das Maus-Event MouseDown für dieses Objekt aus.
        /// </summary>
        /// <param name="e">Mausdaten, die für dieses Event gültig sind.</param>
        internal void RaiseEvent_MouseDown(GDIMouseEventArgs e)
        {
            if (Enabled)
            {
                GDIMouseEventArgs transformedEventArgs = new GDIMouseEventArgs(e.LocalLocation * WorldMatrix.Inverse(), e.LocalLocation, e.ButtonStates, e.Delta);
                OnMouseDown?.Invoke(this, transformedEventArgs);
            }
        }
        /// <summary>
        /// Löst das Maus-Event MouseUp für dieses Objekt aus.
        /// </summary>
        /// <param name="e">Mausdaten, die für dieses Event gültig sind.</param>
        internal void RaiseEvent_MouseUp(GDIMouseEventArgs e)
        {
            if (Enabled)
            {
                GDIMouseEventArgs transformedEventArgs = new GDIMouseEventArgs(e.LocalLocation * WorldMatrix.Inverse(), e.LocalLocation, e.ButtonStates, e.Delta);
                OnMouseUp?.Invoke(this, transformedEventArgs);
            }
        }
        /// <summary>
        /// Löst das Maus-Event MouseMove für dieses Objekt aus.
        /// </summary>
        /// <param name="e">Mausdaten, die für dieses Event gültig sind.</param>
        internal void RaiseEvent_MouseMove(GDIMouseEventArgs e)
        {
            if (Enabled)
            {
                GDIMouseEventArgs transformedEventArgs = new GDIMouseEventArgs(e.LocalLocation * WorldMatrix.Inverse(), e.LocalLocation, e.ButtonStates, e.Delta);
                OnMouseMove?.Invoke(this, transformedEventArgs);
            }
        }

        /// <summary>
        /// Löst das Maus-Event MouseEnter für dieses Objekt aus.
        /// </summary>
        /// <param name="e">Mausdaten, die für dieses Event gültig sind.</param>
        internal void RaiseEvent_MouseEnter(GDIMouseEventArgs e)
        {
            if (Enabled)
            {
                GDIMouseEventArgs transformedEventArgs = new GDIMouseEventArgs(e.LocalLocation * WorldMatrix.Inverse(), e.LocalLocation, e.ButtonStates, e.Delta);
                OnMouseEnter?.Invoke(this, transformedEventArgs);
            }
        }

        /// <summary>
        /// Löst das Maus-Event MouseLeave für dieses Objekt aus.
        /// </summary>
        /// <param name="e">Mausdaten, die für dieses Event gültig sind.</param>
        internal void RaiseEvent_MouseLeave(GDIMouseEventArgs e)
        {
            if (Enabled)
            {
                GDIMouseEventArgs transformedEventArgs = new GDIMouseEventArgs(e.LocalLocation * WorldMatrix.Inverse(), e.LocalLocation, e.ButtonStates, e.Delta);
                OnMouseLeave?.Invoke(this, transformedEventArgs);
            }
        }
        #endregion
    }
}
