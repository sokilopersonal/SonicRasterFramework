using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Toolbars;

namespace UnityEditor.Splines
{
    [EditorToolbarElement("Spline Tool Settings/Handle Rotation")]
    class HandleRotationDropdown : EditorToolbarDropdown
    {
        const string k_ParentRotationIconPath = "Packages/com.unity.splines/Editor/Resources/Icons/ToolHandleParent.png";
        const string k_ElementRotationIconPath = "Packages/com.unity.splines/Editor/Resources/Icons/ToolHandleElement.png";
        
        readonly List<GUIContent> m_OptionContents = new List<GUIContent>();

        public HandleRotationDropdown()
        {
            name = "Handle Rotation";

            RegisterCallback<AttachToPanelEvent>(AttachedToPanel);
            RegisterCallback<DetachFromPanelEvent>(DetachedFromPanel);

            clicked += OpenContextMenu;

            RefreshElementContent();
        }
        
        void OpenContextMenu()
        {
            var menu = new GenericMenu();
            // menu.AddItem(m_OptionContents[(int)HandleOrientation.Global], SplineTool.handleOrientation == HandleOrientation.Global,
            //     () => SetHandleOrientationIfNeeded(HandleOrientation.Global));
            //
            // menu.AddItem(m_OptionContents[(int)HandleOrientation.Local], SplineTool.handleOrientation == HandleOrientation.Local,
            //     () => SetHandleOrientationIfNeeded(HandleOrientation.Local));
            //
            // menu.AddItem(m_OptionContents[(int)HandleOrientation.Parent], SplineTool.handleOrientation == HandleOrientation.Parent,
            //     () => SetHandleOrientationIfNeeded(HandleOrientation.Parent));
            //
            // menu.AddItem(m_OptionContents[(int)HandleOrientation.Element], SplineTool.handleOrientation == HandleOrientation.Element,
            //     () => SetHandleOrientationIfNeeded(HandleOrientation.Element));

            menu.DropDown(worldBound);
        }

        void SetHandleOrientationIfNeeded(HandleOrientation handleOrientation)
        {
            if (SplineTool.handleOrientation != handleOrientation)
            {
                SplineTool.handleOrientation = handleOrientation;                
                RefreshElementContent();
            }
        }

        void RefreshElementContent()
        {
            var content = m_OptionContents[(int)SplineTool.handleOrientation];

            text = content.text;
            tooltip = content.tooltip;
            icon = content.image as Texture2D;
        }

        void AttachedToPanel(AttachToPanelEvent evt)
        {
            SplineTool.handleOrientationChanged += RefreshElementContent;
        }

        void DetachedFromPanel(DetachFromPanelEvent evt)
        {
            SplineTool.handleOrientationChanged -= RefreshElementContent;
        }
    }
}
