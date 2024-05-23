using UnityEditor;

namespace SonicFramework
{
    public static class Folders
    {
        [MenuItem("Framework/Screenshots Folder")]
        public static void OpenScreenshotsFolder()
        {
            var p = Framework.GetPathToScreenshotsFolder();
            EditorUtility.RevealInFinder(System.IO.Path.GetDirectoryName(p) + "/");
        }
    }
}
