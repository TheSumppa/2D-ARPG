using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_ARPG
{
    class Player
    {
        public int Life;
        public int Attack;
        public bool Active;
        public Vector2 PlayerPosition;
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
            PlayerAnimation.Draw(spriteBatch);
        }
    }
}