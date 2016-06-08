using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions{

	[Category("✫ Utility")]
	[Description("Check if an event is received and return true for one frame")]
	[EventReceiver("OnCustomEvent")]
	public class CheckEvent : ConditionTask<GraphOwner> {

		[RequiredField]
		public BBParameter<string> eventName;

		protected override string info{ get {return "[" + eventName + "]"; } }
		protected override bool OnCheck(){ return false; }
		public void OnCustomEvent(EventData receivedEvent){
			if (receivedEvent.name == eventName.value)
				YieldReturn(true);
		}
	}


	[Category("✫ Utility")]
	[Description("Check if an event is received and return true for one frame")]
	[EventReceiver("OnCustomEvent")]
	public class CheckEvent<T> : ConditionTask<GraphOwner> {

		[RequiredField]
		public BBParameter<string> eventName;
		[BlackboardOnly]
		public BBParameter<T> saveEventValue;

		protected override string info{ get {return string.Format("Event [{0}]\n{1} = EventValue", eventName, saveEventValue);} }
		protected override bool OnCheck(){ return false; }
		public void OnCustomEvent(EventData receivedEvent){
			if (receivedEvent.name == eventName.value){
				if (receivedEvent is EventData<T>){
					saveEventValue.value = (receivedEvent as EventData<T>).value;
				} else {
					Debug.Log(string.Format("An Event send which is of different value type than the '{0}' receiving it", this.name));
				}
				YieldReturn(true);
			}
		}		
	}
}