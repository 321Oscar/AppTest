using System.Collections.Generic;

namespace AppTest.Model
{
    public class SignalEqualityComarer : IEqualityComparer<DBCSignal>
    {
        public bool Equals(DBCSignal x, DBCSignal y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.SignalName == y.SignalName && x.MessageID == y.MessageID)
                return true;
            else
                return false;
        }

        public int GetHashCode(DBCSignal obj)
        {
            return (obj.MessageID+obj.SignalName).GetHashCode();
        }
    }

}
