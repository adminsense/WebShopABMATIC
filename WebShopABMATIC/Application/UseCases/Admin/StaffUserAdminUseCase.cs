using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.StaffUsers;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class StaffUserAdminUseCase : IStaffUserAdminPort
{
    private readonly IStaffUserRepository _repository;

    public StaffUserAdminUseCase(IStaffUserRepository repository) => _repository = repository;

    public Task<PagedResult<StaffUserDto>> GetStaffUsersAsync(StaffUserListFilter filter, CancellationToken cancellationToken = default) => _repository.GetStaffUsersAsync(filter, cancellationToken);
    public Task<StaffUserEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(id, cancellationToken);
    public Task<int> SaveAsync(StaffUserEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) => _repository.DeleteAsync(id, cancellationToken);
}