internal interface IRefreshTokenStore
{
	ValueTask StoreAsync(int userId, string refreshToken);

	ValueTask<int> ValidateAndRotateAsync(string refreshToken);

	ValueTask RevokeAsync(string refreshToken);
}
