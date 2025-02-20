using OfficeOpenXml;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmailMarketing.Data;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using EmailMarketing.Migrations;

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

        [HttpPost("UploadCsv") ]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { success = false, message = "Nenhum arquivo enviado." });
            }

            try
            {
                var clientes = new List<Cliente>();
                var fileName = file.FileName;

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    if (file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                    {
                        using (var reader = new StreamReader(stream))
                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            csv.Read();
                            csv.ReadHeader();

                            var colunasEsperadas = new[] { "Nome", "Email", "PromocaoId" };
                            var colunasDeFato = csv.HeaderRecord;

                            var colunasDeFatoArray = string.Join(", ", colunasDeFato);

                            colunasDeFato = colunasDeFatoArray.Split(';');

                            string[] colunas = new string[3];
                            foreach (var coluna in colunasDeFato)
                            {
                                if (coluna == "Nome")
                                {
                                    colunas[0] = coluna;
                                }
                                else if (coluna == "Email")
                                {
                                    colunas[1] = coluna;
                                }
                                else if (coluna == "PromocaoId")
                                {
                                    colunas[2] = coluna;
                                }
                            }

                            if (colunasDeFato == null || !colunasEsperadas.SequenceEqual(colunas))
                            {
                                return BadRequest(new { success = false, message = "Os títulos das colunas não correspondem ao esperado." });
                            }

                            while (csv.Read())
                            {
                                var nome = csv.GetField("Nome");
                                var email = csv.GetField("Email");
                                var promocaoIdStr = csv.GetField("PromocaoId");

                                if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(promocaoIdStr))
                                {
                                    continue;
                                }

                                if (!IsValidName(nome))
                                {
                                    return BadRequest(new { success = false, message = $"O campo 'Nome' é inválido na linha {csv.Context.Parser.Row}." });
                                }

                                if (!IsValidEmail(email))
                                {
                                    return BadRequest(new { success = false, message = $"O campo 'Email' é inválido na linha {csv.Context.Parser.Row}." });
                                }

                                if (!IsValidPromocaoId(promocaoIdStr, out int promocaoId))
                                {
                                    return BadRequest(new { success = false, message = $"O campo 'PromocaoId' na linha {csv.Context.Parser.Row} não é um número válido." });
                                }

                                var promocao = await _context.Promocoes.FindAsync(promocaoId);
                                if (promocao == null)
                                {
                                    return BadRequest(new { success = false, message = $"Promoção com ID {promocaoId} não encontrada na linha {csv.Context.Parser.Row}." });
                                }

                                clientes.Add(new Cliente
                                {
                                    Nome = nome,
                                    Email = email,
                                    PromocaoId = promocaoId,
                                    Promocao = promocao,
                                    ArquivoNome = fileName
                                });
                            }

                        }
                    }  
                    else if (file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                    {

                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets[0];
                            var rowCount = worksheet.Dimension?.Rows ?? 0;

                            if (rowCount == 0)
                            {
                                return BadRequest(new { success = false, message = "O arquivo Excel está vazio ou não contém dados." });
                            }

                            var colunasEsperadas = new[] { "Nome", "Email", "PromocaoId" };
                            var colunasDeFato = new List<string>();

                            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                            {
                                var nomeColuna = worksheet.Cells[1, col].Text;
                                if (string.IsNullOrEmpty(nomeColuna))
                                {
                                    break; 
                                }
                                colunasDeFato.Add(nomeColuna);
                            }

                            if (!colunasEsperadas.SequenceEqual(colunasDeFato))
                            {
                                return BadRequest(new { success = false, message = "Os títulos das colunas não correspondem ao esperado." });
                            }

                            for (int row = 2; row <= rowCount; row++)
                            {
                                var nome = worksheet.Cells[row, 1].Text;
                                var email = worksheet.Cells[row, 2].Text;
                                var promocaoIdStr = worksheet.Cells[row, 3].Text;

                                if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(promocaoIdStr))
                                {
                                    continue;
                                }

                                if (!IsValidName(nome))
                                {
                                    return BadRequest(new { success = false, message = $"O campo 'Nome' é inválido na linha {row}." });
                                }

                                if (!IsValidEmail(email))
                                {
                                    return BadRequest(new { success = false, message = $"O campo 'Email' é inválido na linha {row}." });
                                }

                                if (!IsValidPromocaoId(promocaoIdStr, out int promocaoId))
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
                                    Promocao = promocao,
                                    ArquivoNome = fileName
                                });
                            }
                        }
                    }
                }

                await _context.Clientes.AddRangeAsync(clientes);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Arquivo processado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpPost ("UploadSql")]
        public async Task<IActionResult> UploadSql(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { success = false, message = "Nenhum arquivo enviado." });
            }

            try
            {
                var fileName = file.FileName;
                var sqlCommands = new List<string>();

                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    string line;
                    while ((line = await stream.ReadLineAsync()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line) && !line.TrimStart().StartsWith("--"))
                        {
                            sqlCommands.Add(line);
                        }
                    }
                }

                var clientes = new List<Cliente>();

                foreach (var command in sqlCommands)
                {
                    // Exemplo de extração de valores de um comando SQL INSERT
                    // Ajuste conforme necessário para o formato do seu SQL
                    var match = Regex.Match(command, @"INSERT INTO Clientes \(Nome, Email, PromocaoId\) VALUES \('(?<Nome>[^']*)', '(?<Email>[^']*)', (?<PromocaoId>\d+)\)");
                    if (match.Success)
                    {
                        var nome = match.Groups["Nome"].Value;
                        var email = match.Groups["Email"].Value;
                        var promocaoIdStr = match.Groups["PromocaoId"].Value;

                        if (!IsValidName(nome))
                        {
                            return BadRequest(new { success = false, message = $"O campo 'Nome' é inválido no comando: {command}." });
                        }

                        if (!IsValidEmail(email))
                        {
                            return BadRequest(new { success = false, message = $"O campo 'Email' é inválido no comando: {command}." });
                        }

                        if (!IsValidPromocaoId(promocaoIdStr, out int promocaoId))
                        {
                            return BadRequest(new { success = false, message = $"O campo 'PromocaoId' não é um número válido no comando: {command}." });
                        }

                        var promocao = await _context.Promocoes.FindAsync(promocaoId);
                        if (promocao == null)
                        {
                            return BadRequest(new { success = false, message = $"Promoção com ID {promocaoId} não encontrada no comando: {command}." });
                        }

                        clientes.Add(new Cliente
                        {
                            Nome = nome,
                            Email = email,
                            PromocaoId = promocaoId,
                            Promocao = promocao,
                            ArquivoNome = fileName
                        });
                    }
                    else
                    {
                        return BadRequest(new { success = false, message = $"Comando SQL inválido: {command}." });
                    }
                }

                await _context.Clientes.AddRangeAsync(clientes);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Arquivo SQL processado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            var namePattern = @"^[A-Za-z]{2,}\s[A-Za-z]{2,}$";
            return Regex.IsMatch(name, namePattern);
        }

        public static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();
            if (string.IsNullOrWhiteSpace(trimmedEmail))
            {
                return false;
            }
            try
            {
                var emailAddress = new System.Net.Mail.MailAddress(email);
                return emailAddress.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPromocaoId(string promocaoIdStr, out int promocaoId)
        {
            return int.TryParse(promocaoIdStr, out promocaoId);
        }
    }
}