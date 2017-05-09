namespace DataStructures.QuadTree
{

    /// <summary>
    /// Für Events, welche als Parameter ein IQuadTreeObject übergeben.
    /// </summary>
    /// <param name="Object">IQuadTree Objekt, welches das Event ausgelöst hat.</param>
    public delegate void IQuadTreeObjectEventHandler(IQuadTreeObject Object);
    /// <summary>
    /// Interface, welches implementiert werden muss, wenn eine Klasse/Struktur in einem QuadTree gespeichert werden soll.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    public interface IQuadTreeObject
    {
        /// <summary>
        /// Umschließendes Rechteck für das Objekt
        /// </summary>
        System.Drawing.RectangleF BoundingBox { get; }

        /// <summary>
        /// Wird aufgerufen, wenn sich die Boundingbox eines Objekts ändert.
        /// </summary>
        event IQuadTreeObjectEventHandler OnBoundingBoxChanged;

        /// <summary>
        /// Wird aufgerufen, wenn sich der Knotenpunkt des Objects ändert.
        /// </summary>
        event IQuadTreeObjectEventHandler OnNodeChanged;

        /// <summary>
        /// Knotenpunkt des Objektes hat sich geändert.
        /// </summary>
        void NodeChanged();

    }
}
