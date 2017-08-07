namespace BattleCity
{
    public delegate void ShellEventHandler(object sender, ShellEventArgs e);

    public class ShellEventArgs
    {
        public Shell Shell { get; set; }

        public ShellEventArgs(Shell shell)
        {
            Shell = shell;
        }
    }
}