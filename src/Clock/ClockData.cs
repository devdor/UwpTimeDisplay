using System.Collections.Generic;

namespace TimeDisplayApp.Clock {
    public class ClockData {

        public static List<string> ContentDe = new List<string>() {
            "ESKISTLFÜNF", // 0
            "ZEHNZWANZIG", // 1
            "DREIVIERTEL", // 2
            "TGNACHVORTM", // 3
            "HALBQZWÖLFP", // 4
            "ZWEINSIEBEN", // 5
            "KDREIRHFÜNF", // 6
            "ELFNEUNVIER", // 7
            "WACHTZEHNRS", // 8
            "BSECHSFMUHR"  // 9
        };

        static string GetKey(int row, int col) {
            return $"{row}_{col}";
        }

        static List<string> GetKeyList(int row, int colFrom, int length) {
            List<string> result = new List<string>();
            for (int col = colFrom; col < colFrom + length; col++) {

                result.Add(GetKey(row, col));
            }
            return result;
        }

        public static List<string> CharIndexList(WordType word) {

            var result = new List<string>();
            switch (word) {
                case WordType.DREIVIERTEL:
                    result.AddRange(GetKeyList(2, 0, 11));
                    break;
                case WordType.ESIST:
                    result.AddRange(GetKeyList(0, 0, 2));
                    result.AddRange(GetKeyList(0, 3, 3));
                    break;
                case WordType.FUENF:
                    result.AddRange(GetKeyList(0, 7, 4));
                    break;
                case WordType.HALB:
                    result.AddRange(GetKeyList(4, 0, 4));
                    break;
                case WordType.H_ACHT:
                    result.AddRange(GetKeyList(8, 1, 4));
                    break;
                case WordType.H_DREI:
                    result.AddRange(GetKeyList(6, 1, 4));
                    break;
                case WordType.H_EIN:
                    result.AddRange(GetKeyList(5, 2, 3));
                    break;
                case WordType.H_EINS:
                    result.AddRange(GetKeyList(5, 2, 4));
                    break;
                case WordType.H_ELF:
                    result.AddRange(GetKeyList(7, 0, 3));
                    break;
                case WordType.H_FUENF:
                    result.AddRange(GetKeyList(6, 7, 4));
                    break;
                case WordType.H_NEUN:
                    result.AddRange(GetKeyList(7, 3, 4));
                    break;
                case WordType.H_SECHS:
                    result.AddRange(GetKeyList(9, 1, 5));
                    break;
                case WordType.H_SIEBEN:
                    result.AddRange(GetKeyList(5, 5, 6));
                    break;
                case WordType.H_VIER:
                    result.AddRange(GetKeyList(7, 7, 4));
                    break;
                case WordType.H_ZEHN:
                    result.AddRange(GetKeyList(8, 5, 4));
                    break;
                case WordType.H_ZWEI:
                    result.AddRange(GetKeyList(5, 0, 4));
                    break;
                case WordType.H_ZWOELF:
                    result.AddRange(GetKeyList(4, 5, 5));
                    break;
                case WordType.NACH:
                    result.AddRange(GetKeyList(3, 2, 4));
                    break;
                case WordType.UHR:
                    result.AddRange(GetKeyList(9, 8, 3));
                    break;
                case WordType.VIERTEL:
                    result.AddRange(GetKeyList(2, 4, 7));
                    break;
                case WordType.VOR:
                    result.AddRange(GetKeyList(3, 6, 3));
                    break;
                case WordType.ZEHN:
                    result.AddRange(GetKeyList(1, 0, 4));
                    break;
                case WordType.ZWANZIG:
                    result.AddRange(GetKeyList(1, 4, 7));
                    break;
            }

            return result;
        }
    }
}
