using CoworkingSpaceAPI.Dtos.Auth.Request;
using CoworkingSpaceAPI.Dtos.Auth.Response;
using CoworkingSpaceAPI.Models;
using System.Text.Json;

namespace CoworkingSpaceAPI.Services.JwtToken
{
	public interface IJwtTokenService
	{
		Task<string> GenerateToken(ApplicationUserModel user);

		Task<RefreshTokenResponseDto> VerifyToken(TokenRequestDto tokenRequest);

		public JsonDocument DecodeJwtPayloadToJson(string token);
	}
}