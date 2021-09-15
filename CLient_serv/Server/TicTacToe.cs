using System;
using System.Collections.Generic;
using System.Text;

namespace CLient_serv
{
    public static class TicTacToe
    {
        public static char[,] Field;
        public static bool YourTurn;

        public static void Init()
        {
            Field = new char[3,3]{{ ' ',' ',' '}, { ' ', ' ', ' ' }, { ' ', ' ', ' ' }};

            YourTurn = false;
        }
        public static bool CheckWin()
        {

            return false;
        }

        public static string FieldToString()
        {
            string fieldStr = string.Empty;

            for (int i = 0; i < Field.GetLength(0); i++)
            {
                for (int j = 0; j < Field.GetLength(1); j++)
                {
                    fieldStr += Field[i, j].ToString()+'.';
                }
            }

            return fieldStr;
        }
    }
}
