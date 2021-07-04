/// <summary>
/// Represents Leemo common project layer
/// </summary>
namespace TPSS.Common
{
    /// <summary>
    /// All the constants will be declared here for the application.
    /// </summary>
    public static class Constants
    {
        #region App Settings
        public const string AppSettings = "AppSettings";
        public const string ConnectionSettingPath = "AppSettings:ConnectionStrings:Leemo_API_DbConnection";
        public const string ConnectionStringsSectionName = "ConnectionStrings";
        public const string API_DbConnection = "Leemo_API_DbConnection";
        public static readonly string DisplayDateFormat = "MM/dd/yyyy hh:mm tt";
        public static readonly string RoadmapFlowDisplayMonthFormat = "MMM-yyyy";
        public const string JwtTokenClaimType_UserId = "UserId";
        public const string JwtTokenClaimType_UserEmail = "UserEmail";
        public const string JwtTokenClaimType_UserRespJson = "UserRespJson";
        public const string JwtTokenClaimType_UserLocationID = "UserLocationID";
        public const string JwtTokenClaimType_UserRole = "UserRole";
        public const string ProjectSourceWEB = "WEB";
        public const string ProjectSourceAPI = "API";
        #endregion

        //Constants for logging
        public const string Logging = "Logging";

        /// <summary>
        /// Constants will be declared here for the SwaggerSettings.
        /// </summary>
        public static class SwaggerSettings
        {
            public const string RootSectionName = "SwaggerSettings";
            public const string Name = "Name";
            public const string Url = "Url";
        }

        #region Encoding and Decoding
        public static class RandomString
        {
            public const string RandomString1 = "hiimj";
            public const string RandomString2 = "mjiih";
        }
        #endregion

        /// <summary>
        /// This class contains all the messages throughout the application
        /// </summary>
        /// 
        #region ApiResponse Messages 
        public static class Messages
        {
            public const string Success = "Success";
            public const string Failed = "Failed";
            public const string BadRequest = "Bad Request";
            public const string UserDoesNotExist = "User does not exists.";
            public const string UserDisabled = "Your Account is disabled, Please contact your Admin.";
            public const string UserAlreadyExists = "User already exists for the entered email address.";
            public const string DataAlreadyExists = "Record already exists for the entered Data.";
            public const string InvalidLogin = "Incorrect password, please try again.";
            public const string InternalServerError = "Internal server error.";
            public const string NotDataExistsInTable = "No data exists in table.";
            public const string RecordNotFound = "Record not found.";
            public const string AccessDenied = "Your account does not have permission to perform this action.";
            public const string GroupNotExist = "Group does not exists.";
            public const string GroupAlreadyExist = "Group already exists for the entered name.";
            public const string CheckEmail = "Please check your email to reset password.";
            public const string ResetLinkExpire = "This link has Expired, Please go to Forgot Password Section.";
            public const string EnterName = "Please enter Name.";
            public const string RoleAlreadyExist = "Role already exists for the entered name.";
            public const string DesignationAlreadyExist = "Designation already exists for the entered name.";
            public const string LocationNotExist = "Location does not exists.";
            public const string LocationAlreadyExist = "Location already exists for the entered name.";
            //Password
            public const string PasswordChangedSuccess = "Password has been changed successfully.";
            public const string IncorrectOldPassword = "Old password is incorrect.";
            public const string PasswordResetSuccess = "Password Reset Successfully";
            public const string IncorrectTempPassword = "Incorrect Temporary Password";
            public const string PasswordNotMatch = "Confirm password does not match.";
            public const string SamePasswordMatch = "Old and New Password cannot be Same.";

            //Profile Image Upload Messages
            public const string UserProfileImageUpdated = "User profile image updated successfully";
            public const string InvalidFile = "Please select a valid image to upload.";

            //Company Image Upload Messages
            public const string CompanyImageUpdated = "Company image updated successfully";

            //Group Image Upload Messages
            public const string GroupImageUpdated = "Group image updated successfully";

            public const string LocationDisabled = "Unable to login because location you are mapped with is disabled";

            //ProductLead
            public const string ProductLeadDoesNotExist = "Data does not exists.";
        }
        #endregion

