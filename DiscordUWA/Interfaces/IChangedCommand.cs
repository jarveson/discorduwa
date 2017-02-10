using System.Windows.Input;

namespace DiscordUWA.Interfaces
{
    /// <summary>
    /// Extension of <see cref="ICommand"/> to allow manually call of <see cref="ICommand.CanExecuteChanged"/>
    /// </summary>
    public interface IChangedCommand:ICommand
    {
        void RaiseCanExecuteChanged();
    }
}