using System;

namespace WordGame
{
    /// <summary>
    /// Event that is called when the Frame control Page should change.
    /// </summary>
    /// <param name="sender"> The object instance from which the event is being sent. Usually this will be the (this) pointer keyword. </param>
    /// <param name="e"> Should be a new instance of ChangePageEventArgs. </param>
    public delegate void ChangePageEventDelegate(object sender, EventArgs e);

    public class ChangePageEventArgs : EventArgs
    {
        public PageState CurrentPage { get; private set; }
        public ChangePageEventArgs(PageState currentPage)
        {
            CurrentPage = currentPage;
        }
    }
}