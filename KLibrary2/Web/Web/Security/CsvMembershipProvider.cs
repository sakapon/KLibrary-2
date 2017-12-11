using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Keiho.Web.Security
{
    /// <summary>
    /// ASP.NET アプリケーションのメンバーシップ情報を CSV ファイルで管理します。
    /// </summary>
    /// <remarks>
    /// ユーザー名とパスワードのペアの一覧を Accounts.csv として作成し、Web サイトの App_Data フォルダーに配置してください。
    /// </remarks>
    public class CsvMembershipProvider : MembershipProvider
    {
        /// <summary>
        /// 有効なユーザー名とパスワードのペアの一覧を初期化します。
        /// </summary>
        /// <returns>有効なユーザー名とパスワードのペアの一覧。</returns>
        private static Dictionary<string, string> GetAccounts()
        {
            string accountsPath = HttpContext.Current.Server.MapPath("~/App_Data/Accounts.csv");

            return File.ReadAllLines(accountsPath, SharedObjects.Encodings.ShiftJis)
                .Select(l => l.Split(','))
                .ToDictionary(p => p[0].ToLowerInvariant(), p => p[1]);
        }

        /// <summary>
        /// 現在実行中のアプリケーションの名前を取得または設定します。
        /// </summary>
        /// <value>アプリケーションの名前。</value>
        public override string ApplicationName { get; set; }

        /// <summary>
        /// 有効なユーザー名とパスワードのペアの一覧です。
        /// </summary>
        /// <value>有効なユーザー名とパスワードのペアの一覧。</value>
        private Dictionary<string, string> accounts;

        /// <summary>
        /// プロバイダーを初期化します。
        /// </summary>
        /// <param name="name">プロバイダーの表示名。</param>
        /// <param name="config">このプロバイダーの構成で指定されている、プロバイダー固有の属性を表す名前と値のペアのコレクション。</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            base.Initialize(name, config);

            ApplicationName = config["applicationName"] ?? string.Empty;

            accounts = GetAccounts();
        }

        /// <summary>
        /// 指定されたユーザー名とパスワードがデータ ソースに存在しているかどうかを検証します。
        /// </summary>
        /// <param name="username">検証対象のユーザー名。</param>
        /// <param name="password">指定されたユーザーのパスワード。</param>
        /// <returns>指定されたユーザー名とパスワードが有効な場合は true。それ以外の場合は false。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="username"/> または <paramref name="password"/> が <see langword="null"/> です。</exception>
        /// <exception cref="System.ArgumentException"><paramref name="username"/> または <paramref name="password"/> が空文字列です。</exception>
        public override bool ValidateUser(string username, string password)
        {
            if (username == null)
            {
                throw new ArgumentNullException("username");
            }
            if (username.Length == 0)
            {
                throw new ArgumentException("値を空にすることはできません。", "username");
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            if (password.Length == 0)
            {
                throw new ArgumentException("値を空にすることはできません。", "password");
            }

            string lowerUsername = username.ToLowerInvariant();
            return accounts.ContainsKey(lowerUsername) && accounts[lowerUsername] == password;
        }

        #region Unsupported Methods

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotSupportedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotSupportedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotSupportedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotSupportedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotSupportedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotSupportedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotSupportedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotSupportedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotSupportedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotSupportedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotSupportedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotSupportedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotSupportedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotSupportedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotSupportedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotSupportedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotSupportedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotSupportedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotSupportedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
