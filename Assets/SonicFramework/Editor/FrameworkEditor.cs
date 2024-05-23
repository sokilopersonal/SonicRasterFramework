using UnityEditor;
using UnityEngine;

namespace SonicFramework
{
    public static class FrameworkEditor
    {
        private static float distance = 8f;
        
        [MenuItem("Framework/Splines/Create Single Line Rings", priority = 0)]
        public static void CreateSingleLine()
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SonicFramework/Prefabs/Spawnables/Ring Splines/SingleLineRings.prefab");
            GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            var c = SceneView.lastActiveSceneView.camera;
            var t = c.transform;
            obj.transform.position = t.position + t.forward * distance;
            
            Undo.RegisterCreatedObjectUndo(obj, "Create Single Line Rings");

            Selection.activeGameObject = obj;
        }
        
        [MenuItem("Framework/Splines/Create Double Line Rings", priority = 1)]
        public static void CreateDoubleLine()
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SonicFramework/Prefabs/Spawnables/Ring Splines/DoubleLineRings.prefab");
            GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            var c = SceneView.lastActiveSceneView.camera;
            var t = c.transform;
            obj.transform.position = t.position + t.forward * distance;
            
            Undo.RegisterCreatedObjectUndo(obj, "Create Double Line Rings");

            Selection.activeGameObject = obj;
        }
        
        [MenuItem("Framework/Splines/Create Single Line LSD Rings", priority = 2)]
        public static void CreateLSDSingleLine()
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SonicFramework/Prefabs/Spawnables/Ring Splines/SingleLineLSDRings.prefab");
            GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            var c = SceneView.lastActiveSceneView.camera;
            var t = c.transform;
            obj.transform.position = t.position + t.forward * distance;
            
            Undo.RegisterCreatedObjectUndo(obj, "Create Single Line Rings");

            Selection.activeGameObject = obj;
        }
        
        #if UNITY_EDITOR
        [MenuItem("Framework/Create Start Pack")]
        public static void StartPack()
        {
            if (Camera.main != null) Object.DestroyImmediate(Camera.main.gameObject);

            GameObject stage = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SonicFramework/Prefabs/Spawnables/Stage.prefab");
            GameObject stageInstance = PrefabUtility.InstantiatePrefab(stage) as GameObject;

            GameObject holder = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SonicFramework/Prefabs/Players/CameraHolder.prefab");
            GameObject cam = PrefabUtility.InstantiatePrefab(holder) as GameObject;
            cam.name = cam.name.Replace("(Clone)", "");

            GameObject sceneContext =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SonicFramework/Prefabs/SceneContext/System.prefab");
            GameObject sceneContextInstance = PrefabUtility.InstantiatePrefab(sceneContext) as GameObject;
        }
        #endif
        
        [MenuItem("Framework/Open Screenshots Folder")]
        public static void OpenScreenshotsFolder()
        {
            EditorUtility.RevealInFinder(Framework.GetPathToScreenshotsFolder());
        }
    }
}