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

        public Vector2 PlayerPosition;
        public Texture2D PlayerTexture;
        public Rectangle PlayerRectangle;

        public void Initialize(Texture2D texture, Vector2 position, Rectangle rectangle)
        {
            PlayerTexture = texture;
            PlayerPosition = position;
            PlayerRectangle = rectangle;
            Life = 10;
            Attack = 5;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlayerTexture, PlayerPosition, null, null, null, 0f, null, null, SpriteEffects.None, 0f);
        }
    }
}
