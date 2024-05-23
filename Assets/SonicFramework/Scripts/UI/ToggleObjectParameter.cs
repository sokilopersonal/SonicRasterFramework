using UnityEngine;

namespace SonicFramework.UI
{
    public class ToggleObjectParameter : ObjectParameter
    {
        public void SetToggle(bool value)
        {
            this.value = value.ToString();
            
            Set();
        }
    }
}