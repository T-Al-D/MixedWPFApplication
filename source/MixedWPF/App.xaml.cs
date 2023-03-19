using Microsoft.EntityFrameworkCore.Infrastructure;
using MixedLibrary.DataAccess;
using System.Windows;

namespace MixedWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // method that executes every time the application starts up
        protected override void OnStartup(StartupEventArgs e)
        {
            // simplifies complex implementations -> simple interface
            DatabaseFacade databaseFacade = new DatabaseFacade(new SQLiteContext());

            // ensure that db exist, else create db
            databaseFacade.EnsureCreated();
        }
    }
}
