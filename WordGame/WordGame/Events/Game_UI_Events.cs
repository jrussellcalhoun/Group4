using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WordGame
{
    // Game UI Event System
    //======================================================================================================================================

    public delegate void GuessMadeDelegate(object sender, EventArgs e);
    public class GuessMadeEventArgs : EventArgs
    {
        public string Guess { get; private set; }

        public GuessMadeEventArgs(string guess) => Guess = guess;
    }

    public delegate void HintRequestDelegate(object sender, EventArgs e);
    public class HintRequestEventArgs : EventArgs
    {
        //public string Hint { get; private set; }

        //public HintRequestEventArgs(string hint) => Hint = hint;
    }

}