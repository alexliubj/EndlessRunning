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
using RunningGame.Scene;
using RunningGame.Componets;
using OggSharp;

namespace RunningGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        DrawableGameComponent menuComp;
        DrawableGameComponent mainGame;
        DrawableGameComponent helpComp;
        DrawableGameComponent aboutComp;
        DrawableGameComponent startComp;
        OggSong bgsong1;
        OggSong bgsong2;
        OggSong bgsong3;
        OggSong bgStart;
        OggSong currentBgSong;

        OggSong coinMusic;
        OggSong buttonClick;
        OggSong jump;
        OggSong secondJump;
        OggSong die;
        Random aRandom = new Random(100);
        string[] getStar = new string[] { "r_get_star_01.ogg", "r_get_star_02.ogg", "r_get_star_03.ogg", "r_get_star_04.ogg", "r_get_star_05.ogg", "r_get_star_06.ogg" };
        enum MenuIndexs
        { 
            MenuIndex = 0,
            MainIndex = 1, 
            HelpIndex = 2, 
            AboutIndex = 3, 
            StartIndex = 4,
        };

        public enum SoundInstance
        {
            CoinMusic = 0,
            buttonClick = 1,
            jump,
            secondjump,
            die
        };

        public void PlaySoundInstance(SoundInstance si)
        {
            switch (si)
            {
                case SoundInstance.CoinMusic:
                    {
                        coinMusic.Play();
                    }
                    break;
                case SoundInstance.buttonClick:
                    {
                        buttonClick.Play();
                    }
                    break;
                case SoundInstance.jump:
                    {
                        jump.Play();
                    }
                    break;
                case SoundInstance.secondjump:
                    {
                        secondJump.Play();
                    }
                    break;
                case SoundInstance.die:
                    {
                        die.Play();
                    }
                    break;
                default:
                    break;
            }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 640;
            graphics.PreferredBackBufferWidth = 1136;
            Components.Add(menuComp = new MainScene(this));
            Components.Add(mainGame = new GamingScene(this));
            Components.Add(helpComp = new HelpScene(this));
            Components.Add(aboutComp = new AboutScene(this));
            Components.Add(startComp = new StartScene(this));
            this.ComponetsInitialise();

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// init all the componets
        /// </summary>
        private void ComponetsInitialise()
        {
            startComp.Enabled = true;
            startComp.Visible = true;

            menuComp.Enabled = false;
            menuComp.Visible = false;

            mainGame.Enabled = false;
            mainGame.Visible = false;

            helpComp.Enabled = false;
            helpComp.Visible = false;

            aboutComp.Enabled = false;
            aboutComp.Visible = false;

        }

        /// <summary>
        /// Switch Componets
        /// </summary>
        /// <param name="componetId"></param>
        public void ChangeComponets(int componetId)
        {
            switch (componetId)
            {
                case (int)MenuIndexs.MenuIndex:
                    {
                        menuComp.Enabled = true;
                        menuComp.Visible = true;

                        mainGame.Enabled = false;
                        mainGame.Visible = false;

                        helpComp.Enabled = false;
                        helpComp.Visible = false;

                        aboutComp.Enabled = false;
                        aboutComp.Visible = false;

                        startComp.Enabled = false;
                        startComp.Visible = false;
                    }
                    break;
                case (int)MenuIndexs.AboutIndex:
                    {
                        menuComp.Enabled = false;
                        menuComp.Visible = false;

                        mainGame.Enabled = false;
                        mainGame.Visible = false;

                        helpComp.Enabled = false;
                        helpComp.Visible = false;

                        aboutComp.Enabled = true;
                        aboutComp.Visible = true;

                        startComp.Enabled = false;
                        startComp.Visible = false;
                    }
                    break;
                case (int)MenuIndexs.HelpIndex:
                    {
                        menuComp.Enabled = false;
                        menuComp.Visible = false;

                        mainGame.Enabled = false;
                        mainGame.Visible = false;

                        helpComp.Enabled = true;
                        helpComp.Visible = true;

                        aboutComp.Enabled = false;
                        aboutComp.Visible = false;

                        startComp.Enabled = false;
                        startComp.Visible = false;
                    }
                    break;
                case (int)MenuIndexs.MainIndex:
                    {
                        menuComp.Enabled = false;
                        menuComp.Visible = false;

                        mainGame.Enabled = true;
                        mainGame.Visible = true;

                        helpComp.Enabled = false;
                        helpComp.Visible = false;

                        aboutComp.Enabled = false;
                        aboutComp.Visible = false;

                        startComp.Enabled = false;
                        startComp.Visible = false;
                    }
                    break;

                case (int)MenuIndexs.StartIndex:
                    {
                        menuComp.Enabled = false;
                        menuComp.Visible = false;

                        mainGame.Enabled = false;
                        mainGame.Visible = false;

                        helpComp.Enabled = false;
                        helpComp.Visible = false;

                        aboutComp.Enabled = false;
                        aboutComp.Visible = false;

                        startComp.Enabled = true;
                        startComp.Visible = true;
                    }
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            TestForLoadXml tes = new TestForLoadXml();
            tes.LoadXMLTestMethod();
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


            bgsong3 = new OggSong(TitleContainer.OpenStream("r_bgm_01.ogg"), false);
            bgsong2 = new OggSong(TitleContainer.OpenStream("r_bgm_02.ogg"), false);
            bgsong1 = new OggSong(TitleContainer.OpenStream("u_bgm.ogg"), false);
            bgStart = new OggSong(TitleContainer.OpenStream("r_comic.ogg"), false);

            coinMusic = new OggSong(TitleContainer.OpenStream(getStar[aRandom.Next(5)]), false);
            buttonClick = new OggSong(TitleContainer.OpenStream("u_btn_click.ogg"), false);
            jump = new OggSong(TitleContainer.OpenStream("r_jump.ogg"), false);
            secondJump = new OggSong(TitleContainer.OpenStream("r_second_jump_boy.ogg"), false);
            die = new OggSong(TitleContainer.OpenStream("r_ch_die.ogg"), false);

            bgsong1.Repeat = true;
            bgsong2.Repeat = true;
            bgsong3.Repeat = true;
            bgStart.Repeat = true;
            currentBgSong = bgStart;


            bgStart.Play();

            // TODO: use this.Content to load your game content here
        }

        public void PauseAndResumeSound(int status)
        {
            if (status == 0) // playing
                currentBgSong.Pause();
            else
                currentBgSong.Resume();
        }

        public void PlayBgMusicByIndex(int index)
        {
            switch(index)
            {
                case 0:
                    currentBgSong.Stop();
                    currentBgSong = bgsong1;
                    currentBgSong.Play();
                   break;
                case 1:
                    currentBgSong.Stop();
                    currentBgSong = bgsong2;
                    currentBgSong.Play();
                    
                   break;
                case 2:
                    currentBgSong.Stop();
                    currentBgSong = bgsong3;
                    currentBgSong.Play();
                   break;
                case 3:
                    currentBgSong.Stop();
                    currentBgSong = bgStart;
                    currentBgSong.Play();
                    break;
                default:
                    break;
            }
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
