using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RTSgame.Utilities.IO.UI;
using RTSgame.GameObjects;

namespace RTSgame.Utilities.IO
{
    /// <summary>
    /// The user interface for a Player, GUI: Game User Interface
    /// </summary>
    class GameUserInterface
    {
        private Player associatedPlayer;
        private List<UIComponent> components = new List<UIComponent>();
        private SpriteFont defaultFont = AssetBank.GetInstance().GetFont("gamefont");
        private Camera camera;
        private Viewport viewport;

        //References to components that change
        private ImageComponent buildMenu;
        private ImageComponent commandMenu;
        private InfoText info;
        private Floater tracker;
        /// <summary>
        /// Create new user interface
        /// </summary>
        /// <param name="player">The player whose interface this is</param>
        /// <param name="viewport">The viewport where this interface is drawn</param>
        /// <param name="camera">The camera used by the viewport</param>
        public GameUserInterface(Player player, Viewport viewport, Camera camera)
        {
            this.viewport = viewport;
            this.camera = camera;
            associatedPlayer = player;
            components.Add(new MetalCounter(new Vector2(5, 0), associatedPlayer, defaultFont, Color.Blue));
            components.Add(new PlayerHealthBar(new Vector2(20, 400), "UIBar", Color.Red, UIBar.Direction.Right, false, associatedPlayer, new Vector2(100,100)));
            components.Add(new EnergyBar(new Vector2(30, 500), "UIBar", Color.Blue, UIBar.Direction.Right, false, associatedPlayer, new Vector2(300,10)));

            buildMenu = new ImageComponent(new Vector2(40, 60), "UIBuildMenu2", Color.White);
            buildMenu.SetVisible(false);
            components.Add(buildMenu);
            
            commandMenu = new ImageComponent(new Vector2(40, 60), "UICommandMenu2", Color.White);
            commandMenu.SetVisible(true);
            components.Add(commandMenu);

            info = new InfoText(new Vector2(0, 0), defaultFont, Color.Yellow);
            components.Add(info);

            ImageComponent imgc = new ImageComponent(Vector2.Zero, "UIMarker", Color.Yellow);
            imgc.Position = new Vector2(-imgc.GetWidth()/2, -imgc.GetHeight());
            tracker = new Floater(imgc, this, false);
            tracker.Offset3d = new Vector3(0, 2.5f, 0);
            components.Add(tracker);

            associatedPlayer.SetUI(this);
        }
        public void AddUIComponent(UIComponent uicomp)
        {
            components.Add(uicomp);
        }
        /// <summary>
        /// Draws all components in the UI
        /// </summary>
        /// <param name="spriteBatch">Which spritebatch to use</param>
        /// <param name="offset">Offset the drawing by this vector</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            foreach (UIComponent component in components)
            {

                component.Draw(spriteBatch, offset);
                
            }

        }


        /// <summary>
        /// Checks if build menu is open
        /// </summary>
        /// <returns></returns>
        internal bool BuildMenuOpen()
        {
            return buildMenu.IsVisible();
        }
        /// <summary>
        /// Sets if build menu is open
        /// </summary>
        /// <param name="value"></param>
        public void SetBuildMenuOpen(Boolean value)
        {
            buildMenu.SetVisible(value);
            commandMenu.SetVisible(!value);
        }
        /// <summary>
        /// Projects a point in 3D space to the viewport coordinates used by this UI using the camera associated with this UI
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public Vector2 Project(Vector3 vector)
        {

            Vector3 result = viewport.Project(vector, DrawManager.GetInstance().GetCurrentProjection(), camera.GetViewMatrix(), Matrix.Identity);
           // Console.WriteLine(result.Z);
            return new Vector2(result.X, result.Y) - new Vector2(viewport.X, viewport.Y);
        }
        /// <summary>
        /// Inform player he does not have enough metal
        /// </summary>
        /// <param name="currentMetal"></param>
        /// <param name="amountNeeded"></param>
        internal void SignalNotEnoughMetal(int currentMetal, int amountNeeded)
        {
            info.ShowMessage("NOT ENOUGH METAL: NEED " + (amountNeeded - currentMetal) + " MORE!", 1000);
            SoundPlayer.PlaySound("deny2");
        }


        /// <summary>
        /// Set which object to highlight
        /// </summary>
        /// <param name="highLightedObject"></param>
        internal void SetHighLight(IUITrackable highLightedObject)
        {
            if (tracker != null)
            {
                tracker.SetVisible(true);
                tracker.TrackedObject = highLightedObject;

            }
        }
        /// <summary>
        /// Clear any highlighting
        /// </summary>
        internal void ClearHighLight()
        {
            if (tracker != null)
            {
                tracker.SetVisible(false);
                tracker.TrackedObject = null;
            }
                
        }

        internal void Update(GameTime gameTime)
        {
            foreach (UIComponent component in components)
            {
                component.Update(gameTime);
                /*if (component is UIBar)
                {
                    Console.WriteLine(component);
                }*/
            }
        }
        public float GetHeight()
        {
            return viewport.Height;
        }
        public float GetWidth()
        {
            return viewport.Width;
        }
    }
}
