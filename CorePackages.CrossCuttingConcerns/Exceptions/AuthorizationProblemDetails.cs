using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CorePackages.CrossCuttingConcerns.Exceptions;

public class AuthorizationProblemDetails : ProblemDetails
{
    public override string ToString() => JsonConvert.SerializeObject(this);
}
