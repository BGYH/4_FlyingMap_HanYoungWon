using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlekGames.Systems;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AlekGames.editor
{
    [CustomEditor(typeof(hoverCraft))]
    public class hoverCraftEditor : Editor
    {
#if UNITY_EDITOR
        public override void OnInspectorGUI()
        {
            hoverCraft hc = (hoverCraft)target;

            GUILayout.Label("this is a system making a hoverCraft. use buttons at the bottom for faster setup", GUI.skin.box);
            GUILayout.Space(25);



            DrawDefaultInspector();



            GUILayout.Space(10);

            if (GUILayout.Button("copy HoverPoint settings from hoverPoint 0"))
            {
                hc.setUpHoverPoint();
            }

            GUILayout.Space(5);

            if (GUILayout.Button("find hover points from hoverPoint 0"))
            {
                hc.findHoverPointFromPoint(true);
            }

            GUILayout.Space(30);

            GUILayout.Label("very basic rigidBody setup, doesn't look at set values, it is here to show you working setup", GUI.skin.box);
            if (GUILayout.Button("setup RigidBody"))
            {
                hc.rigidbodySetup();
            }

            GUILayout.Space(5);

            GUILayout.Label("Basic hoverCraft setup, it is here to show you working setup", GUI.skin.box);
            if (GUILayout.Button("setup Deafult Values"))
            {
                hc.valuesSetup();
            }
        }
#endif
    }
}
