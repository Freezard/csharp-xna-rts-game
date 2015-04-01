using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace RTSgame.Utilities
{
    /// <summary>
    /// The assetbank keeps track of all assets/content so that they are easy to assign to objects
    /// </summary>

    class AssetBank
    {

        Dictionary<String, Model> models;
        Dictionary<String, Texture2D> textures;
        Dictionary<String, Effect> shaders;
        Dictionary<String, Song> songs;
        Dictionary<String, SpriteFont> fonts;
        private static AssetBank instance;
        private AssetBank()
        {
           
        }

        public static AssetBank GetInstance()
        {
            if (instance == null)
            {
                instance = new AssetBank();
            }
            return instance;
        }

        //Haxändring
        public static AssetBank GetNewInstance()
        {
            instance = new AssetBank();
            return instance;
        }
        /// <summary>
        /// Fill up the Assetbank with all assets
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            models = LoadContent<Model>(content, "Models");
            textures = LoadContent<Texture2D>(content, "Textures");
            shaders = LoadContent<Effect>(content, "Shaders");
            songs = LoadContent<Song>(content, "Audio/Songs");
            fonts = LoadContent<SpriteFont>(content, "Fonts");
        }

        public Model GetModel(String id)
        {
            return models[id];
        }
        /// <summary>
        /// Gets a texture, returns null if not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Texture2D GetTextureReturnNull(String id)
        {
            if (textures.ContainsKey(id))
            {
                return textures[id];
            }
            else
            {
                return null;
            }

        }
        public Texture2D GetTexture(String id)
        {

                return textures[id];


        }

        public Effect GetShader(String id)
        {
            return shaders[id];
        }
        public Song GetSong(String id)
        {
            return songs[id];
        }
        public SpriteFont GetFont(String id)
        {
            return fonts[id];
        }
        /// <summary>
        /// Load one folder and gives a dictionary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="contentManager"></param>
        /// <param name="contentFolder"></param>
        /// <returns></returns>
        public static Dictionary<String, T> LoadContent<T>(ContentManager contentManager, string contentFolder)
        {
            //Load directory info, abort if none
            DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "\\" + contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
            //Init the resulting list
            Dictionary<String, T> result = new Dictionary<String, T>();

            //Load all files that matches the file filter
            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                
                //Boolean ext = fileName.EndsWith(".fbx");
                string key = Path.GetFileNameWithoutExtension(file.Name);
                try
                {
                    result[key] = contentManager.Load<T>(contentFolder + "/" + key);
                }
                catch (ContentLoadException cle)
                {
                    DebugPrinter.Write("WARNING: Some assets are in the wrong folder and are not loaded: " + key + " in " + contentFolder);
                }
            }
            //Return the result
            return result;
        }
        


    }
}
