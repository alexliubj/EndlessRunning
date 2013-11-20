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
using RunningGame.Utils;
using RunningGame.Model;
using RunningGame.Role;
using RunningGame.Map;
using System.IO;

namespace RunningGame.Scene
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GamingScene : DrawableGameComponent
    {

         Game gameObject;
         public GamingScene(Game game)
            : base(game)
        {
            this.gameObject = game;
            // TODO: Construct any child components here
        }

        //**************SPRITEFONT*************
        SpriteFont distanceFont;
        Vector2 distanceFontPostion;
        int score = 0;
        int distance = 0;
        SpriteFont scoreFont;
        Vector2 scoreFontPosition;
        //**************SPRITEFONT*************
        SpriteBatch spriteBatch;
        Texture2D springbg1;
        Texture2D springbg2;
        Texture2D springbg3;
        Texture2D springbg4;
        Texture2D springbg5;
        Texture2D spring1;
        Texture2D spring2;
        Texture2D alaways;
        Runner aRunner;
        MapManager map1;

        XMLParseUtils aTestParse = new XMLParseUtils();
        TextureObject springbgObj1;
        TextureObject springbgObj2;
        TextureObject alwaysObject;
        KeyboardState oldKeyState;
        float offsetX1 = -10, speed = -0.5f;
        float offsetX2 = 0, offsetX3 = 0, offsetX4 = 0, offsetX5 = 0;
        float offsetY1 = 380, offsetY2 = 0, offsetY3 = 0, offsetY4 = 0, offsetY5 = 250;

        Vector2 position1 = new Vector2();
        Vector2 position2 = new Vector2(300, 0);
        Vector2 position4 = new Vector2();
        Vector2 position3 = new Vector2();
        Vector2 position5 = new Vector2(500, 0);

        //************************ front bg elements******************************
        float frontbgSpeed = -0.4f;
        float offsetFrontbgY1 = 510, offsetFrontbgY2, offsetFrontbgY3;
        float offsetFrontbgX1, offsetFrontbgX2, offsetFrontbgX3;
        Vector2 frontbgPos1 = new Vector2();
        Vector2 frontbgPos2 = new Vector2();
        Vector2 frontbgPos3 = new Vector2();
        Rectangle frontbgSourceRect1 = new Rectangle();
        Rectangle frontbgSourceRect2 = new Rectangle();
        Rectangle frontbgSourceRect3 = new Rectangle();
        int bgFrontWidth1, bgFrontWidth2, bgFrontWidth3;
        //**********************************Road*************************************
        float roadSpeed = -0.6f;
        float offsetRoadY1 = 0, offsetRoadY2 = 0;
        float roadMidOffset1, roadLeftOffset =400, roadMidOffset2 = 450, roadMidOffset3 = 400, roadRightOffset, roadSlabOffset;
        Vector2 roadLefPos = new Vector2();
        Vector2 roadRightPos = new Vector2();
        Vector2 roadMidPos1 = new Vector2(10,0);
        Vector2 roadMidPos2 = new Vector2(100,0);
        Vector2 roadMidPos3 = new Vector2(180,0);
        Vector2 roadSlabPos = new Vector2();
        Rectangle roadMidRect1 = new Rectangle();
        Rectangle roadMidRect2 = new Rectangle();
        Rectangle roadMidRect3 = new Rectangle();
        Rectangle roadRightRect = new Rectangle();
        Rectangle roadLeftRect = new Rectangle();
        Rectangle roadSlabRect = new Rectangle();

        int roadLeftWidth, roadRightWidth, roadMidWidth1, roadMidWidth2, roadMidWidth3, roadSlabWidth;
        //*************************************************************************

      
        int bgWidth1,bgWidth2,bgWidth3,bgWidth4,bgWidth5,screenWidth;

        //**********************************pause and back buttons *****************************

        enum BState
        {
            HOVER,
            UP,
            JUST_RELEASED,
            DOWN
        }
        const int NUMBER_OF_BUTTONS = 2,
            PAUSEBUTTON_INDEX = 0,
            BACK_BUTTON_INDEX = 1,
            PAUSEDBUTTON_INDEX=2,
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
            //take action corresponding to which button was clicked
            switch (i)
            {
                case PAUSEBUTTON_INDEX:
                    {
                        //
                        Program.GameStatus = !Program.GameStatus;
                    }
                    break;
                case BACK_BUTTON_INDEX:
                    {
                        //start the main component
                        ((Game1)gameObject).ChangeComponets(0);
                    }

                    break;
                default:
                    break;
            }
        }
        void load_button_content()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            button_texture[PAUSEBUTTON_INDEX] =
                Game.Content.Load<Texture2D>(@"Sprites/easy");
            button_texture[BACK_BUTTON_INDEX] =
                Game.Content.Load<Texture2D>(@"Sprites/medium");
           
        }
        void initialize_buttons()
        {
            int x = Game.Window.ClientBounds.Width - BUTTON_WIDTH * NUMBER_OF_BUTTONS;
            int y = 20;
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {
                button_state[i] = BState.UP;
                button_color[i] = Color.White;
                button_timer[i] = 0.0;
                button_rectangle[i] = new Rectangle(x, y, BUTTON_WIDTH, BUTTON_HEIGHT);
                x += BUTTON_WIDTH;
            }
            Game.IsMouseVisible = true;
            background_color = Color.CornflowerBlue;
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
                    case 'p':
                    case 'P':
                        take_action_on_button(PAUSEBUTTON_INDEX);
                        button_color[PAUSEBUTTON_INDEX] = Color.Orange;
                        button_timer[PAUSEBUTTON_INDEX] = 0.25;
                        break;
                    case 'b':
                    case 'B':
                        take_action_on_button(BACK_BUTTON_INDEX);
                        button_color[BACK_BUTTON_INDEX] = Color.Orange;
                        button_timer[BACK_BUTTON_INDEX] = 0.25;
                        break;
                    default:
                        break;
                }

            }
        }
        void draw_buttons()
        {
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {
                spriteBatch.Draw(button_texture[i], button_rectangle[i], button_color[i]);
            }
        }
        void update_button(GameTime gt)
        {
            // get elapsed frame time in seconds
            frame_time = gt.ElapsedGameTime.Milliseconds / 1000.0;

            // update mouse variables
            MouseState mouse_state = Mouse.GetState();
            mx = mouse_state.X;
            my = mouse_state.Y;
            prev_mpressed = mpressed;
            mpressed = mouse_state.LeftButton == ButtonState.Pressed;

            update_buttons();
            handle_keyboard();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            //start beyond left of screen and keep drawing until 
            //the right is beyond the right of the screen


                distance = (int)gameTime.TotalGameTime.Seconds * 100;
                //*****************************************************
                float currentLeft1 = position1.X;
                while (currentLeft1 < screenWidth)
                {
                    spriteBatch.Draw(springbg1, new Vector2(currentLeft1, offsetY1), Color.White);
                    currentLeft1 += bgWidth1;
                }

                float currentLeft5 = position5.X;
                while (currentLeft5 < screenWidth)
                {
                    spriteBatch.Draw(springbg5, new Vector2(currentLeft5, offsetY5), Color.White);
                    currentLeft5 += bgWidth2;
                }


                //********************************front bg *********************

                float currentLeftfront = frontbgPos1.X;
                while (currentLeftfront < screenWidth)
                {
                    spriteBatch.Draw(spring1, new Vector2(currentLeftfront, offsetFrontbgY1), frontbgSourceRect1, Color.White);
                    currentLeftfront += bgFrontWidth1;
                }

                //**********************************road sprites*******************


                float currentmid1 = roadMidPos1.X;
                while (currentmid1 < screenWidth)
                {
                    spriteBatch.Draw(spring1, new Vector2(currentmid1, roadMidOffset1), roadMidRect1, Color.White);
                    currentmid1 += roadMidWidth1;
                }

                float currentmid2 = roadMidPos2.X;
                while (currentmid2 < screenWidth)
                {
                    spriteBatch.Draw(spring2, new Vector2(currentmid2, roadMidOffset2), roadMidRect2, Color.White);
                    currentmid2 += roadMidWidth2;
                }

                float currentmid3 = roadMidPos3.X;
                while (currentmid3 < screenWidth)
                {
                    spriteBatch.Draw(spring1, new Vector2(currentmid3, roadMidOffset3), roadMidRect3, Color.White);
                    currentmid3 += roadMidWidth3;
                }

                map1.Draw(spriteBatch);
                //****************************Fonts********************
                string output = "Distance:" + distance;
                Vector2 FontOrigin = distanceFont.MeasureString(output) / 2;
                spriteBatch.DrawString(distanceFont, output, distanceFontPostion, Color.Black,
                    0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.DrawString(distanceFont, output, new Vector2(distanceFontPostion.X + 1, distanceFontPostion.Y + 1), Color.White,
                    0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f); //shadow
                output = "Score:" + score;
                FontOrigin = scoreFont.MeasureString(output) / 2;
                spriteBatch.DrawString(scoreFont, output, scoreFontPosition, Color.Black,
                    0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.DrawString(scoreFont, output, new Vector2(scoreFontPosition.X + 1, scoreFontPosition.Y + 1), Color.White,
                    0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                //***************************ROLE**********************

                aRunner.Draw(spriteBatch);

                //***************************ROLE**********************

                draw_buttons();
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
        /// <summary>
        /// Load content
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            springbg1 = Game.Content.Load<Texture2D>(@"Sprites\spring_bg1_1");
            springbg2 = Game.Content.Load<Texture2D>(@"Sprites\spring_bg1_2");
            springbg3 = Game.Content.Load<Texture2D>(@"Sprites\spring_bg1_3");
            springbg4 = Game.Content.Load<Texture2D>(@"Sprites\spring_bg1_4");
            springbg5 = Game.Content.Load<Texture2D>(@"Sprites\spring_bg1_5");

            spring1 = Game.Content.Load<Texture2D>(@"Sprites\spring_p1");
            spring2 = Game.Content.Load<Texture2D>(@"Sprites\spring_p2");
            alaways = Game.Content.Load<Texture2D>(@"Sprites\always");
            bgWidth1 = springbg1.Width;
            bgWidth2 = springbg2.Width;
            bgWidth3 = springbg3.Width;
            bgWidth4 = springbg4.Width;
            bgWidth5 = springbg5.Width;


            screenWidth = Game.GraphicsDevice.Viewport.Bounds.Width;
            springbgObj1 = aTestParse.parseXML(@"config/spring_p1.xml");
            springbgObj2 = aTestParse.parseXML(@"config/spring_p2.xml");
            alwaysObject = aTestParse.parseXML(@"config/always.xml");

            FrameObjects rectangleObj = springbgObj1.getFrameObjectByName(@"spring_bg3_1.png");
            frontbgSourceRect1 = new Rectangle(rectangleObj.X, rectangleObj.Y, rectangleObj.Width, rectangleObj.Height);
            bgFrontWidth1 = rectangleObj.Width;
            rectangleObj = springbgObj1.getFrameObjectByName(@"spring_bg3_2.png");
            frontbgSourceRect2 = new Rectangle(rectangleObj.X, rectangleObj.Y, rectangleObj.Width, rectangleObj.Height);
            bgFrontWidth2 = rectangleObj.Width;
            rectangleObj = springbgObj1.getFrameObjectByName(@"spring_bg3_3.png");
            frontbgSourceRect3 = new Rectangle(rectangleObj.X, rectangleObj.Y, rectangleObj.Width, rectangleObj.Height);
            bgFrontWidth3  = rectangleObj.Width;

            //**************************************road****************************

            rectangleObj = springbgObj1.getFrameObjectByName(@"spring_road_left.png");
            roadLeftRect = new Rectangle(rectangleObj.X, rectangleObj.Y, rectangleObj.Width, rectangleObj.Height);
            roadLeftWidth = rectangleObj.Width;

            rectangleObj = springbgObj1.getFrameObjectByName(@"spring_road_mid1.png");
            roadMidRect1 = new Rectangle(rectangleObj.X, rectangleObj.Y, rectangleObj.Width, rectangleObj.Height);
            roadMidWidth1 = rectangleObj.Width;

            rectangleObj = springbgObj2.getFrameObjectByName(@"spring_road_mid2.png");
            roadMidRect2 = new Rectangle(rectangleObj.X, rectangleObj.Y, rectangleObj.Width, rectangleObj.Height);
            roadMidWidth2 = rectangleObj.Width;

            rectangleObj = springbgObj1.getFrameObjectByName(@"spring_road_mid3.png");
            roadMidRect3 = new Rectangle(rectangleObj.X, rectangleObj.Y, rectangleObj.Width, rectangleObj.Height);
            roadMidWidth3 = rectangleObj.Width;

            rectangleObj = springbgObj1.getFrameObjectByName(@"spring_road_right.png");
            roadRightRect = new Rectangle(rectangleObj.X, rectangleObj.Y, rectangleObj.Width, rectangleObj.Height);
            roadRightWidth = rectangleObj.Width;

            rectangleObj = springbgObj1.getFrameObjectByName(@"spring_road_slab.png");
            roadSlabRect = new Rectangle(rectangleObj.X, rectangleObj.Y, rectangleObj.Width, rectangleObj.Height);
            roadSlabWidth = rectangleObj.Width;

            Rectangle[] tempRunRectArray = new Rectangle[4];
            Rectangle tempJumpRectangel = new Rectangle();
            rectangleObj = alwaysObject.getFrameObjectByName(@"blue_boy1_run01.png");
            tempRunRectArray[0] = new Rectangle(rectangleObj.X, rectangleObj.Y, rectangleObj.Width, rectangleObj.Height);
            rectangleObj = alwaysObject.getFrameObjectByName(@"blue_boy1_run02.png");
            tempRunRectArray[1] = new Rectangle(rectangleObj.X, rectangleObj.Y, rectangleObj.Width, rectangleObj.Height);
            rectangleObj = alwaysObject.getFrameObjectByName(@"blue_boy1_run03.png");
            tempRunRectArray[2] = new Rectangle(rectangleObj.X, rectangleObj.Y, rectangleObj.Width, rectangleObj.Height);
            rectangleObj = alwaysObject.getFrameObjectByName(@"blue_boy1_run04.png");
            tempRunRectArray[3] = new Rectangle(rectangleObj.X, rectangleObj.Y, rectangleObj.Width, rectangleObj.Height);
            rectangleObj = alwaysObject.getFrameObjectByName(@"blue_boy1_jump.png");
            tempJumpRectangel = new Rectangle(rectangleObj.X, rectangleObj.Y, rectangleObj.Width, rectangleObj.Height);
            aRunner = new Runner(alaways, tempRunRectArray, tempJumpRectangel);
            aRunner.status = Runner.RoleStatus.running;

            //********************************Load coins*************************************
            map1 = new MapManager(Game.Content, Path.Combine(Game.Content.RootDirectory, @"Maps\m01.txt"), @"Sprites\coin_single", new Vector2(32, 32), '-');
            map1.AddRegion('a', new Rectangle(0, 0, 32, 32));
            map1.AddRegion('b', new Rectangle(0, 0, 32, 32));
          //  map1.AddBackground("grungysky");
            //*******************************************************************************

            //********************************Sprite Font************************************

            distanceFont = Game.Content.Load<SpriteFont>(@"Font\SpriteFont1");
            distanceFontPostion = new Vector2(150, 30);

            scoreFont = Game.Content.Load<SpriteFont>(@"Font\SpriteFont1");
            scoreFontPosition = new Vector2(400, 30);
            //********************************Sprite Font************************************

            load_button_content();
            base.LoadContent();

        }



        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            initialize_buttons();
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            if (Program.GameStatus)
            {
                //*****************************************************
                position1.X += speed;
                if (position1.X > 0)
                    position1.X -= bgWidth1;

                position5.X += speed;
                if (position5.X > 0)
                    position5.X -= bgWidth5;
                //************************front bgs*****************************
                frontbgPos1.X += frontbgSpeed;
                if (frontbgPos1.X > 0)
                    frontbgPos1.X -= bgFrontWidth1;

                frontbgPos2.X += frontbgSpeed;
                if (frontbgPos2.X > 0)
                    frontbgPos2.X -= bgFrontWidth2;

                frontbgPos2.X += frontbgSpeed;
                if (frontbgPos2.X > 0)
                    frontbgPos2.X -= bgFrontWidth3;

                //*******************road sprites**********************************

                roadMidPos1.X += roadSpeed;
                if (roadMidPos1.X > 0)
                    roadMidPos1.X -= roadMidWidth1;


                roadMidPos2.X += roadSpeed;
                if (roadMidPos2.X > 0)
                    roadMidPos2.X -= roadMidWidth2;

                roadMidPos3.X += roadSpeed;
                if (roadMidPos3.X > 0)
                    roadMidPos3.X -= roadMidWidth3;
                //**************************for keyboard*****************

                KeyboardState newState = Keyboard.GetState();

                // Is the SPACE key down?
                if (newState.IsKeyDown(Keys.Space))
                {
                    // If not down last update, key has just been pressed.
                    if (!oldKeyState.IsKeyDown(Keys.Space))
                    {
                        if (aRunner.status == Runner.RoleStatus.running)
                            aRunner.status = Runner.RoleStatus.jumping;
                        else if (aRunner.status == Runner.RoleStatus.jumping)
                            aRunner.setSecondJumpStatus();
                        else
                        { }
                    }
                }
                oldKeyState = newState;
                //************************MAP UPDATE***************************
                map1.UpdateTiles();
                //**********************ROLE*******************************

                aRunner.Update(gameTime);

                //**********************ROLE*******************************

            }

            update_button(gameTime);
            base.Update(gameTime);
        }
    }
}
