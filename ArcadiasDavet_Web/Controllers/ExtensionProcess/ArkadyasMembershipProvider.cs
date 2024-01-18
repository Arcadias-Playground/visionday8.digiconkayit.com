using System;
using System.Web.Security;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkezi
{
    public class ArkadyasMembershipProvider : MembershipProvider
    {
        public override bool EnablePasswordRetrieval => throw new NotImplementedException();

        public override bool EnablePasswordReset => throw new NotImplementedException();

        public override bool RequiresQuestionAndAnswer => throw new NotImplementedException();

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int MaxInvalidPasswordAttempts => throw new NotImplementedException();

        public override int PasswordAttemptWindow => throw new NotImplementedException();

        public override bool RequiresUniqueEmail => throw new NotImplementedException();

        public override MembershipPasswordFormat PasswordFormat => throw new NotImplementedException();

        public override int MinRequiredPasswordLength => throw new NotImplementedException();

        public override int MinRequiredNonAlphanumericCharacters => throw new NotImplementedException();

        public override string PasswordStrengthRegularExpression => throw new NotImplementedException();

        public override bool ChangePassword(string username, string oldPassword, string newPassword) => throw new NotImplementedException();

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer) => throw new NotImplementedException();

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status) => throw new NotImplementedException();

        public override bool DeleteUser(string username, bool deleteAllRelatedData) => throw new NotImplementedException();

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords) => throw new NotImplementedException();

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords) => throw new NotImplementedException();

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords) => throw new NotImplementedException();

        public override int GetNumberOfUsersOnline() => throw new NotImplementedException();

        public override string GetPassword(string username, string answer) => throw new NotImplementedException();

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline) => throw new NotImplementedException();

        public override MembershipUser GetUser(string username, bool userIsOnline) => throw new NotImplementedException();

        public override string GetUserNameByEmail(string email) => throw new NotImplementedException();

        public override string ResetPassword(string username, string answer) => throw new NotImplementedException();

        public override bool UnlockUser(string userName) => throw new NotImplementedException();

        public override void UpdateUser(MembershipUser user) => throw new NotImplementedException();

        public override bool ValidateUser(string username, string password)
        {
            VTIslem = new VTOperatorleri("SELECT COUNT(*) FROM KullaniciTablosu WHERE ePosta=@ePosta AND Sifre=@Sifre");
            VTIslem.AddWithValue("ePosta", username);
            VTIslem.AddWithValue("Sifre", password);
            return Convert.ToInt32(VTIslem.ExecuteScalar()).Equals(1);
        }

        VTOperatorleri VTIslem;
    }

    public class ArkadyasRoleProvider : RoleProvider
    {
        VTOperatorleri VTIslem;

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames) => throw new NotImplementedException();

        public override void CreateRole(string roleName) => throw new NotImplementedException();

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole) => throw new NotImplementedException();

        public override string[] FindUsersInRole(string roleName, string usernameToMatch) => throw new NotImplementedException();

        public override string[] GetAllRoles() => throw new NotImplementedException();

        public override string[] GetRolesForUser(string username)
        {
            string[] Rol;
            try
            {
                VTIslem = new VTOperatorleri("SELECT KullaniciTipiTablosu.KullaniciTipi FROM KullaniciTablosu INNER JOIN KullaniciTipiTablosu ON KullaniciTablosu.KullaniciTipiID = KullaniciTipiTablosu.KullaniciTipiID WHERE ePosta=@ePosta");
                VTIslem.AddWithValue("ePosta", username);
                Rol = VTIslem.ExecuteScalar().ToString().Split(',');
            }
            catch
            {
                Rol = new string[0];
            }

            return Rol;
        }

        public override string[] GetUsersInRole(string roleName) => throw new NotImplementedException();

        public override bool IsUserInRole(string username, string roleName) => throw new NotImplementedException();

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames) => throw new NotImplementedException();

        public override bool RoleExists(string roleName) => throw new NotImplementedException();
    }
}
