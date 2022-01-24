using System;

namespace T8
{
    class PlayerInfo
    {
        public string Name { get; private set; }
        public int Age { get; private set; }
    }

    class Player
    {
        private PlayerMover _mover;
        private Weapon _weapon;
        private PlayerInfo _info;
       
        public void Move()
        {
            _mover.Move();
        }

        public void Attack()
        {
            //attack
        }
    }

    class PlayerMover
    {
        public float DirectionX { get; private set; }
        public float DirectionY { get; private set; }
        public float Speed { get; private set; }

        public void Move() 
        {
            //Do move
        }
    }

    class Weapon
    {
        private int _damage;
        private float _shotDelay;

        public Weapon(int damage, float shotDelay)
        {
            _damage = damage;
            _shotDelay = shotDelay;
        }

        public int Damage => _damage;
        public float ShotDelay => _shotDelay;

        public bool IsReloading()
        {
            throw new NotImplementedException();
        }
    }
}
