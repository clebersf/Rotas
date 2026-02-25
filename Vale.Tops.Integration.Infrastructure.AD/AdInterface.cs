using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.Security.Claims;
using Vale.Tops.Domain;

namespace Vale.Tops.Integration.Infrastructure.AD
{
    public class AdInterface
    {        
        public bool IsLogin(string AdServer, string name, string password)
        {                        
            try
            {
                DirectoryEntry directoryEntry = new DirectoryEntry(AdServer, name, password);
                DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry);
                directorySearcher.Filter = "(SAMAccountName=" + name + ")";
                SearchResult searchResult = directorySearcher.FindOne();
                if (searchResult != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }                   
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool IsGroups(string AdServer, string user, string group)
        {
            bool ret = false;
            try
            {
                //Verifica acesso do usuário no domínio
                DirectoryEntry directoryEntry_ = new DirectoryEntry(AdServer, "svc.assetman", "svc.assetman");
                DirectorySearcher directorySearcher_ = new DirectorySearcher(directoryEntry_);
                directorySearcher_.Filter = "(SAMAccountName=" + user + ")";
                SearchResult searchResult = directorySearcher_.FindOne();
                //Verifica se as rules coinsiden com o domínio
                if (searchResult != null)
                {
                    DirectoryEntry person = searchResult.GetDirectoryEntry();
                    PropertyValueCollection groups = person.Properties["memberOf"];
                    foreach (string g in groups)
                    {
                        ret = g.Contains(group) ? true : ret;
                    }
                }
                return ret;
            }
            catch
            {
                return false;
            }            
        }

        public List<string> Groups(string AdServer, string user)
        {
            try
            {
                //Verifica acesso do usuário no domínio
                DirectoryEntry directoryEntry_ = new DirectoryEntry(AdServer, "svc.assetman", "svc.assetman");
                DirectorySearcher directorySearcher_ = new DirectorySearcher(directoryEntry_);
                directorySearcher_.Filter = "(SAMAccountName=" + user + ")";
                SearchResult searchResult = directorySearcher_.FindOne();
                //Verifica se as rules coinsiden com o domínio
                if (searchResult != null)
                {
                    DirectoryEntry person = searchResult.GetDirectoryEntry();
                    PropertyValueCollection groups = person.Properties["memberOf"];
                    List<string> results = new List<string>();

                    foreach (object  result in groups)
                    {
                        var f = result.ToString().Split(',');
                        results.Add(f[0].Replace("CN=",""));
                    }
                    return results;
                }
                return new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        public UserInfo UserInfo(string AdServer, string user)
        {
            UserInfo ui = new UserInfo();
            try
            {
                //Verifica acesso do usuário no domínio
                DirectoryEntry directoryEntry_ = new DirectoryEntry(AdServer, "svc.assetman", "svc.assetman");
                DirectorySearcher directorySearcher_ = new DirectorySearcher(directoryEntry_);
                directorySearcher_.Filter = "(SAMAccountName=" + user + ")";
                SearchResult searchResult = directorySearcher_.FindOne();
                //Verifica se as rules coinsiden com o domínio
                ui.FirstName = (String)searchResult.Properties["givenName"][0];
                ui.LastName = (String)searchResult.Properties["sn"][0];
                ui.EmailAddress = (String)searchResult.Properties["mail"][0];
                ui.UserDisplayName = (String)searchResult.Properties["displayName"][0];
                ui.TelephoneNumber = (String)searchResult.Properties["TelephoneNumber"][0];                
                return ui;
            }
            catch
            {
                return ui;
            }
        }

        public List<UserInfo> Members(string AdServer, string group)
        {
            List <UserInfo> lst = new List<UserInfo>();
            DirectoryEntry entry = new DirectoryEntry(AdServer, "svc.assetman", "svc.assetman");
            DirectorySearcher searcher = new DirectorySearcher("(&(objectCategory=group)(cn=" + group + "))");
            searcher.SearchRoot = entry;
            searcher.SearchScope = SearchScope.Subtree;
            SearchResult result = searcher.FindOne();
            foreach (string member in result.Properties["member"])
            {
                string s = member;
                UserInfo ui = new UserInfo();
                DirectoryEntry de = new DirectoryEntry(String.Concat(AdServer, "/", member.ToString()));
                if (de.Properties["objectClass"].Contains("user") && de.Properties["cn"].Count > 0)
                {
                    ui.account= (String)de.Properties["SAMAccountName"][0].ToString();
                    ui.FirstName = (String)de.Properties["givenName"][0].ToString();
                    if (de.Properties["sn"] != null && de.Properties["sn"].Count > 0)
                    {
                        ui.LastName = de.Properties["sn"].Value.ToString();
                    }
                    if (de.Properties["mail"] != null && de.Properties["mail"].Count > 0)
                    {
                        ui.EmailAddress = de.Properties["mail"].Value.ToString();
                    }
                    if (de.Properties["displayName"] != null && de.Properties["displayName"].Count > 0)
                    {
                        ui.UserDisplayName = de.Properties["displayName"].Value.ToString();
                    }
                    if (de.Properties["TelephoneNumber"] != null && de.Properties["TelephoneNumber"].Count > 0)
                    {
                        ui.TelephoneNumber = de.Properties["TelephoneNumber"].Value.ToString();
                    }
                }
                lst.Add(ui);
            }
            return lst;
        }
    }
}