using Asteroids.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asteroids
{
    public class Asteroids : Game
    {
        GameState state;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D backgroundTexture;
        Player player;
        GameObjectManager gameObjectManager;
        KeyboardState previousKbState;

        public Asteroids()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = Globals.ScreenHeight;
            graphics.PreferredBackBufferWidth = Globals.ScreenWidth;
            Content.RootDirectory = "Content";
        }

        /* Allows the game to perform any initialization it needs to before starting to run.
           This is where it can query for any required services and load any non-graphic
           related content.  Calling base.Initialize will enumerate through any components
           and initialize them as well. */
        protected override void Initialize()
        {
            player = new Player(this);

            //Borde inte gameObjectManager hantera player också?
            Components.Add(player); //den kommer automatiskt att anropa update och draw
            gameObjectManager = new GameObjectManager(this);
            Components.Add(gameObjectManager);

            Messenger.Instance.Register<GameStateChangedMessage>(this, OnGameStateChangedCallback);
            Messenger.Instance.Send(new GameStateChangedMessage() { NewState = GameState.GetReady });
            base.Initialize();
        }

        private void OnGameStateChangedCallback(GameStateChangedMessage msg)
        {
            if (msg.NewState == state)
                return;

            switch(msg.NewState)
            {
                case GameState.GetReady:
                    player.Enabled = gameObjectManager.Enabled = false;
                    break;
                case GameState.Playing:
                    player.Enabled = gameObjectManager.Enabled = true;
                    break;
                case GameState.Dead:
                case GameState.Won:
                    break;
            }

            state = msg.NewState;
        }


        /* LoadContent will be called once per game and is the place to load
           all of your content. */
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundTexture = Content.Load<Texture2D>("backgroundtest");
        }

        /* UnloadContent will be called once per game and is the place to unload
           game-specific content. */
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /* Allows the game to run logic such as updating the world,
           checking for collisions, gathering input, and playing audio. */
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState KbState = Keyboard.GetState();

            switch(state)
            {
                case GameState.GetReady:
                    if (KbState.IsKeyDown(Keys.Space) && previousKbState.IsKeyUp(Keys.Space))
                        Messenger.Instance.Send(new GameStateChangedMessage() { NewState = GameState.Playing });
                        break;
                case GameState.Playing:
                    break;
            }

            // TODO: Add your update logic here

            previousKbState = KbState;
            base.Update(gameTime);
        }

        /* This is called when the game should draw itself.*/
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            for (int y = 0; y < Globals.ScreenHeight; y = y + backgroundTexture.Height)
            {
                for (int x = 0; x < Globals.ScreenWidth; x = x + backgroundTexture.Width)
                {
                    spriteBatch.Draw(backgroundTexture, new Vector2(x, y), Color.White);
                }
            }

            gameObjectManager.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
