using UnityEngine;

namespace RADCharacterController
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile2D : MonoBehaviour
    {
        public GameObject shooter;
        public float bulletSpeed;

        private Rigidbody2D rigidb;

        public float Damage { get; set; }

        private void Awake()
        {
            rigidb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            rigidb.AddForce(transform.right * bulletSpeed);
        }

        private void OnCollisionEnter2D(Collision2D collisionInfo)
        {
            if (collisionInfo.gameObject == shooter)
                return;

            IDamageable damageable = collisionInfo.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
                damageable.TakeDamage(Damage);

            Destroy(gameObject);
        }
    }
}