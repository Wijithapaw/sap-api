using Microsoft.AspNetCore.Authorization;

namespace SAP.Api.Authorization
{
    internal class ClaimAuthorizeAttribute : AuthorizeAttribute
    {
        internal const string POLICY_PREFIX = "REQUIRE_CLAIM_";

        public ClaimAuthorizeAttribute(string claim) => Claim = claim;

        public string Claim
        {
            get
            {
                var claim = Policy.Substring(POLICY_PREFIX.Length);
                return claim;
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value}";
            }
        }
    }
}
