using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace RunningGame.Model
{
    /// <summary>
    /// All texture Objects
    /// </summary>
    class TextureObject
    {
        private int width;
        private int height;
        private String textureFileName;
        private ArrayList myArrayListVar;
        
        public FrameObjects getFrameObjectByName(String name)
        {
            foreach (FrameObjects f in myArrayListVar)
            {
                if (name.Equals(f.TextureName))
                    return f;
            }
            return null;
        }

        public String TextureFileName
        {
            get { return this.textureFileName; }
            set { this.textureFileName = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return this.height; }
            set { height = value; }
        }

        /// <summary>
        /// list of frame objects
        /// </summary>
        public ArrayList FrameObjectArray
        {
            get { return myArrayListVar; }
            set { myArrayListVar = value; }
        }
        
    }
}
