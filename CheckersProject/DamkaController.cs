namespace CheckersProject
{
    using System;
    using System.Threading;

    public class DamkaController
    {
        private User m_Player1;
        private object m_Player2;       // can be Human or Computer
        private eGameMode m_GameMode;   // vs Human or vs Comp 
        private GameDesk m_Desk;        // game board
        private int m_DeskSize;         // size x size  board dimension
        private int m_PointsPlayer1;
        private int m_PointsPlayer2;
        private eGameStatus m_GameStatus;
        private User m_CurrentPlayer;
        private bool m_IsMustKickAgain;
        private string m_Last_move;

        public event Action<eSymbols[,], int> DeskUpdate;

        public event Action<string> AlertShow;

        public event Action<object> CurrentPlayerChange;

        public event Action<eGameStatus> GameOver;

        public DamkaController(int i_Size, string i_NamePlayer1, string i_NamePlayer2, bool i_TwoPlayersGame)
        {
            m_Desk = new GameDesk(i_Size);
            m_DeskSize = i_Size;
            m_Player1 = new User(i_NamePlayer1, 1, eSymbols.Player1, eSymbols.Player1King, GameDesk.ePositionOnDesk.DOWN);
            if (i_TwoPlayersGame)
            {
                m_Player2 = new User(i_NamePlayer2, 2, eSymbols.Player2, eSymbols.Player2King, GameDesk.ePositionOnDesk.UP);
                m_GameMode = eGameMode.HUMAN;
            }
            else
            {
                m_Player2 = new ComputerDummy(i_NamePlayer2);
                m_GameMode = eGameMode.COMPUTER;
            }

            m_GameStatus = eGameStatus.RUNNING;
            m_Last_move = null;
            m_CurrentPlayer = m_Player1;
            m_IsMustKickAgain = false;
            m_PointsPlayer1 = 0;
            m_PointsPlayer2 = 0;
        }

        public int Size
        {
            get { return m_DeskSize; }
        }

        public eGameMode GameMode { get => m_GameMode; }

        public int PointsPlayer1 { get => m_PointsPlayer1; }

        public int PointsPlayer2 { get => m_PointsPlayer2; }

        public eGameStatus GameStatus { get => m_GameStatus; set => m_GameStatus = value; }

        public User CurrentTurnPlayer { get => m_CurrentPlayer; }

        public string Player1Name
        {
            get { return m_Player1.Name; }
        }

        public string Player2Name
        {
            get
            {
                string name;

                if (m_GameMode == eGameMode.HUMAN)
                {
                    name = (m_Player2 as User).Name;
                }
                else
                {
                    name = (m_Player2 as ComputerDummy).Name;
                }

                return name;
            }
        }

        /// <summary>
        /// Start game loop.
        /// </summary>
        public void startSession()
        {
            OnDeskUpdate();
            OnPlayerChange(m_Player2);
        }

        private void resetSession()
        {
            m_Desk = new GameDesk(m_DeskSize);
            m_GameStatus = eGameStatus.RUNNING;
            m_Last_move = null;
            m_CurrentPlayer = m_Player1;
            m_IsMustKickAgain = false;
            startSession();
        }

        public bool PlayerMove(string i_Move)
        {
            bool isMoveDone = false;
            bool isGoodMove = false;
            bool isLastMoveChecker = false;

            if (m_Desk.CheckIfMoveIsLegal(i_Move, m_CurrentPlayer.Symbol, m_CurrentPlayer.SymbolKing, m_CurrentPlayer.Position))
            {
                isGoodMove = true;
            }
            else
            {
                OnAlertShow(m_Desk.Alert);
            }

            if (m_IsMustKickAgain && isGoodMove)
            {
                isLastMoveChecker = m_Last_move.Substring(2, 2).Equals(i_Move.Substring(0, 2)) ? true : false;
                if (isLastMoveChecker)
                {
                    m_CurrentPlayer.MakeMove(m_Desk, i_Move);
                    m_Last_move = i_Move;
                    m_IsMustKickAgain = m_Desk.CheckIfMustKickAfterMove(i_Move, m_CurrentPlayer.Position);
                    isMoveDone = m_IsMustKickAgain ? false : true;
                    OnDeskUpdate();
                }
            }
            else if (isGoodMove)
            {
                m_CurrentPlayer.MakeMove(m_Desk, i_Move);
                OnDeskUpdate();
                if (m_Desk.LastMoveDistance(i_Move, m_CurrentPlayer.Position) == 2)
                {
                    m_Last_move = i_Move;
                    m_IsMustKickAgain = m_Desk.CheckIfMustKickAfterMove(i_Move, m_CurrentPlayer.Position);
                    isMoveDone = m_IsMustKickAgain ? false : true;
                    if (m_CurrentPlayer == m_Player1)
                    {
                        CurrentPlayerChange.Invoke(m_Player2);
                    }
                    else
                    {
                        CurrentPlayerChange.Invoke(m_Player1);
                    }
                }
                else
                {
                    isMoveDone = true;
                }
            }

            return isMoveDone;
        }

        public void computerTurn()
        {
            if (m_GameMode == eGameMode.COMPUTER)
            {
                (m_Player2 as ComputerDummy).MakeMove(m_Desk);
            }

            OnDeskUpdate();
        }

        public void checkGameStatus()
        {
            eGameStatus gameStatus = eGameStatus.RUNNING;

            gameStatus = m_Desk.CheckGameStatus(m_GameMode, m_Player1, m_Player2);
            m_GameStatus = gameStatus;
            if (m_GameStatus != eGameStatus.RUNNING)
            {
                gameOver();
            }
        }

        private int calcPoints(int i_PlayerNum)
        {
            int points = 0;

            if (i_PlayerNum == 1)
            {
                points = m_Desk.calcPlayerPoints(m_Player1.Symbol, m_Player1.SymbolKing);
            }
            else if (m_GameMode == eGameMode.HUMAN)
            {
                points = m_Desk.calcPlayerPoints(((User)m_Player2).Symbol, ((User)m_Player2).SymbolKing);
            }
            else
            {
                points = m_Desk.calcPlayerPoints(((ComputerDummy)m_Player2).Symbol, ((ComputerDummy)m_Player2).SymbolKing);
            }

            return points;
        }

        public void TurnOver()
        {
            if (m_GameMode == eGameMode.COMPUTER)
            {
                computerTurn();
                OnPlayerChange(m_Player2);
            }
            else
            {
                if (m_CurrentPlayer == m_Player1)
                {
                    m_CurrentPlayer = m_Player2 as User;
                    OnPlayerChange(m_Player1);
                }
                else
                {
                    m_CurrentPlayer = m_Player1 as User;
                    OnPlayerChange(m_Player2);
                }
            }
        }

        private void gameOver()
        {
            if (m_GameStatus == eGameStatus.WIN_PLAYER1)
            {
                m_PointsPlayer1 += calcPoints(1);
            }
            else if (m_GameStatus == eGameStatus.WIN_PLAYER2)
            {
                m_PointsPlayer2 += calcPoints(2);
            }
            else  // TIE
            {
                int pointsPlayer1 = 0;
                int pointsPlayer2 = 0;

                pointsPlayer1 = calcPoints(1);
                pointsPlayer2 = calcPoints(2);

                if (pointsPlayer1 > pointsPlayer2)
                {
                    m_PointsPlayer1 += pointsPlayer1 - pointsPlayer2;
                }
                else if (pointsPlayer1 < pointsPlayer2)
                {
                    m_PointsPlayer1 += pointsPlayer2 - pointsPlayer1;
                }
            }

            OnGameOver();
            resetSession();
        }

        protected virtual void OnDeskUpdate()
        {
            if (DeskUpdate != null)
            {
                DeskUpdate.Invoke(m_Desk.Desk, m_DeskSize);
            }
        }

        protected virtual void OnAlertShow(string i_Message)
        {
            if (AlertShow != null)
            {
                AlertShow.Invoke(i_Message);
            }
        }

        protected virtual void OnPlayerChange(object i_Player)
        {
            if (CurrentPlayerChange != null)
            {
                CurrentPlayerChange.Invoke(i_Player);
            }
        }

        protected virtual void OnGameOver()
        {
            if (GameOver != null)
            {
                GameOver.Invoke(m_GameStatus);
            }
        }
    }

    public enum eGameMode
    {
        HUMAN,
        COMPUTER,
    }

    public enum eGameStatus
    {
        RUNNING,
        WIN_PLAYER1,
        WIN_PLAYER2,
        TIE,
        QUIT,
    }
}
