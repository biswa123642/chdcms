namespace CGP.Foundation.ErrorModule.Repositiories
{
    public interface IAudit
    {
        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(string message);
    }
}
