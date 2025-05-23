namespace Again.Runtime.Commands.Transfer
{
    public class HideTransferCommand : Command
    {
        public override void Execute()
        {
            var view = AgainSystem.Instance.TransferView;
            view.Hide(() => { Next(); });
        }
    }
}