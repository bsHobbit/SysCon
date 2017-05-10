using Mathematics.Vector;
using System.Collections.Generic;

namespace Renderer.GDI
{
    /// <summary>
    /// Canvas, welcher zum rendern von RenderObject instanzen verwendet werden kann.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    public class Canvas
    {
        #region Events
        /// <summary>
        /// Maus event für diesen Canvas.
        /// </summary>
        /// <param name="Canvas">Verweis auf den Canvas, auf welchem das Maus-Event stattgefunden hat.</param>
        /// <param name="Location">Position der Maus in World-Koordinaten.</param>
        /// <param name="ButtonStates">Status der Knöpfe der Maus.</param>
        /// <param name="Delta"></param>
        public delegate void CanvasMouseEventHandler(Canvas Canvas, GDIMouseEventArgs e);

        /// <summary>
        /// Wird immer dann aufgerufen, wenn sich die Maus über dem Canvas bewegt.
        /// </summary>
        public event CanvasMouseEventHandler OnMouseMove;
        /// <summary>
        /// Wird immer dann aufgerufen, wenn eine Maustaste innerhalb des Canvas gedrückt wurde.
        /// </summary>
        public event CanvasMouseEventHandler OnMouseDown;
        /// <summary>
        /// Wird immer dann aufgerufen, wenn eine Maustaste innerhalb des Canvas losgelassen wurde.
        /// </summary>
        public event CanvasMouseEventHandler OnMouseUp;
        #endregion

        #region Member
        /// <summary>
        /// Control, auf welchem gerendert wird.
        /// </summary>
        System.Windows.Forms.Control renderTarget;

        /// <summary>
        /// Status der einzelnen Mausknöpfe (Left, Middle, Right)
        /// </summary>
        bool[] MouseStates;

        /// <summary>
        /// Delta des Mausrads.
        /// </summary>
        int MouseDelta;

        /// <summary>
        /// Ruft das Rendertarget des Canvas ab
        /// </summary>
        public System.Windows.Forms.Control RenderTarget { get { return renderTarget; } }

        /// <summary>
        /// Camera, welche zum rendern verwendet wird.
        /// </summary>
        Camera2D camera;

        /// <summary>
        /// Ruft die verwendete Kamera ab.
        /// </summary>
        public Camera2D Camera { get { return camera; } }

        /// <summary>
        /// Liste aller aktuell sichtbaren Objekte.
        /// </summary>
        public List<RenderObject> VisibleRenderObjects { get; private set; }

        /// <summary>
        /// QuadTree, welcher alle zu rendernden Objekte speichert.
        /// </summary>
        DataStructures.QuadTree.QuadTreeRectF<RenderObject> ObjectQuadTree;

        /// <summary>
        /// Eventverwaltung für die gerenderten Objekte.
        /// </summary>
        RenderObjectEvents RenderObjectEventHandler;

        /// <summary>
        /// Speichert die letzte Mausposition zwischen.
        /// </summary>
        Vector2 CurrentMousePosition;

        #endregion

        #region Konstruktor
        /// <summary>
        /// Initialisiert eine neue Instanz der Canvas Klasse
        /// </summary>
        /// <param name="RenderTarget">Control, auf welchem der Canvas rendern soll.</param>
        public Canvas(System.Windows.Forms.Control RenderTarget)
        {
            ObjectQuadTree = new DataStructures.QuadTree.QuadTreeRectF<RenderObject>(new System.Drawing.RectangleF(0, 0, RenderTarget.Width * 1000, RenderTarget.Height * 1000));

            renderTarget = RenderTarget;

            MouseStates = new bool[3];

            RenderTarget.Paint += RenderTarget_Paint;
            RenderTarget.SizeChanged += RenderTarget_SizeChanged;
            RenderTarget.MouseMove += RenderTarget_MouseMove;
            RenderTarget.MouseDown += RenderTarget_MouseDown;
            RenderTarget.MouseUp += RenderTarget_MouseUp;

            camera = new Camera2D(new Vector2(RenderTarget.Width, RenderTarget.Height));
            camera.ViewTransformationChanged += Camera_ViewTransformationChanged;

            var Timer = new System.Windows.Forms.Timer();
            Timer.Enabled = true;
            Timer.Interval = 50;
            Timer.Tick += Redraw_Tick;

            CurrentMousePosition = new Vector2();
            RenderObjectEventHandler = new RenderObjectEvents(this);
        }


        #endregion

        #region Maus Eingabe
        /// <summary>
        /// MouseUp Event des Rendertarget wird in korrektes MouseUp-Event des Canvas umgewandelt
        /// </summary>
        /// <param name="sender">Rendertarget, auf welchem der Canvas rendert.</param>
        /// <param name="e">Wird zum extrahieren der Koordinaten innerhalb des RenderTargets verwendet.</param>
        private void RenderTarget_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                MouseStates[0] = false;
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle)
                MouseStates[1] = false;
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                MouseStates[2] = false;

