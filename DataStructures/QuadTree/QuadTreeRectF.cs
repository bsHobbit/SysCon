using System.Collections.Generic;
using System.Drawing;

namespace DataStructures.QuadTree
{
    /// <summary>
    /// Quadtree, welcher mit der System.Drawing.RectangleF Struktur arbeitet.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    /// <typeparam name="T">Typ der Objekte, die im Quadtree gespeichert werden.</typeparam>
    public class QuadTreeRectF<T>
        where T : class, IQuadTreeObject
    {
        #region Member
        /// <summary>
        /// Wurzel des QuadTrees.
        /// </summary>
        Node<T> Root;

        /// <summary>
        /// Anzahl Elemente im Quadtree.
        /// </summary>
        public int Count { get { return Root.Count; } }

        #endregion

        #region Konstruktor
        /// <summary>
        /// Initialisiert eine neue Instanz der QuadTreeRectF Klasse
        /// </summary>
        public QuadTreeRectF() : this(new RectangleF(-float.MaxValue / 4, -float.MaxValue / 4, float.MaxValue, float.MaxValue)) { }

        /// <summary>
        /// Initialisiert eine neue Instanz der QuadTreeRectF Klasse
        /// </summary>
        /// <param name="Rect">Rechteck, welches die Wurzel des Quadtrees beinhaltet</param>
        public QuadTreeRectF(RectangleF Rect)
        {
            Root = new Node<T>(Rect);
        }
        #endregion

        #region Add / Remove
        /// <summary>
        /// Fügt dem QuadTree ein Item hinzu.
        /// </summary>
        /// <param name="Item">Item, welches dem QuadTree hinzugefügt werden soll</param>
        public void Add(T Item)
        {
            Root.Insert(Item);
        }

        /// <summary>
        /// Entfernt ein Item aus dem QuadTree.
        /// </summary>
        /// <param name="Item">Item, welches aus dem QuadTree entfernt werden soll.</param>
        public void Remove(T Item)
        {
            Root.Remove(Item);
            Root.Balance();
            Root.Cleanup();
        }

        /// <summary>
        /// Leert den kompletten QuadTree.
        /// </summary>
        public void Clear()
        {
            Root.Clear();
        }
        #endregion

        #region Query
        /// <summary>
        /// Erstellt eine Liste aller Rechtecke, die sich sich innerhalb des angegeben Rechteckes befinden.
        /// </summary>
        /// <param name="Rect">Rechteck, in welchem das Item liegen muss, damit es zur Liste hinzugefügt wird.</param>
        /// <returns>Liste mit allen Items, die innerhalb des Rechteckes liegen.</returns>
        public List<T> Query(RectangleF Rect)
        {
            return Root.Query(Rect);
        }
        #endregion

        #region Rechteckerstellung zwecks Debug
        /// <summary>
        /// Ertellt eine Liste aller Rechtecke, die durch den QuadTree und dessen Knoten abgedeckt werden.
        /// </summary>
        /// <returns>Liste aller vom QuadTree verwalteten Rechtecke</returns>
        public List<RectangleF> AllRects()
        {
            List<RectangleF> result = new List<RectangleF>();
            Root.AddRectangles(result);
            return result;
        }
        #endregion
    }
}