        /// <summary>
        /// This class contain all the static return response type
        /// </summary>

        #region Response Type 
        public static class ResponseType
        {
            public const short Insert = 1;
            public const short Update = 2;
            public const short Delete = 3;
            public const short AlreadyExists = 4;
            public const short Error = -1;
            public const short IncorrectOldPassword = 5;
            public const short NotFound = 6;
            public const short AccessDenied = -2;
        }
        #endregion

        /// <summary>
        /// This class contains all the static paramter names throughout the application
        /// </summary>

        public static class ParameterNames
        {
            public const string Id = "Id";
        }

        /// <summary>
        /// This class contains all the static attribute names throughout the application
        /// </summary>
        /// 
        #region Api Attributes 
        public static class Attrributes
        {
            public const string Id = "{id}";
            public const string Api_Leemo_Version_Prefix = "leemo/v1";
            public const string Api_Prefix = "api";
            public const string ApiDefaultRoute = "leemo/v1/api/[controller]/";
            public const string ApiRouteNoController = "leemo/v1/api/";

            public const string GetApiName = "get";
            public const string GetByIdApiName = "get/{id}";
            public const string ListApiName = "all";
            public const string InsertApiName = "insert";
            public const string UpdateApiName = "update/{id}";
            public const string DeleteApiName = "delete";
            public const string Exception = "exception";
        }
        #endregion

        /// <summary>
        /// This class contains all the static urls throughout the application
        /// </summary>
        #region Routes
        public static class Routes
        {
            //User
            public const string Login = "login";
            public const string User = "user";
            public const string ChangePassword = "ChangePassword";
            public const string ForgotPassword = "ForgotPassword";
            public const string ResetPassword = "ResetPassword";
            public const string GetUserFeatures = "UserFeatures/{userId}";
            public const string GetPersonalUser = "GetPersonalUser/{id}";
            public const string UpdatePersonalUser = "UpdatePersonalUser/{id}";
            public const string GetUsersPersonal = "GetUsersPersonal";
            public const string GetCompanyLocationWithUserID = "GetCompanyLocationWithUserID/{userID}";
            public const string GetUserByEmailAndCompanyLocationAndCompanyId = "GetUserByEmailAndCompanyLocationAndCompanyId";
            public const string GetCompanyUsersExceptCurrentCompanyLocation = "GetCompanyUsersExceptCurrentCompanyLocation";
            public const string GetExistingUserData = "GetExistingUserData";
            public const string GetUserCountsByLocation = "GetUserCounts/{companyLocationId}";
            public const string GetUserByOnlyEmail = "GetUserByOnlyEmail/{email}";

            //Role
            public const string SetPosition = "SetPosition";
            public const string GetDesignationStructure = "GetDesignationStructure";
            public const string GetDesignationHierarchy = "GetDesignationHierarchy";
            public const string GetProfilePermissions = "GetProfilePermissions";
            public const string GetRoleUsers = "GetRoleUsers/{roleId}";
            public const string GetActiveUsers = "GetActiveUsers";
            public const string GetAssociatedUsersWithDesignation = "GetAssociatedUsersWithDesignation/{DesignationId}";
            public const string GetUsersWithDesignation = "GetUsersWithDesignation";
            public const string GetAssociatedUsersWithDesignationforPersonal = "GetAssociatedUsersWithDesignationforPersonal/{DesignationId}";
            public const string ResetDesignationHierarchy = "ResetDesignationHierarchy";



            public const string GetProfilePermissionsByProfileId = "Profile/{id}";

            //User Profile
            public const string UpdateProfileImage = "UpdateProfileImage";
            public const string UpdatePersonalProfileImage = "UpdatePersonalProfileImage";
            public const string GetRolesForPersonalUser = "GetRolesForPersonalUser";
            public const string GetRoleByName = "GetRoleByName";

            //Company
            public const string UpdateCompanyImage = "UpdateCompanyImage";

            //Group
            public const string GetGroupByName = "GetGroupByName";
            //public const string GetGroupByName = "GetGroupByName/{groupName}";
            public const string GetGroupsByLocation = "GetGroups/{companyLocationId}";
            public const string GetGroupCountsByLocation = "GetGroupCounts/{companyLocationId}";
           
