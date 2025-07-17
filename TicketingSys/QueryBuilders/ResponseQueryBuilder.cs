using System.Linq.Expressions;
using TicketingSys.Enums;
using TicketingSys.Models;

namespace TicketingSys.QueryBuilders;


// build queries from ResponseQueryParamsDto for Response model
public sealed class ResponseQueryBuilder: QueryBuilderBase<Response>
{
    public ResponseQueryBuilder WithSearch(string? search)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            string lower = search.ToLower();
            AddFilter(x => x.Message.ToLower().Contains(lower));
        }
        return this;
    }

    public ResponseQueryBuilder WithStatus(TicketStatusEnum? status)
    {
        if (status.HasValue)
        {
            AddFilter(x => x.Status == status.Value);
        }
        return this;
    }

    public ResponseQueryBuilder WithCategoryName(string? categoryName)
    {
        if (!string.IsNullOrWhiteSpace(categoryName))
        {
            string lower = categoryName.ToLower();
            AddFilter(x => x.Ticket.Category.Name.ToLower().Contains(lower));
        }
        return this;
    }

    public ResponseQueryBuilder WithDepartmentName(string? departmentName)
    {
        if (!string.IsNullOrWhiteSpace(departmentName))
        {
            var lower = departmentName.ToLower();
            AddFilter(x => x.Ticket.Department.Name.ToLower().Contains(lower));
        }
        return this;
    }

    public ResponseQueryBuilder WithAssignedToName(string? assignedName)
    {
        if (!string.IsNullOrWhiteSpace(assignedName))
        {
            string lower = assignedName.ToLower();
            AddFilter(x => x.Ticket.AssignedTo != null &&
                              x.Ticket.AssignedTo.FirstName.ToLower().Contains(lower));
        }
        return this;
    }

    public ResponseQueryBuilder WithFromDate(DateTime? fromDate)
    {
        if (fromDate.HasValue)
        {
            AddFilter(x => x.CreatedAt >= fromDate.Value);
        }
        return this;
    }

    public ResponseQueryBuilder WithToDate(DateTime? toDate)
    {
        if (toDate.HasValue)
        {
            AddFilter(x => x.CreatedAt <= toDate.Value);
        }
        return this;
    }

    public ResponseQueryBuilder WithHasAttachments(bool? hasAttachments)
    {
        if (hasAttachments == true)
        {
            AddFilter(x => x.Attachments.Any());
        }
        if (hasAttachments == false)
        {
            AddFilter(x => !x.Attachments.Any());
        }

        return this;
    }

}
