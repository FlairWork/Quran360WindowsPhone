using System;

namespace Quran360
{
    public static class StateUtilities
    {
        private static Boolean isLaunching;

        public static Boolean IsLaunching
        {
            get { return isLaunching; }
            set { isLaunching = value; }
        }
    }
}
