using System.Collections.Generic;
using System.Linq;

namespace SerialDetection
{
    internal class SerialIdentifier
    {
        public Dictionary<int, Serial> DetectSerial(List<string> lines)
        {
            List<string> startWord = new List<string>();

            lines.ForEach(l => startWord.Add(l.Trim().Split(new char[] { ' ', '\t' })[0]));

            Dictionary<int, Serial> serialList = new Dictionary<int, Serial>();

            int count = 0;
            startWord.ForEach(w => {

                Serial serial = new Serial();
                if (IsSerial(w, out serial))
                {
                    serialList.Add(count, serial);
                }
                count++;
            });

            return serialList;

        }

        private bool IsSerial(string w, out Serial serial)
        {
            bool isSerial = false;
            if ((w.EndsWith(".") || w.EndsWith(")")))
            {
                serial = new Serial() { Value = w, Type = GetSerialType(w) };
                isSerial = true;
            }
            else if (w.Contains(".") && !w.EndsWith("."))
            {
                serial = new Serial() { Value = w, Type = GetMixedSerialType(w) };
                isSerial = true;
            }
            else
                serial = null;

            return isSerial;
        }


        private SerialType GetMixedSerialType(string w)
        {
            List<string> serialPart = w.Split('.').ToList();
            List<SerialType> mixedSerialTypes = new List<SerialType>();

            SerialType finalSerialType;

            if (serialPart.Count > 1)
            {
                foreach (var item in serialPart)
                {
                    if (IsSerial(item + '.', out Serial serial))
                    {
                        mixedSerialTypes.Add(serial.Type);
                    }
                }
            }
            else
                return SerialType.OTHER;

            if (mixedSerialTypes.Count == serialPart.Count && !mixedSerialTypes.Contains(SerialType.OTHER))
            {
                //int sum = 0;
                //mixedSerialTypes.ForEach(x => sum = sum + (int)x);

                //if (mixedSerialTypes.Count == sum / (int)mixedSerialTypes[0])
                //    finalSerialType = mixedSerialTypes[0];
                //else
                finalSerialType = SerialType.MiXED;
            }
            else
                finalSerialType = SerialType.OTHER;

            return finalSerialType;
        }

        private SerialType GetSerialType(string w)
        {
            w = w.TrimStart('(').TrimEnd(')').TrimEnd('.');
            bool number = CheckForNumber(w);
            bool alpha = false;
            bool roman = false;
            SerialType mixed = SerialType.OTHER;

            if (!number)
            {
                roman = CheckForRoman(w);
                alpha = CheckForAlphabate(w);

                if (!alpha && !roman)
                    mixed = GetMixedSerialType(w);
            }


            if (number)
                return SerialType.NUMBER;
            else if (alpha && roman)
                return SerialType.ALPHA_ROMAN;
            else if (alpha)
                return SerialType.ALPHABATES;
            else if (roman)
                return SerialType.ROMAN;
            else
                return mixed;

        }

        private bool CheckForRoman(string w)
        {
            bool canBeRoman = CheckForRomanParticipant(w[0], out string romanChar);
            bool romanValue = false;
            if (canBeRoman)
            {
                switch (romanChar)
                {
                    case "I":
                        romanValue = RomanCheckFor_I(w);
                        break;
                    case "V":
                        romanValue = RomanCheckFor_V(w);
                        break;
                    case "X":
                        romanValue = RomanCheckFor_X(w);
                        break;
                    default:
                        romanValue = false;
                        break;

                }
            }

            return romanValue;

        }

        private bool RomanCheckFor_X(string v)
        {
            if (string.Equals(v, "X") || string.Equals(v, "XX") || string.Equals(v, "XXX"))
                return true;
            else
            {
                return true && CheckForRoman(v.Substring(1));
            }

        }

        private bool RomanCheckFor_V(string v)
        {
            if (string.Equals(v, "V") || string.Equals(v, "VI") || string.Equals(v, "VII") || string.Equals(v, "VIII"))
                return true;
            else
                return false;
        }

        private bool RomanCheckFor_I(string v)
        {
            if (string.Equals(v, "I") || string.Equals(v, "II") || string.Equals(v, "III") || string.Equals(v, "IV") || string.Equals(v, "IX"))
                return true;
            else
                return false;
        }

        private bool CheckForRomanParticipant(char w, out string romanChar)
        {
            romanChar = w.ToString().ToUpper();
            if (romanChar == "I" || romanChar == "V" || romanChar == "X")
                return true;
            else
            {
                romanChar = null;
                return false;
            }
        }

        private bool CheckForAlphabate(string w)
        {
            if (w.Length == 1)
                return true;
            else
                return false;
        }

        private bool CheckForNumber(string word)
        {
            if (word.Contains('.'))
            {
                if (word.Split('.').Length <= 2 && string.IsNullOrWhiteSpace(word.Split('.')[1]))
                    return int.TryParse(word.Split('.')[0], out int t);
                else
                    return false;
            }
            else
                return int.TryParse(word.Split('.')[0], out int t);

            //return int.TryParse(word.Split('.').Aggregate((i,j) => i+j), out int tempVal);
        }
    }

    internal class Serial
    {
        public SerialType Type { get; set; }
        public string Value { get; set; }

    }

    internal enum SerialType
    {
        NUMBER = 1,
        ALPHABATES = 2,
        ROMAN = 3,
        ALPHA_ROMAN = 4,
        MiXED = 5,
        OTHER = 6
    }
}