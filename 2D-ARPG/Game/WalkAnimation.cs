using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_ARPG
{
    class WalkAnimation
    {
        Texture2D animation;
        float scale;
        int elapsedTime;
        int frameTime;
        int FrameCount;
        int currentFrame;
        Color color;
        Rectangle sourceRect = new Rectangle();
        Rectangle destinationRect = new Rectangle();
        public int FrameWidth;
        public int FrameHeight;
        public bool Active;
        public bool Looping;
        public Vector2 Position;

        public void Initialize(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount, int frametime, Color color, float scale, bool looping)
        {
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.FrameCount = frameCount;
            this.frameTime = frametime;
            this.scale = scale;

            Looping = looping;
            Position = position;
            animation = texture;

            elapsedTime = 0;
            currentFrame = 0;

            Active = true;
        }

        public void Update(GameTime gameTime)
        {
            if (Active == false) return;

            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime > frameTime)
            {
                currentFrame++;

                if (currentFrame == FrameCount)
                {
                    currentFrame = 0;
                    if (Looping == false)
                        Active = false;
                }

                elapsedTime = 0;
            }
            sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);
            destinationRect = new Rectangle((int)Position.X - (int)(FrameWidth * scale),
                (int)Position.Y - (int)(FrameHeight * scale),
                (int)(FrameWidth * scale),
                (int)(FrameHeight * scale));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                spriteBatch.Draw(animation, destinationRect, sourceRect, color);
            }
        }

    }
}