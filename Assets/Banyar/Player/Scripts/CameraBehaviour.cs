using UnityEngine;

namespace Banyar.Player {

    public class CameraBehaviour : MonoBehaviour
    {

        public Transform target;
        public float smoothTime = 0.3f;
        public Vector3 offset; // from camera holder
        private Vector3 velocity = Vector3.zero;

        void Start()
        {
            offset = new Vector3(-2f, 1f, -4f);
        }

        void Update()
        {
            if(target != null)
            {
                Vector3 targetPosition = target.position + offset;
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            }
        }
    }
}