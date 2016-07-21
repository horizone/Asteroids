using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    enum MeteorType
    {
        Big,
        Medium,
        Small
    }


    class Meteor : GameObject
    {
        public MeteorType Type { get; private set; }
        public float ExplosionScale { get; private set; }
        public Meteor(MeteorType type)
        {
            Type = type;

            switch(Type)
            {
                case MeteorType.Big:
                    CollisionRadius = 48;
                    ExplosionScale = 1.0f;
                    break;
                case MeteorType.Medium:
                    CollisionRadius = 21;
                    ExplosionScale = 0.5f;
                    break;
                case MeteorType.Small:
                    CollisionRadius = 8;
                    ExplosionScale = 0.2f;
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            Position = Position + Speed;

            if (Position.X < Globals.GameArea.Left)
                Position = new Vector2(Globals.GameArea.Right, Position.Y);

            if (Position.X > Globals.GameArea.Right)
                Position = new Vector2(Globals.GameArea.Left, Position.Y);

            if (Position.Y > Globals.GameArea.Bottom)
                Position = new Vector2(Position.X, Globals.GameArea.Top);

            if (Position.Y < Globals.GameArea.Top)
                Position = new Vector2(Position.X, Globals.GameArea.Bottom);

            Rotation = Rotation + 0.04f;
            if(Rotation > MathHelper.TwoPi)
            {
                Rotation = 0;
            }
        }

        public static IEnumerable<Meteor> BreakMeteor(Meteor meteor)
        {
            List<Meteor> meteors = new List<Meteor>();
            if (meteor.Type == MeteorType.Small)
                return meteors;

            for(int i = 0; i < 3; i++)
            {
                var angle = (float)Math.Atan2(meteor.Speed.Y, meteor.Speed.X) - MathHelper.PiOver4 + MathHelper.PiOver4 * i;

                meteors.Add( new Meteor(meteor.Type + 1)
                {
                    Position = meteor.Position,
                    Rotation = angle,
                    Speed = new Vector2( 
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * meteor.Speed.Length() * 0.5f
                });
            }
            return meteors;
        }
    }
}
