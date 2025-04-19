namespace Again.Runtime.Commands
{
    public class PlaySoundCommand : Command
    {
        public string Name { get; set; }

        public override void Execute()
        {
            AgainSystem.Instance.AudioManager.PlaySound(Name);
            AgainSystem.Instance.NextCommand();
        }
    }
}