

namespace MixedLibrary.DataAccess
{
    public interface IDataConnection
    {
        /// <summary>
        /// Creating of a Model before saving it !
        /// </summary>
        /// <param name="model"> A model of a Protocol</param>
        /// <returns></returns>
        
        ProtocolModel CreateProtocol(ProtocolModel model);
    }
}
