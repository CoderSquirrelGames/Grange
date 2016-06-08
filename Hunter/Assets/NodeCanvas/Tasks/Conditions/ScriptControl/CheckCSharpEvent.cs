using System;
using System.Reflection;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions{

	[Category("✫ Script Control")]
	[Description("Will subscribe to an event of EventHandler type or custom handler with zero parameters and return type of void")]
	public class CheckCSharpEvent : ConditionTask {

		[SerializeField]
		private System.Type targetType;
		[SerializeField]
		private string eventName;

		public override Type agentType{
			get {return targetType ?? typeof(Transform);}
		}
		
		protected override string info{
			get
			{
				if (string.IsNullOrEmpty(eventName))
					return "No Event Selected";
				return string.Format("'{0}' Raised", eventName);
			}
		}


		protected override string OnInit(){
			
			var eventInfo = agentType.RTGetEvent(eventName);
			if (eventInfo == null)
				return "Event was not found";

			System.Action pointer = delegate { Raised(); };
			System.Action<object, EventArgs> pointer2 = delegate { Raised(); };
			if (eventInfo.EventHandlerType == typeof(EventHandler)){
				eventInfo.AddEventHandler( agent, pointer2 );
			} else {
				eventInfo.AddEventHandler( agent, pointer );
			}
			return null;
		}

		public void Raised(){
			YieldReturn(true);
		}

		protected override bool OnCheck(){
			return false;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnTaskInspectorGUI(){

			if (!Application.isPlaying && GUILayout.Button("Select Event")){
				Action<EventInfo> Selected = (e)=> {
					targetType = e.DeclaringType;
					eventName = e.Name;
				};

				if (agent != null){
					EditorUtils.ShowGameObjectEventSelectionMenu(agent.gameObject, Selected);
				} else {
					var menu = new UnityEditor.GenericMenu();
					foreach (var t in UserTypePrefs.GetPreferedTypesList(typeof(Component)))
						menu = EditorUtils.GetEventSelectionMenu(t, Selected, menu);
					menu.ShowAsContext();
					Event.current.Use();
				}
			}

			if (agentType != null){
				GUILayout.BeginVertical("box");
				UnityEditor.EditorGUILayout.LabelField("Selected Type", agentType.FriendlyName());
				UnityEditor.EditorGUILayout.LabelField("Selected Event", eventName);
				GUILayout.EndVertical();
			}
		}
		
		#endif
	}
}