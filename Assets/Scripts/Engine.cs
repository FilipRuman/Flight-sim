
namespace Plane
{
    using UnityEditor.Callbacks;
    using UnityEngine;

    public class Engine : MonoBehaviour
    {
        public PlaneMain planeMain;



        public float throttle = 0;
        [SerializeField] private float throttleInertia = 2;


        public void UpdateThrottlePosition()
        {
            throttle = Mathf.Clamp01(throttle + planeMain.planeInputManager.throttle * Time.deltaTime / throttleInertia);
        }
        public float thrustModifier = 100000;
        private float GetThrustModifier()
        {
            // TODO later
            return throttle * thrustModifier;
        }

        public void UpdateThrust()
        {
            print("Apply thrust");
            GetThrustModifier();
            planeMain.rb.AddRelativeForce(Vector3.forward * GetThrustModifier(), mode: ForceMode.Force);

        }
    }

}
