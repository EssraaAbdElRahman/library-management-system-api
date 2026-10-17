using LibraryMS.BLL.DTOs.Member;
namespace LibraryMS.BLL.Service.Implementation
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetByIdAsync(int id)
        {
            var member = await _unitOfWork.Members.GetByIdWithBorrowRecordsAsync(id);
            if (member is null)
                throw new NotFoundException($"Member with id {id} not found.");

            return _mapper.Map<MemberDto>(member);
        }

        public async Task<List<MemberDto>> SearchAsync(string? name, bool? isActive, int page, int pageSize)
        {
            var members = await _unitOfWork.Members.SearchAsync(name, isActive, page, pageSize);
            return _mapper.Map<List<MemberDto>>(members);
        }

        public async Task<MemberDto> CreateAsync(CreateMemberDto dto, string createdBy)
        {
            if (!string.IsNullOrWhiteSpace(dto.Email)
                && await _unitOfWork.Members.IsEmailExistsAsync(dto.Email))
                throw new BusinessRuleException($"A member with email {dto.Email} already exists.");

            if (await _unitOfWork.Members.IsPhoneExistsAsync(dto.PhoneNumber))
                throw new BusinessRuleException($"A member with phone number {dto.PhoneNumber} already exists.");

            var member = new Member(dto.FullName, dto.Email, dto.PhoneNumber, DateTime.UtcNow, createdBy);

            await _unitOfWork.Members.AddAsync(member);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<MemberDto>(member);
        }

        public async Task<MemberDto> UpdateAsync(int id, UpdateMemberDto dto, string updatedBy)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id);
            if (member is null)
                throw new NotFoundException($"Member with id {id} not found.");

            if (!string.IsNullOrWhiteSpace(dto.Email)
                && await _unitOfWork.Members.IsEmailExistsAsync(dto.Email, excludeMemberId: id))
                throw new BusinessRuleException($"A member with email {dto.Email} already exists.");

            if (await _unitOfWork.Members.IsPhoneExistsAsync(dto.PhoneNumber, excludeMemberId: id))
                throw new BusinessRuleException($"A member with phone number {dto.PhoneNumber} already exists.");

            member.UpdateMember(dto.FullName, dto.Email, dto.PhoneNumber, updatedBy);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<MemberDto>(member);
        }

        public async Task DeleteAsync(int id, string deletedBy)
        {
            var member = await _unitOfWork.Members.GetByIdWithBorrowRecordsAsync(id);
            if (member is null)
                throw new NotFoundException($"Member with id {id} not found.");

            var hasActiveBorrows = member.BorrowRecords.Any(br => !br.IsReturned);
            if (hasActiveBorrows)
                throw new BusinessRuleException("Cannot delete a member with active borrow records.");

            member.Deleted(deletedBy);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<MemberDto> ActivateAsync(int id, string updatedBy)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id);
            if (member is null)
                throw new NotFoundException($"Member with id {id} not found.");

            member.Activate(updatedBy);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<MemberDto>(member);
        }

        public async Task<MemberDto> DeactivateAsync(int id, string updatedBy)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id);
            if (member is null)
                throw new NotFoundException($"Member with id {id} not found.");

            member.Deactivate(updatedBy);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<MemberDto>(member);
        }
    }
}
