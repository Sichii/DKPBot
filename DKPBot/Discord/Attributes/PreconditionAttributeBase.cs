using System.Threading.Tasks;
using Discord.Commands;
using NLog;

namespace DKPBot.Discord.Attributes
{
    public abstract class PreconditionAttributeBase : PreconditionAttribute
    {
        private static readonly Task<PreconditionResult> SUCCESS = Task.FromResult(PreconditionResult.FromSuccess());
        protected Logger Log { get; set; }

        protected Task<PreconditionResult> Success
        {
            get
            {
                Log.Debug("Success.");
                return SUCCESS;
            }
        }

        protected Task<PreconditionResult> Error
        {
            get
            {
                var errMsg = ErrorMessage ?? string.Empty;
                Log.Warn($"Error. {errMsg}");
                return Task.FromResult(PreconditionResult.FromError(errMsg));
            }
        }

        protected PreconditionAttributeBase() =>
            Log = LogManager.GetLogger(GetType()
                .Name);

        protected virtual Task<PreconditionResult> GenError(string message = null)
        {
            var errMsg = message ?? string.Empty;
            Log.Warn(errMsg);
            return Task.FromResult(PreconditionResult.FromError(errMsg));
        }
    }
}