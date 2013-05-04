using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Shards_CSharp
{

    public class Shards : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        static readonly Random rnd = new Random(DateTime.Now.Millisecond);


        enum GameState {
            MainMenu,
            Options,
            Playing,
        }

        GameState CurrentGameState = GameState.MainMenu;

        cButton btnPlay, btnOpt, btnQuit;

        //create resource variables
        Texture2D background;
        Texture2D border;
        Texture2D logoSmall;
        Texture2D title;
        Texture2D slider;
        Texture2D gem1, gem2, gem3, gem4;
        Song music;
        SoundEffect btnClick;
        Slider basket;
        Rectangle screenRectangle;
        private SpriteFont font;
        public int score = 0;
        public int missed = 0;
        double playTimer;
        List<Gems> gems = new List<Gems>();

        //create screen width/height variables
        int screenWidth;
        int screenHeight;

        public Shards()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 768;   // set this value to the desired height of your window
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";

            playTimer = 0.0f;

            screenRectangle = new Rectangle(
                0,
                0,
                graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight);
        }
        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            IsMouseVisible = true;

            btnPlay = new cButton(Content.Load<Texture2D>("Graphics\\startButton"));
            btnPlay.setPosition(new Vector2(368, 200));

            btnOpt = new cButton(Content.Load<Texture2D>("Graphics\\optionsButton"));
            btnOpt.setPosition(new Vector2(368, 350));

            btnQuit = new cButton(Content.Load<Texture2D>("Graphics\\exitButton"));
            btnQuit.setPosition(new Vector2(368, 500));

            //set variables for screen width/height to make things easier
            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;

            //Load Graphics Resources
            background = Content.Load<Texture2D>("Graphics\\background");
            border = Content.Load<Texture2D>("Graphics\\border2");
            logoSmall = Content.Load<Texture2D>("Graphics\\logosmall");
            title = Content.Load<Texture2D>("Graphics\\title");
            slider = Content.Load<Texture2D>("Graphics\\slider");
            gem1 = Content.Load<Texture2D>("Graphics\\Gems\\Gem1");
            gem2 = Content.Load<Texture2D>("Graphics\\Gems\\Gem2");
            gem3 = Content.Load<Texture2D>("Graphics\\Gems\\Gem3");
            gem4 = Content.Load<Texture2D>("Graphics\\Gems\\Gem4");

            //Load the slider class from Slider.cs
            basket = new Slider(slider, screenRectangle);

            //Load the gems class from Gems.cs
            for (int i = 0; i < 10; i++)
            {
                int gemType = rnd.Next(4);
                Texture2D gem = gem1;

                if (gemType == 0)
                {
                    gem = gem1;
                }
                else if (gemType == 1)
                {
                    gem = gem2;
                }
                else if (gemType == 2)
                {
                    gem = gem3;
                }
                else if (gemType == 3)
                {
                    gem = gem4;
                }

                gems.Add(new Gems(gem, screenRectangle));
            }

            //Load Audio Resources
            btnClick = Content.Load<SoundEffect>("Audio\\Decision3");
            music = Content.Load<Song>("Audio\\music");

            //Load the main game font
            font = Content.Load<SpriteFont>("Font");
        
            //Play the background music
            MediaPlayer.Play(music);
            MediaPlayer.IsRepeating = true; //Makes it loop
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MouseState mouse = Mouse.GetState();

            switch (CurrentGameState)
            {
                case GameState.MainMenu: //Title screen mode
                    if (btnPlay.isClicked == true)
                    {
                        SoundEffectInstance instance = btnClick.CreateInstance();
                        btnClick.Play(0.2f, 0.0f, 0.0f);
                        CurrentGameState = GameState.Playing;
                    }
                    else
                    {
                        btnPlay.Update(mouse);
                    }

                    if (btnOpt.isClicked == true)
                    {
                        SoundEffectInstance instance = btnClick.CreateInstance();
                        btnClick.Play(0.2f, 0.0f, 0.0f);
                        CurrentGameState = GameState.Options;
                    }
                    else
                    {
                        btnOpt.Update(mouse);
                    }

                    if (btnQuit.isClicked == true)
                    {
                        SoundEffectInstance instance = btnClick.CreateInstance();
                        btnClick.Play(0.2f, 0.0f, 0.0f);
                        System.Threading.Thread.Sleep(btnClick.Duration);
                        Exit();
                    }
                    else
                    {
                        btnQuit.Update(mouse);
                    }
                    break;

                case GameState.Playing: //Playing mode, all update code here
                    playTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                    basket.Update();
                    for (int i = 0; i < gems.Count; i++)
                    {
                        if (gems[i] != null)
                        {
                            gems[i].Update();
                        }
                    }
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin();
            switch (CurrentGameState)
            {
                case GameState.MainMenu: //Title screen mode
                    Rectangle screenSize = new Rectangle(0, 0, screenWidth, screenHeight);
                    spriteBatch.Draw(background, screenSize, Color.White); //draws the background image
                    spriteBatch.Draw(logoSmall, new Vector2(368, 16), Color.White); //draws the logo
                    spriteBatch.DrawString(font, "Clone of Raining Shards in C# by OCLU - Derek, TheMaster99", new Vector2(16, 732), Color.Gold); //:D
                    btnPlay.Draw(spriteBatch); //draws the start button
                    btnOpt.Draw(spriteBatch); //draws the options button
                    btnQuit.Draw(spriteBatch); //draws the quit button
                    break;

                case GameState.Playing: //Playing mode, all gameplay drawing code here
                    DrawScenery();
                    for (int i = 0; i < gems.Count; i++)
                    {
                        if (gems[i] != null)
                        {
                            gems[i].Draw(spriteBatch);
                        }
                    }
                    basket.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawScenery() //Handles the drawing of the basic GUI/HUD in a handy class
        {
            Rectangle screenSize = new Rectangle(0, 0, screenWidth, screenHeight); //defines a reactangle parameter for the entire screen
            spriteBatch.Draw(background, screenSize, Color.White); //draws the main background image
            spriteBatch.Draw(border, screenSize, Color.White); //draws the main border image
            spriteBatch.Draw(logoSmall, new Vector2(368, -4), Color.White); //draws the logo image
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(16, 8), Color.GreenYellow); //draws the "Score" variable
            spriteBatch.DrawString(font, "Shards Missed: " + missed, new Vector2(16, 32), Color.OrangeRed); //draws the "missed" variable
            spriteBatch.DrawString(font, "DEV ALPHA", new Vector2(8, 728), Color.Yellow); //draws the Dev version indicator
            spriteBatch.DrawString(font, "Play Time: " + ((int)playTimer / 1000).ToString(), new Vector2(16, 56), Color.Gold); //Draws the play time indicator
        }
    }
}
