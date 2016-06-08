#if UNITY_EDITOR

using System.Collections.Generic;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEditor;
using UnityEngine;


namespace NodeCanvas.Editor{

	public class PreferedTypesEditorWindow : EditorWindow {

		private List<System.Type> typeList;
		private Vector2 scrollPos;

		void OnEnable(){
			title = "Prefered Types";
			typeList = UserTypePrefs.GetPreferedTypesList(typeof(object));
		}

		void OnGUI(){
			
			EditorGUILayout.HelpBox("Here you can specify frequently used types for your game for easier access wherever you need to select a type\nFor example when setting the type of an Object variable as well as when setting the agent type in any 'Script Control' Task", MessageType.Info);

			if (GUILayout.Button("Add New Type")){
				GenericMenu.MenuFunction2 Selected = delegate(object t){
					if (!typeList.Contains( (System.Type)t)){
						typeList.Add( (System.Type)t);
						Save();
					} else {
						ShowNotification(new GUIContent("Type already in list") );
					}
				};	

				var menu = new UnityEditor.GenericMenu();
				foreach(var t in EditorUtils.GetAssemblyTypes(typeof(object))){
					var friendlyName = (string.IsNullOrEmpty(t.Namespace)? "No Namespace/" : t.Namespace.Replace(".", "/") + "/") + t.FriendlyName();
					var category = "Classes/";
					if (t.IsInterface) category = "Interfaces/";
					if (t.IsEnum) category = "Enumerations/";
					menu.AddItem(new GUIContent( category + friendlyName), false, Selected, t);
				}
				menu.ShowAsContext();
				Event.current.Use();
			}

			if (GUILayout.Button("RESET DEFAULTS")){
				UserTypePrefs.ResetTypeConfiguration();
				typeList = UserTypePrefs.GetPreferedTypesList(typeof(object));
				Save();
			}

			scrollPos = GUILayout.BeginScrollView(scrollPos);

			EditorUtils.ReorderableList(typeList, delegate(int i){
				GUILayout.BeginHorizontal("box");
				EditorGUILayout.LabelField(typeList[i].Name, typeList[i].Namespace);
				if (GUILayout.Button("X", GUILayout.Width(18))){
					typeList.RemoveAt(i);
					Save();
				}
				GUILayout.EndHorizontal();
			});

			GUILayout.EndScrollView();

			Repaint();
		}

		void Save(){
			UserTypePrefs.SetPreferedTypesList(typeList);
			ShowNotification(new GUIContent("Configuration Saved!"));
		}


		[MenuItem("Window/NodeCanvas/Prefered Types Editor")]
		public static void ShowWindow(){
			var window = GetWindow<PreferedTypesEditorWindow>();
			window.Show();
		}
	}
}

#endif