using UnityEngine;

namespace RADCharacterController
{
    [RequireComponent(typeof(Weapon))]
    public class PlayerShooting : MonoBehaviour
    {
        public Transform gunParent;

        private Weapon gun;
        private bool isShooting;

        private void Awake()
        {
            gun = GetComponent<Weapon>();
            gun.Shooter = gameObject;
        }

        public void Reset()
        {
            isShooting = false;
        }

        private void Update()
        {
            if (isShooting)
                if (gun.CanShoot())
                    gun.InstantiateBullet();
        }

        public void AimAtMouse(Vector3 mousePosition)
        {
            mousePosition.z = 0;
            gunParent.right = mousePosition - transform.position;
        }

        public void ProcessInput(bool pressedShooting, bool releasedShooting)
        {
            if (pressedShooting)
                SetShoot(true);
            else if (releasedShooting)
                SetShoot(false);
        }

        public void SetShoot(bool value)
        {
            isShooting = value;
        }
    }
}