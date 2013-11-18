using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace RunningGame.Map
{
    /// <summary>
    /// A coin from the map
    /// </summary>
    public class Titles
    {
        public bool isAlive;
        public int xIndex;
        public int yIndex;
        public char tileValue;
        public Vector2 positonTiles;
    }

    public class MapManager
    {
        ContentManager content;
        string filename;
        public List<Titles> listTiles = new List<Titles>();
        public Vector2 deltaSpeed = new Vector2(-0.5f, 0);
        Vector2 tileDimensions = Vector2.Zero;
        public Vector2 TileDimensions
        {
            get { return tileDimensions; }
        }

        Vector2 mapDimensions = Vector2.Zero;

        //char[,] tiles;
        //public char[,] Tiles
        //{
        //    get { return tiles; }
        //    protected set { tiles = value; }
        //}

        char emptyTile;

        public Texture2D TileSheet
        {
            get;
            protected set;
        }

        Texture2D background;

        Dictionary<char, Rectangle> tileRegions;

        public MapManager(ContentManager content, string file, string tileSheetAsset, Vector2 dimensions, char emptyTile)
        {
            this.content = content;
            this.emptyTile = emptyTile;

            filename = file;
            tileRegions = new Dictionary<char, Rectangle>();
            tileDimensions = dimensions;

            LoadTileSheet(tileSheetAsset);

            ReadFile();
        }

        private void LoadTileSheet(string tileSheetAsset)
        {
            TileSheet = content.Load<Texture2D>(tileSheetAsset);
        }

        private void ReadFile()
        {
            StreamReader reader = new StreamReader(TitleContainer.OpenStream(filename));
            int width = 0, height = 0;
            List<string> linesFromFile = new List<string>();

            string line = reader.ReadLine();
            width = line.Length;

            while (line != null)
            {
                height++;
                linesFromFile.Add(line);
                line = reader.ReadLine();
            }

            //tiles = new char[width, height];

            for (int j = 0; j < height; j++)
            {
                string l = linesFromFile[j];
                for (int i = 0; i < width; i++)
                {
                    char c = l[i];
                    Titles atile = new Titles();
                    atile.isAlive = true;
                    atile.xIndex = i;
                    atile.yIndex = j;
                    atile.tileValue = c;
                    atile.positonTiles = new Vector2(tileDimensions.X * i + deltaSpeed.X,
                        tileDimensions.Y * j + deltaSpeed.Y);
                    listTiles.Add(atile);
                }
            }

            mapDimensions = new Vector2(width, height);
        }

        public void AddRegion(char tileKey, Rectangle region)
        {
            if (!tileRegions.ContainsKey(tileKey))
            {
                tileRegions.Add(tileKey, region);
            }
            else
                throw new Exception("Can only have one region per key");
        }

        public void AddBackground(string bgAsset)
        {
            background = content.Load<Texture2D>(bgAsset);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            if (TileSheet == null)
                throw new Exception("Tile sheet cannot be null");
            else if (tileRegions.Count == 0)
                throw new Exception("Tile regions must be populated by calling AddRegion");
            else
            {
                if (background != null)
                    spritebatch.Draw(background, Vector2.Zero, Color.White);

                DrawTiles(spritebatch);
            }
        }

        private void DrawTiles(SpriteBatch spritebatch)
        {
            foreach (Titles t in listTiles)
            {
                if (t.tileValue != emptyTile && t.isAlive) 
                {
                    spritebatch.Draw(TileSheet, t.positonTiles,
                            tileRegions[t.tileValue], Color.White);
                }
            }
        }

        public void UpdateTiles()
        {
            foreach (Titles t in listTiles)
            {
                if (t.isAlive)
                {
                    t.positonTiles = new Vector2(t.positonTiles.X + deltaSpeed.X, t.positonTiles.Y);
                }
            }
        }
    }
}
