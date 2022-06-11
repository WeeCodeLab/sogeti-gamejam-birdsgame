namespace birds_game.Assets.Scripts.Characters
{
    public class Seagull : BirdCharacter
    {
        public override float WalkingSpeed => 3;
        public override float JumpPower => 5;
        public override float TakeOffPower => 1;
        public override float Mass => 1;
        public override string Name => "Seagull";
    }
}