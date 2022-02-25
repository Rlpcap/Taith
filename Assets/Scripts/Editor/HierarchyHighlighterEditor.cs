﻿using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Highlighter;

namespace Assets.Editor
{
    [InitializeOnLoad]
    public class HierarchyHighlightManager
    {
        public static readonly Color DEFAULT_COLOR_HIERARCHY_SELECTED = new Color(0.243f, 0.4901f, 0.9058f, 1f);

        static HierarchyHighlightManager()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= HierarchyHighlight_OnGUI;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyHighlight_OnGUI;
        }

        private static void HierarchyHighlight_OnGUI(int inSelectionID, Rect inSelectionRect)
        {
            GameObject GOLabel = EditorUtility.InstanceIDToObject(inSelectionID) as GameObject;

            if (GOLabel != null)
            {
                HierarchyHighlighter label = GOLabel.GetComponent<HierarchyHighlighter>();

                if (label != null && Event.current.type == EventType.Repaint)
                {
                    #region Determine Styling

                    bool ObjectIsSelected = Selection.instanceIDs.Contains(inSelectionID);

                    Color BKCol = label.backgroundColor;
                    Color TextCol = label.textColor;
                    FontStyle TextStyle = label.TextStyle;

                    if (!label.isActiveAndEnabled)
                    {
                        if (label.customInactiveColors)
                        {
                            BKCol = label.backgroundColorInactive;
                            TextCol = label.textColorInactive;
                            TextStyle = label.textStyleInactive;
                        }
                        else
                        {
                            if (BKCol != HierarchyHighlighter.DEFAULT_BACKGROUND_COLOR)
                                BKCol.a = BKCol.a * 0f;

                            TextCol.a = TextCol.a * 0f;
                        }
                    }

                    #endregion


                    Rect Offset = new Rect(inSelectionRect.position + new Vector2(2f, 0f), inSelectionRect.size);


                    #region Draw Background

                    //Only draw background if background color is not completely transparent
                    if (BKCol.a > 0f)
                    {
                        Rect BackgroundOffset = new Rect(inSelectionRect.position, inSelectionRect.size);

                        //If the background has transparency, draw a solid color first
                        if (label.backgroundColor.a < 1f || ObjectIsSelected)
                        {
                            //ToDo: Pull background color from GUI.skin Style
                            EditorGUI.DrawRect(BackgroundOffset, HierarchyHighlighter.DEFAULT_BACKGROUND_COLOR);
                        }

                        //Draw background
                        if (ObjectIsSelected)
                            EditorGUI.DrawRect(BackgroundOffset, Color.Lerp(GUI.skin.settings.selectionColor, BKCol, 0.3f));
                        else
                            EditorGUI.DrawRect(BackgroundOffset, BKCol);
                    }

                    #endregion


                    EditorGUI.LabelField(Offset, GOLabel.name, new GUIStyle()
                    {
                        normal = new GUIStyleState() { textColor = TextCol },
                        fontStyle = TextStyle
                    });

                    EditorApplication.RepaintHierarchyWindow();
                }
            }
        }
    }
}
