using System;
using System.Windows.Forms;
using Mathematics.Vector;
using Renderer.GDI;
using Mathematics.Geometry;

namespace TestLab
{

    public partial class Form1 : Form
    {
        Canvas Canvas;
        public Form1()
        {
            InitializeComponent();

            Canvas = new Canvas(pictureBox);
            Renderer.GDI.UI.Control window = new Renderer.GDI.UI.Window(new Vector2(250, 250), new Vector2(0, 0), "Fenster-Titel");
            Renderer.GDI.UI.Panel panel = new Renderer.GDI.UI.Panel(new Vector2(500, 500), new Vector2(250, 250));
            Renderer.GDI.UI.Button button = new Renderer.GDI.UI.Button(new Vector2(50, 20), new Vector2());
            panel.SetCanvas(Canvas);
            panel.Add(window);
            panel.ApplyTheme(Renderer.GDI.UI.Theme.Orange);
            window.Add(button);

            //panel.Enabled = false;

            DoubleBuffered = true;
            MouseWheel += Form1_MouseWheel;
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                Canvas.Camera.Scale *= 2;

            else if (e.Delta < 0)
                Canvas.Camera.Scale *= 0.5;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (v != null)
            //{
            //    if (e.KeyCode == Keys.W)
            //        for (int i = 0; i < v.Count; i++)
            //            v[i].Translate(new Vector2(0, r.Next(-200, -100)) * Mathematics.Matrix.Inverse(Mathematics.Matrix.Transform2DRotation(Canvas.Camera.Rotation)));
            //    if (e.KeyCode == Keys.A)
            //        for (int i = 0; i < v.Count; i++)
            //            v[i].Translate(new Vector2(r.Next(-200, -100), 0) * Mathematics.Matrix.Inverse(Mathematics.Matrix.Transform2DRotation(Canvas.Camera.Rotation)));
            //    if (e.KeyCode == Keys.S)
            //        for (int i = 0; i < v.Count; i++)
            //            v[i].Translate(new Vector2(0, r.Next(100, 200)) * Mathematics.Matrix.Inverse(Mathematics.Matrix.Transform2DRotation(Canvas.Camera.Rotation)));
            //    if (e.KeyCode == Keys.D)
            //        for (int i = 0; i < v.Count; i++)
            //            v[i].Translate(new Vector2(r.Next(100, 200), 0) * Mathematics.Matrix.Inverse(Mathematics.Matrix.Transform2DRotation(Canvas.Camera.Rotation)));
            //}

                

            if (e.Shift)
            {
                if (e.KeyCode == Keys.Left)
                    Canvas.Camera.Rotate(-Math.PI / 16);
                if (e.KeyCode == Keys.Right)
                    Canvas.Camera.Rotate(Math.PI / 16);
            }
            else
            {
                if (e.KeyCode == Keys.Up)
                    Canvas.Camera.Translate(new Vector2(0, -20 / Canvas.Camera.Scale.Y));
                if (e.KeyCode == Keys.Down)
                    Canvas.Camera.Translate(new Vector2(0, 20 / Canvas.Camera.Scale.Y));
                if (e.KeyCode == Keys.Left)
                    Canvas.Camera.Translate(new Vector2(-20 / Canvas.Camera.Scale.X, 0));
                if (e.KeyCode == Keys.Right)
                    Canvas.Camera.Translate(new Vector2(20 / Canvas.Camera.Scale.X, 0));
            }
        }
    }
}
