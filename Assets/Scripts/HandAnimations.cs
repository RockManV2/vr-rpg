using UnityEngine;
using UnityEngine.InputSystem;

namespace Animations
{
    public class HandAnimations : MonoBehaviour
    {
        [SerializeField] 
        private InputActionProperty pinchAnimationAction;
        [SerializeField] 
        private InputActionProperty gripAnimationAction;
        [SerializeField] 
        private Animator handAnimator;

        private void Update()
        {
            var triggerValue = pinchAnimationAction.action.ReadValue<float>();
            handAnimator.SetFloat("Trigger", triggerValue);

            var gripValue = pinchAnimationAction.action.ReadValue<float>();
            handAnimator.SetFloat("Grip", gripValue);
        }
    }
}