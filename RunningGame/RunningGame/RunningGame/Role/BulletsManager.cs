using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RunningGame.Role
{
    public class Bullet
    {
        public bool isAlive;
        public Vector2 position;
        public int index;
    }
    public class BulletsManager
    {
        private List<Bullet> listBullet = new List<Bullet>();
        private int widthBullet = 0;
        private int heightBullet = 0;
        private Vector2[] veloArray = new Vector2[] { };
        private Random rand = new Random(1100);
        private Texture2D textures;

        public void Update(GameTime gt)
        {
            foreach (Bullet b in listBullet)
            {
                if (b.position.X < 0)
                {
                    b.isAlive = false;
                    listBullet.Remove(b);
                }
                b.position += veloArray[rand.Next(5)];
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Bullet b in listBullet)
            {
                if (b.isAlive)
                    sb.Draw(textures, b.position, Color.White);
            }
        }

        public void FireBullet(Vector2 pos,Texture2D texture)
        {
            textures = texture;
            Bullet aBullet = new Bullet() { isAlive = true, index = 0, position = pos };
            listBullet.Add(aBullet);
        }

        public List<Rectangle> GetBulletsRectangle()
        {
            List<Rectangle> retRect = new List<Rectangle>();
            foreach (Bullet b in listBullet)
            {
                Rectangle rect = new Rectangle((int)b.position.X,(int) b.position.Y, widthBullet, heightBullet);
                retRect.Add(rect);
            }
            return retRect;
        }
    }
}
