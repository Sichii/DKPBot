using System;
using System.Runtime.CompilerServices;
using Discord.Commands;
using NLog;

namespace DKPBot.Discord.Modules
{
    /// <summary>
    ///     Custom base class to handle logging and other common things done in a module.
    /// </summary>
    public abstract class ModuleBase : ModuleBase<SocketCommandContext>
    {
        protected abstract Logger Log { get; }


        protected Logger GetPrefixedLogger([CallerMemberName] string command = "")
        {
            var newLogger = Log.WithProperty("RequestedBy", $"[{Context.User.Username}]");
            newLogger.SetProperty("Command", $"[{command}]");
            return newLogger;
        }

        protected string Exception(Exception ex, [CallerMemberName] string command = "")
        {
            GetPrefixedLogger(command)
                .Error(ex);

            return ex.Message;
        }

        protected string Error(string errorMsg, [CallerMemberName] string command = "")
        {
            GetPrefixedLogger(command)
                .Error(errorMsg);
            return errorMsg;
        }

        protected string Warn(string warnMsg, [CallerMemberName] string command = "")
        {
            GetPrefixedLogger(command)
                .Warn(warnMsg);
            return warnMsg;
        }

        protected string Info(string infoMsg, [CallerMemberName] string command = "")
        {
            GetPrefixedLogger(command)
                .Info(infoMsg);
            return infoMsg;
        }

        protected string Debug(string debugMsg, [CallerMemberName] string command = "")
        {
            GetPrefixedLogger(command)
                .Debug(debugMsg);
            return debugMsg;
        }

        protected string Trace(string traceMsg, [CallerMemberName] string command = "")
        {
            GetPrefixedLogger(command)
                .Trace(traceMsg);
            return traceMsg;
        }
    }
}