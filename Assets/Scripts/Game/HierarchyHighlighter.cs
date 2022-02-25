using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Highlighter
{
    public class HierarchyHighlighter : MonoBehaviour
    {
        public static readonly Color DEFAULT_BACKGROUND_COLOR = new Color(0.76f, 0.76f, 0.76f, 1f);

        public static readonly Color DEFAULT_BACKGROUND_COLOR_INACTIVE = new Color(0.306f, 0.396f, 0.612f, 1f);

        public static readonly Color DEFAULT_TEXT_COLOR = Color.black;

        public HierarchyHighlighter() { }

        public HierarchyHighlighter(Color inBackgroundColor)
        {
            this.backgroundColor = inBackgroundColor;
        }

        public HierarchyHighlighter(Color inBackgroundColor, Color inTextColor, FontStyle inFontStyle = FontStyle.Normal)
        {
            this.backgroundColor = inBackgroundColor;
            this.textColor = inTextColor;
            this.TextStyle = inFontStyle;
        }

        [Header("Active State")]
        public Color textColor = DEFAULT_TEXT_COLOR;

        public FontStyle TextStyle = FontStyle.Normal;

        public Color backgroundColor = DEFAULT_BACKGROUND_COLOR;

        [Header("Inactive State")]
        public bool customInactiveColors = false;

        public Color textColorInactive = DEFAULT_TEXT_COLOR;

        public FontStyle textStyleInactive = FontStyle.Normal;

        public Color backgroundColorInactive = DEFAULT_BACKGROUND_COLOR_INACTIVE;
    }
}
