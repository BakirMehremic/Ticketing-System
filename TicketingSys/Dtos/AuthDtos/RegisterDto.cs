﻿using System.ComponentModel.DataAnnotations;

namespace TicketingSys.Dtos.AuthDtos;

public record RegisterDto(
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    [Required] string Password);