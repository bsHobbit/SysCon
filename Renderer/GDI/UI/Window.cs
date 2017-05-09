using System;
using System.Drawing;
using Mathematics.Vector;

namespace Renderer.GDI.UI
{
    /// <summary>
    /// Ein Fenster-Control, welches vom Benutzer verschoben werden kann.
    /// 
    /// Autor: Hendrik Rost
    /// </summary>
    public class Window : Container
    {
        #region Member
        /// <summary>
        /// Speichert den zu rendernden Text.
        /// </summary>
        RenderText RenderTitle;
        /// <summary>
        /// Timer zum verifizieren, ob es sich um einen Klick handelt oder nicht.
        /// </summary>
        DateTime MouseDownTimer;
        /// <summary>
        /// Speichert die letzte bekannte Position des Mouse-Move Events zwischen.
        /// </summary>
        Vector2 LastMousePosition;

        /// <summary>
        /// Panel, in welchem der Inhalt des Fenster hinzugefügt werden soll.
        /// </summary>
        Panel Panel;

        /// <summary>
        /// Ruft den Titel des Fensters ab, oder legt diesen fest.
        /// </summary>
        public string Title
        {
            get
            {
                return RenderTitle.Text;
            }
            set
            {
                RenderTitle.Text = value;
                SetTextLocation(eTextLocation.TopCenter, ref RenderTitle);
            }
        }

        #endregion

        #region Konstruktor
        /// <summary>
        /// Initialisiert eine neue Instanz der Window Klasse.
        /// </summary>
        /// <param name="Size">Größe des Fensters.</param>
        /// <param name="Location">Position des Fensters.</param>
        /// <param name="Color">Farbe des Fensters</param>
        public Window(Vector2 Size, Vector2 Location, string Title)
            : base(Size, Location, Color.Black)
        {
            RenderTitle = new RenderText(Title);
            SetTextLocation(eTextLocation.TopCenter, ref RenderTitle);
            LastMousePosition = new Vector2();
            AddText(RenderTitle);
            OnMouseEnter += Window_OnMouseEnter;
            OnMouseLeave += Window_OnMouseLeave;
            OnMouseMove += Window_OnMouseMove;
            OnMouseDown += Window_OnMouseDown;
            OnMouseUp += Window_OnMouseUp;
            OnThemeChanged += Window_OnThemeChanged;
            OnFocusChanged += Window_OnFocusChanged;
            OnEnabledChanged += Window_OnEnabledChanged;
            OnSizeChanged += Window_OnSizeChanged;
            ApplyTheme();
            UpdatePanel();
        }
        #endregion

        #region Änderungen die das Theme betreffen

        /// <summary>
        /// Stellt die Renderfarben und Stil-Breite anhand der Benutzereingaben korrekt ein.
        /// </summary>
        void ApplyTheme()
        {
            Color = Theme.Window.BorderColor_Idle;
            RenderWidth = IsMouseOver && Enabled ? (float)Theme.Window.RenderWidth_MouseOver : (float)Theme.Window.RenderWidth_Idle;

            if (IsMouseOver && Focused)
                Color = Theme.Window.BorderColor_MouseOver_Active;
            else if (IsMouseOver && !Focused)
                Color = Theme.Window.BorderColor_MouseOver;
            else if (!IsMouseOver && Focused)
                Color = Theme.Window.BorderColor_Active;

            if (!Enabled || !ParentEnabled)
                Color = Theme.Window.BorderColor_Disabled;
        }


        /// <summary>
        /// Control wurde deaktiviert / Aktiviert.
        /// </summary>
        /// <param name="Object">This.</param>
        private void Window_OnEnabledChanged(RenderObject Object)
        {
            ApplyTheme();
        }

        /// <summary>
        /// Das Theme hat sich geändert, alles anpassen.
        /// </summary>
        /// <param name="Control">Control, für welches sich das Tehme geäbdert hat.</param>
        /// <param name="Theme">Neues aktives Theme.</param>
        private void Window_OnThemeChanged(Control Control, Theme Theme)
        {
            ApplyTheme();
        }

        /// <summary>
        /// Fokus des Controls hat sich geändert.
        /// </summary>
        /// <param name="Control">This.</param>
        private void Window_OnFocusChanged(Control Control)
        {
            ApplyTheme();
        }
        #endregion

        #region Benutzereingaben
        /// <summary>
        /// Maus wurde losgelassen ggf. wird das Fenster nun nicht mehr vom Benutzer durch die Gegend geschoben.
        /// </summary>
        /// <param name="Object">This.</param>
        /// <param name="e">Mausparameter des Events.</param>
        private void Window_OnMouseUp(RenderObject Object, GDIMouseEventArgs e)
        {
            if (Container != null && Container.ActiveControl == this)
                Container.SetActiveControl(null);
                
            TimeSpan tmr = DateTime.Now - MouseDownTimer;
        }

