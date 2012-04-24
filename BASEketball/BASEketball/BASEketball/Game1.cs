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

        #region Audio and effect support
        // Audio stuff...
        SoundEffect mBeep;              // Sound effect when bouncing off window boundaries
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
                                         GestureType.PinchComplete | 
                                         GestureType.Tap
                                         ;
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
            //DrawFont.LoadFont(Content);
            #endregion

            #region Label L3 content loading for audio/sound effect support
            // Loads audio effets and start background music
            mBeep = Content.Load<SoundEffect>("Beep");
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
            #region Label U1: use Touch Location to check for selection

            //obtain all of the devices TouchLocations
            TouchCollection touchCollection = TouchPanel.GetState();
            //check each of the devices TouchLocations;
            foreach (TouchLocation tl in touchCollection)
            {
                /*
                    * Utilize the touch location state.
                    *   -TouchLocationState.Invalid is when the location's position is invalid. Could be when a new
                    *   touch location attempts to get the previous location of itself.
                    *   -TouchLocationState.Moved is when the touch location position was updated or pressed in the same position.
                    *   -TouchLocationState.Pressed is when the touch location position is new.
                    *   -TouchLocationState.Released is when the location position was released.
                */
                switch (tl.State)
                {
                    case TouchLocationState.Pressed:
                        // Priotize the selction
                        // System.Diagnostics.Debug.WriteLine("Pressed: TouchID=" + tl.Id);
                        mCurrentSelected = null;
                        if (mBall.Selected(tl.Position))
                            mCurrentSelected = mBall;
                        break;

                    case TouchLocationState.Moved:
                        // with TouchID, will not "drop" the image when movement is very fast!
                        // if (null != mCurrentSelected)
                        //    mCurrentSelected.DragTo(tl.Id, tl.Position);
                        //
                        // One should not mix touch.Move with gesture. 
                        //   here we are working with Gesture, so, check for FreeDrag
                        break;

                    case TouchLocationState.Released:
                        if (null != mCurrentSelected)
                            mCurrentSelected.UnSelect();

                        mCurrentSelected = null;
                        break;
                }
            }
            #endregion

            

            #region Label U2: Working with gesture: on selected image
            while (TouchPanel.IsGestureAvailable)
            {
                //detect any gestures that were performed
                GestureSample gesture = TouchPanel.ReadGesture();
                //do code depending on which gesture was detected
                switch (gesture.GestureType)
                {
                    case GestureType.Pinch:
                        // Only work with mBg image. 
                        // Sets the markers to be visible and at pinch position
                        mMarker1.IsVisible = true;
                        mMarker2.IsVisible = true;
                        mMarker1.SetPositionTo(gesture.Position);
                        mMarker2.SetPositionTo(gesture.Position2);
                        break;
                    case GestureType.PinchComplete:
                        // now hid the markers
                        mMarker1.IsVisible = false;
                        mMarker2.IsVisible = false;
                        break;

                    case GestureType.FreeDrag:
                        // drags which the selected image 
                        if (null != mCurrentSelected)
                            mCurrentSelected.SetPositionTo(gesture.Position);
                        break;

                    case GestureType.DoubleTap:
                        break;

                    case GestureType.Tap:
                        // Always reposition the ball
                        mBall.SetPositionTo(gesture.Position);
                        mBall.SetVelocity(Vector2.Zero);
                        mBeep.Play();
                        break;

                    case GestureType.Hold:
                        // Always reposition the ball
                        mBall.SetPositionTo(gesture.Position);
                        mBall.SetVelocity(Vector2.Zero);
                        mBeep.Play();
                        break;

                    case GestureType.Flick:
                        // Transfer the flick-delta to the ball, 
                        //         scale down the veocity to have 
                        //         a resonable speed for the ball
                        if (mBall == mCurrentSelected)
                            mBall.SetVelocity(gesture.Delta * 0.2f);
                        break;
                }
            }
            #endregion

            #region Label U3: In case the ball is kicked
            mBall.Update();
            #endregion

            #region Label U4: Usual XNA stuff: Allows the game to exit and base.Update
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
            #endregion 

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
            #region Label R1: Draw all objects and display the font.
            GraphicsDevice.Clear(Color.Crimson);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            mBall.Draw(spriteBatch);

            mMarker1.Draw(spriteBatch);  // may or may not draw anything
            mMarker2.Draw(spriteBatch);
            spriteBatch.End();
            #endregion 

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
