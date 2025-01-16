namespace Plane
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlaneInputManager : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;
        private const string actionMapName = "Plane";


        private const string rightStickName = "Right stick";
        private const string leftStickName = "Left stick";
        private const string throttleName = "Throttle";



        public Vector2 rightStick { get; private set; }
        public Vector2 leftStick { get; private set; }
        public float throttle { get; private set; }

        private void Awake()
        {
            InputActionMap inputActionMap = inputActions.FindActionMap(actionMapName);
            inputActionMap.FindAction(rightStickName).performed += ctx => rightStick = ctx.ReadValue<Vector2>();
            inputActionMap.FindAction(leftStickName).performed += ctx => leftStick = ctx.ReadValue<Vector2>();
            inputActionMap.FindAction(throttleName).performed += ctx => throttle = ctx.ReadValue<float>();
        }

    }
}