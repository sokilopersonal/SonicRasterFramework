using UnityEngine;

namespace SonicFramework
{
    [CreateAssetMenu(fileName = "Stage Info", menuName = "Framework/Stage Data", order = 0)]
    public class StageInfo : ScriptableObject
    {
        public string stageName;
        public string largeImageName;
    }
}