﻿#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;


namespace NodeCanvas.Editor {

    public class WelcomeWindow : EditorWindow {

        private readonly Texture2D docsIcon = EditorGUIUtility.FindTexture( "cs Script Icon" );
        private readonly Texture2D resourcesIcon = EditorGUIUtility.FindTexture("d_WelcomeScreen.AssetStoreLogo");
        private readonly Texture2D communityIcon = EditorGUIUtility.FindTexture("AudioChorusFilter Icon");

        void OnEnable() {
            title = "Welcome";
            var size = new Vector2(560, 370);
            minSize = size;
            maxSize = size;
        }

        void OnGUI() {

            GUI.skin.label.richText = true;
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginVertical();

            GUILayout.Space(10);
            GUILayout.Label("<size=26><b>Welcome to NodeCanvas v2.x</b></size>");
            GUILayout.Label(
                "<i>Thanks for using NodeCanvas. Following are a few important links to get you started</i>");

            GUILayout.Space(10);




            GUILayout.BeginHorizontal("box");
            GUI.backgroundColor = new Color(1,1,1,0f);
            if ( GUILayout.Button(docsIcon, GUILayout.Width(64), GUILayout.Height(64)) ) {
                Help.BrowseURL("http://nodecanvas.com/documentation/");
            }
            EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
            GUILayout.BeginVertical();
            GUILayout.Space(15);
            GUILayout.Label("<size=14><b>Documentation</b></size>\nRead the full documentation guide as well as API docs online");
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUI.backgroundColor = Color.white;





            GUILayout.BeginHorizontal("box");
            GUI.backgroundColor = new Color(1, 1, 1, 0f);
            if (GUILayout.Button(resourcesIcon, GUILayout.Width(64), GUILayout.Height(64)))
            {
                Help.BrowseURL("http://nodecanvas.com/resources/");
            }
            EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
            GUILayout.BeginVertical();
            GUILayout.Space(15);
            GUILayout.Label("<size=14><b>Resources</b></size>\nDownload samples and other resources");
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUI.backgroundColor = Color.white;





            GUILayout.BeginHorizontal("box");
            GUI.backgroundColor = new Color(1, 1, 1, 0f);
            if (GUILayout.Button(communityIcon, GUILayout.Width(64), GUILayout.Height(64)))
            {
                Help.BrowseURL("http://nodecanvas.com/resources/");
            }
            EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
            GUILayout.BeginVertical();
            GUILayout.Space(15);
            GUILayout.Label("<size=14><b>Community</b></size>\nJoin the online forums and get support");
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUI.backgroundColor = Color.white;



            GUILayout.FlexibleSpace();
            NCPrefs.showWelcomeWindow = EditorGUILayout.ToggleLeft("Show On Startup", NCPrefs.showWelcomeWindow);

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
        }

        [MenuItem("Window/NodeCanvas/WelcomeWindow")]
        public static void OpenWindow() {
            var window = CreateInstance<WelcomeWindow>();
            window.ShowUtility();
        }
    }
}

#endif