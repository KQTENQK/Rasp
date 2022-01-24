using System.Collections.Generic;
using System.Linq;

namespace T5
{
    class Weapon
    {
        private List<Bullet> _bullets = new List<Bullet>();

        public bool CanShoot => _bullets.Count > 0;

        public void Shoot()
        {
            if (CanShoot)
                _bullets.Remove(_bullets.First());
        }
    }

    class Bullet { }
}
