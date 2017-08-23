using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_ARPG
{
    public class Tile
    {
        public Vector2 tilePosition;
        public Texture2D tileTexture;
        public Rectangle tileRectangle;

        public Tile(Vector2 a_pos, Texture2D a_tex, Rectangle a_rect)
        {
            tilePosition = a_pos;
            tileTexture = a_tex;
            tileRectangle = a_rect;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tileTexture, tilePosition, null, tileRectangle, null, 0f, null, null, SpriteEffects.None, 0f);
        }
    }
}