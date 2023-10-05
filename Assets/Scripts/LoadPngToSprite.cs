using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadPngToSprite : MonoBehaviour
{
    [SerializeField] string pngName = "test.png";
    // Start is called before the first frame update

    [SerializeField] bool autoLoad;
    [SerializeField] float waitTime = 2.0f;
    void Start()
    {
        if (autoLoad)
        {
            StartCoroutine(autoLoadImage());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadTextureToSprite()
    {
        string path = UnityEngine.Application.dataPath + "/" + pngName;
        Sprite sprite = LoadNewSprite(path);
        if (sprite == null)
        {
            return;
        }

        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
    {
        Texture2D texture2D = LoadTexture(FilePath);
        
        if (texture2D != null)
        {
            Sprite NewSprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), PixelsPerUnit);
            return NewSprite;
        }
        

        return null;
    }

    public Texture2D LoadTexture(string FilePath)
    {

        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }

    IEnumerator autoLoadImage()
    {
        yield return new WaitForSeconds(waitTime);

        loadTextureToSprite();
    }
}