        /// <summary>
        /// Eine Maustaste wurde gedrückt ggf. möchte der Benutzer das Fenster verschieben.
        /// </summary>
        /// <param name="Object">This.</param>
        /// <param name="e">Mausparameter des Events.</param>
        private void Window_OnMouseDown(RenderObject Object, GDIMouseEventArgs e)
        {
            if (Container != null && e.ButtonStates[0])
                Container.SetActiveControl(this);
            MouseDownTimer = DateTime.Now;

            if (!Focused)
                Focus();

            ApplyTheme();
        }

        /// <summary>
        /// Maus bewegt sich innerhalb des Controls, ggf. wird das Fenster verschoben.
        /// </summary>
        /// <param name="Object">This.</param>
        /// <param name="e">Mausparameter des Events.</param>
        private void Window_OnMouseMove(RenderObject Object, GDIMouseEventArgs e)
        {
            if (Container != null && Container.ActiveControl == this)
            {
                Vector2 offset = LastMousePosition - e.GlobalLocation;
                Vector2 newLocation = Location - offset;

                if (Container != null) //Verhindern, dass das Fentser ggf. aus dem Container geschoben wird.
                {
                    if ((newLocation.X - Size.X / 2) < (-Container.Size.X / 2 + 1))
                        newLocation.X = (-Container.Size.X / 2 + 1) + (Size.X / 2);
                    else if ((newLocation.X + Size.X / 2) > (Container.Size.X / 2 - 1))
                        newLocation.X = (Container.Size.X / 2 - 1) - (Size.X / 2);
                    if ((newLocation.Y - Size.Y / 2) < (-Container.Size.Y / 2 + 1))
                        newLocation.Y = (-Container.Size.Y / 2 + 1) + (Size.Y / 2);
                    else if ((newLocation.Y + Size.Y / 2) > (Container.Size.Y / 2 - 1))
                        newLocation.Y = (Container.Size.Y / 2 - 1) - (Size.Y / 2);
                }
                Location = newLocation;
            }

            LastMousePosition = e.GlobalLocation.Clone();
        }

        /// <summary>
        /// Muas verlässt das Control.
        /// </summary>
        /// <param name="Object">This.</param>
        /// <param name="e">Mausparameter für das Event.</param>
        private void Window_OnMouseLeave(RenderObject Object, GDIMouseEventArgs e)
        {
            ApplyTheme();
        }

        /// <summary>
        /// Maus wird in das Control bewegt.
        /// </summary>
        /// <param name="Object">This.</param>
        /// <param name="e">Mausparameter des Events.</param>
        private void Window_OnMouseEnter(RenderObject Object, GDIMouseEventArgs e)
        {
            ApplyTheme();
        }
        #endregion

        #region Container Overrides für das Panel

        /// <summary>
        /// Ein Control soll dem Panel hinzugefügt werden, nicht direkt dem Fenster.
        /// </summary>
        /// <param name="Control">Control, welches dem Panel hinzugefügt werden soll.</param>
        public override void Add(Control Control)
        {
            if (Panel != null)
                Panel.Add(Control);
            else
                base.Add(Control);
        }

        /// <summary>
        /// Entfernt ein Control aus dem Panel.
        /// </summary>
        /// <param name="Control">Control, welches entfernt werden soll.</param>
        public override void Remove(Control Control)
        {
            if (Panel != null)
                Panel.Remove(Control);
            else
                base.Remove(Control);
        }

        /// <summary>
        /// Entfernt alle Controls aus dem Panel.
        /// </summary>
        public override void Clear()
        {
            if (Panel != null)
                Panel.Clear();
            else
                base.Clear();
        }
        #endregion

        #region Panel innerhalb des Fensters erstellen
        /// <summary>
        /// Panel des Controls aktualisieren.
        /// </summary>
        void UpdatePanel()
        {
            Vector2 titleSize = TextSize(RenderTitle);
            Vector2 panelSize = Size - new Vector2(-2, titleSize.Y);
            Vector2 panelLocation = new Vector2(-1, titleSize.Y / 2);
            if (Panel == null)
            {
                Panel = new Panel(panelSize, panelLocation);
                base.Add(Panel);
            }
            else
            {
                Panel.UpdateSize(panelSize);
                Panel.Location = panelLocation;
            }

            Panel.ApplyTheme(Theme);
        }
        #endregion

        #region Größenänderung des Fenster
        /// <summary>
        /// Größe des Fensters hat sich geändert, das Panel muss ebenfalls angepasst werden.
        /// </summary>
        /// <param name="Control">This.</param>
        private void Window_OnSizeChanged(Control Control)
        {
            UpdatePanel();
        }
        #endregion
    }
}
