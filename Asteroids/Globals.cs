﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class Globals
    {
        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

        public static Rectangle GameArea
        {
            get
            {
                return new Rectangle(-50, -50, ScreenWidth + 100, ScreenHeight + 100);
            }
        }

        public static Rectangle RespawnArea
        {
            get
            {
                return new Rectangle((int)CenterScreen.X - 200, (int)CenterScreen.Y - 200, 400, 400);
            }
        }

        public static Vector2 CenterScreen
        {
            get
            {
                return new Vector2(ScreenWidth / 2, ScreenHeight /2);
            }
        }
    }
}
