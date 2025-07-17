using System.Linq.Expressions;
using TicketingSys.Models;

namespace TicketingSys.QueryBuilders;

public sealed class UserQueryBuilder: QueryBuilderBase<User>
{
    public UserQueryBuilder WithFirstName(string? firstName)
    {
        if (!string.IsNullOrWhiteSpace(firstName))
        {
            string lower = firstName.ToLower();
            AddFilter(x => x.FirstName.ToLower().Contains(lower));
        }
        return this;
    }


    public UserQueryBuilder WithLastName(string? lastName)
    {
        if (!string.IsNullOrWhiteSpace(lastName))
        {
            var lower = lastName.ToLower();
            AddFilter(x => x.LastName.ToLower().Contains(lower));
        }
        return this;
    }

    public UserQueryBuilder WithFullName(string? fullName)
    {
        if (!string.IsNullOrWhiteSpace(fullName))
        {
            string lower = fullName.ToLower();
            AddFilter(x => (x.FirstName + " " + x.LastName).ToLower().Contains(lower));
        }
        return this;
    }

    public UserQueryBuilder WithEmail(string? email)
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            string lower = email.ToLower();
            AddFilter(x => x.Email.ToLower().Contains(lower));
        }
        return this;
    }

    public UserQueryBuilder WithIsAdmin(bool? isAdmin)
    {
        if (isAdmin.HasValue)
            AddFilter(x => x.IsAdmin == isAdmin.Value);

        return this;
    }

    public UserQueryBuilder WithHasDepartmentAccess(bool? hasAccess)
    {
        if(hasAccess==true)
            AddFilter(x=> x.DepartmentAccesses.Count>0);

        if(hasAccess==false)
            AddFilter(x=> x.DepartmentAccesses.Count==0);

        return this;
    }

}
