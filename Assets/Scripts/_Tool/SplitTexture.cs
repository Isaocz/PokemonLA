// SplitTexture.cs

using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// ��ͼƬ����ɶ���Сͼ
/// </summary>
public class SplitTexture
{
    /*
    [MenuItem("Tools/SplitTexture")]
    static void DoSplitTexture()
    {
        // ��ȡ��ѡͼƬ
        Texture2D selectedImg = Selection.activeObject as Texture2D;
        string rootPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(selectedImg));
        string path = rootPath + "/" + selectedImg.name + ".png";
        TextureImporter texImp = AssetImporter.GetAtPath(path) as TextureImporter;
        // ����Ϊ�ɶ�
        texImp.isReadable = true;
        AssetDatabase.ImportAsset(path);

        // �����ļ���
        AssetDatabase.CreateFolder(rootPath, selectedImg.name);


        foreach (SpriteMetaData metaData in texImp.spritesheet)
        {
            var width = (int)metaData.rect.width;
            var height = (int)metaData.rect.height;
            Texture2D smallImg = new Texture2D(width, height);
            var pixelStartX = (int)metaData.rect.x;
            var pixelEndX = pixelStartX + width;
            var pixelStartY = (int)metaData.rect.y;
            var pixelEndY = pixelStartY + height;
            for (int x = pixelStartX; x <= pixelEndX; ++x)
            {
                for (int y = pixelStartY; y <= pixelEndY; ++y)
                {
                    smallImg.SetPixel(x - pixelStartX, y - pixelStartY, selectedImg.GetPixel(x, y));
                }
            }

            //  ת������EncodeToPNG���ݸ�ʽ
            if (TextureFormat.ARGB32 != smallImg.format && TextureFormat.RGB24 != smallImg.format)
            {
                Texture2D img = new Texture2D(smallImg.width, smallImg.height);
                img.SetPixels(smallImg.GetPixels(0), 0);
                smallImg = img;
            }

            // ����Сͼ�ļ�
            string smallImgPath = rootPath + "/" + selectedImg.name + "/" + metaData.name + ".png";
            File.WriteAllBytes(smallImgPath, smallImg.EncodeToPNG());
            // ˢ����Դ���ڽ���
            AssetDatabase.Refresh();
            // ����Сͼ�ĸ�ʽ
            TextureImporter smallTextureImp = AssetImporter.GetAtPath(smallImgPath) as TextureImporter;
            // ����Ϊ�ɶ�
            smallTextureImp.isReadable = true;
            // ����alphaͨ��
            smallTextureImp.alphaIsTransparency = true;
            // ������mipmap
            smallTextureImp.mipmapEnabled = false;
            AssetDatabase.ImportAsset(smallImgPath);
        }
    }
    */
}