            //Get User by Email
            public const string GetUserByEmail = "GetUserByEmail/{email}";

            //Group
            public const string UpdateGroupImage = "UpdateGroupImage";

            //RoleFeatures
            public const string GetProfilePermissionsByAuth_RoleId = "GetProfilePermissionsByAuth_RoleId";
            public const string InsertUpdateAuth_RoleFeatureMappingTemp = "InsertUpdateAuth_RoleFeatureMappingTemp";
            public const string BulkUpdateAuth_RoleFeatureMappingTemp = "BulkUpdateAuth_RoleFeatureMappingTemp";
            public const string UpdateAuth_RoleFeatureMappingChanges = "UpdateAuth_RoleFeatureMappingChanges";

            //Auth_Role
            public const string GetAuth_RoleJoinedUsers = "GetAuth_RoleJoinedUsers";

            //Designation
            public const string GetPersonalDesignation = "GetPersonalDesignation/{companyLocationId}";
            public const string GetDesignationByName = "GetDesignationByName";
            public const string GetDesignationsByLocation = "GetDesignations/{companyLocationId}";

            //CompanyLocationUserMapping
            public const string InsertCompanyLocationUserMapping = "InsertCompanyLocationUserMapping";
            public const string UpdateCompanyLocationUserMapping = "UpdateCompanyLocationUserMapping";
            public const string GetUsersWithCompanyLocation = "GetUsersWithCompanyLocation/{companyLocationId}";

            //Company Location
            public const string GetCompanyLocationByUserId = "GetCompanyLocationByUserId/{UserId}";
            public const string GetLocationCounts = "GetLocationCounts/{CompanyId}";
            public const string UpdateHeadOffice = "UpdateHeadOffice/{id}";
            public const string GetLocationByName = "GetLocationByName";

            //Product
            public const string GetProductsOfCompany = "GetProductsOfCompany/{CompanyId}";

            //ProductLead
            public const string GetProductLeadByEmail = "GetProductLeadByEmail/{email}";
            public const string GetProductLeadByCompanyName = "GetProductLeadByCompanyName/{CompanyName}";
            public const string GetProductLeadCheckAvailableDomain = "GetProductLeadCheckAvailableDomain/{domainName}";
            public const string VerifyProductLead = "VerifyProductLead/{ProductLeadId}";





        }
        #endregion


        #region Json Web Token
        public static class JwtSettings
        {
            public const string Jwt = "Jwt";
            public const string Issuer = "Issuer";
            public const string Audience = "Audience";
            public const string Subject = "Subject";
            public const string ExpiryTime = "ExpiryTime";
            public const string Key = "Key";
        }
        #endregion

        /// <summary>
        /// Constants declared here for the procedrue names.
        /// </summary>
        /// 
        #region Stored Procedurs
        public static class SP
        {
            public const string GetProfilePermissionsByProfileId = "sp_GetProfilePermissionsByProfileId";
        }

        /// <summary>
        /// Constants declared here for the procedure paramters.
        /// </summary>
        public static class SPParameters
        {
            public const string ProflieId = "@ProflieId";
        }
        #endregion

        /// <summary>
        /// Constants declared here for the profie permission names same as db.
        /// </summary>
        /// 

        #region Profile Permission 
        public static class ProfilePermissionNames
        {
            public const string DesignationManagement = "Designation Management";
            public const string UserManagement = "User Management";
            public const string CompanyManagement = "Company Management";
            public const string GroupManagement = "Group Management";
            public const string ProfileManagement = "Profile Management";
            public const string ProfilePermissionManagement = "Profile Permission Management";
        }
        #endregion

        /// <summary>
        /// Constants declared here for the profie permission access requested.
        /// </summary>
        /// 
        #region AccessRequested 
        public static class AccessRequested
        {
            public const string Insert = "Insert";
            public const string Detail = "Detail";
            public const string Update = "Update";
            public const string Delete = "Delete";
            public const string Export = "Export";
            public const string Email = "Email";
            public const string Download = "Download";
        }
        #endregion

        /// <summary>
        /// Constants declared here for the profie permission names same as db.
        /// </summary>
        public static class ContentType
        {
            public const string ApplicationJson = "application/json";
        }

