using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class Shot : GameObject
    {

        public Shot()
        {
            CollisionRadius = 16;
        }

        public void Update(GameTime gameTime)
        {
            Position = Position + Speed;
            Rotation = Rotation + 0.08f;
            if (Rotation > MathHelper.TwoPi)
            {
                Rotation = 0;
            }
        }
    }
}
