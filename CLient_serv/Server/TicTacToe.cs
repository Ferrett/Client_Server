using System;
using System.Collections.Generic;
using System.Text;

namespace CLient_serv
{
    public static class TicTacToe
    {
        public static char[,] Field;
        public static bool Player2Turn;
        public static char Player1Symb;
        public static char Player2Symb;


        public static void Init()
        {
            Field = new char[3,3]{{ ' ',' ',' '}, { ' ', ' ', ' ' }, { ' ', ' ', ' ' }};
            Player2Turn = false;
            Player1Symb = 'X';
            Player2Symb = 'O';
        }
        public static bool CheckWin()
        {
            for (int i = 0; i < Field.GetLength(0); i++)
            {
                if (Field[i, 0].Equals(Field[i, 1]) && Field[i, 1].Equals(Field[i, 2])&& Field[i, 0]!=' ')
                    return true;

                if (Field[0, i].Equals(Field[1, i]) && Field[1, i].Equals(Field[2, i]) && Field[0, i] != ' ')
                    return true;
            }

            if (Field[0, 0].Equals(Field[1, 1]) && Field[1, 1].Equals(Field[2, 2]) && Field[1,1] != ' ')
                return true;

            if (Field[0, 2].Equals(Field[1, 1]) && Field[1, 1].Equals(Field[2, 0]) && Field[1,1] != ' ')
                return true;

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
