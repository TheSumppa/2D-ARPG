using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Comora;
using System.Diagnostics;
namespace _2D_ARPG
{
    /// This is the main type for your game.
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;                     // Our GraphicsManager
        SpriteBatch spriteBatch;                            // Spritebatch we use to draw stuff
        Camera camera;                                      // Game camera class
        Player player;                                      // Player class
        Town1_map town1Map;                                 // Town1 map class
        WorldMap worldMap;                                  // World map class
        KeyboardState currentKeyboardState;                 // Current Keyboardstate used in movement
        KeyboardState previousKeyboardState;                // previous Keyboardstate used in movement
        Tile[,] tilesetWorldMap;                            // Multidimensional array for worldmap tiles
        Tile[,] tilesetTown1;                               // Multidimensional array for town1 tiles
        Tile[,] currentTileset;                             // Multidimensional array for currently used tiles
        float playerMoveSpeed = 16;                         // Player movespeed
        float keyRepeatTime;                                // repeattime used for movement
        float elapsedTime;                                  // Elapsed time used for movement
        float keyRepeatDelay = 0.25f;                       // Repeat rate           
        int[,] mapCollisionIDs = new int[100, 100];         // ID's used for collision on worldmap
        int[,] TownCollisionIDs = new int[100, 100];        // ID's used for collision in towns
        int[,] currentCollisionIDs = new int[100, 100];     // Currently used collision IDs
        int currentTileValue;

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
            camera = new Camera(this.GraphicsDevice);               // Initializing our gameCamera

            worldMap = new WorldMap();                              // Init worldMap
            tilesetWorldMap = worldMap.getMapTiles(this.Content);   // Getting our worldmap tiles
            mapCollisionIDs = worldMap.worldMapCollisionIDs;        // Map Collision Ids

            town1Map = new Town1_map();                             // Initializing our Town1 Map
            tilesetTown1 = town1Map.getTownTiles(this.Content);     // Getting our town1 tiles
            TownCollisionIDs = town1Map.townCollisions;             // Town collision Ids

            currentCollisionIDs = mapCollisionIDs;                  // set currentCollisionIDs to mapCollisionIDs as default when game starts
            currentTileset = tilesetWorldMap;                       // set currentTileset to tilesetWorldMap as default when game starts

            player = new Player();                                  // Initializing our player
            player.Attack = 5;                                      // Player Attack value
            player.Life = 10;                                       // Player Life value      

