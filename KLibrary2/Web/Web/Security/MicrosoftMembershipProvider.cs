using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using Keiho.Text.RegularExpressions;

namespace Keiho.Web.Security
{
    public class MicrosoftMembershipProvider : MembershipProvider
    {
        private const string LiveConnectProfileUriFormat = "https://apis.live.net/v5.0/me?access_token={0}";

        /// <summary>
        /// 現在実行中のアプリケーションの名前を取得または設定します。
        /// </summary>
        /// <value>アプリケーションの名前。</value>
        public override string ApplicationName { get; set; }

        /// <summary>
        /// プロバイダーを初期化します。
        /// </summary>
        /// <param name="name">プロバイダーの表示名。</param>
        /// <param name="config">このプロバイダーの構成で指定されている、プロバイダー固有の属性を表す名前と値のペアのコレクション。</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null) throw new ArgumentNullException("config");

            base.Initialize(name, config);

            ApplicationName = config["applicationName"] ?? string.Empty;
        }

        /// <summary>
        /// 指定されたユーザー名が有効かどうかを検証します。
        /// </summary>
        /// <param name="username">検証対象のユーザー名。</param>
        /// <param name="password">指定されたユーザーのパスワード。使用されません。</param>
        /// <returns>指定されたユーザー名が有効な場合は true。それ以外の場合は false。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="username"/> が <see langword="null"/> です。</exception>
        /// <exception cref="System.ArgumentException"><paramref name="username"/> が空文字列です。</exception>
        public override bool ValidateUser(string username, string password)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (username.Length == 0) throw new ArgumentException("値を空にすることはできません。", "username");

            var cookie = HttpContext.Current.Request.Cookies["wl_auth"];
            if (cookie == null)
            {
                return false;
            }

            string accessToken = cookie.Values["access_token"];
            if (string.IsNullOrEmpty(accessToken))
            {
                return false;
            }
#if DEBUG
            HttpContext.Current.Trace.Write("Access Token", accessToken);
#endif

            string profileJson;
            try
            {
                using (var client = new WebClient())
                {
                    profileJson = client.DownloadString(string.Format(LiveConnectProfileUriFormat, accessToken));
                }
            }
            catch (WebException ex)
            {
                return false;
            }

            string userId = profileJson.Match("\"id\"\\s*:\\s*\"([a-zA-Z0-9]+)\"", 1);
            return userId == username;
        }

        #region Unsupported Methods

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
