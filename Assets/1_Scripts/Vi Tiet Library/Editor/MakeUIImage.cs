using UnityEngine;
using UnityEditor;
using ViTiet.Utils;

namespace ViTiet.Editor
{
    public class MakeUIImage : AssetPostprocessor
    {
        void OnPreprocessTexture()
        {
            if (assetPath.Contains("UIImages") || assetPath.Contains("UISprites"))
            {
                EditorHelper.Log(this, "Importing new UI Images...");
                TextureImporter textureImporter = (TextureImporter)assetImporter;
                textureImporter.textureType = TextureImporterType.Sprite;
                textureImporter.wrapMode = TextureWrapMode.Repeat;
            }
        }
    }
}