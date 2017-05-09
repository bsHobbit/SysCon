using Mathematics.Vector;
using System.Collections.Generic;

namespace Renderer.GDI
{
    /// <summary>
    /// Verwaltet die Events für RenderObjects innerhalb eines Canvas und leitet diese Weiter.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    internal class RenderObjectEvents
    {
        #region Member
        /// <summary>
        /// Canvas, von welchem die Events verarbeitet werden.
        /// </summary>
        Canvas Canvas;
        /// <summary>
        /// Objekt, über welchem sich die Maus befindet.
        /// </summary>
        RenderObject MouseOverObject;

        #endregion


        #region Konstruktor
        /// <summary>
        /// INitialisiert eine neue Instanz der RenderObjectEvents Klasse.
        /// </summary>
        /// <param name="Canvas">Canvas, aus welchem die Objekte verwaltet werden sollen.</param>
        public RenderObjectEvents(Canvas Canvas)
        {
            this.Canvas = Canvas;
            Canvas.OnMouseDown += Canvas_OnMouseDown;
            Canvas.OnMouseUp += Canvas_OnMouseUp;
            Canvas.OnMouseMove += Canvas_OnMouseMove;
        }
        #endregion



        #region Canvas-Events

        /// <summary>
        /// Sucht in einer Liste nach dem ersten Objekt, über welchem sich die Maus befindet.
        /// </summary>
        /// <param name="Objects">Liste der Objekt, die durchsucht werden sollen.</param>
        /// <param name="e">Mausdaten, welche die Position der Mausenthalten um prüfen zu können, ob sich die Maus über einem Objekt befindet.</param>
        /// <returns>Objekt an der Maus oder null, wenn sich die Maus über keinem Objekt befindet.</returns>
        RenderObject GetObjectAtMouse(List<RenderObject> Objects, GDIMouseEventArgs e)
        {
            RenderObject result = null;
            for (int i = Objects.Count - 1; i >= 0; i--)
            {
                if (Objects[i].Visible && Objects[i].Enabled && Objects[i].Contains(e.LocalLocation))
                {
                    result = Objects[i];

                    if (Objects[i].SubObjects != null)
                    {
                        RenderObject resSubObject = GetObjectAtMouse(Objects[i].SubObjects, e);
                        if (resSubObject != null)
                            result = resSubObject;
                    }
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// Aktualisiert das Objekt, über welchem sich die Maus befindet und ruft entsprechend MouseEnter und Leave Events der Objekte auf.
        /// </summary>
        /// <param name="c">Canvas, zu welchem das MouseMove Event gehört</param>
        /// <param name="e">Daten der Maus, die zu dem Event gehören.</param>
        void MouseMove_UpdateMouseOver(Canvas c, GDIMouseEventArgs e)
        {
            RenderObject objAtMouse = null;
            if (c.VisibleRenderObjects != null)
                objAtMouse = GetObjectAtMouse(c.VisibleRenderObjects, e);

            if (MouseOverObject != objAtMouse)
            {
                if (MouseOverObject != null && objAtMouse == null) //Kein Objekt mehr bei der Maus
                {
                    List<RenderObject> owners = MouseOverObject.GetOwners();
                    MouseOverObject.RaiseEvent_MouseLeave(e);
                    for (int i = owners.Count - 1; i >= 0; i--)
                        owners[i].RaiseEvent_MouseLeave(e);
                }
                else if (MouseOverObject == null && objAtMouse != null) //Vorher keins jetzt eins
                {
                    List<RenderObject> owners = objAtMouse.GetOwners();
                    for (int i = owners.Count - 1; i >= 0; i--)
                        owners[i].RaiseEvent_MouseEnter(e);
                    objAtMouse.RaiseEvent_MouseEnter(e);
                }
                else if (MouseOverObject != null && objAtMouse != null) //Wechsel
                {
                    RenderObject o = MouseOverObject; //Old
                    RenderObject n = objAtMouse; //New

                    List<RenderObject> currentOwners = MouseOverObject.GetOwners();
                    List<RenderObject> newOwners = objAtMouse.GetOwners();

                    if (currentOwners.Contains(n))
                    {
                        int startIndex = currentOwners.IndexOf(n);
                        o.RaiseEvent_MouseLeave(e);
                        for (int i = startIndex - 1; i >= 0; i--)
                            currentOwners[i].RaiseEvent_MouseLeave(e);
                    }
                    else
                    {
                        o.RaiseEvent_MouseLeave(e);
                        for (int i = currentOwners.Count - 1; i >= 0; i--)
                            currentOwners[i].RaiseEvent_MouseLeave(e);

                        for (int i = newOwners.Count - 1; i >= 0; i--)
                            newOwners[i].RaiseEvent_MouseEnter(e);
                        n.RaiseEvent_MouseEnter(e);
                    }
                }
                MouseOverObject = objAtMouse;
            }
        }

        /// <summary>
        /// Mouse-Move Event des Canvas.
        /// </summary>
        /// <param name="Canvas">Canvas, in welchem das Event stattgefunden hat.</param>
        /// <param name="e">Status der Maus.</param>
        private void Canvas_OnMouseMove(Canvas Canvas, GDIMouseEventArgs e)
        {
            if (MouseOverObject != null)
                MouseOverObject.RaiseEvent_MouseMove(e);
            MouseMove_UpdateMouseOver(Canvas, e);
        }
        /// <summary>
        /// Mouse-Up Event des Canvas.
        /// </summary>
        /// <param name="Canvas">Canvas, in welchem das Event stattgefunden hat.</param>
        /// <param name="e">Status der Maus.</param>
        private void Canvas_OnMouseUp(Canvas Canvas, GDIMouseEventArgs e)
        {
            if (MouseOverObject != null)
                MouseOverObject.RaiseEvent_MouseUp(e);
        }
        /// <summary>
        /// Mouse-Down Event des Canvas.
        /// </summary>
        /// <param name="Canvas">Canvas, in welchem das Event stattgefunden hat.</param>
        /// <param name="e">Status der Maus.</param>
        private void Canvas_OnMouseDown(Canvas Canvas, GDIMouseEventArgs e)
        {
            if (MouseOverObject != null)
                MouseOverObject.RaiseEvent_MouseDown(e);
        }

        /// <summary>
        /// Zwingt dem System ein Maus-Move event auf, damit auch sich bewegende Objekte erfasst werden.
        /// </summary>
        internal void ForceMouseMove(GDIMouseEventArgs e)
        {
            Canvas_OnMouseMove(Canvas, e);
        }
        #endregion
    }
}
