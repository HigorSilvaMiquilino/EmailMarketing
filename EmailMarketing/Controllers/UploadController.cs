using OfficeOpenXml;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmailMarketing.Data;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailMarketing.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UploadController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { success = false, message = "Nenhum arquivo enviado." });
            }

            try
            {
                var clientes = new List<Cliente>();

                using (var stream = new MemoryStream())
                {
                    // Copy the uploaded file to a memory stream
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    // Use EPPlus to read the Excel file
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0]; // Get the first sheet
                        var rowCount = worksheet.Dimension?.Rows ?? 0;

                        if (rowCount == 0)
                        {
                            return BadRequest(new { success = false, message = "O arquivo Excel está vazio ou não contém dados." });
                        }

                        // Start from row 2 to skip the header
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var nome = worksheet.Cells[row, 1].Text;
                            var email = worksheet.Cells[row, 2].Text;
                            var promocaoIdStr = worksheet.Cells[row, 3].Text;

                            if (string.IsNullOrEmpty(nome))
                            {
                                return BadRequest(new { success = false, message = $"O campo 'Nome' está vazio na linha {row}." });
                            }

                            if (string.IsNullOrEmpty(email))
                            {
                                return BadRequest(new { success = false, message = $"O campo 'Email' está vazio na linha {row}." });
                            }

                            if (!int.TryParse(promocaoIdStr, out int promocaoId))
                            {
                                return BadRequest(new { success = false, message = $"O campo 'PromocaoId' na linha {row} não é um número válido." });
                            }

                            var promocao = await _context.Promocoes.FindAsync(promocaoId);
                            if (promocao == null)
                            {
                                return BadRequest(new { success = false, message = $"Promoção com ID {promocaoId} não encontrada na linha {row}." });
                            }

                            clientes.Add(new Cliente
                            {
                                Nome = nome,
                                Email = email,
                                PromocaoId = promocaoId,
                                Promocao = promocao
                            });
                        }
                    }
                }

                // Save to the database
                await _context.Clientes.AddRangeAsync(clientes);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Arquivo processado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Erro interno no servidor: {ex.Message}" });
            }
        }
    }
}