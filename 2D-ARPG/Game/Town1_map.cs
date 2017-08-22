using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content;

namespace _2D_ARPG
{
    public class Town1_map
    {
        public Tile[,] town1Tiles;
        public int[,] townCollisions = new int[100, 100];
        public Tile[,] getTownTiles(ContentManager Content)
        {
            XDocument townXdoc = XDocument.Load("Content/Town_1.tmx");
            int mapWidth = int.Parse(townXdoc.Root.Attribute("width").Value);
            int mapHeight = int.Parse(townXdoc.Root.Attribute("height").Value);
            int tileCount = int.Parse(townXdoc.Root.Element("tileset").Attribute("tilecount").Value);
            int columns = int.Parse(townXdoc.Root.Element("tileset").Attribute("columns").Value);
            string townIDArray = townXdoc.Root.Element("layer").Element("data").Value;
            string[] townIDSplit = townIDArray.Split(',');
            int[,] tileIDs = new int[mapWidth, mapHeight];

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    tileIDs[x, y] = int.Parse(townIDSplit[x + y * mapWidth]);
                    townCollisions[x, y] = int.Parse(townIDSplit[x + y * mapWidth]);
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
            Tile[,] townTiles = new Tile[mapWidth, mapHeight];
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    townTiles[x, y] = new Tile(new Vector2(x * 16, y * 16), sourceTexture, new Rectangle((int)sourcePos[tileIDs[x, y] - 1].X, (int)sourcePos[tileIDs[x, y] - 1].Y, 16, 16));
                }
            }
            return townTiles;
        }
    }
}
