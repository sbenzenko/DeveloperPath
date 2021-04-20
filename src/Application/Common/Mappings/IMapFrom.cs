using AutoMapper;

namespace DeveloperPath.Application.Common.Mappings
{
  /// <summary>
  /// Common mapping interface
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IMapFrom<T>
  {
    /// <summary>
    /// Default mapping implementation
    /// </summary>
    /// <param name="profile"></param>
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
  }
}
