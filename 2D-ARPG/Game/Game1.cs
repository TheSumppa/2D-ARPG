
using Microsoft.Xna.Framework;
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
        const float keyRepeatDelay = 0.33f;         // Repeat rate
        public SpriteFont font;                     // Sprite font used for text
        Camera camera;                              // Game camera
        int[,] CollisionIDs = new int[100, 100];    // ID's used for collision
  

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferMultiSampling = false;
            graphics.IsFullScreen = false;
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
                    CollisionIDs[x, y] = int.Parse(splitArray[x + y * MapWidth]);
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
            Vector2 playerPosition = new Vector2(768, 800);
            Rectangle playerRectangle = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, 16, 16);
            playerAnimation.Initialize(playerTexture, playerPosition, 16, 16, 2, 200, Color.White, 1.0f, true);
            player.Initialize(playerAnimation, playerPosition);
            font = Content.Load<SpriteFont>("Text");
            camera.Scale = 5.0f;                            // Camera "zoom" level
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
            this.camera.Update(gameTime);
            this.camera.Position = player.PlayerPosition;
            UpdatePlayer(gameTime);
            base.Update(gameTime);

        }
        // To check if key is pressed
        bool KeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }

        // Collision check to Up
        public bool isPassableUp(int x, int y)
        {
            int tileValueUp = CollisionIDs[((int)player.PlayerPosition.X / 16 - 1), (((int)player.PlayerPosition.Y / 16 - 1)) - 1];
            if (tileValueUp == 1 || tileValueUp == 3 || tileValueUp == 5 || tileValueUp == 6 || tileValueUp == 7 || tileValueUp == 8 || tileValueUp == 9
                || tileValueUp == 10 || tileValueUp == 11 || tileValueUp == 12 || tileValueUp == 14 || tileValueUp == 15 || tileValueUp == 16 || tileValueUp == 17
                || tileValueUp == 19 || tileValueUp == 21 || tileValueUp == 22 || tileValueUp == 24 || tileValueUp == 29 || tileValueUp == 30 || tileValueUp == 31 || tileValueUp == 32
                || tileValueUp == 33 || tileValueUp == 34 || tileValueUp == 35 || tileValueUp == 36)
                return true;
            return false;
        }
        // Collision check to Down
        public bool isPassableDown(int x, int y)
        {
            int tileValueDown = CollisionIDs[((int)player.PlayerPosition.X / 16 - 1), (((int)player.PlayerPosition.Y / 16 - 1)) + 1];
            if (tileValueDown == 1 || tileValueDown == 3 || tileValueDown == 5 || tileValueDown == 6 || tileValueDown == 7 || tileValueDown == 8 || tileValueDown == 9
                || tileValueDown == 10 || tileValueDown == 11 || tileValueDown == 12 || tileValueDown == 14 || tileValueDown == 15 || tileValueDown == 16 || tileValueDown == 17
                || tileValueDown == 19 || tileValueDown == 21 || tileValueDown == 22 || tileValueDown == 24 || tileValueDown == 29 || tileValueDown == 30 || tileValueDown == 31 || tileValueDown == 32
                || tileValueDown == 33 || tileValueDown == 34 || tileValueDown == 35 || tileValueDown == 36)
                return true;
            return false;
        }
        // Collision check to Left
        public bool isPassableLeft(int x, int y)
        {
            int tileValueLeft = CollisionIDs[(((int)player.PlayerPosition.X / 16 - 1)) - 1, (((int)player.PlayerPosition.Y / 16 - 1))];
            if (tileValueLeft == 1 || tileValueLeft == 3 || tileValueLeft == 5 || tileValueLeft == 6 || tileValueLeft == 7 || tileValueLeft == 8 || tileValueLeft == 9
                || tileValueLeft == 10 || tileValueLeft == 11 || tileValueLeft == 12 || tileValueLeft == 14 || tileValueLeft == 15 || tileValueLeft == 16 || tileValueLeft == 17
                || tileValueLeft == 19 || tileValueLeft == 21 || tileValueLeft == 22 || tileValueLeft == 24 || tileValueLeft == 29 || tileValueLeft == 30 || tileValueLeft == 31 || tileValueLeft == 32
                || tileValueLeft == 33 || tileValueLeft == 34 || tileValueLeft == 35 || tileValueLeft == 36)
                return true;
            return false;
        }
        // Collision check to right
        public bool isPassableRight(int x, int y)
        {
            int tileValueRight = CollisionIDs[(((int)player.PlayerPosition.X / 16 - 1)) + 1, ((int)player.PlayerPosition.Y / 16 - 1)];
            if (tileValueRight == 1 || tileValueRight == 3 || tileValueRight == 5 || tileValueRight == 6 || tileValueRight == 7 || tileValueRight == 8 || tileValueRight == 9
                || tileValueRight == 10 || tileValueRight == 11 || tileValueRight == 12 || tileValueRight == 14 || tileValueRight == 15 || tileValueRight == 16 || tileValueRight == 17
                || tileValueRight == 19 || tileValueRight == 21 || tileValueRight == 22 || tileValueRight == 24 || tileValueRight == 29 || tileValueRight == 30 || tileValueRight == 31 || tileValueRight == 32
                || tileValueRight == 33 || tileValueRight == 34 || tileValueRight == 35 || tileValueRight == 36)
                return true;
            return false;
        }

        // Player movement
        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update(gameTime);
            if (worldmap == 1)
            {

                if (currentKeyboardState.IsKeyDown(Keys.A) && currentKeyboardState.IsKeyUp(Keys.W) && currentKeyboardState.IsKeyUp(Keys.S)
                && currentKeyboardState.IsKeyUp(Keys.D))
                {
                    if (previousKeyboardState.IsKeyUp(Keys.A) || keyRepeatTime < 0)
                    {
                        keyRepeatTime = keyRepeatDelay;
                        if (isPassableLeft((int)player.PlayerPosition.X / 16 - 1, (int)player.PlayerPosition.Y))
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
                        if (isPassableRight((int)player.PlayerPosition.X / 16 + 1, (int)player.PlayerPosition.Y))
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
                        if (isPassableUp((int)player.PlayerPosition.X, (int)player.PlayerPosition.Y / 16 - 1))
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
                        if (isPassableDown((int)player.PlayerPosition.X, (int)player.PlayerPosition.Y / 16 + 1))
                            player.PlayerPosition.Y += playerMoveSpeed;
                    }
                    else
                        keyRepeatTime -= elapsedTime;
                }

            }

            if (KeyPressed(Keys.Enter))                    //sets worldmap active
            {
                worldmap = 1;
            }
        }

        /// This is called when the game should draw itself.
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(this.camera, SpriteSortMode.Deferred,
            BlendState.AlphaBlend, SamplerState.PointClamp);
            // Draw WorldMap
            if (worldmap == 1)
            {
                foreach (Tile tile in tileset)
                {
                    tile.Draw(spriteBatch);
                }
                // Draw Player
                player.Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}