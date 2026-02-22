internal class InMemoryRefreshTokenStore : IRefreshTokenStore
{
	private readonly Dictionary<string, int> _tokenToUser = new();

	public ValueTask StoreAsync(int userId, string refreshToken)
	{
		_tokenToUser[refreshToken] = userId;
		return ValueTask.CompletedTask;
	}

	public ValueTask<int> ValidateAndRotateAsync(string refreshToken)
	{
		if (_tokenToUser.TryGetValue(refreshToken, out var userId))
		{
			_tokenToUser.Remove(refreshToken);
			return ValueTask.FromResult<int>(userId);
		}

		return ValueTask.FromResult<int>(0);
	}

	public ValueTask RevokeAsync(string refreshToken)
	{
		_tokenToUser.Remove(refreshToken);
		return ValueTask.CompletedTask;
	}
}
