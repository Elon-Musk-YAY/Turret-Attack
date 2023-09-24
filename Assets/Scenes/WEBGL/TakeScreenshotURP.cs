using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TakeScreenshotURP : MonoBehaviour {

#if UNITY_EDITOR
    private bool takeScreenshot;

    private void OnEnable() {
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
    }

    private void OnDisable() {
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
    }

    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext arg1, Camera arg2) {
        if (takeScreenshot) {
            takeScreenshot = false;
            int width = 256;
            int height = 256;
            Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, width, height);
            screenshotTexture.ReadPixels(rect, 0, 0);
            screenshotTexture.Apply();

            byte[] byteArray = screenshotTexture.EncodeToPNG();
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            System.Random r = new System.Random();
            ScreenCapture.CaptureScreenshot("GameScreenshot" + r.Next(100)+".png");
            //StartCoroutine(CoroutineScreenshot());
            takeScreenshot = true;
        }
    }

    private IEnumerator CoroutineScreenshot() {
        yield return new WaitForEndOfFrame();

        int width = 1024;
        int height = 1024;
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, width, height);
        screenshotTexture.ReadPixels(rect, 0, 0);
        screenshotTexture.Apply();

        byte[] byteArray = screenshotTexture.EncodeToPNG();
        //System.IO.File.WriteAllBytes(Application.dataPath + "/GameScreenshot"+r.Next(100)+".png", byteArray);
    }

#endif

}