namespace ChitChat.Data.Helpers
{
    [Flags]
    public enum GroupPermissions
    {
        None = 0,
        SendMessages = 1 << 0,  // 1
        EditGroup = 1 << 1,     // 2
        AddMembers = 1 << 2     // 4
    }
}

/*
// Add More than one permission
GroupPermissions userPermissions = GroupPermissions.SendMessages | GroupPermissions.AddMembers;

// Check if permission is exist
bool canSendMessages = (userPermissions & GroupPermissions.SendMessages) != 0;
bool canEditGroup = (userPermissions & GroupPermissions.EditGroup) != 0;

// Remove a Permission
userPermissions &= ~GroupPermissions.AddMembers;

// Add new Permission
userPermissions |= GroupPermissions.EditGroup;

 */