using UnityEditor;
using UnityEngine;

/// <summary>
/// Configura automaticamente a importação dos sprites do player (gato do ateliê).
///
/// Cada PNG é uma sprite sheet com 8 quadros em linha (768x80 total, 96x80 por quadro).
/// Este script define:
///   - Sprite Mode = Multiple
///   - Pixels Per Unit = 32 (ajustável)
///   - Slicing automático em grid de 8 colunas x 1 linha, com tamanho 96x80
///   - Filtro Point (pixel art nítida)
///   - Compressão desligada (qualidade máxima para pixel art)
///
/// Selecione os arquivos na pasta Assets/Sprites/Player e execute:
///   Assets > Aplicar Configuração de Sprites do Player
/// </summary>
public class SpriteImportSettings : MonoBehaviour
{
    [MenuItem("Assets/Aplicar Configuração de Sprites do Player")]
    private static void ApplySettings()
    {
        var guids = Selection.assetGUIDs;
        int configurados = 0;

        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (!path.EndsWith(".png")) continue;

            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null) continue;

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.spritePixelsPerUnit = 32f;
            importer.filterMode = FilterMode.Point;
            importer.compression = TextureImporterCompression.Uncompressed;
            importer.isReadable = false;
            importer.mipmapEnabled = false;
            importer.sRGBTexture = true;
            importer.alphaIsTransparency = true;

            // Slicing: 8 quadros em linha, 96x80 px cada
            var sheetConfig = new SpriteMetaData[8];
            for (int i = 0; i < 8; i++)
            {
                sheetConfig[i] = new SpriteMetaData
                {
                    name = $"frame_{i}",
                    rect = new Rect(i * 96, 0, 96, 80),
                    alignment = SpriteAlignment.Center,
                    pivot = new Vector2(0.5f, 0.5f)
                };
            }
            importer.spritesheet = sheetConfig;

            importer.SaveAndReimport();
            configurados++;
        }

        Debug.Log($"[SpriteImportSettings] {configurados} sprite(s) configurado(s) como múltiplo com slicing 8x1 (96x80).");
    }

    [MenuItem("Assets/Aplicar Configuração de Sprites do Player", validate = true)]
    private static bool ValidateApplySettings()
    {
        foreach (var guid in Selection.assetGUIDs)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.EndsWith(".png")) return true;
        }
        return false;
    }
}
