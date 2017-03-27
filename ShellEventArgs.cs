namespace BattleCity
{
    public delegate void ShellEventHandler(object sender, BattleCity.ShellEventArgs e);

    public class ShellEventArgs
    {
        private Shell shell;

        public ShellEventArgs(Shell shell)
        {
            this.shell = shell;
        }

        public Shell Shell
        {
            get { return shell; }
            set { shell = value; }
        }
    }
}