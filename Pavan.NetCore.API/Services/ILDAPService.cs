using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using Pavan.NetCore.Api.Models;

namespace Pavan.NetCore.Api.Services
{
    public interface ILDAPService
    {
        List<String> GetMembersInGroup(string groupName);
    }

    public class LDAPService : ILDAPService
    {
        private readonly LDAPConfig ldapConfig;
        private LdapConnection ldapConnection;
        private int searchScope = LdapConnection.ScopeSub;
        private int ldapVersion = LdapConnection.LdapV3;
        //private string[] groupAttributes = new string[] { "objectClass",
        //                                                    "memberQueryURL",
        //                                                    "dgIdentity",
        //                                                    "excludedMember",
        //                                                    "member"
        //                                                };

        private string[] groupAttributes = new string[] { "member" };

        public LDAPService(IOptions<LDAPConfig> ldapConfig)
        {
            this.ldapConfig = ldapConfig.Value;
        }

        public List<string> GetMembersInGroup(string groupName)
        {
            ldapConnection = new LdapConnection();
            ldapConnection.Connect(ldapConfig.LDAPServer, LdapConnection.DefaultSslPort);
            ldapConnection.Bind(ldapVersion, ldapConfig.DomainUser, ldapConfig.DomainPassword);
            LdapSearchConstraints constraints = new LdapSearchConstraints();
            constraints.TimeLimit = 10000;
            Console.WriteLine($"Reading Results for {groupName}");

            var groupMemberNames = new List<string>() { String.Empty };

            var groupSearchResults = ldapConnection.Search(groupName, searchScope, null, groupAttributes, false, constraints);

            LdapEntry nextEntry = null;

            try
            {
                nextEntry = groupSearchResults.Next();
            }
            catch (LdapException ex)
            {
                Console.WriteLine($"Exception Occured with msg: {ex.Message}");
            }

            var groupMembers = nextEntry.GetAttribute("member").StringValues;
            if (groupMembers != null && groupMembers.MoveNext())
            {
                groupMemberNames.RemoveAll(i => 1 == 1);
                while (groupMembers.MoveNext())
                {
                    groupMemberNames.Add(groupMembers.Current);
                }
            }
            ldapConnection.Disconnect();
            return groupMemberNames;
        }       
    }
}


