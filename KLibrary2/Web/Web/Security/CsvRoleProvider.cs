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
    /// ASP.NET アプリケーションのロール情報を CSV ファイルで管理します。
    /// </summary>
    /// <remarks>
    /// ユーザー名とロール名のペアの一覧を UsersInRoles.csv として作成し、Web サイトの App_Data フォルダーに配置してください。
    /// </remarks>
    public class CsvRoleProvider : RoleProvider
    {
        /// <summary>
        /// ユーザー名とロール名のペアの一覧を初期化します。
        /// </summary>
        /// <returns>ユーザー名とロール名のペアの一覧。</returns>
        private static Dictionary<string, string[]> GetUsersInRoles()
        {
            string usersInRolesPath = HttpContext.Current.Server.MapPath("~/App_Data/UsersInRoles.csv");

            return File.ReadAllLines(usersInRolesPath, SharedObjects.Encodings.ShiftJis)
                .Select(l => l.Split(','))
                .GroupBy(p => p[0].ToLowerInvariant(), p => p[1])
                .ToDictionary(g => g.Key, g => g.ToArray());
        }

        /// <summary>
        /// 現在実行中のアプリケーションの名前を取得または設定します。
        /// </summary>
        /// <value>アプリケーションの名前。</value>
        public override string ApplicationName { get; set; }

        /// <summary>
        /// ユーザー名とロール名のペアの一覧です。
        /// </summary>
        /// <value>ユーザー名とロール名のペアの一覧。</value>
        private Dictionary<string, string[]> usersInRoles;

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

            usersInRoles = GetUsersInRoles();
        }

        /// <summary>
        /// 指定されたユーザーに割り当てられたロールの一覧を取得します。
        /// </summary>
        /// <param name="username">ロールの一覧を取得するユーザーの名前。</param>
        /// <returns>ロールの一覧。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="username"/> が <see langword="null"/> です。</exception>
        /// <exception cref="System.ArgumentException"><paramref name="username"/> が空文字列です。</exception>
        public override string[] GetRolesForUser(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException("username");
            }
            if (username.Length == 0)
            {
                throw new ArgumentException("値を空にすることはできません。", "username");
            }

            string lowerUsername = username.ToLowerInvariant();
            return usersInRoles.ContainsKey(lowerUsername) ? usersInRoles[lowerUsername] : new string[0];
        }

        #region Unsupported Methods

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotSupportedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotSupportedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotSupportedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotSupportedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotSupportedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotSupportedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
