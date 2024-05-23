using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace SonicFramework.UI
{
    public class SliderObjectParameter : ObjectParameter
    {
        public void SetSlider(float value)
        {
            this.value = value.ToString(CultureInfo.InvariantCulture);
            
            Set();
        }
    }
}