// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

namespace LeaveManagementSystem.Web.Areas.Identity.Pages.Account;

public class LoginModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty] //Bind Model to the Form
    public InputModel Input { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string ErrorMessage { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    // ============================
    // GET: Load the Login Page
    // ============================
    public async Task OnGetAsync(string returnUrl = null)
    {
        // If there is an error message stored (TempData), add it to ModelState
        // so it can be displayed on the page
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        // If no return URL is provided, default to Home page ("/")
        returnUrl ??= Url.Content("~/");

        // Clear any existing external login cookie (e.g., Google login)
        // This ensures a clean login process without leftover authentication data
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        // Get list of external login providers (Google, Facebook, etc.)
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        // Store return URL so we can redirect after successful login
        ReturnUrl = returnUrl;
    }

    // ============================
    // POST: Submit Login Form
    // ============================
    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        // If no return URL is provided, default to Home page
        returnUrl ??= Url.Content("~/");

        // Reload external login providers (needed if page reloads due to error)
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        // Check if input data passed validation (attributes like [Required])
        if (ModelState.IsValid)
        {
            // Attempt to sign in user with email and password
            // lockoutOnFailure: false → failed attempts do NOT lock the account
            var result = await _signInManager.PasswordSignInAsync(
                Input.Email,
                Input.Password,
                Input.RememberMe,
                lockoutOnFailure: false);

            // ============================
            // SUCCESS: Login successful
            // ============================
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");

                // Redirect user to original page (or Home if none)
                return LocalRedirect(returnUrl);
            }

            // ============================
            // TWO-FACTOR AUTH REQUIRED
            // ============================
            if (result.RequiresTwoFactor)
            {
                // Redirect to 2FA page
                return RedirectToPage("./LoginWith2fa", new
                {
                    ReturnUrl = returnUrl,
                    RememberMe = Input.RememberMe
                });
            }

            // ============================
            // ACCOUNT LOCKED
            // ============================
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");

                // Redirect to lockout info page
                return RedirectToPage("./Lockout");
            }
            else
            {
                // ============================
                // LOGIN FAILED
                // ============================
                // Add general error message (not tied to a specific field)
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                // Redisplay the login page with error
                return Page();
            }
        }

        // If validation failed (e.g., missing email/password)
        // Redisplay the form with validation errors
        return Page();
    }
}
