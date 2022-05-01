using System;

namespace WordGame.Utilities
{
    // Static class containing only small static methods to help with repeat tasks.
    // Mark all methods internal so that the scope is this assembly only.
    internal static class Helpers
    {
        public static bool Toggle(this bool value) => !value;
    }
}
