namespace LandConquest.Forces
{
    public class ForcesEnum
    {
        public enum Infantry: int
        {
            Health = 10,
            Damage = 5,
            Range = 1,
            Movement = 1,
            Defence = 3
        }
        public enum Archers : int
        {
            Health = 10,
            Damage = 3,
            Range = 5,
            Movement = 2,
            Defence = 1
        }
        public enum Knights : int
        {
            Health = 30,
            Damage = 10,
            Range = 1,
            Movement = 4,
            Defence = 10
        }
        public enum Siege : int
        {
            Health = 10,
            Damage = 50,
            Range = 10,
            Movement = 1,
            Defence = 0
        }
    }
}
