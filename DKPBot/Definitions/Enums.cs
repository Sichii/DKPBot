namespace DKPBot.Definitions
{
    /// <summary>
    ///     Alignment of text within a field of fixed width.
    /// </summary>
    internal enum TextAlignment
    {
        LeftAlign = 0,
        Center = 1,
        RightAlign = 2
    }

    /// <summary>
    ///     Required privilege level.
    /// </summary>
    internal enum Privilege
    {
        /// <summary>
        ///     No privilege required.
        /// </summary>
        None = 0,

        /// <summary>
        ///     Checks ability to participate. (write messages AND join voice)
        /// </summary>
        Normal = 1,

        /// <summary>
        ///     Checks ability to moderate a server. (manage channels OR kick)
        /// </summary>
        Elevated = 2,

        /// <summary>
        ///     Checks server administrator privilege
        /// </summary>
        Administrator = 3
    }
}