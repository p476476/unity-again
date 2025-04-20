namespace Again.Runtime.Commands
{
    public class PlaySoundCommand : Command
    {
        public string Name { get; set; }

        public override void Execute()
        {
            if (!IsSkip)
                AgainSystem.Instance.AudioManager.PlaySound(Name);
            AgainSystem.Instance.NextCommand();
        }
    }
}