namespace BattleCity
{
    public delegate void ShellEventHandler(object sender, ShellEventArgs e);

    public class ShellEventArgs
    {
        private Shell _shell;

        public ShellEventArgs(Shell shell)
        {
            _shell = shell;
        }

        public Shell Shell
        {
            get { return _shell; }
            set { _shell = value; }
        }
    }
}