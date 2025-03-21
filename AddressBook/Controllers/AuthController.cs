﻿using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;

namespace AddressBook.Controllers
{
    /// <summary>
    /// API Controller for User Authentication.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserBL _userService;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public AuthController(IUserBL userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registers a new user if the email does not already exist.
        /// </summary>
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDTO request)
        {
            try
            {
                bool isRegistered = _userService.RegisterUser(request);
                if (!isRegistered)
                {
                    logger.Warn("Registration failed: User with email {0} already exists.", request.Email);
                    return BadRequest(new ResponseBody<string> { Success = false, Message = "User with this email already exists." });
                }
                logger.Info("User registered successfully: {0}", request.Email);
                return Ok(new ResponseBody<string> { Success = true, Message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred while registering user.");
                return StatusCode(500, new ResponseBody<string> { Success = false, Message = "Internal Server Error." });
            }
        }

        /// <summary>
        /// Authenticates the user and returns a JWT token if credentials are valid.
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUserDTO request)
        {
            try
            {
                var token = _userService.LoginUser(request);
                if (token == null)
                {
                    logger.Warn("Login failed: Invalid credentials for {0}", request.Email);
                    return Unauthorized(new ResponseBody<string> { Success = false, Message = "Invalid email or password." });
                }
                logger.Info("User logged in successfully: {0}", request.Email);
                return Ok(new ResponseBody<string> { Success = true, Message = "Login successful.", Data = token });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred during login.");
                return StatusCode(500, new ResponseBody<string> { Success = false, Message = "Internal Server Error." });
            }
        }

        /// <summary>
        /// Sends a password reset email if the provided email exists.
        /// </summary>
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordDTO request)
        {
            try
            {
                bool isSent = _userService.ForgotPassword(request.Email);
                if (!isSent)
                {
                    logger.Warn("Password reset failed: Email not found {0}", request.Email);
                    return BadRequest(new ResponseBody<string> { Success = false, Message = "Email not found." });
                }
                logger.Info("Password reset email sent successfully: {0}", request.Email);
                return Ok(new ResponseBody<string> { Success = true, Message = "Reset password email sent successfully." });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred during password reset request.");
                return StatusCode(500, new ResponseBody<string> { Success = false, Message = "Internal Server Error." });
            }
        }


        /// <summary>
        /// Resets the user's password using the provided token and new password.
        /// </summary>
        /// <param name="request">Token and new password details.</param>
        /// <returns>Success message if reset is successful, otherwise an error.</returns>
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.NewPassword))
                return BadRequest(new ResponseBody<string> { Success = false, Message = "Token and new password are required." });

            bool isResetSuccessful = _userService.ResetPassword(request.Token, request.NewPassword);
            if (!isResetSuccessful)
                return Unauthorized(new ResponseBody<string> { Success = false, Message = "Invalid or expired token." });

            return Ok(new ResponseBody<string> { Success = true, Message = "Password reset successfully." });
        }

    }
}
