namespace SonicFramework
{
    public interface IPlayerContactable
    {
        public PlayerBase player { get; set; }

        public void OnContact();
        public void OnDiscontact();
    }
}
