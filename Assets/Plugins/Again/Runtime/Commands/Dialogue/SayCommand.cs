namespace Again.Runtime.Commands.Dialogue
{
    public class SayCommand : Command
    {
        public string Text { get; set; }
        public string Character { get; set; }
        public string Voice { get; set; }

        public string Key { get; set; }

        public float Scale { get; set; } = 1f;

        public override void Execute()
        {
            AgainSystem.Instance.DialogueManager.ShowDialogue(this, Next);
        }
    }
}