using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotHandler : MonoBehaviour
{
    private static ScreenShotHandler instance;

    private Camera myCamera;
    private bool takeScreenShotNextFrame;

    private void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
    }

    private void TakeScreenShot(int width, int height)
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenShotNextFrame = true;
    }

    private void OnPostRender()
    {
        if (takeScreenShotNextFrame)
        {
            takeScreenShotNextFrame = false;
            RenderTexture renderTexture = myCamera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScreenshot-" + System.DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss") + ".png", byteArray);

            Debug.Log("Saved CameraScreenShot.png");

            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;
        }
    }

    public static void ScreenShot_Static(int width, int height)
    {
        instance.TakeScreenShot(width, height);
    }
}
