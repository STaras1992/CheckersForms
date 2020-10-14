namespace CheckersProject
{
    using System;
    using System.Collections.Generic;

    public class ComputerDummy
    {
        private readonly string r_Name;
        private readonly int r_PlayerNumber;
        private readonly eSymbols r_Symbol;
        private readonly eSymbols r_SymbolKing;
        private readonly GameDesk.ePositionOnDesk r_Position;
        private List<Move> m_LegalMoves;
        private Move m_LastMove;
        private Random m_Rand;

        public ComputerDummy()
        {
            r_Name = "Dummy Comp";
            r_PlayerNumber = 2;
            r_Symbol = eSymbols.Player2;
            r_SymbolKing = eSymbols.Player2King;
            r_Position = GameDesk.ePositionOnDesk.UP;
            m_Rand = new Random();
            m_LegalMoves = new List<Move>();
            m_LastMove = new Move();
        }

        public ComputerDummy(string i_Name)
        {
            r_Name = i_Name;
            r_PlayerNumber = 2;
            r_Symbol = eSymbols.Player2;
            r_SymbolKing = eSymbols.Player2King;
            r_Position = GameDesk.ePositionOnDesk.UP;
            m_Rand = new Random();
            m_LegalMoves = new List<Move>();
            m_LastMove = new Move();
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

        private void refreshLegalMoves(GameDesk i_Desk, bool i_IsAfterKick) // find all available moves on desk for computer
        {
            bool isKickOption = false;
            bool isMustKick = false;
            string coordinates;

            m_LegalMoves.Clear();
            if (i_IsAfterKick)
            {
                isMustKick = i_Desk.CheckIfMustKick(m_LastMove.RowTo, m_LastMove.ColTo, r_Position);
                if (isMustKick)
                {
                    coordinates = i_Desk.GetKickOptions(m_LastMove.RowTo, m_LastMove.ColTo, r_Position);

                    for (int index = 0; index < coordinates.Length; index += 2)
                    {
                        m_LegalMoves.Add(new Move(m_LastMove.RowTo, m_LastMove.ColTo, int.Parse(coordinates[index].ToString()), int.Parse(coordinates[index + 1].ToString())));
                    }
                }
            }
            else
            {
                isKickOption = i_Desk.CheckForKickOption(r_Symbol, r_SymbolKing, r_Position); // player must kick some cell
                for (int i = 0; i < i_Desk.Size; i++)
                {
                    for (int j = 0; j < i_Desk.Size; j++)
                    {
                        if (i_Desk.Desk[i, j] == r_Symbol || i_Desk.Desk[i, j] == r_SymbolKing)
                        {
                            if (isKickOption && i_Desk.CheckIfCanKick(i, j, r_Position))
                            {
                                coordinates = i_Desk.GetKickOptions(i, j, r_Position);

                                for (int index = 0; index < coordinates.Length; index += 2)
                                {
                                    m_LegalMoves.Add(new Move(i, j, int.Parse(coordinates[index].ToString()), int.Parse(coordinates[index + 1].ToString())));
                                }
                            }
                            else if (!isKickOption)
                            {
                                if (i_Desk.CheckIfCanMove(i, j, i_Desk.Desk[i, j] == r_SymbolKing, r_Position))
                                {
                                    coordinates = i_Desk.GetMoveOptions(i, j, i_Desk.Desk[i, j] == r_SymbolKing, r_Position);
                                    for (int index = 0; index < coordinates.Length; index += 2)
                                    {
                                        m_LegalMoves.Add(new Move(i, j, int.Parse(coordinates[index].ToString()), int.Parse(coordinates[index + 1].ToString())));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void MakeMove(GameDesk i_Desk)
        {
            Move move;

            refreshLegalMoves(i_Desk, false);
            if (m_LegalMoves.Count != 0)
            {
                move = m_LegalMoves[m_Rand.Next(0, m_LegalMoves.Count)];
                i_Desk.MakeMove(move.RowFrom, move.ColFrom, move.RowTo, move.ColTo, r_Symbol, r_SymbolKing, r_Position);
                if (i_Desk.CalcMoveDistance(move.RowFrom, move.ColFrom, move.RowTo, move.ColTo, r_Position) == 2)
                {
                    while (i_Desk.CheckIfMustKick(move.RowTo, move.ColTo, r_Position))
                    {
                        m_LastMove = move;
                        refreshLegalMoves(i_Desk, true);
                        if (m_LegalMoves.Count == 0)
                        {
                            break;
                        }

                        move = m_LegalMoves[m_Rand.Next(0, m_LegalMoves.Count)];
                        i_Desk.MakeMove(move.RowFrom, move.ColFrom, move.RowTo, move.ColTo, r_Symbol, r_SymbolKing, r_Position);
                    }
                }
            }
        }
    }

    public struct Move
    {
        private int m_RowFrom;
        private int m_ColFrom;
        private int m_RowTo;
        private int m_ColTo;

        public Move(int i_RowFrom, int i_ColFrom, int i_RowTo, int i_ColTo)
        {
            m_RowFrom = i_RowFrom;
            m_ColFrom = i_ColFrom;
            m_RowTo = i_RowTo;
            m_ColTo = i_ColTo;
        }

        public int RowFrom
        {
            get { return m_RowFrom; }
        }

        public int RowTo
        {
            get { return m_RowTo; }
        }

        public int ColFrom
        {
            get { return m_ColFrom; }
        }

        public int ColTo
        {
            get { return m_ColTo; }
        }
    }       
}
