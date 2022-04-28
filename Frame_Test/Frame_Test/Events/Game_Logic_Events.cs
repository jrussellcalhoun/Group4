using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace Word_Game
{
    // Game Logic Event System
    //======================================================================================================================================

    /// <summary>
    /// Event that is called when the Frame control Page should change.
    /// </summary>
    /// <param name="sender"> The object instance from which the event is being sent. Usually this will be the (this) pointer keyword. </param>
    /// <param name="e"> Should be a new instance of ChangePageEventArgs. </param>
    public delegate void ChangePageEventDelegate(object sender, EventArgs e);

    /// <summary>
    /// Event that is called when one second has elapsed during the game round.
    /// </summary>
    /// <param name="sender"> The object instance from which the event is being sent. Usually this will be the (this) pointer keyword. </param>
    /// <param name="e"> Should be a new instance of TimerSecondElapsedEventArgs </param>
    //public delegate void TimerSecondElapsedEventDelegate(object sender, EventArgs e);

    /// <summary>
    /// Event that is called when it is determined that the current guess is incorrect.
    /// </summary>
    /// <param name="sender"> The object instance from which the event is being sent. Usually this will be the (this) pointer keyword. </param>
    /// <param name="e"> Should be a new instance of IncorrectGuessEventArgs </param>
    //public delegate void IncorrectGuessEventDelegate(object sender, EventArgs e);

    public class ChangePageEventArgs : EventArgs
    {
        public PageState CurrentPage { get; private set; }
        public ChangePageEventArgs(PageState currentPage)
        {
            CurrentPage = currentPage;
        }
    }

    //public class TimerSecondElapsedEventArgs : EventArgs
    //{
    //    public TimeSpan TimeLeft { get; private set; }
    //
    //    public TimerSecondElapsedEventArgs(TimeSpan timeSpan)
    //    {
    //        TimeLeft = timeSpan;
    //    }
    //}
    //
    //public class IncorrectGuessEventArgs : EventArgs
    //{
    //    public bool CheckApproved { get; private set; }
    //
    //    public string Guess { get; private set; }
    //
    //    public IncorrectGuessEventArgs(string guess, bool checkApproved)
    //    {
    //        Guess = guess;
    //        CheckApproved = checkApproved;
    //    }
    //}
}