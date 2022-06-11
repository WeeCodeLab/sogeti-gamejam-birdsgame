namespace birds_game.Assets.Scripts.Characters
{
    public abstract class BirdCharacter
    {
        public abstract float WalkingSpeed { get; }
        public abstract float JumpPower { get; }
        public abstract float TakeOffPower { get; }
        public abstract float Mass { get; }

        public abstract string Name { get; }
    }
}