        #region Log Type
        public static class LogType
        {
            public const string Exception = "EXCEPTION";
            public const string Information = "INFORMATION";
            public const string Warning = "WARNING";
        }
        #endregion

        #region Images Extension 
        public static class Extensions
        {
            public const string PNG = ".png";
            public const string JPG = ".jpg";
            public const string JPEG = ".jpeg";
        }
        #endregion  

        #region WebConstants
        public static class WebConstants
        {

            public const string Data = "data";
            public const string Message = "message";
            public const string ResponseType = "responseType";
            public const string Login = "login";
            public const string Succeeded = "succeeded";
            public const string TempModelKeep = "TempModelKeep";
            public const string TempModelCheck = "TempModelCheck";

            public const string PersonalSettings = "PersonalSettings";
            public const string UserSettings = "UserSettings";
            public const string GroupSettings = "GroupSettings";

            //Roles
            public const string Owner = "owner";

            #region Controllers 
            public static class Controllers
            {
                public const string Account = "Account";
                public const string Home = "Home";
                public const string User = "User";
                public const string Company = "Company";
                public const string Leafs = "Leafs";
                public const string Error = "Error";
            }
            #endregion

            #region Action 
            public static class Actions
            {
                public const string Index = "Index";
                public const string ChangePassword = "ChangePassword";
            }
            #endregion

            #region PartialView 
            public static class PartialViews
            {
                public const string NewProfile = "_NewProfile";
                public const string SecurityProfile = "_SecurityProfile";
                public const string PersonalDetails = "_PersonalDetails";
                public const string ChangePassword = "_ChangePassword";
                public const string UserDetails = "_UserDetails";
                public const string CreateUser = "_CreateUser";
                public const string EditUser = "_EditUser";
                public const string UserList = "_UserList";
                public const string GroupList = "_GroupList";
                public const string GroupDetails = "_GroupDetail";
                public const string CreateGroup = "_CreateGroup";
                public const string EditCompany = "_EditCompany";
                public const string CompanyDetails = "_CompanyDetails";
                public const string DesignationDetails = "_DesignationDetails";
                public const string DesignationCreate = "_CreateUpdateDesignation";
                public const string DesignationUsers = "_DesignationUsers";

                public const string ProfilePermissions = "_ProfilePermissions";
                public const string ProfileUsers = "_ProfileUsers";

                // Location Partial views
                public const string CreateLocation = "_CreateLocation";
                public const string AllLocation = "_LocationList";
                public const string LocationDetails = "_LocationDetails";

                // Billing Partial Views
                public const string BillingList = "_BillingAddresses";

                //CompanySetup
                public const string PasswordGenrate = "_PasswordGenrate";
                public const string VerifyAddress = "_Address";
                public const string Comments = "_Comments";
                public const string OrderInfo = "_OrderInfo";



            }
            #endregion

            #region Urls 
            public static class Urls
            {
                public const string WEB_UserIndex = "/User/Index";
                public const string WEB_SecurityControls = "/SecurityControls/Index";
                public const string WEB_AccountLogin = "/Account/Login";
                public const string WEB_AccountLogout = "/Account/Logout";
                public const string WEB_ErrorIndexPage = "/Error/Index";
                public const string WEB_PersonalDetails = "/User/Details";
                public const string WEB_CompanyLocation = "/Location/CompanyLocation";
                public const string WEB_ResfreshToken = "/Account/ResfreshToken";

                public const string API_GetAllUsers = "User/all";
                public const string API_GetAllUsersWithLocation = "User/GetUsersWithCompanyLocation";
                public const string API_GetAllUsersPersonal = "User/GetUsersPersonal";
                public const string API_GetUser = "User/get";
                public const string API_GetPersonalUser = "User/GetPersonalUser";
                public const string API_PostUser = "User/insert";
                public const string API_PutUser = "User/update";
                public const string API_EditPersonalUser = "User/UpdatePersonalUser";
                public const string API_UserLogin = "User/Login";
                public const string API_GetAllDesignations = "Designation/all";
                public const string API_GetAllDesignationsByLocation = "Designation/GetDesignations";
                public const string API_GetPersonalDesignations = "Designation/GetPersonalDesignation";
                public const string API_GetUserByOnlyEmail = "User/GetUserByOnlyEmail";

