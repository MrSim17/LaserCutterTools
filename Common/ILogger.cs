using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public interface ILogger
    {
        void Log(string Message);
        void OutputLog();
    }

    public sealed class NullLogger : ILogger
    {
        void ILogger.Log(string Message)
        {
            // do nothing
        }

        public void OutputLog()
        {
            // do nothing
        }
    }

    public sealed class StringLogger : ILogger
    {
        StringBuilder sb = new StringBuilder();

        public string Log
        {
            get
            {
                return sb.ToString();
            }
        }

        void ILogger.Log(string Message)
        {
            sb.AppendLine(Message);
        }

        public void OutputLog()
        {
            // do nothing
        }
    }
}
