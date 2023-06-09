using UnityEngine;

/// <summary>
/// スクショをF12入力で指定のディレクトリに保存。普通にUnityEditorのディレクトリじゃなくてもイケるよん。
/// </summary>
public class ScreenShotTakeSystem : MonoBehaviour
{
    public string screenshotPath = "Screenshots";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            string fileName = string.Format("{0}/screenshot_{1}.png", screenshotPath, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
            ScreenCapture.CaptureScreenshot(fileName, 1);
            Debug.Log("Screenshot saved: " + fileName);
        }
    }
}
