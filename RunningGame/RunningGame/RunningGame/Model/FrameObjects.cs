using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunningGame.Model
{
    class FrameObjects
    {
        private int x;
        private int y;
        private int width;
        private int height;
        private int offsetX;
        private int offsetY;
        private int originalWidth;
        private int originalHeigh;
        private String textureName;


        public String TextureName
        {
            get { return this.textureName; }
            set { this.textureName = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        public int OffsetX
        {
            get { return offsetX; }
            set { offsetX = value; }
        }
        public int OffsetY
        {
            get { return offsetY; }
            set { offsetY = value; }
        }
        public int OriginalWidth
        {
            get { return originalWidth; }
            set { originalWidth = value; }
        }
        public int OriginalHeight
        {
            get { return originalHeigh; }
            set { originalHeigh = value; }
        }
    }
}