                public const string API_GetAllRoles = "Role/all";
                public const string API_GetAllPersonalRoles = "Role/GetRolesForPersonalUser";
                public const string API_RolesJoinedUser = "Role/GetAuth_RoleJoinedUsers";
                public const string API_User_UpdateProfileImage = "User/UpdateProfileImage";
                public const string API_UpdatePersonalProfileImage = "User/UpdatePersonalProfileImage";
                public const string API_LogException = "log/exception";
                public const string API_Company_UpdateCompanyImage = "Company/UpdateCompanyImage";
                public const string API_GetUserByEmail = "User/GetUserByEmail";
                public const string API_GetCompanyUsersExceptCurrentCompanyLocation = "User/GetCompanyUsersExceptCurrentCompanyLocation";
                public const string API_GetExistingUserData = "User/GetExistingUserData";
                public const string API_GetUsersCounts = "User/GetUserCounts";
                public const string API_GetUserByEmailAndCompanyLocationAndCompanyId = "User/GetUserByEmailAndCompanyLocationAndCompanyId";

                //Company Urls
                public const string API_GetCompany = "Company/get";
                public const string API_PutCompany = "Company/update";
                public const string WEB_CompanyIndex = "/Company/Index/";

                //Web Routes
                public const string WEB_Route_404 = "error/404";
                public const string WEB_Route_Error_Codes = "/error/{0}";

                //Groups
                public const string API_GetAllGroups = "Group/all";
                public const string API_GetAllGroupsByLocation = "Group/GetGroups";
                public const string API_PostGroup = "Group/insert";
                public const string API_GetGroupName = "Group/GetGroupByName";
                public const string API_Group_UpdateGroupImage = "Group/UpdateGroupImage";
                public const string API_GetGroup = "Group/get";
                public const string API_UpdateGroup = "Group/update";
                public const string API_GetGroupCounts = "Group/GetGroupCounts";

                //Billing
                public const string API_GetAllBillings = "BillingAddresses/all";
                public const string API_GetBillingById = "BillingAddresses/get";
                public const string API_PostBillingAddresses = "BillingAddresses/insert";
                public const string API_UpdateBillingAddresses = "BillingAddresses/update";
                public const string API_DeleteBillingAddresses = "BillingAddresses/delete";
                public const string WEB_BillingIndex = "/Billing/Index";
                //RoleFeatures
                public const string API_GetAllProfilePermission = "ProfilePermission/all";
                public const string API_PostProfilePermission = "ProfilePermission/insert";
                public const string API_PutProfilePermission = "ProfilePermission/update";
                public const string API_GetProfilePermissions = "ProfilePermission/Profile";
                public const string API_GetProfilePermissionsByAuth_RoleId = "RoleFeatures/GetProfilePermissionsByAuth_RoleId";
                public const string API_PostInsertUpdateAuth_RoleFeatureMappingTemp = "RoleFeatures/InsertUpdateAuth_RoleFeatureMappingTemp";
                public const string API_PostBulkUpdateAuth_RoleFeatureMappingTemp = "RoleFeatures/BulkUpdateAuth_RoleFeatureMappingTemp";
                public const string API_GetProfilePermissionsByAuth_UserId = "User/UserFeatures";
                public const string API_PostAuth_RoleFeatureMappingChanges = "RoleFeatures/UpdateAuth_RoleFeatureMappingChanges";



                //Profile
                public const string API_GetRoleUsers = "Role/GetRoleUsers";
                public const string API_DeleteRoleUsers = "Role/delete";
                public const string API_GetRoleById = "Role/get";
                public const string API_UpdateRole = "Role/update";
                public const string API_CreateRole = "Role/insert";
                public const string API_GetRoleByName = "Role/GetRoleByName";

                //Reset Password 
                public const string WEB_ResetPassword = "Account/ResetPassword";
                public const string API_ResetPassword = "User/ResetPassword";

                //Forgot Password
                public const string API_ForgotPassword = "User/ForgotPassword";

                //Change Password
                public const string WEB_ChangePassword = "Account/ChangePassword";
                public const string API_ChangePassword = "User/ChangePassword";

