# Modular User service.

## Extra configuration for `Web Project`.

##### Ensure HTTP request pipeline configuration.

```cs

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}").RequireAuthorization(); // enabling authentication for all endpoints
```


##### Add new authentication service and `UseUserconfiguration` which is the extension method `IServiceCollection` located at `Application\DiConfigutaion`.
```cs
  services.UseUserConfiguration();
  services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(x => { x.LoginPath = "/Login"; });
```

##### Create new authentication manager on `Web Project` 

```cs
public interface IAuthManager
    {
        Task<AuthResult> Login(string identity, string password);
    }
```

```cs
public class AuthenticationManager : IAuthenticationManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public AuthManager(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<AuthResult> Login(string identity, string password)
        {
            var user = await _userRepository.GetItemAsync(x => x.Email.ToLower() == identity.ToLower().Trim());
            var result = new AuthResult();
            if (user == null)
            {
                result.Success = false;
                result.Errors.Add("User not found");
                return result;
            }

            if (!Crypter.Verify(password, user.Password))
            {
                result.Success = false;
                result.Errors.Add("Invalid password");
                return result;
            }

            var httpContext = _httpContextAccessor.HttpContext;
            var claims = new List<Claim>
            {
                new("Id", user.Id.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
            result.Success = true;
            return result;
        }
    }

    public class AuthResult
    {
        public List<string> Errors = new();
        public bool Success;
    }
```

##### Login / Register User

```cs
public IActionResult Index() => View(new LoginVm());

        [HttpPost]
        public async Task<IActionResult> Index(LoginVm vm)
        {
            try
            {
                var result = await _authManager.Login(vm.Email, vm.Password);
                if (result.Success) return RedirectToAction("Index", "Home");
                ModelState.AddModelError(nameof(vm.Password), result.Errors.FirstOrDefault()!);
                vm.Password = "";
                return View(vm);
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message; // Send error notification.
                return View();
            }
        }
```
```cs
[HttpGet]
        public IActionResult Register() => View(new UserVm());

        [HttpPost]
        public async Task<IActionResult> Register(UserVm vm)
        {
            try
            {
                var userDto = new UserDto(vm.Name, vm.Gender, vm.Email, vm.Password, vm.Address, vm.Phone);
                var user = await _userService.CreateUser(userDto);
                TempData["success"] = $"User {user.Name} has been added"; // Send register success notificatio.
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message; // // Send error notification.
                return View();
            }
        }
```

##### Creating current user provider 

```cs
public interface ICurrentUserProvider
    {
        bool IsLoggedIn();
        Task<AuthUser> GetCurrentUser();
        long? GetCurrentUserId();
    }
```

```cs
public class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserRepository _userRepository;

        public CurrentUserProvider(IHttpContextAccessor contextAccessor, IUserRepository userRepository)
        {
            _contextAccessor = contextAccessor;
            _userRepository = userRepository;
        }

        public bool IsLoggedIn() 
            => GetCurrentUserId() != null;

        public async Task<AuthUser> GetCurrentUser()
        {
            var userId = GetCurrentUserId();
            if (userId.HasValue) return await _userRepository.FindOrThrowAsync(userId.Value);
            return null;
        }

        public long? GetCurrentUserId()
        {
            var userId = _contextAccessor.HttpContext?.User.FindFirstValue("Id");
            if (string.IsNullOrWhiteSpace(userId)) return null;
            return Convert.ToInt64(userId);
        }
    }
```