            base.Initialize();
        }

        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            WalkAnimation playerAnimation = new WalkAnimation();
            Texture2D playerTexture = Content.Load<Texture2D>("knightwalkanimation");
            Vector2 playerPosition = new Vector2(880, 768);
            Rectangle playerRectangle = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, 16, 16);
            playerAnimation.Initialize(playerTexture, playerPosition, 16, 16, 2, 200, Color.White, 1.0f, true);
            player.Initialize(playerAnimation, playerPosition);
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
            Vector2 PlayerOffset = new Vector2(player.PlayerPosition.X - 8, player.PlayerPosition.Y - 8);
            float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            elapsedTime = seconds;
            this.camera.Update(gameTime);  
            this.camera.Position = PlayerOffset;
            checkTileValue();
            UpdatePlayer(gameTime);
            Debug.WriteLine("PosX: " + player.PlayerPosition.X);
            Debug.WriteLine("PosY: " + player.PlayerPosition.Y);
            Debug.WriteLine("Tilevalue: " + currentTileValue);
            base.Update(gameTime);
        }

        // Check which tile we are standing on at the moment
        public void checkTileValue()
        {
            currentTileValue = currentCollisionIDs[((int)player.PlayerPosition.X / 16 - 1), ((int)player.PlayerPosition.Y / 16 - 1)];
        }

        // Collision check to Up
        public bool isPassableUp(int x, int y)
        {
            int tileValueUp = currentCollisionIDs[((int)player.PlayerPosition.X / 16 - 1), (((int)player.PlayerPosition.Y / 16 - 1)) - 1];
            if (tileValueUp == 1 || tileValueUp == 3 || tileValueUp == 5 || tileValueUp == 6 || tileValueUp == 7 || tileValueUp == 8 || tileValueUp == 9
                || tileValueUp == 10 || tileValueUp == 11 || tileValueUp == 12 || tileValueUp == 14 || tileValueUp == 15 || tileValueUp == 16 || tileValueUp == 17
                || tileValueUp == 19 || tileValueUp == 21 || tileValueUp == 22 || tileValueUp == 24 || tileValueUp == 28 || tileValueUp == 29 || tileValueUp == 30 || tileValueUp == 31 || tileValueUp == 32
                || tileValueUp == 33 || tileValueUp == 34 || tileValueUp == 35 || tileValueUp == 36 || tileValueUp == 39 || tileValueUp == 40 || tileValueUp == 46 || tileValueUp == 47 || tileValueUp == 48)
                return true;
            return false;
        }
        // Collision check to Down
        public bool isPassableDown(int x, int y)
        {
            int tileValueDown = currentCollisionIDs[((int)player.PlayerPosition.X / 16 - 1), (((int)player.PlayerPosition.Y / 16 - 1)) + 1];
            if (tileValueDown == 1 || tileValueDown == 3 || tileValueDown == 5 || tileValueDown == 6 || tileValueDown == 7 || tileValueDown == 8 || tileValueDown == 9
                || tileValueDown == 10 || tileValueDown == 11 || tileValueDown == 12 || tileValueDown == 14 || tileValueDown == 15 || tileValueDown == 16 || tileValueDown == 17
                || tileValueDown == 19 || tileValueDown == 21 || tileValueDown == 22 || tileValueDown == 24 || tileValueDown == 28 || tileValueDown == 29 || tileValueDown == 30 || tileValueDown == 31 || tileValueDown == 32
                || tileValueDown == 33 || tileValueDown == 34 || tileValueDown == 35 || tileValueDown == 36 || tileValueDown == 39 || tileValueDown == 40 || tileValueDown == 46 || tileValueDown == 47 || tileValueDown == 48)
                return true;
            return false;
        }
        // Collision check to Left
        public bool isPassableLeft(int x, int y)
        {
            int tileValueLeft = currentCollisionIDs[(((int)player.PlayerPosition.X / 16 - 1)) - 1, (((int)player.PlayerPosition.Y / 16 - 1))];
            if (tileValueLeft == 1 || tileValueLeft == 3 || tileValueLeft == 5 || tileValueLeft == 6 || tileValueLeft == 7 || tileValueLeft == 8 || tileValueLeft == 9
                || tileValueLeft == 10 || tileValueLeft == 11 || tileValueLeft == 12 || tileValueLeft == 14 || tileValueLeft == 15 || tileValueLeft == 16 || tileValueLeft == 17
                || tileValueLeft == 19 || tileValueLeft == 21 || tileValueLeft == 22 || tileValueLeft == 24 || tileValueLeft == 28 || tileValueLeft == 29 || tileValueLeft == 30 || tileValueLeft == 31 || tileValueLeft == 32
                || tileValueLeft == 33 || tileValueLeft == 34 || tileValueLeft == 35 || tileValueLeft == 36 || tileValueLeft == 39 || tileValueLeft == 40 || tileValueLeft == 46 || tileValueLeft == 47 || tileValueLeft == 48)
                return true;
            return false;
        }
        // Collision check to right
        public bool isPassableRight(int x, int y)
        {
            int tileValueRight = currentCollisionIDs[(((int)player.PlayerPosition.X / 16 - 1)) + 1, ((int)player.PlayerPosition.Y / 16 - 1)];
            if (tileValueRight == 1 || tileValueRight == 3 || tileValueRight == 5 || tileValueRight == 6 || tileValueRight == 7 || tileValueRight == 8 || tileValueRight == 9
                || tileValueRight == 10 || tileValueRight == 11 || tileValueRight == 12 || tileValueRight == 14 || tileValueRight == 15 || tileValueRight == 16 || tileValueRight == 17
                || tileValueRight == 19 || tileValueRight == 21 || tileValueRight == 22 || tileValueRight == 24 || tileValueRight == 28 || tileValueRight == 29 || tileValueRight == 30 || tileValueRight == 31 || tileValueRight == 32
                || tileValueRight == 33 || tileValueRight == 34 || tileValueRight == 35 || tileValueRight == 36 || tileValueRight == 39 || tileValueRight == 40 || tileValueRight == 46 || tileValueRight == 47 || tileValueRight == 48)
                return true;
            return false;
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

            if (currentTileValue == 15)                     // Check if player want's to enter a town1 and set correct tileSet, collisionIDS and player position
            {
                currentTileset = tilesetTown1;
                currentCollisionIDs = TownCollisionIDs;
                player.PlayerPosition.X = 592;
                player.PlayerPosition.Y = 720;

            }
            if (currentTileValue == 39)                      // Check if player want's to leave a town1 and set correct tileSet, collisionIDS and player positionv
            {
                currentTileset = tilesetWorldMap;
                currentCollisionIDs = mapCollisionIDs;
                player.PlayerPosition.X = 880;
                player.PlayerPosition.Y = 768;

            }
        }

        /// This is called when the game should draw itself.
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(this.camera, SpriteSortMode.Deferred,
            BlendState.AlphaBlend, SamplerState.PointClamp);
            // Draw WorldMap           
            foreach (Tile tile in currentTileset)
            {
                tile.Draw(spriteBatch);
            }
            player.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}