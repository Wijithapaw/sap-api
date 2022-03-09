using Microsoft.AspNetCore.Authorization;
using SAP.Domain.Constants;
using System.Threading.Tasks;

namespace SAP.Api.Authorization
{
    internal class CustomAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser().Build());
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return Task.FromResult(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser().Build());
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var requiredClaim = policyName.Substring(ClaimAuthorizeAttribute.POLICY_PREFIX.Length);

            var policy = new AuthorizationPolicyBuilder();

            policy.RequireClaim(CustomClaimTypes.SapPermission, requiredClaim);

            return Task.FromResult(policy.Build());
        }
    }
}
