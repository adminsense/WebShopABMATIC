using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.UserGroups;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class UserGroupAdminUseCase : IUserGroupAdminPort
{
    private readonly IUserGroupRepository _repository;

    public UserGroupAdminUseCase(IUserGroupRepository repository) => _repository = repository;

    public Task<PagedResult<UserGroupDto>> GetUserGroupsAsync(UserGroupListFilter filter, CancellationToken cancellationToken = default) => _repository.GetUserGroupsAsync(filter, cancellationToken);
    public Task<UserGroupEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(id, cancellationToken);
    public Task<int> SaveAsync(UserGroupEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) => _repository.DeleteAsync(id, cancellationToken);
}