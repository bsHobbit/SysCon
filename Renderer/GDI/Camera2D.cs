using Mathematics;
using Mathematics.Vector;

namespace Renderer.GDI
{
    /// <summary>
    /// Kamera, die für das Rendern in einer GDI Umgebung verwendet werden kann.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    public class Camera2D
    {
        #region Events
        /// <summary>
        /// Wird ausgelöst, sobald sich die ViewTransformation Matrix dieser Kamera ändert.
        /// </summary>
        public event Matrix.MatrixEventHandler ViewTransformationChanged;
        #endregion

        #region Member
        /// <summary>
        /// Speichert die Größe, auf welcher gerendert wird.
        /// </summary>
        Vector2 screenSize;
        /// <summary>
        /// Ruft die Größe, auf welcher gerendert werden soll ab, oder legt diese fest.
        /// </summary>
        public Vector2 ScreenSize { get { return screenSize; } set { screenSize = value; UpdateViewTransformMatrix(); } }
        /// <summary>
        /// Speichert das Zentrum der Kamera.
        /// </summary>
        Vector2 lookAt;
        /// <summary>
        /// Ruft das Zentrum der Kamera ab, oder legt dieses Fest.
        /// </summary>
        public Vector2 LookAt { get { return lookAt; } set { lookAt = value; UpdateViewTransformMatrix(); } }
        /// <summary>
        /// Speichert die Skalierung des Sichtbereichs der Kamera
        /// </summary>
        Vector2 scale;
        /// <summary>
        /// Ruft den Sichtbereich der Kamera ab, oder legt diesen fest.
        /// </summary>
        public Vector2 Scale { get { return scale; } set { scale = value; UpdateViewTransformMatrix(); } }
        /// <summary>
        /// Speichert die Rotation der Kamera.
        /// </summary>
        double rotation;
        /// <summary>
        /// Ruft die Rotation der Kamera ab, oder legt diese fest.
        /// </summary>
        public double Rotation { get { return rotation; } set { rotation = value; UpdateViewTransformMatrix(); } }
        /// <summary>
        /// Ruft die Transformationsmatrix, die durch die Kamera definiert wird ab.
        /// </summary>
        public Matrix ViewTransformMatrix { get; private set; }
        /// <summary>
        /// Ruft die Invertierte Transformationsmatrix, die durch die Kamera definiert wird ab.
        /// </summary>
        public Matrix InverseViewTransformMatrix { get; private set; }
        /// <summary>
        /// Rechteck, welches von der Kamera erfasst wird.
        /// </summary>
        Vector2[] viewRect;
        /// <summary>
        /// Ruft die Eckpunkte des Sichtbereiches der Kamera ab.
        /// </summary>
        public Vector2[] ViewRect { get { return viewRect; } }

        #endregion

        #region Konstruktoren
        /// <summary>
        /// Initialisiert eine neue Instanz der Camera2D Klasse.
        /// </summary>
        /// <param name="ScreenSize">Größe, auf welcher gerendert werden soll.</param>
        public Camera2D(Vector2 ScreenSize) : this(ScreenSize, new Vector2(), new Vector2(1, 1), 0) {  }
        /// <summary>
        /// Initialisiert eine neue Instanz der Camera2D Klasse.
        /// </summary>
        /// <param name="ScreenSize">Größe, auf welcher gerendert werden soll.</param>
        /// <param name="LookAt">Mittelpunkt der Kamera.</param>
        /// <param name="Scale">Sichtfeld der Kamera.</param>
        /// <param name="Rotation">Rotation der Kamera.</param>
        public Camera2D(Vector2 ScreenSize, Vector2 LookAt, Vector2 Scale, double Rotation)
        {
            screenSize = ScreenSize;
            lookAt = LookAt.Clone();
            scale = Scale.Clone();
            rotation = Rotation;
            UpdateViewTransformMatrix();
        }
        #endregion

        #region Transformationen
        /// <summary>
        /// Rotiert die Kamera um einen definierten Winkel weiter.
        /// </summary>
        /// <param name="Angle">Winkel, um welchen die Kamera weiter rotiert werden soll.</param>
        public void Rotate(double Angle) { Rotation += Angle; }
        /// <summary>
        /// Verschiebt die Kamera um einen Offset.
        /// </summary>
        /// <param name="Offset">Offset, um welchen die Kamera verschoben werden soll.</param>
        public void Translate(Vector2 Offset)
        {
            Vector2 vOffset = Offset * Matrix.Inverse(Matrix.Transform2DRotation(rotation));
            LookAt += vOffset;
        }
        #endregion

        #region ViewTransformation Matrix
        /// <summary>
        /// Berechnet die ViewTransformation-Matrix anhand der Kameraparameter.
        /// Löst ggf. das ViewTransformationChanged Event aus.
        /// </summary>
        void UpdateViewTransformMatrix()
        {

            ViewTransformMatrix = Matrix.Transform2DTranslate(screenSize.X / 2, screenSize.Y / 2) *
                                  Matrix.Transform2DRotation(rotation) *
                                  Matrix.Transform2DTranslate(-lookAt.X * scale.X, -lookAt.Y * scale.Y) *
                                  Matrix.Transform2DScale(scale.X, scale.Y);
            InverseViewTransformMatrix = ViewTransformMatrix.Inverse();

            Matrix m = Matrix.Transform2DRotation(rotation) * 
                       Matrix.Transform2DTranslate((-lookAt.X * scale.X), (-lookAt.Y * scale.Y)) *
                       Matrix.Transform2DScale(scale.X, scale.Y);

            viewRect = Matrix.Transform(RenderObject.CreateRectangle(screenSize.X, screenSize.Y), m.Inverse());

            ViewTransformationChanged?.Invoke(ViewTransformMatrix);
        }
        #endregion
    }
}
