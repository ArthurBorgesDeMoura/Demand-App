namespace IDemandApp.Endpoints.Security;

public class TokenPost
{
    public static string Template => "/token";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static IResult Action
        (LoginRequestDTO request, 
        UserManager<IdentityUser> userManager, 
        IConfiguration configuration, 
        ILogger<TokenPost> logger, 
        IWebHostEnvironment env)
    {
        logger.LogInformation("Getting Token");

        var user = userManager.FindByEmailAsync(request.Email).Result;

        if (!userManager.CheckPasswordAsync(user, request.Password).Result || user == null)
            Results.BadRequest();

        var claims = userManager.GetClaimsAsync(user).Result;
        var subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, request.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            });
        subject.AddClaims(claims);

        var key = Encoding.ASCII.GetBytes(configuration["Security:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = configuration["Security:Audience"],
            Issuer = configuration["Security:Issuer"],
            Expires = env.IsDevelopment() || env.IsStaging() ? DateTime.UtcNow.AddYears(1) : DateTime.UtcNow.AddSeconds(120)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Results.Ok(new
        {
            token = tokenHandler.WriteToken(token),
        });
    }
}
