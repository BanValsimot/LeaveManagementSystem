// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

namespace LeaveManagementSystem.Web.Areas.Identity.Pages.Account;

// Razor PageModel for ConfirmEmail.cshtml
public class ConfirmEmailModel : PageModel
{
    // ============================
    // IDENTITY SERVICE
    // ============================

    private readonly UserManager<ApplicationUser> _userManager;
    // UserManager → ASP.NET Identity service
    // Used to read/update users in AspNetUsers table

    public ConfirmEmailModel(UserManager<ApplicationUser> userManager)
    {
        // Injected via Dependency Injection (configured in Program.cs)
        _userManager = userManager;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    // TempData → stores data between requests (uses cookies/session)
    // Used here to display success/error message in UI
    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(string userId, string code)
    {
        // This method runs when user clicks email confirmation link (HTTP GET)

        // ============================
        // VALIDATION OF INPUT
        // ============================

        // userId and code come from URL query string:
        // e.g. /ConfirmEmail?userId=123&code=ABC

        if (userId == null || code == null)
        {
            // If something is missing → redirect to Home page
            return RedirectToPage("/Index");
        }

        // ============================
        // LOAD USER FROM DATABASE
        // ============================

        // _userManager.FindByIdAsync → queries AspNetUsers table
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            // If user not found → return 404
            return NotFound($"Unable to load user with ID '{userId}'.");
        }

        // ============================
        // DECODE TOKEN
        // ============================

        // Token was encoded before sending email
        // Now we decode it back to original value
        code = Encoding.UTF8.GetString(
            WebEncoders.Base64UrlDecode(code));

        // ============================
        // CONFIRM EMAIL
        // ============================

        // _userManager → updates user in database
        // Internally:
        // ✔ verifies token
        // ✔ sets EmailConfirmed = true in AspNetUsers table
        var result = await _userManager.ConfirmEmailAsync(user, code);

        // ============================
        // SET STATUS MESSAGE
        // ============================

        // TempData → persists message to the view
        StatusMessage = result.Succeeded
            ? "Thank you for confirming your email."
            : "Error confirming your email.";

        // Return same page (ConfirmEmail.cshtml)
        return Page();
    }
}