                // Role Url
                // GetAssociatedUsersWithRoles
                //Designations
                public const string API_GetDesignationByID = "Designation/GetAssociatedUsersWithDesignation";
                public const string API_GetDesignationByIDPersonal = "Designation/GetAssociatedUsersWithDesignationforPersonal";
                public const string API_GetDesignationHierarchy = "DesignationHierarchy/GetDesignationHierarchy";
                public const string API_InsertDesignationHierarchy = "DesignationHierarchy/insert";
                public const string API_InsertDesignation = "Designation/insert";
                public const string API_ResetDesignationHierarchy = "DesignationHierarchy/ResetDesignationHierarchy";
                public const string API_GetDesignationById = "Designation/get";
                public const string API_UpdateDesignationById = "Designation/update";
                public const string API_DeleteDesignations = "Designation/delete";
                public const string API_GetDesignationByName = "Designation/GetDesignationByName";
                public const string API_GetDesignationUsers = "Designation/GetUsersWithDesignation";

                //Location
                public const string API_PostCompanyLocation = "Location/insert";
                public const string API_UpdateCompanyLocation = "Location/update";
                public const string API_GetAllCompanyLocation = "Location/all";
                public const string API_GetLocationById = "Location/get";
                public const string API_GetCompanyLocationByUserId = "Location/GetCompanyLocationByUserId";
                public const string API_GetLocationCountsByCompanyId = "Location/GetLocationCounts";
                public const string API_GetLocationName = "Location/GetLocationByName";

                //Product
                public const string API_GetProductsOfCompany = "Product/GetProductsOfCompany";
                //CompanySetup
                public const string API_PostCompanySetup = "CompanySetup/insert";
                public const string API_PostCompanySetupUpdate = "CompanySetup/update";
                public const string API_GetCompanySetupById = "CompanySetup/get";
                public const string API_GetVerifyProductLead = "CompanySetup/VerifyProductLead";
                public const string CompanyVerifyProductLead = "CompanySetup/Index";
                public const string API_GetProductLeadByEmail = "CompanySetup/GetProductLeadByEmail";
                public const string API_GetProductLeadByCompanyName = "CompanySetup/GetProductLeadByCompanyName";
                public const string API_GetProductLeadCheckAvailableDomain = "CompanySetup/GetProductLeadCheckAvailableDomain";

                //CompanyRegistration
                public const string CompanyRegistration = "CompanyRegistration/CompleteCompanyProfile";



            }
            #endregion
        }
        #endregion

        #region  ApiRequest
        public static class ApiRequestResponse
        {
            public const bool ResponseSuccess = true;
            public const bool ResponseFailed = false;
        }
        #endregion

        #region  addressType
        public static class AddressTypeNames
        {
            public const string UserAddress = "UserAddress";
            public const string CompanyAddress = "CompanyAddress";
            public const string BillingAddress = "BillingAddress";
        }
        #endregion

        #region Email 
        public static class EmailConstants
        {
            public static class EmailSubjects
            {
                public const string ResetPassword = "Reset Password";
                public const string ChangePassword = "Change Password";
                public const string WelcomeEmail = "Welcome Email";
                public const string Verfiy = "Verification email";
            }
        }
        #endregion

        #region permission and  featur
        public static class PermissionConstants
        {
            public static class FeatureName
            {
                public const string Users = "Users";
                public const string OrganizationSettings = "Organization Settings";
                public const string Groups = "Groups";
                public const string SecurityControls_Roles = "Security Controls|Roles";
                public const string SecurityControls_Designation = "Security Controls|Designation";
                public const string Locations = "Locations";
                public const string BillingAddresses = "Billing Details|Billing Addresses";
            }
            public static class CodeValue
            {
                public const string View = "View";
                public const string Add = "Add";
                public const string Update = "Update";
                public const string Delete = "Delete";
                public const string ViewPermissions = "View Permissions";
                public const string UpdatePermissions = "Update Permissions";
                public const string ViewUsers = "View Users";
            }
        }
        #endregion

        #region Api Constants
        public static class ApiConstants
        {
            public const string RootDesignationId = "00000000-0000-0000-0000-000000000000";
        }
        #endregion
    }
}

