using System;
using System.Collections;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SonicFramework
{
    public static class Framework
    {
        private const string PATH_TO_SCREENSHOTS_FOLDER = "/Screenshots/";

        private static bool IsScreenshotsFolderExists()
        {
            return Directory.Exists(Application.persistentDataPath + PATH_TO_SCREENSHOTS_FOLDER);
        }
        
        private static void CreateScreenshotsFolder()
        {
            try
            {
                Directory.CreateDirectory(Application.persistentDataPath + PATH_TO_SCREENSHOTS_FOLDER);
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong, IDK REALLY", ex);
            }
        }
        
        public static string GetPathToScreenshotsFolder()
        {
            if (!IsScreenshotsFolderExists())
            {
                CreateScreenshotsFolder();
                return Application.persistentDataPath + PATH_TO_SCREENSHOTS_FOLDER;
            }
            
            return Application.persistentDataPath + PATH_TO_SCREENSHOTS_FOLDER;
        }

        public static void BlockCursor()
        {
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;
        }
        
        public static async void MakeScreenshot()
        {
            string path = GetPathToScreenshotsFolder();
            var files = Directory.GetFiles(path).Length;
            int index = files;

            await UniTask.Delay(TimeSpan.FromSeconds(0.2f), DelayType.Realtime);
            ScreenCapture.CaptureScreenshot(path + $"/screenshot_{index}.png", 2);
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f), DelayType.Realtime);
        }

        public static string GetTimeInString(float time)
        {
            int milliseconds = Mathf.FloorToInt(time * 100f) % 100;
            int seconds = Mathf.FloorToInt(time % 60);
            int minutes = Mathf.FloorToInt(time / 60);
            return $"{minutes:00}:{seconds:00}:{milliseconds:00}";
        }
    }
}