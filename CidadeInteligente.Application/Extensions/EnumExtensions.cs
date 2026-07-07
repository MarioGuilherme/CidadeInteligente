using System.ComponentModel;
using System.Reflection;

namespace CidadeInteligente.Application.Extensions;

public static class EnumExtensions
{
    extension(Enum @enum)
    {
        public string GetDescription()
        {
            FieldInfo? fieldInfo = @enum.GetType().GetField(@enum.ToString());

            if (fieldInfo is null) return @enum.ToString();

            if (fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Length != 0)
                return attributes.First().Description;

            return @enum.ToString();
        }
    }
}
