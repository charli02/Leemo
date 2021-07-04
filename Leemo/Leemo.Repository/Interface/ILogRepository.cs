using Leemo.Model;
using Leemo.Model.Domain;

/// <summary>
/// Represents repository project namespace
/// </summary>
namespace Leemo.Repository.Interface
{
    public interface ILogReposiory
    {
        void InsertLog(ErrorLog log);
    }
}
