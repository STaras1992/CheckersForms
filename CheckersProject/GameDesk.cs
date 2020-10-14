namespace CheckersProject
{
    using System;
    using System.Text;

    public class GameDesk
    {
        private eSymbols[,] m_Desk;
        private int m_Size;
        private string m_FailMessage;

        public GameDesk(int i_Size)
        {
            m_Size = i_Size;
            m_Desk = new eSymbols[i_Size, i_Size];
            SetDefaultDesk(i_Size);
        }

        public eSymbols[,] Desk
        {
            get { return m_Desk; }
        }
    
        public int Size
        {
            get { return m_Size; }
        }

        public string Alert
        {
            get { return m_FailMessage; }
        }
    
        public void SetDefaultDesk(int i_Size)
        {
            int row = 0;

            while (row < ((i_Size / 2) - 1))
            {
                int i;

                if (row % 2 == 0)
                {
                    i = 1;
                    while (i < i_Size)
                    {
                        m_Desk[row, i] = eSymbols.Player2;
                        m_Desk[row, i - 1] = eSymbols.EmptyCell;
                        m_Desk[i_Size - row - 1, i - 1] = eSymbols.Player1;
                        m_Desk[i_Size - row - 1, i] = eSymbols.EmptyCell;
                        i += 2;
                    }
                }
                else
                {
                    i = 0;
                    while (i < i_Size)
                    {
                        m_Desk[row, i] = eSymbols.Player2;
                        m_Desk[row, i + 1] = eSymbols.EmptyCell;
                        m_Desk[i_Size - row - 1, i + 1] = eSymbols.Player1;
                        m_Desk[i_Size - row - 1, i] = eSymbols.EmptyCell;
                        i += 2;
                    }
                }

                ++row;
            }

            for (int k = 0; k < i_Size; k++)
            {
                m_Desk[row, k] = eSymbols.EmptyCell;
                m_Desk[row + 1, k] = eSymbols.EmptyCell;
            }
        }

        public bool CheckIfMoveIsLegal(string i_Move, eSymbols i_PlayerSymbol, eSymbols i_PlayerSymbolKing, ePositionOnDesk i_PlayerPosition)
        {
            bool isLegal = true;
            bool isMustKick = false;
            string coordinates;
            int rowFrom;
            int rowTo;
            int colFrom;
            int colTo;
            int distance;

            if (int.TryParse(i_Move, out int temp))
            {
                rowFrom = int.Parse(i_Move[0].ToString());
                colFrom = int.Parse(i_Move[1].ToString());
                rowTo = int.Parse(i_Move[2].ToString());
                colTo = int.Parse(i_Move[3].ToString());
            }
            else
            {
                coordinates = lettersToCoordinates(i_Move);
                rowFrom = int.Parse(coordinates[1].ToString());
                colFrom = int.Parse(coordinates[0].ToString());
                rowTo = int.Parse(coordinates[3].ToString());
                colTo = int.Parse(coordinates[2].ToString());
            }

            if (((rowFrom + colFrom) % 2 == 0) || ((rowTo + colTo) % 2 == 0)) // checkers played only on cells with odd sum of row and column
            {
                isLegal = false;
                m_FailMessage = "Move is illegal!Wrong cell selected.";
            }
            else if (!checkCellIsCorrect(rowFrom, colFrom, rowTo, colTo, i_PlayerSymbol, i_PlayerSymbolKing))
            {
                isLegal = false;
                m_FailMessage = "Move is illegal!Wrong cell selected.";
            }
            else if (!checkDirectionIsCorrect(rowFrom, colFrom, rowTo, colTo, i_PlayerPosition))
            {
                isLegal = false;
                m_FailMessage = "Move is illegal!Wrong direction.";
            }

            if (isLegal)
            {
                distance = checkDistance(rowFrom, rowTo, colFrom, colTo, i_PlayerPosition); // distance between "from cell" to "To cell".3 kick enemy ,2 regular move.
                if (distance < 0)
                {
                    isLegal = false;
                    m_FailMessage = "Move is illegal!";
                }
                else // check if we kick enemy checker or 3 step move
                {
                    /*if distance 1 we know that cell is empty (we check that before) so its legal.*/
                    isMustKick = CheckForKickOption(i_PlayerSymbol, i_PlayerSymbolKing, i_PlayerPosition);
                    if (distance == 1 && isMustKick)
                    {
                        isLegal = false; // beacause only if distance ==2 checker kicked
                        m_FailMessage = "You must eat enemy checker!";
                    }
                    else if (distance == 2)
                    {
                        isLegal = checkThreeCellMove(rowFrom, colFrom, rowTo, colTo, i_PlayerSymbol, i_PlayerSymbolKing, i_PlayerPosition);
                        if (!isLegal)
                        {
                            m_FailMessage = "Move is illegal!";
                        }
                    }
                }
            }

            return isLegal;
        }

        public bool CheckIfMustKickAfterMove(string i_LastMove, ePositionOnDesk i_PlayerPosition) // last move check if must eat again
        {
            bool isMustKick = false;
            string coordinates;
            int rowTo;
            int colTo;

            if (int.TryParse(i_LastMove, out int temp))
            {
                rowTo = int.Parse(i_LastMove[2].ToString());
                colTo = int.Parse(i_LastMove[3].ToString());
            }
            else
            {
                coordinates = lettersToCoordinates(i_LastMove);
                rowTo = int.Parse(coordinates[3].ToString());
                colTo = int.Parse(coordinates[2].ToString());
            }

            isMustKick = CheckIfMustKick(rowTo, colTo, i_PlayerPosition);

            return isMustKick;
        }  
        
        public bool CheckIfMustKick(int i_Row, int i_Col, ePositionOnDesk i_PlayerPosition)
        {
            bool isMustKick = false;
            string coordinates;
            eSymbols enemySymbol;

            if (m_Desk[i_Row, i_Col] == eSymbols.Player1 || m_Desk[i_Row, i_Col] == eSymbols.Player1King)
            {
                enemySymbol = eSymbols.Player2;
            }
            else
            {
                enemySymbol = eSymbols.Player1;
            }

            coordinates = checkForEnemyToKill(i_Row, i_Col, enemySymbol);
            if (!coordinates.Equals(string.Empty))
            {
                isMustKick = checkIfEnemyAvailable(coordinates, i_Row, i_Col, i_PlayerPosition);
            }

            return isMustKick;
        }

        public bool CheckForKickOption(eSymbols i_PlayerSymbol, eSymbols i_PlayerSymbolKing, ePositionOnDesk i_PlayerPosition)
        {
            bool isKickOption = false;
            bool isMustKick = false;

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (m_Desk[i, j] == i_PlayerSymbol || m_Desk[i, j] == i_PlayerSymbolKing)
                    {
                        isMustKick = CheckIfMustKick(i, j, i_PlayerPosition);
                        if (isMustKick)
                        {
                            isKickOption = true;
                            break;
                        }
                    }
                }

                if (isMustKick)
                {
                    break;
                }
            }

            return isKickOption;
        }

        public string GetKickOptions(int i_PlayerRow, int i_PlayerCol, ePositionOnDesk i_PlayerPosition)
        {          
            StringBuilder kickCoordinates = new StringBuilder();
            string enemyCoordinates = null;
            eSymbols enemySymbol;
            int enemyRow;
            int enemyCol;

            if (m_Desk[i_PlayerRow, i_PlayerCol] == eSymbols.Player2 || m_Desk[i_PlayerRow, i_PlayerCol] == eSymbols.Player2King)
            {
                enemySymbol = eSymbols.Player1;
            }
            else
            {
                enemySymbol = eSymbols.Player2;
            }

            enemyCoordinates = checkForEnemyToKill(i_PlayerRow, i_PlayerCol, enemySymbol);
            for (int i = 0; i < enemyCoordinates.Length; i += 2)
            {
                enemyRow = int.Parse(enemyCoordinates[i].ToString());
                enemyCol = int.Parse(enemyCoordinates[i + 1].ToString());
                if (m_Desk[i_PlayerRow, i_PlayerCol] == eSymbols.Player1 || m_Desk[i_PlayerRow, i_PlayerCol] == eSymbols.Player2) // not king, 
                {
                    if ((i_PlayerPosition == ePositionOnDesk.DOWN) && (enemyRow > i_PlayerRow))
                    {
                        continue;
                    }
                    else if ((i_PlayerPosition == ePositionOnDesk.UP) && (enemyRow < i_PlayerRow))
                    {
                        continue;
                    }
                }

                if (enemyRow > i_PlayerRow && enemyCol > i_PlayerCol)          // enemy down and right from player
                {
                    if (((enemyRow + 1) < Size) && ((enemyCol + 1) < Size))
                    {
                        if (m_Desk[enemyRow + 1, enemyCol + 1] == eSymbols.EmptyCell)
                        {
                            kickCoordinates.AppendFormat("{0}{1}", enemyRow + 1, enemyCol + 1);
                        }
                    }
                }
                else if (enemyRow > i_PlayerRow && enemyCol < i_PlayerCol)    // enemy down and left from player
                {
                    if (((enemyRow + 1) < Size) && ((enemyCol - 1) >= 0))
                    {
                        if (m_Desk[enemyRow + 1, enemyCol - 1] == eSymbols.EmptyCell)
                        {
                            kickCoordinates.AppendFormat("{0}{1}", enemyRow + 1, enemyCol - 1);
                        }
                    }
                }
                else if (enemyRow < i_PlayerRow && enemyCol > i_PlayerCol)    // enemy up and right from player
                {
                    if (((enemyRow - 1) >= 0) && ((enemyCol + 1) < Size))
                    {
                        if (m_Desk[enemyRow - 1, enemyCol + 1] == eSymbols.EmptyCell)
                        {
                            kickCoordinates.AppendFormat("{0}{1}", enemyRow - 1, enemyCol + 1);
                        }
                    }
                }
                else if (enemyRow < i_PlayerRow && enemyCol < i_PlayerCol)    // enemy up and left from player
                {
                    if (((enemyRow - 1) >= 0) && ((enemyCol - 1) >= 0))
                    {
                        if (m_Desk[enemyRow - 1, enemyCol - 1] == eSymbols.EmptyCell)
                        {
                            kickCoordinates.AppendFormat("{0}{1}", enemyRow - 1, enemyCol - 1);
                        }
                    }
                }
            }

            return kickCoordinates.ToString();
        }

        public string GetMoveOptions(int i_Row, int i_Col, bool i_IsKing, ePositionOnDesk i_Position) // return coordinates of available moves
        {
            StringBuilder moveCoordinates = new StringBuilder();

            if (i_IsKing)
            {
                if ((i_Row - 1) >= 0)
                {
                    if ((i_Col - 1) >= 0)
                    {
                        if (m_Desk[i_Row - 1, i_Col - 1] == eSymbols.EmptyCell)
                        {
                            moveCoordinates.AppendFormat("{0}{1}", i_Row - 1, i_Col - 1);
                        }
                    }

                    if ((i_Col + 1) <= (Size - 1))
                    {
                        if (m_Desk[i_Row - 1, i_Col + 1] == eSymbols.EmptyCell)
                        {
                            moveCoordinates.AppendFormat("{0}{1}", i_Row - 1, i_Col + 1);
                        }
                    }
                }
                else if ((i_Row + 1) <= (Size - 1))
                {
                    if ((i_Col - 1) >= 0)
                    {
                        if (m_Desk[i_Row + 1, i_Col - 1] == eSymbols.EmptyCell)
                        {
                            moveCoordinates.AppendFormat("{0}{1}", i_Row + 1, i_Col - 1);
                        }
                    }

                    if ((i_Col + 1) <= (Size - 1))
                    {
                        if (m_Desk[i_Row + 1, i_Col + 1] == eSymbols.EmptyCell)
                        {
                            moveCoordinates.AppendFormat("{0}{1}", i_Row + 1, i_Col + 1);
                        }
                    }
                }
            }
            else
            {
                if (i_Position == ePositionOnDesk.DOWN)
                {
                    if ((i_Row - 1) >= 0)
                    {
                        if ((i_Col - 1) >= 0)
                        {
                            if (m_Desk[i_Row - 1, i_Col - 1] == eSymbols.EmptyCell)
                            {
                                moveCoordinates.AppendFormat("{0}{1}", i_Row - 1, i_Col - 1);
                            }
                        }

                        if ((i_Col + 1) <= (Size - 1))
                        {
                            if (m_Desk[i_Row - 1, i_Col + 1] == eSymbols.EmptyCell)
                            {
                                moveCoordinates.AppendFormat("{0}{1}", i_Row - 1, i_Col + 1);
                            }
                        }
                    }
                }
                else
                {
                    if ((i_Row + 1) <= (Size - 1))
                    {
                        if ((i_Col - 1) >= 0)
                        {
                            if (m_Desk[i_Row + 1, i_Col - 1] == eSymbols.EmptyCell)
                            {
                                moveCoordinates.AppendFormat("{0}{1}", i_Row + 1, i_Col - 1);
                            }
                        }

                        if ((i_Col + 1) <= (Size - 1))
                        {
                            if (m_Desk[i_Row + 1, i_Col + 1] == eSymbols.EmptyCell)
                            {
                                moveCoordinates.AppendFormat("{0}{1}", i_Row + 1, i_Col + 1);
                            }
                        }
                    }
                }
            }

            return moveCoordinates.ToString();
        }

        private string checkForEnemyToKill(int i_StartRow, int i_StartCol, eSymbols i_EnemySymbol) // check for enemys around
        {
            StringBuilder enemyCoordinates = new StringBuilder();
            eSymbols kingEnemySymbol;

            if (i_EnemySymbol == eSymbols.Player2)
            {
                kingEnemySymbol = eSymbols.Player2King;
            }
            else
            {
                kingEnemySymbol = eSymbols.Player1King;
            }

            /*Check for enemy checker near cell that move starts from*/
            if (i_StartRow == 0)               // Move starts in upper row
            {
                if (i_StartCol == (Size - 1))  // right upper corner
                {
                    if (m_Desk[1, Size - 2] == i_EnemySymbol || m_Desk[1, Size - 2] == kingEnemySymbol)
                    {
                        enemyCoordinates.AppendFormat("{0}{1}", 1, Size - 2);
                    }
                }
                else                          // upper row without corners
                {
                    if (m_Desk[1, i_StartCol - 1] == i_EnemySymbol || m_Desk[1, i_StartCol - 1] == kingEnemySymbol)
                    {
                        enemyCoordinates.AppendFormat("{0}{1}", 1, i_StartCol - 1);
                    }

                    if (m_Desk[1, i_StartCol + 1] == i_EnemySymbol || m_Desk[1, i_StartCol + 1] == kingEnemySymbol)
                    {
                        enemyCoordinates.AppendFormat("{0}{1}", 1, i_StartCol + 1);
                    }
                }
            }
            else if (i_StartRow == (Size - 1)) // Move starts in down row
            {
                if (i_StartCol == 0)           // left down corner
                {
                    if (m_Desk[Size - 2, 1] == i_EnemySymbol || m_Desk[Size - 2, 1] == kingEnemySymbol)
                    {
                        enemyCoordinates.AppendFormat("{0}{1}", Size - 2, 1);
                    }
                }
                else                          // down row without corners                     
                {
                    if (m_Desk[Size - 2, i_StartCol - 1] == i_EnemySymbol || m_Desk[Size - 2, i_StartCol - 1] == kingEnemySymbol)
                    {
                        enemyCoordinates.AppendFormat("{0}{1}", Size - 2, i_StartCol - 1);
                    }

                    if (m_Desk[Size - 2, i_StartCol + 1] == i_EnemySymbol || m_Desk[Size - 2, i_StartCol + 1] == kingEnemySymbol)
                    {
                        enemyCoordinates.AppendFormat("{0}{1}", Size - 2, i_StartCol + 1);
                    }
                }
            }
            else if (i_StartCol == 0)                   // Move starts in left column no corners
            {
                if (m_Desk[i_StartRow - 1, 1] == i_EnemySymbol || m_Desk[i_StartRow - 1, 1] == kingEnemySymbol)
                {
                    enemyCoordinates.AppendFormat("{0}{1}", i_StartRow - 1, 1);
                }

                if (m_Desk[i_StartRow + 1, 1] == i_EnemySymbol || m_Desk[i_StartRow + 1, 1] == kingEnemySymbol)
                {
                    enemyCoordinates.AppendFormat("{0}{1}", i_StartRow + 1, 1);
                }
            }
            else if (i_StartCol == (Size - 1))          // Move starts in right column no corners
            {
                if (m_Desk[i_StartRow - 1, Size - 2] == i_EnemySymbol || m_Desk[i_StartRow - 1, Size - 2] == kingEnemySymbol)
                {
                    enemyCoordinates.AppendFormat("{0}{1}", i_StartRow - 1, Size - 2);
                }

                if (m_Desk[i_StartRow + 1, Size - 2] == i_EnemySymbol || m_Desk[i_StartRow + 1, Size - 2] == kingEnemySymbol)
                {
                    enemyCoordinates.AppendFormat("{0}{1}", i_StartRow + 1, Size - 2);
                }
            }
            else // move on middle of desk
            {
                if (m_Desk[i_StartRow - 1, i_StartCol - 1] == i_EnemySymbol || m_Desk[i_StartRow - 1, i_StartCol - 1] == kingEnemySymbol)
                {
                    enemyCoordinates.AppendFormat("{0}{1}", i_StartRow - 1, i_StartCol - 1);
                }

                if (m_Desk[i_StartRow - 1, i_StartCol + 1] == i_EnemySymbol || m_Desk[i_StartRow - 1, i_StartCol + 1] == kingEnemySymbol)
                {
                    enemyCoordinates.AppendFormat("{0}{1}", i_StartRow - 1, i_StartCol + 1);
                }

                if (m_Desk[i_StartRow + 1, i_StartCol - 1] == i_EnemySymbol || m_Desk[i_StartRow + 1, i_StartCol - 1] == kingEnemySymbol)
                {
                    enemyCoordinates.AppendFormat("{0}{1}", i_StartRow + 1, i_StartCol - 1);
                }

                if (m_Desk[i_StartRow + 1, i_StartCol + 1] == i_EnemySymbol || m_Desk[i_StartRow + 1, i_StartCol + 1] == kingEnemySymbol)
                {
                    enemyCoordinates.AppendFormat("{0}{1}", i_StartRow + 1, i_StartCol + 1);
                }
            }

            return enemyCoordinates.ToString();
        } 
        
        private bool checkIfEnemyAvailable(string i_EnemyCoordinates, int i_PlayerRow, int i_PlayerCol, ePositionOnDesk i_PlayerPosition) // check for legal eating move 
        {
            bool isAvailable = false;
            int enemyRow;
            int enemyCol;

            for (int i = 0; i < i_EnemyCoordinates.Length; i += 2)
            {
                enemyRow = int.Parse(i_EnemyCoordinates[i].ToString());
                enemyCol = int.Parse(i_EnemyCoordinates[i + 1].ToString());
                if (m_Desk[i_PlayerRow, i_PlayerCol] == eSymbols.Player2 || m_Desk[i_PlayerRow, i_PlayerCol] == eSymbols.Player1) // not king
                {
                    if ((i_PlayerPosition == ePositionOnDesk.DOWN) && (enemyRow > i_PlayerRow))
                    {
                        continue;
                    }
                    else if ((i_PlayerPosition == ePositionOnDesk.UP) && (enemyRow < i_PlayerRow))
                    {
                        continue;
                    }
                }

                if (enemyRow > i_PlayerRow && enemyCol > i_PlayerCol)          // enemy down and right from player
                {
                    if (((enemyRow + 1) < Size) && ((enemyCol + 1) < Size))
                    {
                        if (m_Desk[enemyRow + 1, enemyCol + 1] == eSymbols.EmptyCell)
                        {
                            isAvailable = true;
                            break;
                        }
                    }
                }
                else if (enemyRow > i_PlayerRow && enemyCol < i_PlayerCol)    // enemy down and left from player
                {
                    if (((enemyRow + 1) < Size) && ((enemyCol - 1) >= 0))
                    {
                        if (m_Desk[enemyRow + 1, enemyCol - 1] == eSymbols.EmptyCell)
                        {
                            isAvailable = true;
                            break;
                        }
                    }
                }
                else if (enemyRow < i_PlayerRow && enemyCol > i_PlayerCol)    // enemy up and right from player
                {
                    if (((enemyRow - 1) >= 0) && ((enemyCol + 1) < Size))
                    {
                        if (m_Desk[enemyRow - 1, enemyCol + 1] == eSymbols.EmptyCell)
                        {
                            isAvailable = true;
                            break;
                        }
                    }
                }
                else if (enemyRow < i_PlayerRow && enemyCol < i_PlayerCol)    // enemy up and left from player
                {
                    if (((enemyRow - 1) >= 0) && ((enemyCol - 1) >= 0))
                    {
                        if (m_Desk[enemyRow - 1, enemyCol - 1] == eSymbols.EmptyCell)
                        {
                            isAvailable = true;
                            break;
                        }
                    }
                }
            }

            return isAvailable;
        } 
        
        private string lettersToCoordinates(string i_MoveString) // convert Af>Be to numeric coordinates
        {
            StringBuilder coordString = new StringBuilder();

            coordString.Append(i_MoveString[0] - 65);
            coordString.Append(i_MoveString[1] - 97);
            coordString.Append(i_MoveString[3] - 65);
            coordString.Append(i_MoveString[4] - 97);

            return coordString.ToString();
        } 
        
        private bool checkCellIsCorrect(int i_RowFrom, int i_ColFrom, int i_RowTo, int i_ColTo, eSymbols i_PlayerSymbol, eSymbols i_PlayerSymbolKing) // celected cells are legal or not
        {
            bool isCorrect = true;

            if ((m_Desk[i_RowFrom, i_ColFrom] != i_PlayerSymbol) && (m_Desk[i_RowFrom, i_ColFrom] != i_PlayerSymbolKing))
            {
                isCorrect = false;
            }

            if (isCorrect && (m_Desk[i_RowTo, i_ColTo] != eSymbols.EmptyCell))
            {
                isCorrect = false;
            }

            return isCorrect;
        } 
        
        private int checkDistance(int i_RowFrom, int i_RowTo, int i_ColFrom, int i_ColTo, ePositionOnDesk i_PlayerPosition) // diagonal distance between two cells.if move =1,if eating =2,else -1
        {
            bool isCorrect = true;
            int row_distance;
            int col_distance;
            int returnDistance = -1;
            eSymbols playerSymbol;

            playerSymbol = Desk[i_RowFrom, i_ColFrom];
            if (playerSymbol == eSymbols.EmptyCell)
            {
                playerSymbol = Desk[i_RowTo, i_ColTo];
            }

            col_distance = Math.Abs(i_ColFrom - i_ColTo);
            if (playerSymbol == eSymbols.Player1King || playerSymbol == eSymbols.Player2King) // king can move every direction
            {
                row_distance = Math.Abs(i_RowFrom - i_RowTo);
            }
            else
            {
                if (i_PlayerPosition == ePositionOnDesk.DOWN)
                {
                    row_distance = i_RowFrom - i_RowTo;
                }
                else
                {
                    row_distance = i_RowTo - i_RowFrom;
                }
            }

            if ((row_distance != col_distance) || (row_distance > 2))
            {
                isCorrect = false;
            }

            if (isCorrect)
            {
                returnDistance = row_distance;
            }

            return returnDistance;
        }

        public int LastMoveDistance(string i_Move, ePositionOnDesk i_Position)
        {
            string coordinates;
            int rowFrom;
            int rowTo;
            int colFrom;
            int colTo;
            int distance;

            if (int.TryParse(i_Move, out int temp))
            {
                rowFrom = int.Parse(i_Move[0].ToString());
                colFrom = int.Parse(i_Move[1].ToString());
                rowTo = int.Parse(i_Move[2].ToString());
                colTo = int.Parse(i_Move[3].ToString());
            }
            else
            {
                coordinates = lettersToCoordinates(i_Move);
                rowFrom = int.Parse(coordinates[1].ToString());
                colFrom = int.Parse(coordinates[0].ToString());
                rowTo = int.Parse(coordinates[3].ToString());
                colTo = int.Parse(coordinates[2].ToString());
            }

            distance = checkDistance(rowFrom, rowTo, colFrom, colTo, i_Position);

            return distance;
        }

        public int CalcMoveDistance(int i_RowFrom, int i_ColFrom, int i_RowTo, int i_ColTo, ePositionOnDesk i_PlayerPosition)
        {
            int distance;

            distance = checkDistance(i_RowFrom, i_RowTo, i_ColFrom, i_ColTo, i_PlayerPosition);

            return distance;
        }    

        private bool checkDirectionIsCorrect(int i_RowFrom, int i_ColFrom, int i_RowTo, int i_ColTo, ePositionOnDesk i_PlayerPosition) // check if moving up or down desk direction if not king.
        {
            bool isCorrect = true;

            if (!(m_Desk[i_RowFrom, i_ColFrom] == eSymbols.Player2King || m_Desk[i_RowFrom, i_ColFrom] == eSymbols.Player1King))
            {
                if (i_PlayerPosition == ePositionOnDesk.DOWN && i_RowFrom < i_RowTo)
                {
                    isCorrect = false;
                }
                else if (i_PlayerPosition == ePositionOnDesk.UP && i_RowFrom > i_RowTo)
                {
                    isCorrect = false;
                }
            }

            return isCorrect;
        } 
    
        private bool checkThreeCellMove(int i_RowFrom, int i_ColFrom, int i_RowTo, int i_ColTo, eSymbols i_PlayerSymbol, eSymbols i_PlayerSymbolKing, ePositionOnDesk i_PlayerPosition) // check if three cell move i slegal
        {
            bool isLegal = true;

            if (m_Desk[i_RowFrom, i_ColFrom] == i_PlayerSymbol) // not king   
            {
                if (i_PlayerPosition == ePositionOnDesk.DOWN && (i_ColTo > i_ColFrom)) // right side from "start move cell"
                {
                    if (!((m_Desk[i_RowFrom - 1, i_ColFrom + 1] != eSymbols.EmptyCell) && (m_Desk[i_RowFrom - 1, i_ColFrom + 1] != i_PlayerSymbol) && (m_Desk[i_RowFrom - 1, i_ColFrom + 1] != i_PlayerSymbolKing))) // not enemy checker
                    {
                        isLegal = false;
                    }
                }
                else if (i_PlayerPosition == ePositionOnDesk.DOWN && (i_ColTo < i_ColFrom)) // left side from "start move cell"
                {
                    if (!((m_Desk[i_RowFrom - 1, i_ColFrom - 1] != eSymbols.EmptyCell) && (m_Desk[i_RowFrom - 1, i_ColFrom - 1] != i_PlayerSymbol) && (m_Desk[i_RowFrom - 1, i_ColFrom - 1] != i_PlayerSymbolKing))) // not enemy checker
                    {
                        isLegal = false;
                    }
                }
                else if (i_PlayerPosition == ePositionOnDesk.UP && (i_ColTo > i_ColFrom)) // right side from "start move cell"
                {
                    if (!((m_Desk[i_RowFrom + 1, i_ColFrom + 1] != eSymbols.EmptyCell) && (m_Desk[i_RowFrom + 1, i_ColFrom + 1] != i_PlayerSymbol) && (m_Desk[i_RowFrom + 1, i_ColFrom + 1] != i_PlayerSymbolKing))) // not enemy checker
                    {
                        isLegal = false;
                    }
                }
                else if (i_PlayerPosition == ePositionOnDesk.DOWN && (i_ColTo < i_ColFrom)) // left side from "start move cell"
                {
                    if (!((m_Desk[i_RowFrom + 1, i_ColFrom - 1] != eSymbols.EmptyCell) && (m_Desk[i_RowFrom + 1, i_ColFrom - 1] != i_PlayerSymbol) && (m_Desk[i_RowFrom + 1, i_ColFrom - 1] != i_PlayerSymbolKing))) // not enemy checker
                    {
                        isLegal = false;
                    }
                }
            }
            else // king
            {
                if ((i_ColTo < i_ColFrom) && (i_RowTo < i_RowFrom)) // Up and  left
                {
                    if (!((m_Desk[i_RowFrom - 1, i_ColFrom - 1] != eSymbols.EmptyCell) && (m_Desk[i_RowFrom - 1, i_ColFrom - 1] != i_PlayerSymbol) && (m_Desk[i_RowFrom - 1, i_ColFrom - 1] != i_PlayerSymbolKing))) // not enemy checker
                    {
                        isLegal = false;
                    }
                }
                else if ((i_ColTo < i_ColFrom) && (i_RowTo > i_RowFrom)) // Down and left
                {
                    if (!((m_Desk[i_RowFrom + 1, i_ColFrom - 1] != eSymbols.EmptyCell) && (m_Desk[i_RowFrom + 1, i_ColFrom - 1] != i_PlayerSymbol) && (m_Desk[i_RowFrom + 1, i_ColFrom - 1] != i_PlayerSymbolKing))) // not enemy checker
                    {
                        isLegal = false;
                    }
                }
                else if ((i_ColTo > i_ColFrom) && (i_RowTo < i_RowFrom)) // Up and right
                {
                    if (!((m_Desk[i_RowFrom - 1, i_ColFrom + 1] != eSymbols.EmptyCell) && (m_Desk[i_RowFrom - 1, i_ColFrom + 1] != i_PlayerSymbol) && (m_Desk[i_RowFrom - 1, i_ColFrom + 1] != i_PlayerSymbolKing))) // not enemy checker
                    {
                        isLegal = false;
                    }
                }
                else if ((i_ColTo > i_ColFrom) && (i_RowTo > i_RowFrom)) // Down and right
                {
                    if (!((m_Desk[i_RowFrom + 1, i_ColFrom + 1] != eSymbols.EmptyCell) && (m_Desk[i_RowFrom + 1, i_ColFrom + 1] != i_PlayerSymbol) && (m_Desk[i_RowFrom + 1, i_ColFrom + 1] != i_PlayerSymbolKing))) // not enemy checker
                    {
                        isLegal = false;
                    }
                }
            }

            return isLegal;
        }
    
        public eGameStatus CheckGameStatus(eGameMode gameMode, User i_Player1, object i_Player2)
        {
            bool isPlayer1CanResume = false;
            bool isPlayer2CanResume = false;
            eGameStatus gameStatus = eGameStatus.RUNNING;
            
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (m_Desk[i, j] == eSymbols.EmptyCell)
                    {
                        continue;
                    }
                    else
                    {
                        if ((m_Desk[i, j] == i_Player1.Symbol) && !isPlayer1CanResume)
                        {
                            isPlayer1CanResume = CheckIfCanMove(i, j, false, i_Player1.Position);
                            if (!isPlayer1CanResume)
                            {
                                isPlayer1CanResume = CheckIfCanKick(i, j, i_Player1.Position);
                            }
                        }
                        else if (m_Desk[i, j] == i_Player1.SymbolKing && !isPlayer1CanResume)
                        {
                            isPlayer1CanResume = CheckIfCanMove(i, j, true, i_Player1.Position);
                            if (!isPlayer1CanResume)
                            {
                                isPlayer1CanResume = CheckIfCanKick(i, j, i_Player1.Position);
                            }
                        }
                        else if (!isPlayer2CanResume)
                        {
                            if (gameMode == eGameMode.HUMAN)
                            {
                                if (m_Desk[i, j] == ((User)i_Player2).Symbol)
                                {
                                    isPlayer2CanResume = CheckIfCanMove(i, j, false, ((User)i_Player2).Position);
                                    if (!isPlayer2CanResume)
                                    {
                                        isPlayer2CanResume = CheckIfCanKick(i, j, ((User)i_Player2).Position);
                                    }
                                }
                                else if (m_Desk[i, j] == ((User)i_Player2).SymbolKing)
                                {
                                    isPlayer2CanResume = CheckIfCanMove(i, j, true, ((User)i_Player2).Position);
                                    if (!isPlayer2CanResume)
                                    {
                                        isPlayer2CanResume = CheckIfCanKick(i, j, ((User)i_Player2).Position);
                                    }
                                }
                            }
                            else
                            {
                                if (m_Desk[i, j] == ((ComputerDummy)i_Player2).Symbol)
                                {
                                    isPlayer2CanResume = CheckIfCanMove(i, j, false, ((ComputerDummy)i_Player2).Position);
                                    if (!isPlayer2CanResume)
                                    {
                                        isPlayer2CanResume = CheckIfCanKick(i, j, ((ComputerDummy)i_Player2).Position);
                                    }
                                }
                                else if (m_Desk[i, j] == ((ComputerDummy)i_Player2).SymbolKing)
                                {
                                    isPlayer2CanResume = CheckIfCanMove(i, j, true, ((ComputerDummy)i_Player2).Position);
                                    if (!isPlayer2CanResume)
                                    {
                                        isPlayer2CanResume = CheckIfCanKick(i, j, ((ComputerDummy)i_Player2).Position);
                                    }
                                }
                            }
                        }
                    }
                }

                if (isPlayer1CanResume && isPlayer2CanResume)
                {
                    break;
                }
            }

            if (!isPlayer1CanResume && !isPlayer2CanResume)
            {
                gameStatus = eGameStatus.TIE;
            }
            else if (isPlayer1CanResume && !isPlayer2CanResume)
            {
                gameStatus = eGameStatus.WIN_PLAYER1;
            }
            else if (!isPlayer1CanResume && isPlayer2CanResume)
            {
                gameStatus = eGameStatus.WIN_PLAYER2;
            }

            return gameStatus;
        }
        
        public bool CheckIfCanMove(int i_Row, int i_Col, bool i_IsKing, ePositionOnDesk i_Position) // check if there some move from current cell 
        {
            bool isCanMove = false;

            if (i_IsKing)
            {
                if ((i_Row - 1) >= 0)
                {
                    if ((i_Col - 1) >= 0)
                    {
                        if (m_Desk[i_Row - 1, i_Col - 1] == eSymbols.EmptyCell)
                        {
                            isCanMove = true;
                        }
                    }

                    if ((i_Col + 1) <= (Size - 1))
                    {
                        if (m_Desk[i_Row - 1, i_Col + 1] == eSymbols.EmptyCell)
                        {
                            isCanMove = true;
                        }
                    }
                }
                else if ((i_Row + 1) <= (Size - 1))
                {
                    if ((i_Col - 1) >= 0)
                    {
                        if (m_Desk[i_Row + 1, i_Col - 1] == eSymbols.EmptyCell)
                        {
                            isCanMove = true;
                        }
                    }

                    if ((i_Col + 1) <= (Size - 1))
                    {
                        if (m_Desk[i_Row + 1, i_Col + 1] == eSymbols.EmptyCell)
                        {
                            isCanMove = true;
                        }
                    }
                }
            }
            else
            {
                if (i_Position == ePositionOnDesk.DOWN)
                {
                    if ((i_Row - 1) >= 0)
                    {
                        if ((i_Col - 1) >= 0)
                        {
                            if (m_Desk[i_Row - 1, i_Col - 1] == eSymbols.EmptyCell)
                            {
                                isCanMove = true;
                            }
                        }

                        if ((i_Col + 1) <= (Size - 1))
                        {
                            if (m_Desk[i_Row - 1, i_Col + 1] == eSymbols.EmptyCell)
                            {
                                isCanMove = true;
                            }
                        }
                    }
                }
                else
                {
                    if ((i_Row + 1) <= (Size - 1))
                    {
                        if ((i_Col - 1) >= 0)
                        {
                            if (m_Desk[i_Row + 1, i_Col - 1] == eSymbols.EmptyCell)
                            {
                                isCanMove = true;
                            }
                        }

                        if ((i_Col + 1) <= (Size - 1))
                        {
                            if (m_Desk[i_Row + 1, i_Col + 1] == eSymbols.EmptyCell)
                            {
                                isCanMove = true;
                            }
                        }
                    }
                }
            }

            return isCanMove;
        } 

        public bool CheckIfCanKick(int i_Row, int i_Col, ePositionOnDesk i_PlayerPosition) // check if ther some legal kick from this cell
        {
            bool isCanKick = false;

            isCanKick = CheckIfMustKick(i_Row, i_Col, i_PlayerPosition);

            return isCanKick;
        }
    
        public void MakeMove(string i_Move, eSymbols i_Symbol, eSymbols i_SymbolKing, ePositionOnDesk i_Position)
        {
            string coordinates;
            int rowFrom;
            int rowTo;
            int colFrom;
            int colTo;

            if (int.TryParse(i_Move, out int temp))
            {
                rowFrom = int.Parse(i_Move[0].ToString());
                colFrom = int.Parse(i_Move[1].ToString());
                rowTo = int.Parse(i_Move[2].ToString());
                colTo = int.Parse(i_Move[3].ToString());
            }
            else
            {
                coordinates = lettersToCoordinates(i_Move);
                rowFrom = int.Parse(coordinates[1].ToString());
                colFrom = int.Parse(coordinates[0].ToString());
                rowTo = int.Parse(coordinates[3].ToString());
                colTo = int.Parse(coordinates[2].ToString());
            }

            MakeMove(rowFrom, colFrom, rowTo, colTo, i_Symbol, i_SymbolKing, i_Position);
        }

        public void MakeMove(int i_RowFrom, int i_ColFrom, int i_RowTo, int i_ColTo, eSymbols i_Symbol, eSymbols i_SymbolKing, ePositionOnDesk i_Position)
        {
            int distance;
            bool isKing = false;

            if (m_Desk[i_RowFrom, i_ColFrom] == i_SymbolKing)
            {
                isKing = true;
            }

            distance = checkDistance(i_RowFrom, i_RowTo, i_ColFrom, i_ColTo, i_Position);
            if (distance == 1) // only move
            {
                Desk[i_RowFrom, i_ColFrom] = eSymbols.EmptyCell;
                if (i_RowTo == 0 || i_RowTo == (Size - 1))
                {
                    Desk[i_RowTo, i_ColTo] = i_SymbolKing;
                }
                else
                {
                    if (isKing)
                    {
                        Desk[i_RowTo, i_ColTo] = i_SymbolKing;
                    }
                    else
                    {
                        Desk[i_RowTo, i_ColTo] = i_Symbol;
                    }
                }
            }
            else // kick enemy checker
            {
                if (m_Desk[i_RowFrom, i_ColFrom] == i_SymbolKing)
                {
                    m_Desk[i_RowTo, i_ColTo] = i_SymbolKing;
                }
                else
                {
                    if (i_RowTo == 0 || i_RowTo == (Size - 1))
                    {
                        Desk[i_RowTo, i_ColTo] = i_SymbolKing;
                    }
                    else
                    {
                        m_Desk[i_RowTo, i_ColTo] = i_Symbol;
                    }
                }

                Desk[i_RowFrom, i_ColFrom] = eSymbols.EmptyCell;
                if (i_RowFrom < i_RowTo && i_ColFrom < i_ColTo)
                {
                    Desk[i_RowFrom + 1, i_ColFrom + 1] = eSymbols.EmptyCell;
                }
                else if (i_RowFrom < i_RowTo && i_ColFrom > i_ColTo)
                {
                    Desk[i_RowFrom + 1, i_ColFrom - 1] = eSymbols.EmptyCell;
                }
                else if (i_RowFrom > i_RowTo && i_ColFrom < i_ColTo)
                {
                    Desk[i_RowFrom - 1, i_ColFrom + 1] = eSymbols.EmptyCell;
                }
                else if (i_RowFrom > i_RowTo && i_ColFrom > i_ColTo)
                {
                    Desk[i_RowFrom - 1, i_ColFrom - 1] = eSymbols.EmptyCell;
                }
            }
        }

        public int calcPlayerPoints(eSymbols i_Symbol, eSymbols i_SymbolKing)
        {
            int points = 0;

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (m_Desk[i, j] == i_Symbol)
                    {
                        ++points;
                    }
                    else if (m_Desk[i, j] == i_SymbolKing)
                    {
                        points += 4;
                    }
                }
            }

            return points;
        }

        public enum ePositionOnDesk
        {
            UP,
            DOWN,
        }
    }

    public enum eSymbols
    {
        Player1 = 'X',
        Player2 = 'O',
        Player1King = 'K',
        Player2King = 'U',
        EmptyCell = ' ',
    }
}
