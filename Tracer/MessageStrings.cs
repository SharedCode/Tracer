using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracing
{
    /// <summary>
    /// Tracer's messages. Feel free to update the messages below or change
    /// to retrieve from data source such as ResourceString file, for example.
    /// </summary>
    public class MessageStrings
    {
        public string OnEnterEventFailedCall = "Failed calling OnEnter event.";
        public string OnLeaveEventFailedCall = "Failed calling OnLeave event.";
        public string UnsupportedResultEvaluatorAndOnLeave = 
            "Can't specify both ResultEvaluator & OnLeave event handlers.";
        public string MessageWithRunTimeMessageTemplate = "{0}, run time(secs): {1}";
        public string ResultEvaluatorCallFailedMessageTemplate = "ResultEvaluator call failed, details: {0}.";
        public string SuccessfulCallMessageTemplate = "Successful call {0}.";
        public string SuccessfulCallWithDetailsMessageTemplate = "Successful call {0}, details: {1}.";
        public string FailedCallMessageTemplate = "Failed calling {0}.";
        public string FailedCallWithDetailsMessageTemplate = "Failed calling {0}, details: {1}.";
        public string UnsupportedResultActionMessageTemplate = "ResultAction {0} not supported.";

        public string EnteringMessageTemplate = "Entering {0}.";
        public string LeavingMessageTemplate = "Leaving {0}.";
        public string LeavingWithDetailsMessageTemplate = "Leaving {0}, details: {1}.";

        public string NoFunctionInfo = "<no function info>";

        public string MessageWithDetailsMessageTemplate = "{0} Details: {1}";
        public string FailedLoggingMessageTemplate = "Failed logging {0}.";
    }
}
