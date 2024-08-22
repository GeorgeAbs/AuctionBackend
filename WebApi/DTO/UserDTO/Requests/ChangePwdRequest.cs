namespace WebApi.DTO.UserDTO.Requests
{
    public class ChangePwdRequest
    {

        public string CurrendPwd { get; set; }

        public string NewPwd { get; set; }

        public ChangePwdRequest(string currentPwd, string mewPwd)
        {
            CurrendPwd = currentPwd;
            NewPwd = mewPwd;
        }
    }
}
