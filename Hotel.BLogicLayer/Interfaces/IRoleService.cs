using System;
using System.Collections.Generic;
using Hotel.BLogicLayer.DTO;

namespace Hotel.BLogicLayer.Interfaces
{
    public interface IRoleService : IDisposable
    {
        public void CreateRole(RoleDto role);
        public IEnumerable<RoleDto> GetRoles();
        public RoleDto GetRole(Guid id);

        public RoleDto GetRoleByName(String roleName);
        public bool IsRoleInDB(Guid roleId);
        public void DeleteRole(Guid roleId);
        public void UpdateRole(RoleDto guest);
    }
}