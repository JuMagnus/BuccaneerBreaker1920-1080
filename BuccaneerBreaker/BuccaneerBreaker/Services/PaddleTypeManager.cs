namespace BuccaneerBreaker
{
    public interface IPaddle
    {
        public string currentType { get; set; }
    }

    public class PaddleTypeManager : IPaddle
    {
        //Variable modifiée par le choix du paddle et qui sera récupérée par le paddle lors de son instanciation pour savoir quelle attaque spéciale lancer
        public string currentType { get; set; }
        public PaddleTypeManager()
        {
            ServicesLocator.Register<IPaddle>(this);
        }
    }
}
