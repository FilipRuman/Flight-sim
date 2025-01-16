namespace Plane
{
    using System.Collections;
    using UnityEngine;

    public class PlaneMain : MonoBehaviour
    {


        public Rigidbody rb;
        public PlaneInputManager planeInputManager;
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

                yield return new WaitForSeconds(1 / physicUpdatePerSecond);
            }
        }




    }
}
