using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1
{
    public interface IDamagable
    {
        void TakeDamage(int damage);
    }

    public class Weapon
    {
        private int _damage;
        private int _magazineSize;
        private Stack<Bullet> _bullets = new Stack<Bullet>();

        public IReadOnlyCollection<Bullet> Bullets => _bullets;

        public void Shoot()
        {
            _bullets.Pop().MoveForward();
        }

        public void Reload()
        {
            for (int i = 0; i < _magazineSize; i++)
            {
                _bullets.Push(new Bullet(_damage));
            }
        }
    }

    public class Bullet
    {
        private int _damage;
        
        public Bullet(int damage)
        {
            _damage = damage;
        }

        public int Damage => _damage;

        public void OnHitIDamagable(IDamagable damagable)
        {
            damagable.TakeDamage(_damage);
        }

        public void MoveForward() { }
    }

    public class Player : IDamagable
    {
        private int _health;

        public int Health => _health;

        public void TakeDamage(int damage)
        {
            _health -= damage;
        }
    }

    public class Bot
    {
        private Weapon _weapon;

        public void FireAt(Player player)
        {
            LookAt(player);
            _weapon.Shoot();
        }

        public void OnSeePlayer(Player player)
        {
            FireAt(player);
        }

        public void LookAt(Player player) { }
    }
}
