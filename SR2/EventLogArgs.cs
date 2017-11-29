using System;
using System.Reflection;

namespace SR2
{
    public class EventLogArgs : EventArgs
    {

        //enumeration
        public enum LogEntryTypeEnum
        {
            INFO,
            INITIATIVE,
            ROLL,
            ERROR
        }

        //public properties
        public bool isException = false;        
        public Exception propertyException;
        public String propertyExceptionKey = System.Guid.NewGuid().ToString();
        public String propertyClassName;
        public String propertyMethodName;
        public String propertyMessage;
        public LogEntryTypeEnum propertyLogEntryType = LogEntryTypeEnum.INFO;


        public EventLogArgs(LogEntryTypeEnum entryType, MethodBase methodBase, String message)
            : this(methodBase, message)
        {
            propertyLogEntryType = entryType;
        }

        public EventLogArgs(MethodBase methodBase, String message)
            : this(message)
        {
            propertyClassName = methodBase.DeclaringType.ToString();
            propertyMethodName = methodBase.Name.ToString();
        }

        public EventLogArgs(String message)
        {
            propertyMessage = message;
        }

        public EventLogArgs(LogEntryTypeEnum entryType, String message)
        {
            propertyLogEntryType = entryType;
            propertyMessage = message;
        }

        public EventLogArgs(MethodBase methodBase, Exception ex)
        {
            isException = true;
            propertyLogEntryType = LogEntryTypeEnum.ERROR;
            propertyClassName = methodBase.DeclaringType.ToString();
            propertyMethodName = methodBase.Name.ToString();
            propertyException = ex;
        }

        public EventLogArgs(MethodBase methodBase, String message, bool isError)
        {
            isException = isError;
            propertyClassName = methodBase.DeclaringType.ToString();
            propertyMethodName = methodBase.Name.ToString();
            if (isError)
            {
                propertyException = new Exception(message);
                propertyLogEntryType = LogEntryTypeEnum.ERROR;
            }
            else
            {
                propertyMessage = message;
                propertyLogEntryType = LogEntryTypeEnum.INFO;
            }
        }

    }
}
