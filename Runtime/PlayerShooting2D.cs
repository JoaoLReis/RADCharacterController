using UnityEngine;

namespace RADCharacterController
{
    [RequireComponent(typeof(Weapon2D))]
    public class PlayerShooting2D : MonoBehaviour
    {
        public Transform gunParent;

        private Weapon2D gun;
        private bool isShooting;

        private void Awake()
        {
            gun = GetComponent<Weapon2D>();
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