using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RunningGame.Model;
using System.Xml;
using System.IO;
using CE.iPhone.PList;

namespace RunningGame.Utils
{
    class XMLParseUtils
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public XMLParseUtils()
        {

        }
        public TextureObject parseXML(String fileName)
        {
            TextureObject ret = new TextureObject();
            PListRoot root = PListRoot.Load(fileName);
            PListDict dic = (PListDict)root.Root;
            PListDict dicTexture = (PListDict)dic["texture"];
            PListDict frameArray = (PListDict)dic["frames"];
            Dictionary<string, CE.iPhone.PList.IPListElement>.KeyCollection testKeys = dicTexture.Keys;
            foreach (String s in testKeys)
            {
                PListInteger p = (PListInteger)dicTexture[s];
                if (String.Compare(s, @"width") == 0)
                    ret.Width = (int) p.Value;
                else
                    ret.Height = (int)p.Value;
            }
            ret.FrameObjectArray = new System.Collections.ArrayList();
            testKeys = frameArray.Keys;
            foreach (String s in testKeys)
            {
                FrameObjects aFrameObject = new FrameObjects();
                PListDict pro = (PListDict)frameArray[s];
                aFrameObject.Width = (int)(((PListInteger)pro["width"]).Value);
                aFrameObject.Height = (int)(((PListInteger)pro["height"]).Value);
                aFrameObject.X =  (int)(((PListInteger)pro["x"]).Value);
                aFrameObject.Y =(int)(((PListInteger)pro["y"]).Value);
                aFrameObject.OffsetX =(int)(((PListReal)pro["offsetX"]).Value);
                aFrameObject.OffsetY = (int)(((PListReal)pro["offsetY"]).Value);
                aFrameObject.OriginalWidth = (int)(((PListInteger)pro["originalWidth"]).Value);
                aFrameObject.OriginalHeight = (int)(((PListInteger)pro["originalHeight"]).Value);
                aFrameObject.TextureName = s;
                ret.FrameObjectArray.Add(aFrameObject);
            }
           
           // PListInteger PListString

            return ret;
        }
    }

    class TestForLoadXml
    {
        public void LoadXMLTestMethod()
        {
            XMLParseUtils aTestParse = new XMLParseUtils();
            aTestParse.parseXML(@"config/winter_p1.xml");
        }
    }
}
