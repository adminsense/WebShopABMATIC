using WebShopABMATIC.Application.Admin.StaffUsers;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Validation;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class StaffUserAdminUseCase : IStaffUserAdminPort
{
    private readonly IStaffUserRepository _repository;

    public StaffUserAdminUseCase(IStaffUserRepository repository) => _repository = repository;

    public Task<PagedResult<StaffUserDto>> GetStaffUsersAsync(StaffUserListFilter filter, CancellationToken cancellationToken = default) =>
        _repository.GetStaffUsersAsync(filter, cancellationToken);

    public Task<StaffUserEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        _repository.GetForEditAsync(id, cancellationToken);

    public Task<IReadOnlyList<StaffUserGroupLookupDto>> GetGroupLookupsAsync(CancellationToken cancellationToken = default) =>
        _repository.GetGroupLookupsAsync(cancellationToken);

    public Task<int> SaveAsync(StaffUserEditDto dto, CancellationToken cancellationToken = default)
    {
        var errors = AdminEditFormValidator.Validate(dto);
        if (errors.Count > 0)
        {
            throw new InvalidOperationException(string.Join(" ", errors.Select(e => e.ErrorMessage)));
        }

        return _repository.SaveAsync(dto, cancellationToken);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) =>
        _repository.DeleteAsync(id, cancellationToken);
}
