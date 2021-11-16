using System;
using System.Collections.Generic;
using System.Linq;
using Hotel.BLogicLayer.DTO;
using Hotel.BLogicLayer.Interfaces;
using Hotel.DAL.Interfaces;
using Hotel.DAL.Models;

namespace Hotel.BLogicLayer.Services
{
    public class RoleService : IRoleService
    {
        private IUnitOfWork _dbWorkUnit;
        private IMapperItem _mapperItem;
        public RoleService(IUnitOfWork dbWorkUnit, IMapperItem mapperItem)
        {
            _dbWorkUnit = dbWorkUnit;
            _mapperItem = mapperItem;
        }

        public void Dispose()
        {
            _dbWorkUnit.Dispose();
        }


        public bool IsRoleInDB(Guid roleId)
        {
            return _dbWorkUnit.Roles.Get(roleId) is not null;
        }

        public void CreateRole(RoleDto roleToCreate)
        {
            if (_dbWorkUnit.Roles.GetAll().All(role => role.RoleName != roleToCreate.RoleName))
            {
                _dbWorkUnit.Roles.Create(_mapperItem.Mapper.Map<Role>(roleToCreate));
            }
        }

        public RoleDto GetRoleByName(String name)
        {
            var roleGetedByName =  _dbWorkUnit.Roles.GetAll(filter: role => role.RoleName == name).LastOrDefault();
            if (roleGetedByName == null) throw new KeyNotFoundException("Can't find role with given name");

            return _mapperItem.Mapper.Map<RoleDto>(roleGetedByName);
        }

        public IEnumerable<RoleDto> GetRoles()
        {
            var rolesFromDb = _dbWorkUnit.Roles.GetAll();
            return _mapperItem.Mapper.Map<IEnumerable<Role>, IEnumerable<RoleDto>>(rolesFromDb);
        }

        public RoleDto GetRole(Guid id)
        {
            var role = _dbWorkUnit.Roles.Get(id);
            return _mapperItem.Mapper.Map<RoleDto>(role);
        }

        public void DeleteRole(Guid roleId)
        {
            if (!IsRoleInDB(roleId))
                throw new ArgumentException("Can't delete role that not contained in db");
            
            _dbWorkUnit.Roles.Delete(roleId);
        }

        public void UpdateRole(RoleDto roleToUpdate)
        {
            if (!IsRoleInDB(roleToUpdate.Id))
                throw new ArgumentException("Can't delete role that not container in db");
            

            _dbWorkUnit.Roles.Update(_mapperItem.Mapper.Map<Role>(roleToUpdate));
        }
    }
}