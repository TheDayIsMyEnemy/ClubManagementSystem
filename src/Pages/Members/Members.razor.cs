using ClubManagementSystem.Data.Entities;
using ClubManagementSystem.Interfaces;
using ClubManagementSystem.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClubManagementSystem.Pages.Members
{
    public class MembersBase : ComponentBase, IDisposable
    {
        [Inject]
        private IMemberService _memberService { get; set; } = null!;

        [Inject]
        private IDialogService _dialogService { get; set; } = null!;

        [Inject]
        private ISnackbar _snackbar { get; set; } = null!;

        protected List<Member> Members { get; set; } = new();
        protected string? SearchQuery { get; set; }
        protected bool IsLoading { get; set; }

        private async Task LoadMembers()
        {
            IsLoading = true;
            Members = await _memberService.GetAllMembers();
            IsLoading = false;
        }

        protected override async Task OnInitializedAsync()
            => await LoadMembers();

        protected bool Filter(Member member) => FilterMembers(member, SearchQuery);

        protected bool FilterMembers(Member member, string? searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
                return true;
            if (member.FirstName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                return true;
            if (member.LastName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                return true;
            if (member.Gender.ToString().Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                return true;
            if (member.PhoneNumber != null && member.PhoneNumber.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        protected async Task CreateMember()
        {
            var result = await _dialogService.Show<CreateMember>("Add Member").Result;

            if (!result.Cancelled)
            {
                var member = (Member)result.Data;
                var isCreated = await _memberService.CreateMember(member);
                if (isCreated)
                {
                    _snackbar.Add(string.Format(Messages.SuccessfulCreationFormat, member.FullName),
                        Severity.Success);
                    Members.Add(member);
                }
                else { _snackbar.Add(Messages.Error, Severity.Error); }
            }
        }

        protected async Task EditMember(Member member)
        {
            var dialogParams = new DialogParameters { ["Member"] = member };
            var result = await _dialogService.Show<EditMember>("Edit Member", dialogParams).Result;

            if (!result.Cancelled)
            {
                var isUpdated = await _memberService.UpdateMember(member);
                if (isUpdated)
                {
                    _snackbar.Add(string.Format(Messages.SuccessfulUpdateFormat, member.FullName),
                        Severity.Success);
                }
                else { _snackbar.Add(Messages.Error, Severity.Error); }
            }
        }

        protected async Task DeleteMember(Member member)
        {
            var dialogParams = new DialogParameters
            {
                ["TitleText"] = "Delete Member",
                ["ContentText"] = $"Are you sure you want to delete {member.FullName}?"
            };
            var result = await _dialogService.Show<DeleteEntityModal>("", dialogParams).Result;

            if (!result.Cancelled)
            {
                var isDeleted = await _memberService.DeleteMember(member);
                if (isDeleted)
                {
                    _snackbar.Add(string.Format(Messages.SuccessfulDeletionFormat, member.FullName),
                        Severity.Success);
                    Members.Remove(member);
                }
                else { _snackbar.Add(Messages.Error, Severity.Error); }
            }
        }

        public void Dispose() => _memberService.Dispose();
    }
}
