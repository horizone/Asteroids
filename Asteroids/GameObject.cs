using Microsoft.Xna.Framework;

namespace Asteroids
{
    abstract class GameObject : IGameObject
    {
        public bool IsDead { get; set; }
        public Vector2 Position { get; set; }
        public float CollisionRadius { get; set; }
        public Vector2 Speed { get; set; }
        public float Rotation { get; set; }

        public bool CollidesWith(IGameObject other)
        {
            return (this.Position - other.Position).LengthSquared() <
                (CollisionRadius + other.CollisionRadius) * (CollisionRadius + other.CollisionRadius);
        }
    }
}
