using Microsoft.AspNetCore.Http;
using System;

namespace Leemo.Web
{
    public class SessionManager
    {
        private readonly ISession _session;
        private const String ID_KEY = "_ID";
        private const String LOGIN_KEY = "_LoginName";
        private const String BEARER_TOKEN = "token_value";
        private const String COMPANY_ID = "CompanyId";
        private const String PROFILEIMAGE_NAME = "ProfileImage";
        private const String USER_EMAIL = "UserEmail";
        private const String USER_Auth_Role = "Auth_Role";
        private const String COMPANY_LOCATION_ID = "CompanyLocationID";
        private const String COMPANYLOCATION_NAME = "CompanyLocationName";
        private const String ResreshToken_Data = "ResreshTokenData";
        private const String IS_SINGLELOCATION = "SingleLocation";
        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public string ID
        {
            get
            {
                var v = _session.GetString(ID_KEY);
                if (!string.IsNullOrEmpty(v))
                    return Convert.ToString(v.Trim());
                else
                    return string.Empty;
            }
            set
            {
                _session.SetString(ID_KEY, value);
            }
        }
      
        public String LoginName
        {
            get
            {
                return _session.GetString(LOGIN_KEY);
            }
            set
            {
                _session.SetString(LOGIN_KEY, value);
            }
        }
        public Boolean IsLoggedIn
        {
            get
            {
                if (!string.IsNullOrEmpty(ID))
                    return true;
                else
                    return false;
            }
        }

        public String BearerToken
        {
            get
            {
                return _session.GetString(BEARER_TOKEN);
            }
            set
            {
                _session.SetString(BEARER_TOKEN, value);
            }
        }
        public String CompanyId
        {
            get
            {
                return _session.GetString(COMPANY_ID);
            }
            set
            {
                _session.SetString(COMPANY_ID, value);
            }
        }

        public String UserProfileImage
        {
            get
            {
                return _session.GetString(PROFILEIMAGE_NAME);
            }
            set
            {
                _session.SetString(PROFILEIMAGE_NAME, value);
            }
        }
        public String UserEmail
        {
            get
            {
                return _session.GetString(USER_EMAIL);
            }
            set
            {
                _session.SetString(USER_EMAIL, value);
            }
        }
        public void EmptySession()
        {
            _session.Clear();
        }
        public String USERAuthRole
        {
            get
            {
                return _session.GetString(USER_Auth_Role);
            }
            set
            {
                _session.SetString(USER_Auth_Role, value);
            }
        }
        public String CompanyLocationID
        {
            get
            {
                return _session.GetString(COMPANY_LOCATION_ID);
            }
            set
            {
                _session.SetString(COMPANY_LOCATION_ID, value);
            }
        }
        public String CompanyLocationName
        {
            get
            {
                return _session.GetString(COMPANYLOCATION_NAME);
            }
            set
            {
                _session.SetString(COMPANYLOCATION_NAME, value);
            }
        }

        public String ResreshTokenData
        {
            get
            {
                return _session.GetString(ResreshToken_Data);
            }
            set
            {
                _session.SetString(ResreshToken_Data, value);
            }
        }

        public String SingleLocation
        {
            get
            {
                return _session.GetString(IS_SINGLELOCATION);
            }
            set
            {
                _session.SetString(IS_SINGLELOCATION, value);
            }
        }
    }


}
