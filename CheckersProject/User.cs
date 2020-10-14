namespace CheckersProject
{
    public class User
    {
        private readonly string r_Name;
        private readonly int r_PlayerNumber;
        private readonly eSymbols r_Symbol;
        private readonly eSymbols r_SymbolKing;
        private readonly GameDesk.ePositionOnDesk r_Position;

        public User(string i_Playername, int i_Number, eSymbols i_Symb, eSymbols i_KingSymb, GameDesk.ePositionOnDesk i_Position)
        {
            r_Name = i_Playername;
            r_PlayerNumber = i_Number;
            r_Symbol = i_Symb;
            r_SymbolKing = i_KingSymb;
            r_Position = i_Position;
        }

        public int PlayerNumber
        {
            get { return r_PlayerNumber; }
        }

        public string Name
        {
            get { return r_Name; }
        }

        public eSymbols Symbol
        {
            get { return r_Symbol; }
        }

        public eSymbols SymbolKing
        {
            get { return r_SymbolKing; }
        }

        public GameDesk.ePositionOnDesk Position
        {
            get { return r_Position; }
        }

        public void MakeMove(GameDesk i_Desk, string i_Move)
        {
            i_Desk.MakeMove(i_Move, r_Symbol, r_SymbolKing, Position);
        }
    }   
}
