// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Areas.Identity.Pages.Account;

// Razor PageModel (code-behind for Register.cshtml)
public class RegisterModel : PageModel
{
    // ========================================
    // IDENTITY SERVICES (Dependency Injection)
    // ========================================

    private readonly SignInManager<ApplicationUser> _signInManager;
    // Handles sign-in operations (login, external login, etc.)

    private readonly RoleManager<IdentityRole> _roleManager;
    // Works with roles (AspNetRoles table)
    // IdentityRole is a built-in entity type from ASP.NET Identity

    private readonly UserManager<ApplicationUser> _userManager;
    // Main service for User operations (create, validate, assign roles)
    // Works with AspNetUsers table

    private readonly IUserStore<ApplicationUser> _userStore;
    // Low-level storage abstraction used internally by UserManager

    private readonly IUserEmailStore<ApplicationUser> _emailStore;
    // Specialized store for handling email operations

    private readonly ILogger<RegisterModel> _logger;
    // Logging service

    private readonly IEmailSender _emailSender;
    // Your custom email sender (SMTP / PaperCut)

    public RegisterModel(
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<RegisterModel> logger,
        IEmailSender emailSender)
    {
        // These services are automatically injected by ASP.NET Core
        // because they were registered in Program.cs
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _signInManager = signInManager;
        _roleManager = roleManager;
        _logger = logger;
        _emailSender = emailSender;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    // Binds form values (POST) to this property automatically
    // Initialized to avoid null reference issues on GET
    public InputModel Input { get; set; } = new InputModel();

    // Holds role names loaded from database (used in UI radio buttons)
    public string[] RoleNames { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure 
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public string ReturnUrl { get; set; }
    // URL to redirect user after registration

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure 
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public IList<AuthenticationScheme> ExternalLogins { get; set; }
    // External providers (Google, Facebook, etc.)

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure 
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>

    // InputModel represents data coming from the registration form
    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        public string RoleName { get; set; }
        // Selected role from UI
    }

    // GET Registration Page
    public async Task OnGetAsync(string returnUrl = null)
    {
        // This method runs when the page is first loaded (HTTP GET request)
        // No form submission yet — just preparing data for the UI

        // Store return URL (used later after successful registration/login)
        // Comes from query string (?returnUrl=...)
        ReturnUrl = returnUrl;

        // ============================
        // EXTERNAL LOGIN PROVIDERS
        // ============================

        // _signInManager (ASP.NET Identity service)
        // Gets list of external authentication providers (Google, Facebook, etc.)
        // Returned as AuthenticationScheme objects
        ExternalLogins = (await _signInManager
            .GetExternalAuthenticationSchemesAsync()).ToList();

        // ============================
        // LOAD ROLES FROM DATABASE
        // ============================

        // _roleManager (ASP.NET Identity service)
        // .Roles → IQueryable<IdentityRole> (connected to database via EF Core)

        var roles = await _roleManager.Roles

            // Select only the Name column from AspNetRoles table
            .Select(role => role.Name)

            // Filter out Administrator role (not allowed for selection in UI)
            .Where(name => name != Roles.Administrator)

            // Execute query against database (EF Core)
            // Converts result into string[]
            .ToArrayAsync();

        // Assign roles to property used in Razor page
        // This is used to generate radio buttons for role selection
        RoleNames = roles;
    }

    // User submits a Form from Register.cshtml
    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        // If returnUrl is null → default to Home page "/"
        // Url.Content("~/") is a Razor helper that resolves to root URL
        returnUrl ??= Url.Content("~/");

        // _signInManager (ASP.NET Identity service)
        // Gets external login providers (Google, Facebook, etc.)
        // Needed if page reloads after validation errors
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        // ModelState (ASP.NET MVC/Razor Pages)
        // Checks validation attributes from InputModel ([Required], [EmailAddress], etc.)
        if (ModelState.IsValid)
        {
            // Create empty ApplicationUser object (in memory, NOT database yet)
            var user = CreateUser();

            // IUserStore (low-level Identity abstraction)
            // Sets username (stored later in AspNetUsers.UserName column)
            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);

            // IUserEmailStore (specialized store for email)
            // Sets Email (AspNetUsers.Email column)
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

            // Add your custom properties BEFORE saving to database
            user.DateOfBirth = Input.DateOfBirth;
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;

            // ============================
            // 🔥 THIS IS WHERE USER IS SAVED TO DATABASE
            // ============================

            // _userManager (ASP.NET Identity service)
            // Internally:
            // ✔ validates user (password rules, etc.)
            // ✔ hashes password
            // ✔ inserts into AspNetUsers table via EF Core
            var result = await _userManager.CreateAsync(user, Input.Password);

            // IdentityResult → tells if operation succeeded or failed
            if (result.Succeeded)
            {
                // ILogger (built-in logging)
                _logger.LogInformation("User created a new account with password.");

                // ============================
                // ROLE ASSIGNMENT (AspNetUserRoles table)
                // ============================

                // Roles class = your constants (e.g. "Employee", "Supervisor")

                if (Input.RoleName == Roles.Supervisor)
                {
                    // Adds TWO roles → creates 2 records in AspNetUserRoles
                    await _userManager.AddToRolesAsync(
                        user, [Roles.Employee, Roles.Supervisor]);
                }
                else
                {
                    // Adds ONE role
                    await _userManager.AddToRoleAsync(user, Roles.Employee);
                }

                // ============================
                // EMAIL CONFIRMATION FLOW
                // ============================

                // Gets user ID from AspNetUsers table
                var userId = await _userManager.GetUserIdAsync(user);

                // Generates secure token (NOT stored in DB directly)
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                // Encode token to make it URL-safe
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                // Url.Page (Razor helper)
                // Builds URL to another Razor Page (ConfirmEmail.cshtml)
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",   // page path
                    pageHandler: null,         // no specific handler method
                    values: new
                    {
                        area = "Identity",     // Identity area
                        userId = userId,
                        code = code,
                        returnUrl = returnUrl
                    },
                    protocol: Request.Scheme); // http or https

                // _emailSender → your custom service (SMTP / PaperCut)
                // Sends confirmation email with link
                await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    $"Please confirm your account by " +
                    $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                // ============================
                // REDIRECT / LOGIN DECISION
                // ============================

                // Option configured in Program.cs (RequireConfirmedAccount)
                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    // Redirect to confirmation page (NOT same page)
                    return RedirectToPage("RegisterConfirmation",
                        new { email = Input.Email, returnUrl = returnUrl });
                }
                else
                {
                    // _signInManager → creates authentication cookie (logs user in)
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Redirect to original page or home
                    return LocalRedirect(returnUrl);
                }
            }

            // If CreateAsync failed → loop through Identity errors
            // Example: weak password, duplicate email, etc.
            foreach (var error in result.Errors)
            {
                // Add error to ModelState → displayed in UI
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // ============================
        // RELOAD ROLES (IMPORTANT)
        // ============================

        // If validation fails, page reloads → roles must be reloaded
        // otherwise radio buttons will be empty
        var roles = await _roleManager.Roles
        // _roleManager → reads from AspNetRoles table

        .Select(role => role.Name)
        // Get only role names

        .Where(name => name != Roles.Administrator)
        // Exclude admin

        .ToArrayAsync();

        RoleNames = roles;

        // Return SAME page with validation errors
        return Page();
    }

    private ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of " +
                $"'{nameof(ApplicationUser)}'. " +
                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class " +
                $"and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException(
                "The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)_userStore;
    }
}
