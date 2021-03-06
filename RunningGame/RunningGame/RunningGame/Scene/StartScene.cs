using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace RunningGame.Scene
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class StartScene : DrawableGameComponent
    {
        private Texture2D startTexture1;
        private Texture2D startTexture2;
        private Texture2D startTexture3;
        private Texture2D startTexture4;
        private Texture2D startTexture5;
        private double countTime;
        private double countTimeUpdate;
        private Vector2[] startPosition = new Vector2[] {
            new Vector2(800,0),
            new Vector2(800,0),
            new Vector2(800,0),
            new Vector2(800,0),
            new Vector2(800,0)
        };
        private Vector2[] veloforStart = new Vector2[] {
            new Vector2(-5.5f,0),
            new Vector2(-5.5f,0),
            new Vector2(-5.5f,0),
            new Vector2(-5.5f,0),
            new Vector2(-5.5f,0),
        };

        Game gameObject;
        public StartScene(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            this.gameObject = game;
        }

        SpriteBatch spriteBatch;
        enum BState
        {
            HOVER,
            UP,
            JUST_RELEASED,
            DOWN
        }
        const int NUMBER_OF_BUTTONS = 1,
            START_BUTTON_INDEX = 0,
            BUTTON_HEIGHT = 40,
            BUTTON_WIDTH = 88;


        Color background_color;
        Color[] button_color = new Color[NUMBER_OF_BUTTONS];
        Rectangle[] button_rectangle = new Rectangle[NUMBER_OF_BUTTONS];
        BState[] button_state = new BState[NUMBER_OF_BUTTONS];
        Texture2D[] button_texture = new Texture2D[NUMBER_OF_BUTTONS];
        double[] button_timer = new double[NUMBER_OF_BUTTONS];
        //mouse pressed and mouse just pressed
        bool mpressed, prev_mpressed = false;
        //mouse location in window
        int mx, my;
        double frame_time;
        // for simulating button clicks with keyboard
        KeyboardState keyboard_state, last_keyboard_state;


        // wrapper for hit_image_alpha taking Rectangle and Texture
        Boolean hit_image_alpha(Rectangle rect, Texture2D tex, int x, int y)
        {
            return hit_image_alpha(0, 0, tex, tex.Width * (x - rect.X) /
                rect.Width, tex.Height * (y - rect.Y) / rect.Height);
        }

        // wraps hit_image then determines if hit a transparent part of image 
        Boolean hit_image_alpha(float tx, float ty, Texture2D tex, int x, int y)
        {
            if (hit_image(tx, ty, tex, x, y))
            {
                uint[] data = new uint[tex.Width * tex.Height];
                tex.GetData<uint>(data);
                if ((x - (int)tx) + (y - (int)ty) *
                    tex.Width < tex.Width * tex.Height)
                {
                    return ((data[
                        (x - (int)tx) + (y - (int)ty) * tex.Width
                        ] &
                                0xFF000000) >> 24) > 20;
                }
            }
            return false;
        }

        // determine if x,y is within rectangle formed by texture located at tx,ty
        Boolean hit_image(float tx, float ty, Texture2D tex, int x, int y)
        {
            return (x >= tx &&
                x <= tx + tex.Width &&
                y >= ty &&
                y <= ty + tex.Height);
        }

        // determine state and color of button
        void update_buttons()
        {
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {

                if (hit_image_alpha(
                    button_rectangle[i], button_texture[i], mx, my))
                {
                    button_timer[i] = 0.0;
                    if (mpressed)
                    {
                        // mouse is currently down
                        button_state[i] = BState.DOWN;
                        button_color[i] = Color.Blue;
                    }
                    else if (!mpressed && prev_mpressed)
                    {
                        // mouse was just released
                        if (button_state[i] == BState.DOWN)
                        {
                            // button i was just down
                            button_state[i] = BState.JUST_RELEASED;
                        }
                    }
                    else
                    {
                        button_state[i] = BState.HOVER;
                        button_color[i] = Color.LightBlue;
                    }
                }
                else
                {
                    button_state[i] = BState.UP;
                    if (button_timer[i] > 0)
                    {
                        button_timer[i] = button_timer[i] - frame_time;
                    }
                    else
                    {
                        button_color[i] = Color.White;
                    }
                }

                if (button_state[i] == BState.JUST_RELEASED)
                {
                    take_action_on_button(i);
                }
            }
        }


        // Logic for each button click goes here
        void take_action_on_button(int i)
        {
            ((Game1)gameObject).PlaySoundInstance(RunningGame.Game1.SoundInstance.buttonClick);
            //take action corresponding to which button was clicked
            switch (i)
            {
                case START_BUTTON_INDEX:
                    {
                        //start the main component
                        ((Game1)gameObject).ChangeComponets(0);
                        ((Game1)gameObject).PlayBgMusicByIndex(0);
                    }
                    break;
                default:
                    break;
            }
        }

        // Logic for each key down event goes here
        void handle_keyboard()
        {
            last_keyboard_state = keyboard_state;
            keyboard_state = Keyboard.GetState();
            Keys[] keymap = (Keys[])keyboard_state.GetPressedKeys();
            foreach (Keys k in keymap)
            {

                char key = k.ToString()[0];
                switch (key)
                {
                    case 'b':
                    case 'E':
                        take_action_on_button(START_BUTTON_INDEX);
                        button_color[START_BUTTON_INDEX] = Color.Orange;
                        button_timer[START_BUTTON_INDEX] = 0.25;
                        break;
                    default:
                        break;
                }

            }
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Game.Exit();

            countTimeUpdate += gameTime.ElapsedGameTime.TotalMilliseconds;

            // get elapsed frame time in seconds
            frame_time = gameTime.ElapsedGameTime.Milliseconds / 1000.0;

            // update mouse variables
            MouseState mouse_state = Mouse.GetState();
            mx = mouse_state.X;
            my = mouse_state.Y;
            prev_mpressed = mpressed;
            mpressed = mouse_state.LeftButton == ButtonState.Pressed;

            update_buttons();
            handle_keyboard();

            if (countTimeUpdate <= 4000)
            {
                startPosition[0] = startPosition[0] + veloforStart[0];
            }
            else if (countTimeUpdate <= 8000)
            {
                startPosition[1] = startPosition[1] + veloforStart[1];
            }
            else if (countTimeUpdate <= 12000)
            {
                startPosition[2] = startPosition[2] + veloforStart[2];
            }
            else if (countTimeUpdate <= 16000)
            {
                startPosition[3] = startPosition[3] + veloforStart[3];
            }
            else if (countTimeUpdate <= 45000)
            {
                if(startPosition[4].X >= 50)
                startPosition[4] = startPosition[4] + veloforStart[4];
            }
            else
            {
                //do nothing
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            countTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            
            

            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

          //  spriteBatch.Draw(backgroundTexture, new Vector2(), Color.White);

            if (countTime <= 4000)
            {
                spriteBatch.Draw(startTexture1, startPosition[0], Color.White);
            }
            else if (countTime <= 8000)
            {
                spriteBatch.Draw(startTexture2, startPosition[1], Color.White);
            }
            else if (countTime <= 12000)
            {
                spriteBatch.Draw(startTexture3, startPosition[2], Color.White);
            }
            else if (countTime <= 16000)
            {
                spriteBatch.Draw(startTexture4, startPosition[3], Color.White);
            }
            else if (countTime <= 45000)
            {
                spriteBatch.Draw(startTexture5, startPosition[4], Color.White);
            }
            else
            {
                //do nothing
            }

            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {
                spriteBatch.Draw(button_texture[i], button_rectangle[i], button_color[i]);
            }



            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            // TODO: Add your initialization logic here
            // starting x and y locations to stack buttons 
            // vertically in the middle of the screen
            int x = Game.Window.ClientBounds.Width / 2 - BUTTON_WIDTH / 2;
            int y = Game.Window.ClientBounds.Height / 2 -
                NUMBER_OF_BUTTONS / 2 * BUTTON_HEIGHT -
                (NUMBER_OF_BUTTONS % 2) * BUTTON_HEIGHT / 2;
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {
                button_state[i] = BState.UP;
                button_color[i] = Color.White;
                button_timer[i] = 0.0;
                button_rectangle[i] = new Rectangle(30, 30, BUTTON_WIDTH, BUTTON_HEIGHT);
                y += BUTTON_HEIGHT;
            }
            Game.IsMouseVisible = true;
            background_color = Color.CornflowerBlue;
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

            button_texture[START_BUTTON_INDEX] =
                Game.Content.Load<Texture2D>(@"Sprites/back");

            startTexture1 = Game.Content.Load<Texture2D>(@"Start/photo1");
            startTexture2 = Game.Content.Load<Texture2D>(@"Start/photo2");
            startTexture3 = Game.Content.Load<Texture2D>(@"Start/photo3");
            startTexture4 = Game.Content.Load<Texture2D>(@"Start/photo4");
            startTexture5 = Game.Content.Load<Texture2D>(@"Start/photo5");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
    }
}
