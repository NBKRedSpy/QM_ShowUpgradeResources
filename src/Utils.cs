using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QM_ShowUpgradeResources
{
    public static class Utils
    {
        /// <returns></returns>
        public static void LoadSprite(string filePath, out Sprite sprite)
        {
            Texture2D texture = LoadPNG(filePath);
            sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
        }
        private static Texture2D LoadPNG(string filePath)
        {
            Texture2D tex = null;
            byte[] fileData;

            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                tex = new Texture2D(2,2);

                //This is how to do a non aliasing image upscale.  Useful for pixel games.
                //tex = new Texture2D(2, 2, TextureFormat.RGBA32, mipChain: false, linear: true);
                //tex.filterMode = FilterMode.Point;


                tex.LoadImage(fileData); //This will auto-resize the texture dimensions.
            }
            else
            {
                throw new FileNotFoundException($"Unable to find {filePath}");
            }

            return tex;
        }

    }
}
