using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using SimpleLibrary; 
namespace BASEketball
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Constants
        private const int kScreenWidth = 480;       // Hard code Portrait dimension
        private const int kScreenHeight = 800;
        #endregion 

        #region Usual XNA graphics rendering
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        #endregion 

        #region Objects/images drawn on the application screen
        BounceableImage mBall;          // To demonstrate flick
        SimpleImage mMarker1, mMarker2; // The two positions of pinch-zoom
        //    for illustration only

        SelectableImage mCurrentSelected = null;        // work reference, point to the currently selected image
        // can be either: mVase, mBall, mBg

        #endregion 

        public Game1()
        {
            #region usual XNA graphics initialization 
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
            #endregion 

            
            #region force/local portrait orientation for this application
            // This will force/lock the orientation to be portrait
            //      will ignore phone orientation at run time.
            graphics.PreferredBackBufferWidth = kScreenWidth;
            graphics.PreferredBackBufferHeight = kScreenHeight;
            #endregion
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            #region Label I1: TouchPanel/Multi-touch support
            //multitouch gesture support
            //Set up the gestures that you are interested in
            //can do the following gesture types:
            /*
             *  .None: represents no gestures.
             *  .Tap: The user briefly touched a single point on the screen.
             *  .DoublTap: The user tapped the screen twice in quick succession. This is always preceded by a tap gesture at the same location.
             *  .Hold: The user touched a single point on the screen for about one second.
             *  .HorizontalDrag: The user touched the screen, and then slid horizontal.
             *  .VerticalDrag: The user touched the screen, and then slid vertical.
             *  .FreeDrag: The user touched the screen, and then slid in any direction.
             *  .Pinch: The user touched two points and then converged or diverged. Takes precidence over two finger drags.
             *  .Flick: The user performed a touch combined with a quick swipe of the screen. Flicks are positionless. 
             *          You can read the velocity by .Delta of the GestureSample.
             *  .DragComplete: Any drag gesture is completed. Will only let you know if it is complete, no other data like position is stored.
             *  .PinchComplete: Like DragComplete, just tells you if the pinch gesture is complete
             */
            TouchPanel.EnabledGestures = GestureType.DoubleTap |
                                         GestureType.Hold |
                                         GestureType.Flick |
                                         GestureType.Pinch |
                                         GestureType.FreeDrag |
                                         GestureType.PinchComplete;
            #endregion


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // use this.Content to load game content here
            #region Label L2: content loading for what will appear in the application screen
            // Label L2a: Creates SimpleImage
            mBall = new BounceableImage(Content.Load<Texture2D>("BBall"), Content.Load<SoundEffect>("bounce"), 200, 400, 100, 100);

            mMarker1 = new SimpleImage(Content.Load<Texture2D>("TouchMarker"), 0, 0, 40, 40);
            mMarker2 = new SimpleImage(Content.Load<Texture2D>("TouchMarker"), 0, 0, 40, 40);
            mMarker1.IsVisible = false;
            mMarker2.IsVisible = false;

            // Label L2b: Loads the font
            DrawFont.LoadFont(Content);
            #endregion

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
