using System;
using System.ComponentModel;
using System.Reflection;

namespace MeetAndPlay.Core.Infrastructure.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enumElement)
        {
            var type = enumElement.GetType();

            var memInfo = type.GetMember(enumElement.ToString());
            if (memInfo.Length <= 0) 
                return enumElement.ToString();
            
            var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs.Length > 0)
                return ((DescriptionAttribute)attrs[0]).Description;

            return enumElement.ToString();
        }
    }
}