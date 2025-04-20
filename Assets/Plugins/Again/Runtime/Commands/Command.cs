namespace Again.Runtime.Commands
{
    public abstract class Command
    {
        public int Id { get; set; }
        public bool IsSkip { get; set; }
        public bool IsJoin { get; set; } = false;
        public abstract void Execute();

        public void Next()
        {
            if (IsJoin) return;
            AgainSystem.Instance.NextCommand();
        }
    }
}