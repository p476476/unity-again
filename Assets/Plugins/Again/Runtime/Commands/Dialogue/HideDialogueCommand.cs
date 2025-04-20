namespace Again.Runtime.Commands.Dialogue
{
    public class HideDialogueCommand : Command

    {
        public override void Execute()
        {
            AgainSystem.Instance.DialogueManager.Hide();
            Next();
        }
    }
}