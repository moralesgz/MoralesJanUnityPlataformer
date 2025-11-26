using UnityEngine;

namespace Platformer2DSystem.Example
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float speed;

        private void FixedUpdate()
        {
            Vector3 position = transform.position;

            // Seguir en X
            position.x = Maths.Damp(position.x, target.position.x, speed, Time.fixedDeltaTime);

            // Seguir en Y
            position.y = Maths.Damp(position.y, target.position.y, speed, Time.fixedDeltaTime);

            // Mantener la Z original
            transform.position = position;
        }
    }
}
