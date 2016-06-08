﻿using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Actions{

	[Category("✫ Blackboard")]
	[Description("Set a blackboard float variable")]
	public class SetInt : ActionTask{

		[BlackboardOnly]
		public BBParameter<int> valueA;
		public OperationMethod Operation = OperationMethod.Set;
		public BBParameter<int> valueB;

		protected override string info{
			get	{return valueA + OperationTools.GetOperationString(Operation) + valueB;}
		}

		protected override void OnExecute(){
			valueA.value = OperationTools.Operate(valueA.value, valueB.value, Operation);
			EndAction(true);
		}
	}
}