            Vector2 mousePos = new Vector2(e.X, e.Y) * camera.InverseViewTransformMatrix;
            OnMouseUp?.Invoke(this, new GDIMouseEventArgs(mousePos, mousePos, MouseStates, MouseDelta));
        }

        /// <summary>
        /// MouseDown-Event, des Rendertargets umwandeln in korrektes MouseDown-Event des Canvas
        /// </summary>
        /// <param name="sender">Rendertarget, auf welchem der Canvas rendert.</param>
        /// <param name="e">Wird zum extrahieren der Koordinaten innerhalb des RenderTargets verwendet.</param>
        private void RenderTarget_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                MouseStates[0] = true;
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle)
                MouseStates[1] = true;
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                MouseStates[2] = true;

            Vector2 mousePos = new Vector2(e.X, e.Y) * camera.InverseViewTransformMatrix;
            OnMouseDown?.Invoke(this, new GDIMouseEventArgs(mousePos, mousePos, MouseStates, MouseDelta));
        }

        /// <summary>
        /// Mousemove-Event des Rendertarget umwandeln in korrektes MouseMove-Event des Canvas.
        /// </summary>
        /// <param name="sender">Rendertarget, auf welchem der Canvas rendert.</param>
        /// <param name="e">Wird zum extrahieren der Koordinaten innerhalb des RenderTargets verwendet.</param>
        private void RenderTarget_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Vector2 mousePos = new Vector2(e.X, e.Y) * camera.InverseViewTransformMatrix;
            OnMouseMove?.Invoke(this, new GDIMouseEventArgs(mousePos, mousePos, MouseStates, MouseDelta));
        }
        #endregion

        #region Timer zum Automatischen Rendern
        /// <summary>
        /// Automatisch alle x Millisekunden das Rendertarget erneuern.
        /// </summary>
        /// <param name="sender">Timer, welcher im Konstruktor erstellt wurde</param>
        /// <param name="e">Argumente, die mich nicht interessieren.</param>
        private void Redraw_Tick(object sender, System.EventArgs e)
        {
            renderTarget.Invalidate();
        }
        #endregion

        #region Vertices neu Transformieren 

        /// <summary>
        /// Aktualisiert die Liste der momentan sichtbaren Objekte.
        /// </summary>
        void UpdateVisibleObjects()
        {
            VisibleRenderObjects = ObjectQuadTree.Query(Mathematics.Calculations.GetBoundingRect(Camera.ViewRect));
        }

        /// <summary>
        /// Knotenpunk eines Objekts hat sich geändert, es kann sein, das er nun nicht mehr gerendert werden muss.
        /// </summary>
        /// <param name="Object">Objekt, welcher den Knotenpunkt im QuadTree geändert hat.</param>
        private void ObjectNodeChanged(DataStructures.QuadTree.IQuadTreeObject Object)
        {
            UpdateVisibleObjects();
        }

        /// <summary>
        /// Die ViewTransformation Matrix der Kamera hat sich verändert, Vertices für alle Objekte müssen neu berechnet werden.
        /// </summary>
        /// <param name="m">ViewTransformation Matrix der Kamera.</param>
        private void Camera_ViewTransformationChanged(Mathematics.Matrix m)
        {
            UpdateVisibleObjects();
            for (int i = 0; i < VisibleRenderObjects.Count; i++)
                VisibleRenderObjects[i].Transform(m);

            //RenderObjectEventHandler.ForceMouseMove(new GDIMouseEventArgs(CurrentMousePosition * camera.InverseViewTransformMatrix, MouseStates, MouseDelta));
        }

        /// <summary>
        /// Die WorldMatrix eines Objekts hat sich geändert, Vertices für dieses Objekt müssen neu berechnet werden.
        /// </summary>
        /// <param name="o">Objekt, bei welchem die WorldMatrix sich verändert hat.</param>
        /// <param name="WorldMatrix">Neue WorldMatrix des Objekts.</param>
        private void ObjectWorldMatrixChanged(RenderObject o, Mathematics.Matrix WorldMatrix)
        {
            o.Transform(camera.ViewTransformMatrix);
            //RenderObjectEventHandler.ForceMouseMove(new GDIMouseEventArgs(CurrentMousePosition * camera.InverseViewTransformMatrix, MouseStates, MouseDelta));
        }

        /// <summary>
        /// Die Größe des Controls, auf welchem gerendert wurde hat sich verändert, das hat Auswirkung auf die ViewTransformation Matrix der Kamera.
        /// </summary>
        /// <param name="sender">Rendertarget, welches in den Konstruktur übergeben wurde.</param>
        /// <param name="e">EventArgs, die mich nicht interessieren.</param>
        private void RenderTarget_SizeChanged(object sender, System.EventArgs e)
        {
            camera.ScreenSize = new Vector2(renderTarget.Width, renderTarget.Height);
        }
        #endregion

        #region Add, Remove, Clear
        /// <summary>
        /// Fügt dem Canvas ein Objekt hinzu, welches mit gerendert werden soll.
        /// </summary>
        /// <param name="Obj">Objekt, welches mit auf dem Canvas gerendert werden soll.</param>
        public void Add(RenderObject Obj)
        {
            ObjectQuadTree.Add(Obj);

            Obj.OnWorldMatrixChanged += ObjectWorldMatrixChanged;
            Obj.OnNodeChanged += ObjectNodeChanged;

            Obj.Transform(camera.ViewTransformMatrix);

            UpdateVisibleObjects();
        }



        /// <summary>
        /// Entfernt ein Objekt vom Canvas, dieses wird in Zukunft nicht mehr gerendert.
        /// </summary>
        /// <param name="Obj">Objekt, welches vom Canvas entfernt werden soll.</param>
        public void Remove(RenderObject Obj)
        {
            ObjectQuadTree.Remove(Obj);

            Obj.OnWorldMatrixChanged -= ObjectWorldMatrixChanged;
            Obj.OnNodeChanged -= ObjectNodeChanged;

            UpdateVisibleObjects();
        }

        /// <summary>
        /// Entfernt alle Objekte vom Canvas.
        /// </summary>
        public void Clear()
        {
            ObjectQuadTree.Clear();
        }
        #endregion

        #region Rendern

        /// <summary>
        /// Rendert den text eines Objekts.
        /// </summary>
        /// <param name="g">Graphics-Objekt, auf welchem gerendert werden soll.</param>
        /// <param name="Item">Item, zu welchem der Text gehört.</param>
        /// <param name="l">Liste aller Texteinträge, die gerendert werden sollen.</param>
        /// <param name="DrawCalls">Zählt die Aufrufe der GDI Draw Methoden.</param>
        void RenderText(System.Drawing.Graphics g, RenderObject Item, List<RenderText> l, ref int DrawCalls)
        {
            
            for (int i = 0; i < l.Count; i++)
            {
                DrawCalls++;
                Vector2 vCenter = new Vector2() * (camera.ViewTransformMatrix * Item.WorldMatrix);
                g.TranslateTransform((float)vCenter.X, (float)vCenter.Y, System.Drawing.Drawing2D.MatrixOrder.Append);
                g.RotateTransform((float)Mathematics.Calculations.RadianToDegree(camera.Rotation) + (float)Mathematics.Calculations.RadianToDegree(Item.TotalRotation));
                if (Mathematics.FloatingPointComparison.Equals(camera.Scale.X, 1))
                    g.DrawString(l[i].Text, l[i].Font, new System.Drawing.SolidBrush(l[i].Color), Conversion.ToPointF(l[i].Location * camera.Scale.X));
                else
                    g.DrawString(l[i].Text, new System.Drawing.Font(l[i].Font.Name, l[i].Font.Size * (float)camera.Scale.X), new System.Drawing.SolidBrush(l[i].Color), Conversion.ToPointF(l[i].Location * camera.Scale.X));
            }
            g.ResetTransform();
        }

        /// <summary>
        /// Rendert ggf. die Textur eines Objekts.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="Item"></param>
        /// <param name="DrawCalls"></param>
        void RenderTexture(System.Drawing.Graphics g, RenderObject Item, ref int DrawCalls)
        {
            if (Item.Texture != null )
            {
                DrawCalls++;

                Vector2 vCenter = new Vector2() * (camera.ViewTransformMatrix * Item.WorldMatrix);
                
                g.TranslateTransform((float)vCenter.X, (float)vCenter.Y);
                g.ScaleTransform((float)camera.Scale.X, (float)camera.Scale.Y);
                g.RotateTransform((float)Mathematics.Calculations.RadianToDegree(camera.Rotation) + (float)Mathematics.Calculations.RadianToDegree(Item.TotalRotation));
                System.Drawing.RectangleF boundingRect = Mathematics.Calculations.GetBoundingRect(Item.Vertices);
                System.Drawing.RectangleF renderRect = Imaging.ImageMath.GetRenderRect(Item.Texture.Width, Item.Texture.Height, (int)boundingRect.Width, (int)boundingRect.Height);
                g.DrawImage(Item.Texture, new System.Drawing.Rectangle((int)(-boundingRect.Width / 2 + renderRect.X), (int)(-boundingRect.Height / 2 + renderRect.Y), (int)renderRect.Width, (int)renderRect.Height), new System.Drawing.Rectangle(0, 0, Item.Texture.Width, Item.Texture.Height), System.Drawing.GraphicsUnit.Pixel);

                g.ResetTransform();
            }
        }

        /// <summary>
        /// Rendert ein RenderObjekt auf dem übergebenem Graphics-Objekt.
        /// </summary>
        /// <param name="g">Graphics-Objekt, auf welchem gerendert werden soll</param>
        /// <param name="Item">RenderObjekt, welches gerendert werden soll</param>
        /// <param name="DrawCalls">Zählt die Anzahl der Draw-Aufrufe innerhalb der GDI Umgebung.</param>
        void RenderObject(System.Drawing.Graphics g, RenderObject Item, ref int DrawCalls)
        {
            if (Item.Visible)
            {
                DrawCalls++;
                if (!Item.Fill)
                    g.DrawPolygon(new System.Drawing.Pen(new System.Drawing.SolidBrush(Item.Color), (float)Item.RenderWidth * (float)camera.Scale.X), Item.TransformedVerticesForGDI);
                else
                {
                    g.FillPolygon(new System.Drawing.SolidBrush(Item.Color), Item.TransformedVerticesForGDI);
                    if (Item.BorderColor != Item.Color)
                        g.DrawPolygon(new System.Drawing.Pen(new System.Drawing.SolidBrush(Item.BorderColor), (float)Item.RenderWidth * (float)camera.Scale.X), Item.TransformedVerticesForGDI);
                }

                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddPolygon(Item.TransformedClippingVerticesForGDI);
                g.SetClip(path);

                if (Item.TextToRender != null)
                    RenderText(g, Item, Item.TextToRender, ref DrawCalls);

                RenderTexture(g, Item, ref DrawCalls);

                if (Item.SubObjects != null)
                    for (int i = 0; i < Item.SubObjects.Count; i++)
                        RenderObject(g, Item.SubObjects[i], ref DrawCalls);

                g.ResetClip();
            }
        }

        /// <summary>
        /// Rendert alle Objekte, die dem Canvas hinzugefügt wurden und im Sichtbereich der Kamera sind.
        /// </summary>
        /// <param name="sender">RenderTarget, welches im Konstruktor übergeben wurde.</param>
        /// <param name="e">Ermöglicht den Zugriff auf das Graphics objekt des RenderTargets.</param>
        private void RenderTarget_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

            System.Drawing.Graphics g = e.Graphics;

            if (VisibleRenderObjects != null)
            {
                int drawCalls = 0;

                for (int i = 0; i < VisibleRenderObjects.Count; i++)
                {
                    try { RenderObject(e.Graphics, VisibleRenderObjects[i], ref drawCalls); }
                    catch {  }
                }

                e.Graphics.DrawString("Draw Calls: " + drawCalls.ToString(), new System.Drawing.Font("Arial", 12), new System.Drawing.SolidBrush(System.Drawing.Color.Red), new System.Drawing.PointF());
            }
            MouseDelta = 0;
            // DEBUG RENDERN DES QUADTREE
            //List<System.Drawing.RectangleF> rects = ObjectQuadTree.AllRects();
            //foreach (var item in rects)
            //{
            //    try
            //    {
            //        RenderObject r = Renderer.GDI.RenderObject.CreateRectangle(item.Width, item.Height, new Vector2(item.X + item.Width / 2, item.Y + item.Height / 2), System.Drawing.Color.Black, false);
            //        //r.Transform(camera.ViewTransformMatrix);
            //        g.DrawPolygon(new System.Drawing.Pen(new System.Drawing.SolidBrush(r.Color)), r.TransformedVerticesForGDI);
            //    }
            //    catch { }
            //}
        }
        #endregion
    }
}
