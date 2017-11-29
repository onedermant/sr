// ====================================================================================================================================
// EVENT CLASS
//
// Singleton for broadcasting events as events
//



// ====================================================================================================================================
// USING
//
using System;

namespace SR2
{
    public static class EventClass
    {
        // ============================================================================================================================
        // DELEGATES
        //
        public delegate void EventHandler(DateTime dateTime, String message, string fileName = null);
        public delegate void EventLogHandler(DateTime dateTime, EventLogArgs eventLogArgs, string fileName = null);



        // ============================================================================================================================
        // PROPERTIES
        //




        // ============================================================================================================================
        // EVENTS
        //
        public static event EventHandler eventLog;
        public static event EventLogHandler eventLog2;




        // ============================================================================================================================
        // CONSTRUCTORS / DECONSTRUCTORS
        //




        // ============================================================================================================================
        // PRIMARY
        //
        public static void LogEvent(string eventString)
        {
            DateTime currentTime = DateTime.Now;

            // RAISE EVENT
            if (eventLog2 != null) eventLog2(currentTime, new EventLogArgs(eventString));
        }

        public static void LogEvent(EventLogArgs eventLogArgs)
        {
            DateTime currentTime = DateTime.Now;

            // RAISE EVENT
            if (eventLog2 != null) eventLog2(currentTime, eventLogArgs);
        }

        public static void LogEvent(string eventString, string fileName)
        {
            DateTime currentTime = DateTime.Now;

            // RAISE EVENT
            if (eventLog2 != null) eventLog2(currentTime, new EventLogArgs(eventString), fileName);
        }

    }
}