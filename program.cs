using System;

namespace purpleplatypus
{
    class Program
    {
#if NETFX_CORE 
        [MTAThread]
#else
        [STAThread]
#endif
        static void Main()
        {
            using (var program = new EmeClient())
                program.Run();
        }
    }
}