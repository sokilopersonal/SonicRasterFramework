using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SonicFramework
{
    public class ObjectParameter : MonoBehaviour
    {
        [SerializeField] protected Object targetComponent;
        [SerializeField] protected string variableToSet; 
        [SerializeField] protected List<string> variables;
        
        protected string value;

        private void OnValidate()
        {
            Build();
        }

        private void Build()
        {
            if (targetComponent == null) return;

            var fields = targetComponent.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            variables.Clear();

            foreach (var field in fields)
            {
                variables.Add(field.Name + " = " + field.GetValue(targetComponent) + " (type: " + field.FieldType + ")");
            }
        }
        
        protected void Set()
        {
            var field = targetComponent.GetType().GetField(variableToSet, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            
            if (field.FieldType == typeof(float))
            {
                field.SetValue(targetComponent, float.Parse(value, CultureInfo.InvariantCulture));
            }
            else if (field.FieldType == typeof(int))
            {
                field.SetValue(targetComponent, int.Parse(value));
            }
            else if (field.FieldType == typeof(bool))
            {
                field.SetValue(targetComponent, bool.Parse(value));
            }
            else if (field.FieldType == typeof(Enum))
            {
                field.SetValue(targetComponent, Enum.Parse(field.FieldType, value));
            }
            
            Build();
        }
    }
}