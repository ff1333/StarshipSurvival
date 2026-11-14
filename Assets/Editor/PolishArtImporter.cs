using UnityEditor;
using UnityEngine;

public sealed class PolishArtImporter : AssetPostprocessor
{
    private void OnPreprocessTexture()
    {
        if (!assetPath.StartsWith("Assets/Resources/Polish/")) return;
        var importer = (TextureImporter)assetImporter;
        importer.textureType = TextureImporterType.Sprite;
        if (assetPath.EndsWith("Floor.png"))
        {
            importer.spritePixelsPerUnit = 64;
            var settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            settings.spriteMeshType = SpriteMeshType.FullRect;
            importer.SetTextureSettings(settings);
        }
        else if (assetPath.Contains("Weapon")) importer.spritePixelsPerUnit = 32;
        else if (assetPath.Contains("Projectile")) importer.spritePixelsPerUnit = 80;
        else if (assetPath.Contains("Energy")) importer.spritePixelsPerUnit = 36;
        else importer.spritePixelsPerUnit = 72;
        importer.mipmapEnabled = false;
        importer.alphaIsTransparency = true;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.filterMode = FilterMode.Bilinear;
        importer.spriteImportMode = SpriteImportMode.Single;
    }
}
