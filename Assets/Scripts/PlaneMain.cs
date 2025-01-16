namespace Plane
{
    using System.Collections;
    using Plane.Wing;
    using UnityEngine;

    public class PlaneMain : MonoBehaviour
    {


        public Rigidbody rb;
        public PlaneInputManager planeInputManager;
        public PlaneWingsManager planeWingsManager;
        public Engine engine;


        void Update()
        {
            engine.UpdateThrottlePosition();
        }

        private void Start()
        {
            StartCoroutine(PhysicsTick());
        }

        public float physicUpdatePerSecond = 120;
        public IEnumerator PhysicsTick()
        {
            while (true)
            {
                print("Test??");
                engine.UpdateThrust();
                planeWingsManager.CalculateWingsPhysics();
                yield return new WaitForSeconds(1 / physicUpdatePerSecond);
            }
        }




    }
}
