using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;


namespace _2D_ARPG
{
    class Player
    {
        public int Life;
        public int Attack;
        public bool Active;
        public Vector2 PlayerPosition;
        // public Texture2D PlayerTexture;
        public WalkAnimation PlayerAnimation;

        public int Width
        {
            get { return PlayerAnimation.FrameWidth; }
        }

        public int Height
        {
            get { return PlayerAnimation.FrameHeight; }
        }

        public void Initialize(WalkAnimation animation, Vector2 position)
        {
            //PlayerTexture = texture;
            PlayerAnimation = animation;
            PlayerPosition = position;
            Active = true;
            Life = 10;
            Attack = 5;
        }

        public void Update(GameTime gameTime)
        {
            PlayerAnimation.Position = PlayerPosition;
            PlayerAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(PlayerTexture, PlayerPosition, null, null, null, 0f, null, null, SpriteEffects.None, 0f);
            PlayerAnimation.Draw(spriteBatch);
        }
    }
}
