using static Domain.CoreEnums.Enums;

namespace Domain.BackendResponses
{
    /// <summary>
    /// Object of messages and result type
    /// </summary>
    public class MethodResult
    {
        /// <summary>
        /// Method result. Ok - everything is ok, conflict - something went wrong
        /// </summary>
        public MethodResults Result { get; set; }

        /// <summary>
        /// Messages for response. Messages of errors, success, whatever
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// Object of messages and result type
        /// </summary>
        /// <param name="messages">Messages connected to service method result</param>
        /// <param name="result">Result type of method (success of fault)</param>
        public MethodResult(List<string> messages, MethodResults result)
        {
            Messages = messages;
            Result = result;

        }
    }

    /// <summary>
    /// Object of TEntity, messages and result type
    /// </summary>
    /// <typeparam name="T">Result entity</typeparam>
    public class MethodResult<T>
    {
        /// <summary>
        /// Result entity of current method according method dedication
        /// </summary>
        public T? ResultEntity { get; }

        /// <summary>
        /// Method result. Ok - everything is ok, conflict - something went wrong
        /// </summary>
        public MethodResults Result { get; set; }

        /// <summary>
        /// Messages for response. Messages of errors, success, whatever
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// Object of TEntity, messages and result type
        /// </summary>
        /// <param name="resultEntity">T Entity, which is returned form service method</param>
        /// <param name="messages">Messages connected to service method result</param>
        /// <param name="result">Result type of method (success of fault)</param>
        public MethodResult(T? resultEntity, List<string> messages, MethodResults result)
        {
            ResultEntity = resultEntity;
            Messages = messages;
            Result = result;

        }
    }

    /// <summary>
    /// Object of TEntity as result entity and R as auxiliary, messages and result type
    /// </summary>
    /// <typeparam name="T">Result entity</typeparam>
    /// <typeparam name="R">Auxiliary result entity</typeparam>
    public class MethodResult<T, R>
    {
        /// <summary>
        /// Result entity of current method according method dedication
        /// </summary>
        public T? ResultEntityFirst { get; }

        /// <summary>
        /// Result entity of current method according method dedication
        /// </summary>
        public R? ResultEntitySecond { get; }

        /// <summary>
        /// Method result. Ok - everything is ok, conflict - something went wrong
        /// </summary>
        public MethodResults Result { get; set; }

        /// <summary>
        /// Messages for response. Messages of errors, success, whatever
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// Object of TEntity, messages and result type
        /// </summary>
        /// <param name="resultEntity">T Entity, which is returned form service method</param>
        /// <param name="messages">Messages connected to service method result</param>
        /// <param name="result">Result type of method (success of fault)</param>
        public MethodResult(T? resultEntityFirst,R? resultEntitySecond, List<string> messages, MethodResults result)
        {
            ResultEntityFirst = resultEntityFirst;
            ResultEntitySecond = resultEntitySecond;
            Messages = messages;
            Result = result;

        }

    }
}
