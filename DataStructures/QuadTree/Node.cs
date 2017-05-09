using System.Collections.Generic;
using System.Drawing;

namespace DataStructures.QuadTree
{
    /// <summary>
    /// Knotenpunkt eines Quadtree.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    /// <typeparam name="T">Typ, welcher im Knotenpunkt gespeichert wird.</typeparam>
    internal class Node<T>
        where T : class, IQuadTreeObject
    {
        #region Member
        /// <summary>
        /// Maximale anzahl an Objekte, die ein Knotenpunkt speichert, bevor er aufgeteilt wird.
        /// </summary>
        const int MaxItemsPerNode = 10;
        /// <summary>
        /// Array mit Objekten, welche dieser Knotenpunkt beinhaltet.
        /// </summary>
        T[] Objects;
        /// <summary>
        /// Anzahl der Objekte in diesem Knotenpunkt
        /// </summary>
        internal int ObjectCount { get; private set; }
        /// <summary>
        /// Rechteck, welches diesen Knotenpunkt umschließt
        /// </summary>
        protected RectangleF Rect { get; private set; }
        /// <summary>
        /// Elternknoten für diesesn Knoten
        /// </summary>
        protected Node<T> Parent { get; private set; }
        /// <summary>
        /// Kindknoten für die obere linke Ecke.
        /// </summary>
        protected Node<T> ChildUL { get; private set; }
        /// <summary>
        /// Kindknoten für die obere rechte Ecke.
        /// </summary>
        protected Node<T> ChildUR { get; private set; }
        /// <summary>
        /// Kindknoten für die untere linke Ecke.
        /// </summary>
        protected Node<T> ChildBL { get; private set; }
        /// <summary>
        /// Kindknoten für die untere rechte Ecke.
        /// </summary>
        protected Node<T> ChildBR { get; private set; }

        /// <summary>
        /// Ursprung des Knotenpunktes.
        /// </summary>
        protected Node<T> Root
        {
            get
            {
                if (Parent == null)
                    return this;
                return Parent.Root;
            }
        }

        /// <summary>
        /// Anzahl der Elemente in diesem Knotenpunkt und allen Kindknoten.
        /// </summary>
        public int Count
        {
            get
            {
                int result = ObjectCount;
                if (ChildUL != null)
                    result += ChildBL.Count + ChildBR.Count + ChildUL.Count + ChildUR.Count;
                return result;
            }
        }

        /// <summary>
        /// Flächeninhalt des Knotenpunktes
        /// </summary>
        private float Area { get { return Rect.Height * Rect.Width; } }
        #endregion

        #region Konstruktoren
        /// <summary>
        /// Initialisiert eine neue Instanz der Node Klasse.
        /// </summary>
        /// <param name="Rect">Rechteck, welches der Knotenpunkt abdecken soll.</param>
        internal Node(RectangleF Rect)
        {
            this.Rect = Rect;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Node Klasse.
        /// </summary>
        /// <param name="Parent">Elternknoten den Knotenpunktes</param>
        /// <param name="Rect">Rechteck, welches der Knotenpunkt abdecken soll.</param>
        protected Node(Node<T> Parent, RectangleF Rect)
            : this(Rect)
        {
            this.Parent = Parent;
        }
        #endregion

        #region Add/Remove/Insert/Clear
        /// <summary>
        /// Fügt dem Knotenpunk ein neues Objekt hinzu.
        /// </summary>
        /// <param name="Item">Objekte, welches diesem Knotenpunkt hinzugefügt werden soll.</param>
        void Add(T Item)
        {
            if (Objects == null)
                Objects = new T[MaxItemsPerNode];
            else if (ObjectCount == Objects.Length)
            {
                T[] tmpArray = Objects;
                Objects = new T[tmpArray.Length * 2];
                System.Array.Copy(tmpArray, Objects, ObjectCount);
            }

            Objects[ObjectCount++] = Item;
            Item.OnBoundingBoxChanged += Item_BoundingBoxChanged;
        }
        

        /// <summary>
        /// Entfernt ein Objekt aus diesem Knotenpunkt.
        /// </summary>
        /// <param name="Item">Objekt, welches entfernt werden soll.</param>
        /// <returns>True, wenn das Objekt entfernt wurde, ansonsten false.</returns>
        internal bool Remove(T Item)
        {
            int RemoveIndex = -1;
            if (Objects != null)
                RemoveIndex = System.Array.IndexOf(Objects, Item);

            if (RemoveIndex < 0)
            {
                if (ChildBL != null)
                    return ChildBL.Remove(Item) || ChildBR.Remove(Item) || ChildUL.Remove(Item) || ChildUR.Remove(Item);
                return false;
            }

            if (ObjectCount == 1)
            {
                Objects = null;
                ObjectCount = 0;
            }
            else
            {
                Objects[RemoveIndex] = Objects[--ObjectCount];
                Objects[ObjectCount] = null;
            }

            Item.OnBoundingBoxChanged -= Item_BoundingBoxChanged;

            return true;
        }

        /// <summary>
        /// Fügt dem Knotenpunkt ein neues Objekt hinzu.
        /// </summary>
        /// <param name="Item">Objekt, welches dem Knotenpunkt hinzugefügt werden soll.</param>
        /// <returns>True, wenn das Objekt diesem oder einem Kindknoten hinzugefügt wurde.</returns>
        internal bool Insert(T Item)
        {
            if (Contains(Item))
            {
                if (Area <= 1)
                    Add(Item);
                else
                {
                    Add(Item);
                    if (ObjectCount >= MaxItemsPerNode)
                    {
                        Subdivide();
                        MoveObjects_Down();
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Entfernt alle Einträge aus diesem Knoten und allen Kindknoten.
        /// </summary>
        internal void Clear()
        {
            if (ChildBL != null)
            {
                ChildBL.Clear();
                ChildBR.Clear();
                ChildUL.Clear();
                ChildUR.Clear();
            }
            Objects = null;
            ObjectCount = 0;
        }
        #endregion

        #region Erstellung von Kindknoten
        /// <summary>
        /// Fügt vier Kindknoten hinzu, welche das Rechteck dieses Knotenpunktes in vier gleichgroße unterteilt.
        /// </summary>
        void Subdivide()
        {
            if (ChildBL == null)
            {
                ChildUL = new Node<T>(this, new RectangleF(Rect.X, Rect.Y, Rect.Width / 2, Rect.Height / 2));
                ChildUR = new Node<T>(this, new RectangleF(Rect.X + Rect.Width / 2, Rect.Y, Rect.Width / 2, Rect.Height / 2));
                ChildBL = new Node<T>(this, new RectangleF(Rect.X, Rect.Y + Rect.Height / 2, Rect.Width / 2, Rect.Height / 2));
                ChildBR = new Node<T>(this, new RectangleF(Rect.X + Rect.Width / 2, Rect.Y + Rect.Height / 2, Rect.Width / 2, Rect.Height / 2));
            }
        }
        #endregion

        #region Verschiebung von Objekten in Kindknoten

        /// <summary>
        /// BoundingBox eines Items hat sich geänert, ggf. muss es in einen anderen Knotenpunkt verschoben werden
        /// </summary>
        private void Item_BoundingBoxChanged(IQuadTreeObject Object)
        {
            if (ObjectCount > 0)
            {
                if (!Contains((T)Object))
                {
                    Remove((T)Object);
                    if (Parent != null)
                    {
                        if (!Parent.Insert((T)Object))
                            Root.Insert((T)Object);
                    }
                    else
                        Root.Insert((T)Object);

                    Root.Cleanup();
                    Object.NodeChanged();
                }
            }
        }


        /// <summary>
        /// Verschiebt Objekte, die in ein Kindknoten passen in ein Kindknoten.
        /// </summary>
        void MoveObjects_Down()
        {
            if (ChildBL != null)
            {
                for (int i = ObjectCount - 1; i >= 0; i--)
                {
                    if (ChildUL.Contains(Objects[i])) { ChildUL.Insert(Objects[i]); Remove(Objects[i]); }
                    else if (ChildUR.Contains(Objects[i])) { ChildUR.Insert(Objects[i]); Remove(Objects[i]); }
                    else if (ChildBL.Contains(Objects[i])) { ChildBL.Insert(Objects[i]); Remove(Objects[i]); }
                    else if (ChildBR.Contains(Objects[i])) { ChildBR.Insert(Objects[i]); Remove(Objects[i]); }

                    if (ObjectCount < MaxItemsPerNode)
                        break;
                }
            }
        }


        #endregion

        #region Verschieben von Objekten aus Kindknoten in den eigenen
        /// <summary>
        /// Verschiebt Objekte aus Kindknoten in Elternknoten, wenn dieser noch Kapazität besitzt
        /// </summary>
        internal void Balance()
        {
            if (ChildBL != null)
            {
                ChildBL.Balance();
                ChildBR.Balance();
                ChildUL.Balance();
                ChildUR.Balance();

                int count = MaxItemsPerNode - ObjectCount;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (ChildBL.ObjectCount > 0)
                        {
                            T item = ChildBL.Objects[ChildBL.ObjectCount - 1];
                            Add(item);
                            ChildBL.Remove(item);
                        }
                        else if (ChildBR.ObjectCount > 0)
                        {
                            T item = ChildBR.Objects[ChildBR.ObjectCount - 1];
                            Add(item);
                            ChildBR.Remove(item);
                        }
                        else if (ChildUL.ObjectCount > 0)
                        {
                            T item = ChildUL.Objects[ChildUL.ObjectCount - 1];
                            Add(item);
                            ChildUL.Remove(item);
                        }
                        else if (ChildUR.ObjectCount > 0)
                        {
                            T item = ChildUR.Objects[ChildUR.ObjectCount - 1];
                            Add(item);
                            ChildUR.Remove(item);
                        }
                    }
                }
            }

        }
        #endregion

        #region Aufräumen von leeren Kindknoten
        /// <summary>
        /// Entfernt Kindknoten, sollten diese leer sein.
        /// </summary>
        internal void Cleanup()
        {
            RemoveEmptyChilds();
            if (ChildBL != null)
            {
                ChildUL.Cleanup();
                ChildBL.Cleanup();
                ChildBR.Cleanup();
                ChildUR.Cleanup();
            }
        }

        /// <summary>
        /// Entfernt alle Kindknoten, sollten diese leer sein.
        /// </summary>
        protected void RemoveEmptyChilds()
        {
            int count = 0;

            if (ChildBL != null)
                count += ChildBL.Count + ChildBR.Count + ChildUL.Count + ChildUR.Count;

            if (count == 0)
            {
                ChildBL = null;
                ChildBR = null;
                ChildUL = null;
                ChildUR = null;
            }
        }
        #endregion

        #region Bereichsprüfungen
        /// <summary>
        /// Prüft, ob ein Objekt innerhalb des Rechteckes welches dieser Knotenpunkt repräsentiert liegt.
        /// </summary>
        /// <param name="Item">Objekt, welches geprüft werden soll.</param>
        /// <returns>True, wenn das Objekt vollständig in diesem Knotenpunkt liegt.</returns>
        protected bool Contains(T Item)
        {
            return Rect.Contains(Item.BoundingBox);
        }

        /// <summary>
        /// Gibt eine Liste mit allen Objekten zurück, die innerhalb eines angegeben Rechteckes liegen.
        /// </summary>
        /// <param name="Rect">Rechteck, in welchem die Knotenpunkte liegen müssen</param>
        /// <returns>Liste mit alle Objekten der Knotenpunkte, die innerhalb des Rechteckes liegen.</returns>
        internal List<T> Query(RectangleF Rect)
        {
            List<T> res = new List<T>();
            if (this.Rect.IntersectsWith(Rect))
            {
                bool contains = Rect.Contains(this.Rect);
                for (int i = 0; i < ObjectCount; i++)
                {
                    if (contains || Objects[i].BoundingBox.IntersectsWith(Rect))
                        res.Add(Objects[i]);
                }
                if (ChildBL != null)
                {
                    res.AddRange(ChildBL.Query(Rect));
                    res.AddRange(ChildBR.Query(Rect));
                    res.AddRange(ChildUL.Query(Rect));
                    res.AddRange(ChildUR.Query(Rect));
                }
            }
            return res;
        }
        #endregion

        /// <summary>
        /// Fügt einer Liste alle Rechtecke hinzu, welche diesen Knotenpunkt und alle Kindknotenpunkte umschließen.
        /// </summary>
        /// <param name="l">Liste, zu welcher die Rechtecke hinzugefügt werden sollen</param>
        public void AddRectangles(List<RectangleF> l)
        {
            l.Add(Rect);
            ChildBL?.AddRectangles(l);
            ChildBR?.AddRectangles(l);
            ChildUL?.AddRectangles(l);
            ChildUR?.AddRectangles(l);
        }

    }
}
