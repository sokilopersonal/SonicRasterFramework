namespace SonicFramework
{
    public class PlayerSoundData
    {
        public string name;

        public static readonly PlayerSoundData Homing = new()
        {
            name = "Homing"
        };

        public static readonly PlayerSoundData Jump = new()
        {
            name = "Jump"
        };
        
        public static readonly PlayerSoundData Roll = new()
        {
            name = "Roll"
        };
        
        public static readonly PlayerSoundData ChargeDropDash = new()
        {
            name = "ChargeDropDash"
        };
        
        public static readonly PlayerSoundData DropDash = new()
        {
            name = "DropDash"
        };

        public static readonly PlayerSoundData Land = new()
        {
            name = "Land"
        };
    }
}