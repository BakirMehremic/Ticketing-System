using TicketingSys.Dtos.ResponseDtos;
using TicketingSys.Dtos.TicketDtos;
using TicketingSys.Dtos.UserDtos;
using TicketingSys.QueryBuilders;

namespace TicketingSys.Utils;

public static class QueryUtils
{
    public static bool IsEmpty<T>(this T dto) where T : class
    {
        var properties = typeof(T).GetProperties();

        foreach (var prop in properties)
        {
            var value = prop.GetValue(dto);

            if (value != null)
                return false;
        }

        return true;
    }
}
