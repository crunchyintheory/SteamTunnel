using System;

namespace Trileans
{
    public struct trilean
    {
        /// <summary>
        /// The numerical value of the trilean
        /// </summary>
        public short value { get; set; }
        /// <summary>
        /// The boolean table representation of the trilean
        /// </summary>
        public bool[] table;
        /// <summary>
        /// The embedded object of the trilean
        /// </summary>
        public object embedded;

        /// <summary>
        /// Initializes a new <see cref="trilean"/> object using a short as reference.
        /// </summary>
        /// <param name="value">Accepted values are 0, 1, or 2</param>
        /// <param name="embedded">The object to be embedded in the <see cref="trilean"/></param>
        public trilean(short value, object embedded = null)
        {
            if(value <= 2)
            {
                this.value = value;
                table = Trilean.createTable(value);
                this.embedded = embedded;
            } else
            {
                throw new Exception("Error: A Trilean's value can only be 0, 1, or 2!");
            }
        }
        /// <summary>
        /// Initializes a new <see cref="trilean"/> object using an int as reference
        /// </summary>
        /// <param name="value">Accepted values are 0, 1, or 2</param>
        /// <param name="embedded">The object to be embedded in the <see cref="trilean"/></param>
        public trilean(int value, object embedded = null)
        {
            this.value = (short)value;
            table = Trilean.createTable((short)value);
            this.embedded = embedded;
        }
        /// <summary>
        /// Initializes a new <see cref="trilean"/> object using two bools as reference.
        /// </summary>
        /// <param name="b1">If <paramref name="b2"/> is false, this will be the value of the trilean.</param>
        /// <param name="b2">If this is true, the trilean will be middle</param>
        /// <param name="embedded">The object to be embedded in the <see cref="trilean"/></param>
        public trilean(bool b1, bool b2, object embedded = null)
        {
            value = Trilean.Parse(b1, b2).value;
            table = Trilean.createTable(value);
            this.embedded = embedded;
        }
        /// <summary>
        /// Initializes a new <see cref="trilean"/> object using two bools as reference.
        /// </summary>
        /// <param name="b1">This will be the value of the trilean</param>
        /// <param name="embedded">The object to be embedded in the <see cref="trilean"/></param>
        public trilean(bool b1, object embedded = null)
        {
            value = Trilean.Parse(b1, false).value;
            table = Trilean.createTable(value);
            this.embedded = embedded;
        }
        /// <summary>
        /// Initializes a new <see cref="trilean"/> using a string as a reference.
        /// </summary>
        /// <param name="s">Accepted values are "true", "0", "middle", "1", "false", and "2"</param>
        /// <param name="embedded">The object to be embedded in the <see cref="trilean"/></param>
        public trilean(string s, object embedded = null)
        {
            value = Trilean.Parse(s).value;
            table = Trilean.createTable(value);
            this.embedded = embedded;
        }
        public static implicit operator trilean(int i)
        {
            return new trilean(i);
        }
        public static implicit operator trilean(short s)
        {
            return new trilean(s);
        }
        public static implicit operator trilean(bool b)
        {
            return Trilean.Parse(b, false);
        }
        public static implicit operator trilean(bool[] b)
        {
            if(b.Length == 1)
            {
                return Trilean.Parse(b[0], false);
            } else if(b.Length == 2)
            {
                return Trilean.Parse(b[0], b[1]);
            } else
            {
                throw new Exception("Only accepts an array of two bools or a single bool");
            }
        }
        public static implicit operator trilean(string s)
        {
            return Trilean.Parse(s);
        }
        public static implicit operator int(trilean t)
        {
            return t.value;
        }
        public static implicit operator short(trilean t)
        {
            return t.value;
        }
        public static implicit operator bool[] (trilean t)
        {
            return t.table;
        }
        public static implicit operator bool(trilean t)
        {
            if(t.value == 0)
            {
                return true;
            } else
            {
                return false;
            }
        }
        public static implicit operator string(trilean t)
        {
            if(t.value == 0)
            {
                return "true";
            } else if(t.value == 1)
            {
                return "middle";
            } else if(t.value == 2)
            {
                return "false";
            } else
            {
                throw new Exception("Error: Invalid Trilean");
            }
        }
    }
    public class Trilean
    {
        public static trilean Parse(bool b1, bool b2)
        {
            if(b2)
            {
                return new trilean(1);
            } else if(b1)
            {
                return new trilean(0);
            } else
            {
                return new trilean(2);
            }
        }
        public static trilean Parse(string s)
        {
            if (s == "true" || s == "0")
            {
                return 0;
            } else if(s == "middle" || s == "1")
            {
                return 1;
            } else if(s == "false" || s == "2")
            {
                return 2;
            } else
            {
                throw new Exception("Error: Invalid String");
            }
        }
        public static trilean Parse(short s)
        {
            return new trilean(s);
        }
        public static trilean Parse(int i)
        {
            return new trilean(i);
        }
        internal static bool[] createTable(short s)
        {
            if(s == 0)
            {
                return new[] { true, false };
            }
            else if(s == 1)
            {
                return new[] { false, true };
            }
            else if(s == 2)
            {
                return new[] { false, false };
            } else
            {
                throw new Exception("Error: Invalid value provided.");
            }
        }
    }
}
