namespace Mawo.Auth.Service.Data;

public static class UserStorage
{
	public static readonly User[] Users = [
		 new User{
			 Id =1,
			 Email = "test1@test.pl",
			 Password = "Test1234",
			Name = "Uzytkownik 1",
			 Role = "Admin"
		 },
		 new User{
			 Id =2,
			 Email = "test2@test.pl",
			 Password = "Test1234",
			Name = "Uzytkownik 2",
			 Role = "Worker"
		 }
		];
}
