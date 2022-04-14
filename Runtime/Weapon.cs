using UnityEngine;

namespace RADCharacterController
{
    public class Weapon : MonoBehaviour
    {
        public float damage;
        public float attackSpeed;
        public GameObject bulletPrefab;
        public Transform gunEnd;

        private float lastShotTime;

        public GameObject Shooter { get; set; }

        private void Start()
        {
            lastShotTime = Time.timeSinceLevelLoad;
        }

        public bool CanShoot()
        {
            return Time.timeSinceLevelLoad - lastShotTime >= attackSpeed;
        }

        public void InstantiateBullet()
        {
            lastShotTime = Time.timeSinceLevelLoad;
            var go = Instantiate(bulletPrefab, gunEnd.position, gunEnd.rotation);
            var bullet = go.GetComponent<Projectile>();
            bullet.shooter = Shooter;
            bullet.Damage = damage;
            Destroy(go, 2f);
        }
    }
}