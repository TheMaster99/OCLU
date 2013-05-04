using System;

namespace Shards_CSharp
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (Shards game = new Shards())
            {
                game.Run();
            }
        }
    }
#endif
}
//No touchy ;)
