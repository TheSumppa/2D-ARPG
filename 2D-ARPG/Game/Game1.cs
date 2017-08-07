﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Xml.Linq;
using Comora;

namespace _2D_ARPG
{
    /// This is the main type for your game.
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;                              // Player class
        KeyboardState currentKeyboardState;         // Current Keyboardstate used in movement
        KeyboardState previousKeyboardState;        // previous Keyboardstate used in movement
        Tile[,] tileset;                            // Multidimensional array for tiles
        float playerMoveSpeed = 16;                 // Player movespeed
        int worldmap = 0;                           // Variable used for drawing woldmap
        float keyRepeatTime;                        // repeattime used for movement
        float elapsedTime;                          // Elapsed time used for movement
        const float keyRepeatDelay = 0.2f;          // Repeat rate
        public SpriteFont font;                     // Sprite font used for text
        Camera camera;                              // Game camera

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferMultiSampling = false;
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

        }

        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player();                          // Initializing our player
            tileset = getTileset();                         // Initializing our tileset
            camera = new Camera(this.GraphicsDevice);       // Initializing our gameCamera
            base.Initialize();
        }

        // WorldMap data
        public Tile[,] getTileset()
        {
            XDocument xDoc = XDocument.Load("Content/WorldMap.tmx");
            int MapWidth = int.Parse(xDoc.Root.Attribute("width").Value);
            int MapHeight = int.Parse(xDoc.Root.Attribute("height").Value);
            int TileCount = int.Parse(xDoc.Root.Element("tileset").Attribute("tilecount").Value);
            int Columns = int.Parse(xDoc.Root.Element("tileset").Attribute("columns").Value);
            string IdArray = xDoc.Root.Element("layer").Element("data").Value;
            string[] splitArray = IdArray.Split(',');

            int[,] intIDs = new int[MapWidth, MapHeight];

            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    intIDs[x, y] = int.Parse(splitArray[x + y * MapWidth]);
                }
            }

            int num = 0;

            Vector2[] sourcePosition = new Vector2[TileCount];
            for (int x = 0; x < TileCount / Columns; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    sourcePosition[num] = new Vector2(y * 16, x * 16);
                    num++;
                }
            }

            Texture2D sourceTex = Content.Load<Texture2D>("Tileset");

            Tile[,] tiles = new Tile[MapWidth, MapHeight];
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    tiles[x, y] = new Tile(new Vector2(x * 16, y * 16), sourceTex, new Rectangle((int)sourcePosition[intIDs[x, y] - 1].X, (int)sourcePosition[intIDs[x, y] - 1].Y, 16, 16));
                }
            }
            return tiles;
        }

        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            WalkAnimation playerAnimation = new WalkAnimation();
            Texture2D playerTexture = Content.Load<Texture2D>("knightwalkanimation");
            Vector2 playerPosition = new Vector2(800, 800);
            Rectangle playerRectangle = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, 16, 16);
            playerAnimation.Initialize(playerTexture, playerPosition, 16, 16, 2, 200, Color.White, 1.0f, true);
            player.Initialize(playerAnimation, playerPosition);
            font = Content.Load<SpriteFont>("Text");
            camera.Scale = 6.0f;

        }
        

        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// Here we update our game
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            elapsedTime = seconds;
            UpdatePlayer(gameTime);
            this.camera.Update(gameTime);
            this.camera.Position = player.PlayerPosition;
            base.Update(gameTime);
        }

        // To check if key is pressed
        bool KeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }

        // Player movement
        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update(gameTime);
            if (worldmap == 1)  // You can move only when game is active
            {
                if (currentKeyboardState.IsKeyDown(Keys.A) && currentKeyboardState.IsKeyUp(Keys.W) && currentKeyboardState.IsKeyUp(Keys.S)
                    && currentKeyboardState.IsKeyUp(Keys.D))
                {
                    if (previousKeyboardState.IsKeyUp(Keys.A) || keyRepeatTime < 0)
                    {
                        keyRepeatTime = keyRepeatDelay;
                        player.PlayerPosition.X -= playerMoveSpeed;
                    }
                    else
                        keyRepeatTime -= elapsedTime;
                }

                if (currentKeyboardState.IsKeyDown(Keys.D) && currentKeyboardState.IsKeyUp(Keys.S) && currentKeyboardState.IsKeyUp(Keys.W)
                    && currentKeyboardState.IsKeyUp(Keys.A))
                {
                    if (previousKeyboardState.IsKeyUp(Keys.D) || keyRepeatTime < 0)
                    {
                        keyRepeatTime = keyRepeatDelay;
                        player.PlayerPosition.X += playerMoveSpeed;
                        
                    }
                    else
                        keyRepeatTime -= elapsedTime;
                }

                if (currentKeyboardState.IsKeyDown(Keys.W) && currentKeyboardState.IsKeyUp(Keys.A) && currentKeyboardState.IsKeyUp(Keys.S)
                    && currentKeyboardState.IsKeyUp(Keys.D))
                {
                    if (previousKeyboardState.IsKeyUp(Keys.W) || keyRepeatTime < 0)
                    {
                        keyRepeatTime = keyRepeatDelay;
                        player.PlayerPosition.Y -= playerMoveSpeed;                      
                    }
                    else
                        keyRepeatTime -= elapsedTime;
                }

                if (currentKeyboardState.IsKeyDown(Keys.S) && currentKeyboardState.IsKeyUp(Keys.W) && currentKeyboardState.IsKeyUp(Keys.A)
                    && currentKeyboardState.IsKeyUp(Keys.D))
                {
                    if (previousKeyboardState.IsKeyUp(Keys.S) || keyRepeatTime < 0)
                    {
                        keyRepeatTime = keyRepeatDelay;
                        player.PlayerPosition.Y += playerMoveSpeed;                       
                    }
                    else
                        keyRepeatTime -= elapsedTime;
                }
            }

            if (KeyPressed(Keys.Enter))
            {
                worldmap = 1;
            }

            // This keeps player inside the bounds
            // player.PlayerPosition.X = MathHelper.Clamp(player.PlayerPosition.X, 0, GraphicsDevice.Viewport.Width - player.PlayerAnimation.FrameWidth);
            // player.PlayerPosition.Y = MathHelper.Clamp(player.PlayerPosition.Y, 0, GraphicsDevice.Viewport.Height - player.PlayerAnimation.FrameHeight);
        }

        /// This is called when the game should draw itself.
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(this.camera, SpriteSortMode.Deferred,
                BlendState.AlphaBlend, SamplerState.PointClamp);
            if(worldmap == 0)
            {
                spriteBatch.DrawString(font, "Press enter to play", new Vector2(400, 400), Color.Black);
            }
            // Drawing WoldMap
            if(worldmap == 1)
            {
                foreach (Tile tile in tileset)
                {
                    tile.Draw(spriteBatch);
                }

                // Drawing Player
                player.Draw(spriteBatch);  
                
            }
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}