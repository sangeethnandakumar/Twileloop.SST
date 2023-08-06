namespace Twileloop.SST.Demo
{
    internal class OtherService
    {
        private SST<MyAppState> session = SST<MyAppState>.Instance;

        public void ModifyState()
        {
            session.SetState( state => state.CurrentUserFName = "Navaneeth");
        }
    }
}
