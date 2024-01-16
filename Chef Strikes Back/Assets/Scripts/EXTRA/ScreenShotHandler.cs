using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotHandler : MonoBehaviour
{
    public string screenshotFileName = "screenshot.png";
    private string screenshotPath;

    public Image displayImage; 
    void Awake()
    {
        screenshotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), screenshotFileName);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(CaptureScreenshot());
        }
    }

    IEnumerator CaptureScreenshot()
    {

        yield return new WaitForEndOfFrame();


        ScreenCapture.CaptureScreenshot(screenshotPath);

        yield return new WaitForSeconds(0.5f);

        Texture2D screenshot = new Texture2D(2, 2);
        byte[] imageData = File.ReadAllBytes(screenshotPath);
        screenshot.LoadImage(imageData);

        displayImage.sprite = Sprite.Create(screenshot, new Rect(0.0f, 0.0f, screenshot.width, screenshot.height), new Vector2(0.5f, 0.5f), 100.0f);
        displayImage.gameObject.SetActive(true);
    }

    private void OnApplicationQuit()
    {
        if (File.Exists(screenshotPath))
        {
            File.Delete(screenshotPath);
        }
    }
}
