using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    /// <summary>
    /// Validating user permissions in team/project/messenger areas. Throws specially defined error
    /// if user is unable to perform specific action.
    /// </summary>
    public interface IPermissionValidator
    {
        Task ValidateTeamPermission(long teamId, PermissionEnum permssion);

        Task ValidateProjectManagerPermission(long projectId, PermissionEnum permssion);

        Task ValidateChatPermission(long chatId, PermissionEnum permssion);
    }
}