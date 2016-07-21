using Asteroids.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asteroids
{
    class GameObjectManager : DrawableGameComponent
    {

        Random rnd = new Random();

        List<Meteor> meteors = new List<Meteor>();
        Texture2D meteorBigTexture;
        Texture2D meteorMediumTexture;
        Texture2D meteorSmallTexture;

        List<Shot> shots = new List<Shot>();
        Texture2D laserTexture;
        SoundEffect laserSound;

        SoundEffect explosionSound;
        Texture2D explosionTexture;
        List<Explosion> explosions = new List<Explosion>();

        public GameObjectManager(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            ResetMeteors();
            Messenger.Instance.Register<AddShotMessage>(this, AddShotMessageCallback);
            base.Initialize();
        }

        private void AddShotMessageCallback(Messages.AddShotMessage msg)
        {
            shots.Add(msg.Shot);
            laserSound.Play();
        }

        public void ResetMeteors()
        {
            while (meteors.Count < 10)
            {
                var angle = rnd.Next() * MathHelper.TwoPi;
                var m = new Meteor(MeteorType.Big)
                {
                    Position = new Vector2(Globals.GameArea.Left + (float)rnd.NextDouble() * Globals.GameArea.Width,
                        Globals.GameArea.Top + (float)rnd.NextDouble() * Globals.GameArea.Height),
                    Rotation = angle,
                    Speed = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * rnd.Next(20, 60) / 30.0f
                };

                if (!Globals.RespawnArea.Contains(m.Position))
                    meteors.Add(m);
            }
        }


        protected override void LoadContent()
        {
            meteorBigTexture = Game.Content.Load<Texture2D>("meteorbrown_big4");
            meteorMediumTexture = Game.Content.Load<Texture2D>("meteorbrown_med1");
            meteorSmallTexture = Game.Content.Load<Texture2D>("meteorbrown_tiny1");

            laserTexture = Game.Content.Load<Texture2D>("laser");
            laserSound = Game.Content.Load<SoundEffect>("laserSound");

            explosionSound = Game.Content.Load<SoundEffect>("ReasonableExplosion");
            explosionTexture = Game.Content.Load<Texture2D>("explosion");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Shot shot in shots)
            {
                shot.Update(gameTime);

                //Om det här skottet krockar med en meteor så hämta den till meteor
                Meteor meteor = meteors.FirstOrDefault(m => m.CollidesWith(shot));

                //Ta bort meteoren om den var träffad av ett skott
                if (meteor != null)
                {
                    explosionSound.Play(1.0f, 0f, 0f);
                    meteors.Remove(meteor);
                    meteors.AddRange(Meteor.BreakMeteor(meteor));
                    explosions.Add(new Explosion() { Position = meteor.Position, Scale = meteor.ExplosionScale });
                    shot.IsDead = true;
                }
            }

            foreach (Explosion explosion in explosions)
            {
                explosion.Update(gameTime);
            }

            foreach (Meteor meteor in meteors)
            {
                meteor.Update(gameTime);
            }

            //LINQ för att ta bort alla skott som är döda eller utanför PlayArea
            shots.RemoveAll(s => s.IsDead || !Globals.GameArea.Contains(s.Position));

            //Ta bort explosioner som exploderat färdigt
            shots.RemoveAll(e => e.IsDead);

            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Shot s in shots)
            {
                spriteBatch.Draw(laserTexture, s.Position, null, Color.White, s.Rotation + MathHelper.PiOver2, new Vector2(laserTexture.Width / 2, laserTexture.Height / 2), 1.0f, SpriteEffects.None, 0f);
            }

            foreach (Meteor meteor in meteors)
            {
                Texture2D meteorTexture = meteorSmallTexture;

                switch (meteor.Type)
                {
                    case MeteorType.Big:
                        meteorTexture = meteorBigTexture;
                        break;
                    case MeteorType.Medium:
                        meteorTexture = meteorMediumTexture;
                        break;
                }

                spriteBatch.Draw(meteorTexture, meteor.Position, null, Color.White, meteor.Rotation + MathHelper.PiOver2, new Vector2(meteorTexture.Width / 2, meteorTexture.Height / 2), 1.0f, SpriteEffects.None, 0f);
            }

            foreach (Explosion explosion in explosions)
            {
                spriteBatch.Draw(explosionTexture, explosion.Position, null, explosion.Color, explosion.Rotation + MathHelper.PiOver2, new Vector2(explosionTexture.Width / 2, explosionTexture.Height / 2), explosion.Scale, SpriteEffects.None, 0f);
            }
        }
    }
}
