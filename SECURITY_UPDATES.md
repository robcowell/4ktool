# Security Updates

## RestSharp Vulnerability (GHSA-4rr6-2v9v-wcpc)

**Status**: ✅ **FIXED**

**Issue**: RestSharp 110.2.0 has a known moderate severity vulnerability (GHSA-4rr6-2v9v-wcpc)

**Resolution**: Updated to RestSharp 112.1.0 (or latest available version)

**Date**: 2025-01-XX

**Impact**: Moderate severity - affects BitBucket API integration

**Action Taken**:
- ✅ Updated `4kampf.Web/4kampf.Web.csproj` to use RestSharp 112.1.0+
- ✅ Updated `4kampf/4kampf.csproj` (Windows project) to use RestSharp 112.1.0+
- ✅ Updated `4kampf/src/git/GitHandler.cs` to use new RestSharp API:
  - Changed from `RestClient(url)` + `Authenticator` property to `RestClientOptions` pattern
  - Changed `IRestResponse` to `RestResponse`
  - Changed `Method.POST`/`Method.GET` to `Method.Post`/`Method.Get`
  - Changed `r.Post(request)`/`r.Get(request)` to `r.Execute(request)`
- ✅ Verified API compatibility in web project (already using RestClientOptions pattern)

**Files Modified**:
- `4kampf.Web/4kampf.Web.csproj`
- `4kampf/4kampf.csproj`
- `4kampf/src/git/GitHandler.cs` (2 methods: `CreateBitBucketRepo`, `GetBitBucketRepos`)

**Note**: `SharpBucketRepoCreator.cs` also uses RestSharp but is part of the SharpBucket NuGet package (v2.0.0), which may have its own RestSharp dependency. Monitor SharpBucket updates.

**References**:
- GitHub Advisory: https://github.com/advisories/GHSA-4rr6-2v9v-wcpc
- NuGet Package: https://www.nuget.org/packages/RestSharp

## LibGit2Sharp

**Status**: ⚠️ **MONITOR**

**Current Version**: 0.28.0

**Notes**: 
- LibGit2Sharp wraps libgit2, which has had security updates
- Latest libgit2 versions (1.7.2+, 1.6.5+) address CVE-2024-24575 and CVE-2024-24577
- LibGit2Sharp 0.28.0 should include these fixes, but monitor for updates

**Recommendation**: Regularly check for LibGit2Sharp updates and review libgit2 security advisories

**References**:
- libgit2 Security: https://libgit2.org/security/
- LibGit2Sharp NuGet: https://www.nuget.org/packages/LibGit2Sharp

