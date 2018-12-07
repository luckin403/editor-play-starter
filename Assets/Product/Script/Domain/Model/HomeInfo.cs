namespace Product.Domain.Model
{
    public class HomeInfo
    {
        public string PlayerName { get; private set; }

        public string Comment { get; private set; }

        public int PlayerLevel { get; private set; }

        public HomeInfo(string playerName, string comment, int playerLevel)
        {
            PlayerName = playerName;
            Comment = comment;
            PlayerLevel = playerLevel;
        }
    }
}