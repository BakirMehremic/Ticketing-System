using System.Linq.Expressions;
using TicketingSys.Enums;
using TicketingSys.Models;

namespace TicketingSys.QueryBuilders;

public class TicketQueryBuilder : QueryBuilderBase<Ticket>
{
    public TicketQueryBuilder WithStatus(TicketStatusEnum? status)
    {
        if(status.HasValue) AddFilter(x=> x.Status == status.Value);
        return this;
    }

    public TicketQueryBuilder WithUrgency(TicketUrgencyEnum? urgency)
    {
        if(urgency.HasValue) AddFilter(x=> x.Urgency == urgency.Value);
        return this;
    }

    public TicketQueryBuilder WithAssignedToName(string? assignedName)
    {
        if (!string.IsNullOrWhiteSpace(assignedName))
        {
            string lowerName = assignedName.ToLower();
            AddFilter(x => x.AssignedTo != null &&
                              x.AssignedTo.FirstName.ToLower() == lowerName);
        }
        return this;
    }

    public TicketQueryBuilder WithCategoryName(string? categoryName)
    {
        if (!string.IsNullOrWhiteSpace(categoryName))
        {
            string lower = categoryName.ToLower();
            AddFilter(x => x.Category.Name.ToLower().Contains(lower));
        }
        return this;
    }

    public TicketQueryBuilder WithDepartmentName(string? departmentName)
    {
        if (!string.IsNullOrWhiteSpace(departmentName))
        {
            string lower = departmentName.ToLower();
            AddFilter(x => x.Department.Name.ToLower().Contains(lower));
        }
        return this;
    }

    public TicketQueryBuilder WithFromDate(DateTime? fromDate)
    {
        if (fromDate.HasValue)
            AddFilter(x => x.CreatedAt >= fromDate.Value);
        return this;
    }

    public TicketQueryBuilder WithToDate(DateTime? toDate)
    {
        if (toDate.HasValue)
            AddFilter(x => x.CreatedAt <= toDate.Value);
        return this;
    }

    public TicketQueryBuilder WithIsAssigned(bool? isAssigned)
    {
        if (isAssigned==true)
            AddFilter(x => x.AssignedToId != null);

        if (isAssigned == false)
            AddFilter(x => x.AssignedToId == null);

        return this;
    }

    public TicketQueryBuilder WithHasResponses(bool? hasResponses)
    {
        if (hasResponses == true)
            AddFilter(x => x.Responses != null && x.Responses.Any());
        if (hasResponses == false)
            AddFilter(x => x.Responses == null || !x.Responses.Any());

        return this;
    }

    public TicketQueryBuilder WithAssignedToId(string? assignedToId)
    {
        if (!string.IsNullOrWhiteSpace(assignedToId))
            AddFilter(x => x.AssignedToId == assignedToId);
        return this;
    }

    public TicketQueryBuilder WithSearch(string? search)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            string lower = search.ToLower();
            AddFilter(x =>
                x.Title.ToLower().Contains(lower) ||
                x.Description.ToLower().Contains(lower));
        }
        return this;
    }


}
