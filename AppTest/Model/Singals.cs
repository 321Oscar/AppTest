﻿using System.Collections.Generic;

namespace AppTest.Model
{
    public class DBCSignals
    {
        /// <summary>
        /// 
        /// </summary>
        public List<DBCSignal> SignalList { get; set; }

        public DBCSignals()
        {
            SignalList = new List<DBCSignal>();
        }
    }

    public class XCPSignals
    {
        public List<XCPSignal> xCPSignalList;
        public XCPSignals()
        {
            xCPSignalList = new List<XCPSignal>();
        }
    }

}
