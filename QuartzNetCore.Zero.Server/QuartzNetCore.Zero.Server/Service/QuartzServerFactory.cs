using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuartzNetCore.Zero.Server.Service
{
    /// <summary>
    /// Factory class to create Quartz server implementations from.
    /// </summary>
    public class QuartzServerFactory
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(QuartzServerFactory));

        /// <summary>
        /// Creates a new instance of an Quartz.NET server core.
        /// </summary>
        /// <returns></returns>
        public static QuartzServer CreateServer()
        {
            string typeName = typeof(QuartzServer).AssemblyQualifiedName;
            Type t = Type.GetType(typeName, true);
            logger.Debug("Creating new instance of server type '" + typeName + "'");
            QuartzServer retValue = (QuartzServer)Activator.CreateInstance(t);
            logger.Debug("Instance successfully created");
            return retValue;
        }
    }
}