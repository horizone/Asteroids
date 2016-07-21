﻿using Asteroids.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class Player : DrawableGameComponent, IGameObject
    {
        public bool IsDead { get; set; }
        public Vector2 Position { get; set; }
        public float CollisionRadius { get; set; }
        public Vector2 Speed { get; set; }
        public float Rotation { get; set; }

        public bool CanShoot { get { return reloadTimer == 0; } }
        private Texture2D playerTexture;
        private int reloadTimer = 0;
        private Random rnd = new Random();

        public Player(Game game) : base(game)
        {
            Position = new Vector2(Globals.ScreenWidth / 2, Globals.ScreenHeight / 2);
        }

        protected override void LoadContent()
        {
            playerTexture = Game.Content.Load<Texture2D>("player");

            base.LoadContent();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerTexture, Position, null, Color.White, Rotation + MathHelper.PiOver2, new Vector2(playerTexture.Width / 2, playerTexture.Height / 2), 1.0f, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Up))
                Accelerate();
            if (state.IsKeyDown(Keys.Left))
                Rotation -= 0.05f;
            else if (state.IsKeyDown(Keys.Right))
                Rotation += 0.05f;

            if (state.IsKeyDown(Keys.Space) && CanShoot)
            {
                Messenger.Instance.Send(new AddShotMessage() { Shot = Shoot() });
            }

            Position = Position + Speed;

            if (reloadTimer > 0) reloadTimer--;

            if (Position.X < Globals.GameArea.Left)
                Position = new Vector2(Globals.GameArea.Right, Position.Y);

            if (Position.X > Globals.GameArea.Right)
                Position = new Vector2(Globals.GameArea.Left, Position.Y);

            if (Position.Y > Globals.GameArea.Bottom)
                Position = new Vector2(Position.X, Globals.GameArea.Top);

            if (Position.Y < Globals.GameArea.Top)
                Position = new Vector2(Position.X, Globals.GameArea.Bottom);

            base.Update(gameTime);
        }

        public void Accelerate()
        {
            Speed = Speed + new Vector2((float) Math.Cos(Rotation), (float) Math.Sin(Rotation)) * 0.08f;

            if(Speed.LengthSquared() > 25)
                Speed = Vector2.Normalize(Speed) * 5;
        }

        public Shot Shoot()
        {
            if (!CanShoot) return null;

            reloadTimer = 20;

            return new Shot()
            {
                Position = Position,
                Speed = Speed + 10f * new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)),
                Rotation = rnd.Next() * MathHelper.TwoPi
            };
        }
    }
}
