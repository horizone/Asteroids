using System;

namespace Asteroids
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Asteroids())
                game.Run();
        }
    }
#endif
}
