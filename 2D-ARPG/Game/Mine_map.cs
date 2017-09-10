using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content;

namespace _2D_ARPG
{
    class Mine_map
    {
        public int[,] mineCollisions = new int[100, 100];
        public Tile[,] getMineTiles(ContentManager Content)
        {
            XDocument mineXdoc = XDocument.Load("Content/mine.tmx");
            int mapWidth = int.Parse(mineXdoc.Root.Attribute("width").Value);
            int mapHeight = int.Parse(mineXdoc.Root.Attribute("height").Value);
            int tileCount = int.Parse(mineXdoc.Root.Element("tileset").Attribute("tilecount").Value);
            int columns = int.Parse(mineXdoc.Root.Element("tileset").Attribute("columns").Value);
            string mineIDArray = mineXdoc.Root.Element("layer").Element("data").Value;
            string[] mineIDSplit = mineIDArray.Split(',');
            int[,] tileIDs = new int[mapWidth, mapHeight];

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    tileIDs[x, y] = int.Parse(mineIDSplit[x + y * mapWidth]);
                    mineCollisions[x, y] = int.Parse(mineIDSplit[x + y * mapWidth]);
                }
            }

            int num = 0;
            Vector2[] sourcePos = new Vector2[tileCount];
            for (int x = 0; x < tileCount / columns; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    sourcePos[num] = new Vector2(y * 16, x * 16);
                    num++;
                }
            }

            Texture2D sourceTexture = Content.Load<Texture2D>("Tileset");
            Tile[,] mineTiles = new Tile[mapWidth, mapHeight];
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    mineTiles[x, y] = new Tile(new Vector2(x * 16, y * 16), sourceTexture, new Rectangle((int)sourcePos[tileIDs[x, y] - 1].X, (int)sourcePos[tileIDs[x, y] - 1].Y, 16, 16));
                }
            }
            return mineTiles;
        }
    }
}