using System.Collections.Generic;
using UnityEngine;

namespace SonicFramework
{
    public class DebugDrawer
    {
        public List<DebugBox> boxes = new List<DebugBox>();

        public int count;
        
        public void DrawLabels()
        {
            foreach (DebugBox box in boxes)
            {
                GUI.Label(box.rect, box.info, box.style);
            }
        }

        public void Add(DebugBox box)
        {
            boxes.Add(box);
            count++;
        }
    }

    public class DebugBox
    {
        public Rect rect;
        public string info;
        public GUIStyle style;

        public DebugBox(Rect rect, string info, GUIStyle style)
        {
            this.rect = rect;
            this.info = info;
            this.style = style;
        }

        public void Draw()
        {
            GUI.Label(rect, info, style);
        }
    }
}