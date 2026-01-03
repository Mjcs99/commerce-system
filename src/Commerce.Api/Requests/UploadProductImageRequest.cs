namespace Commerce.Api.Requests;
using Microsoft.AspNetCore.Http;

public sealed record UploadProductImageRequest(
    IFormFile File,
    bool IsPrimary
);