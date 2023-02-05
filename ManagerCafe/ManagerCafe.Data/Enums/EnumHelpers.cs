using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ManagerCafe.Data.Enums
{
    public static class EnumHelpers
    {
        public static List<CommonEnumDto<T>> GetEnumList<T>()
        {
            var list = new List<CommonEnumDto<T>>();
            var enumType = typeof(T);
            if (enumType.IsEnum)
            {
                foreach (var value in Enum.GetValues(enumType))
                {
                    //Get display name
                    var displayName = value.GetType()
                        .GetMember(value.ToString() ?? string.Empty)
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        ?.Name;

                    list.Add(new CommonEnumDto<T>
                    {
                        Id = (T)value,
                        Name = displayName ?? Enum.GetName(enumType, value)
                    });
                }
            }
            return list;
        }
    }
}
