using jrowley_CapstoneSGPIA.Risk;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace jrowley_CapstoneSGPIA
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        Manager m = new Manager();
        MainWindow mw;

        protected override void OnStartup(StartupEventArgs e)
        {
            mw = new MainWindow();
            this.MainWindow = mw;
            mw.Show();
            m.tokens = mw.getTokens();
            m.mw = mw;

            ConsoleHelper.Show();
            for (int i = 0; i < 10; i++)
            {
                m.fSetup();
                //m.HvsLAISetup();
                m.vDistribute();
                m.StartGame();
                m = new Manager();
                m.tokens = mw.getTokens();
            }
            Console.WriteLine("Done.");


            Console.ReadKey();
            Environment.Exit(0);
            
        }
    }
}
