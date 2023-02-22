namespace Domain.Enums
{
    public enum PermissionEnum
    {
        TEAM_EDIT = 1,
        TEAM_DELETE,
        TEAM_ADD_USER,
        TEAM_KICK_USER,
        TEAM_ASSIGN_ROLE,
        TEAM_MANAGE_ROLE,
        TEAM_CREATE_PROJECT,
        TEAM_ADD_TO_PROJECT,
        TEAM_LEAVE_PROJECT,
        TEAM_DELETE_PROJECT,

        PM_EDIT,
        PM_ADD_TEAM,
        PM_KICK_TEAM,
        PM_SET_LIMIT_ROLE,
        PM_CREATE_TASK,
        PM_ASSIGN_TASK,
        PM_CREATE_SUBTASK,
        PM_EDIT_TASK,
        PM_DELETE_TASK,

        CHAT_EDIT,
        CHAT_DELETE,
        CHAT_ADD_USER,
        CHAT_SEND_IMAGE
    }
}