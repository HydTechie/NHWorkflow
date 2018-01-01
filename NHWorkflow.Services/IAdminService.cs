using NHWorkflow.Core;
using System;

namespace NHWorkflow.Services
{
    public interface IAdminService
    {
       int ValidateCustomer();
       IPagedList<ApprovalQueue> GetApprovalQueue(string firstname, string lastname, DateTime dateOfBirth, int pageIndex =0, int pageSize = int.MaxValue, bool includeApproved=false);

    }
}
