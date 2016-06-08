using DG.Tweening;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

    [Category("DOTween")]
    public class TweenPunchPosition : ActionTask<Transform> {

        public BBParameter<Vector3> ammount;
        public BBParameter<float> delay = 0f;
        public BBParameter<float> duration = 0.5f;
        public Ease easeType = Ease.Linear;
        public int vibrato = 10;
        public float elasticity = 1f;
        public bool useSnapping = false;
        public bool waitActionFinish = true;


        protected override void OnExecute() {

            if ( (agent.position - ammount.value).sqrMagnitude <= 0.01f)
                EndAction();
            
            var tween = agent.DOPunchPosition(ammount.value, duration.value, vibrato, elasticity, useSnapping);
            tween.SetDelay(delay.value);
            tween.SetEase(easeType);

            if (!waitActionFinish) EndAction();
        }

        protected override void OnUpdate() {
            if (elapsedTime >= duration.value + delay.value)
                EndAction();
        }
    }
}