using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RunningGame.Coins
{
    public class CoinsManager
    {
        public Texture2D coinTexture;
        public List<Coin> listCoins = new List<Coin>();
        public class Coin
        {
            public bool isAlive;
            public Vector2 position;
            public int index;
        }
    }
}
