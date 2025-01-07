using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.SO.Models;

public class User
{
    public Guid? Id { get; set; }
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public bool? EmailVerified { get; set; }
    public bool? Enabled { get; set; }
    public string[]? RequiredActions { get; set; }
    public long? CreatedTimestamp { get; set; }
    public string? ErrorMessage { get; set; }
    public Dictionary<string, string[]> Attributes { get; set; } = new Dictionary<string, string[]>();
    public Dictionary<string, string[]> ClientRoles { get; set; } = new Dictionary<string, string[]>();
    public Dictionary<string, string[]> ApplicationRoles { get; set; } = new Dictionary<string, string[]>();
    public List<string> Groups { get; set; }
    public List<SocialLinkRepresentation> SocialLinks { get; set; } = new List<SocialLinkRepresentation>();
    public List<FederatedIdentityRepresentation> FederatedIdentities { get; set; } = new List<FederatedIdentityRepresentation>();
}
public class SocialLinkRepresentation
{
    public string SocialProvider { get; set; }
    public string SocialUserId { get; set; }
    public string SocialUsername { get; set; }

}
public class FederatedIdentityRepresentation
{
    public string IdentityProvider { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }

}

