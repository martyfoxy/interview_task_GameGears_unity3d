using Code.UI;
using UnityEngine;

namespace Code.Players
{
    public sealed class PlayerView : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        
        [SerializeField]
        private HpBarView hpBarView;

        public HpBarView HpBarView => hpBarView;

        private static readonly int Health = Animator.StringToHash("Health");
        private static readonly int Attack = Animator.StringToHash("Attack");

        public void ResetAnimator(Parameter hpParameter)
        {
            animator.SetInteger(Health, (int) hpParameter.InitValue);
            animator.SetBool(Attack, false);
        }
        
        public void SetHpParameter(Parameter hpParameter)
        {
            animator.SetInteger(Health, (int) hpParameter.Value);
        }

        public void PlayAttack()
        {
            animator.SetTrigger(Attack);
        }
    }
}