using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content;

namespace _2D_ARPG
{
    class WorldMap
    {
        public int[,] worldMapCollisionIDs = new int[100, 100];
        public Tile[,] getMapTiles(ContentManager Content)
        {
            XDocument mapXdoc = XDocument.Load("Content/WorldMap.tmx");
            int mapWidth = int.Parse(mapXdoc.Root.Attribute("width").Value);
            int mapHeight = int.Parse(mapXdoc.Root.Attribute("height").Value);
            int tileCount = int.Parse(mapXdoc.Root.Element("tileset").Attribute("tilecount").Value);
            int columns = int.Parse(mapXdoc.Root.Element("tileset").Attribute("columns").Value);
            string mapIDArray = mapXdoc.Root.Element("layer").Element("data").Value;
            string[] mapIDSplit = mapIDArray.Split(',');
            int[,] tileIDs = new int[mapWidth, mapHeight];

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    tileIDs[x, y] = int.Parse(mapIDSplit[x + y * mapWidth]);
                    worldMapCollisionIDs[x, y] = int.Parse(mapIDSplit[x + y * mapWidth]);
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
            Tile[,] mapTiles = new Tile[mapWidth, mapHeight];
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    mapTiles[x, y] = new Tile(new Vector2(x * 16, y * 16), sourceTexture, new Rectangle((int)sourcePos[tileIDs[x, y] - 1].X, (int)sourcePos[tileIDs[x, y] - 1].Y, 16, 16));
                }
            }
            return mapTiles;
        }
    }
}