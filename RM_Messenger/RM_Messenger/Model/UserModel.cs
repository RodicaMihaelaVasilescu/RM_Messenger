namespace RM_Messenger.Model
{
  class UserModel
  {
    private static UserModel _instance;
    public static UserModel Instance => _instance ?? (_instance = new UserModel());
    public string Status { get; set; }
    public string Username { get; set; }
    public string ID { get; set; }
    public byte[] ProfilePicture { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsOnline { get; set; } = true;
    public string Email { get; set; }
    public string PostalCode { get; set; }
    public string EncryptedPassword { get; set; }

  }
}
